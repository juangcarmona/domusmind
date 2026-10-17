using DomusMind.Application.Abstractions.Integrations.Calendar;
using DomusMind.Application.Features.Calendar;
using DomusMind.Domain.Calendar.ExternalConnections;
using DomusMind.Infrastructure.Persistence;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace DomusMind.Infrastructure.Integrations.Calendar.Microsoft;

/// <summary>Owns the server-side Microsoft OAuth transaction and delegated token cache.</summary>
public sealed class MicrosoftGraphCalendarAuthService : IExternalCalendarAuthService
{
    private const string CachedAccessTokenProperty = "CachedAccessToken";
    private const string AccessTokenExpiresAtUtcProperty = "AccessTokenExpiresAtUtc";
    private const string EncryptedTokenCacheProperty = "EncryptedRefreshToken";
    private const string GrantedScopesProperty = "GrantedScopes";
    private const string CalendarReadScope = "https://graph.microsoft.com/Calendars.Read";
    private static readonly string[] DelegatedScopes = [CalendarReadScope];

    private readonly DomusMindDbContext _dbContext;
    private readonly MicrosoftGraphOptions _options;
    private readonly MicrosoftTokenCacheProtector _protector;
    private readonly ILogger<MicrosoftGraphCalendarAuthService> _logger;

    public MicrosoftGraphCalendarAuthService(
        DomusMindDbContext dbContext,
        IOptions<MicrosoftGraphOptions> options,
        ILogger<MicrosoftGraphCalendarAuthService> logger)
    {
        _dbContext = dbContext;
        _options = options.Value;
        _protector = new MicrosoftTokenCacheProtector(_options);
        _logger = logger;
    }

    public async Task<string?> CreateAuthorizationUrlAsync(
        Guid familyId,
        Guid memberId,
        Guid requestedByUserId,
        string redirectUri,
        CancellationToken cancellationToken = default)
    {
        if (!_options.IsConfigured)
            return null;

        if (!_options.IsRedirectUriAllowed(redirectUri))
            throw new CalendarException(CalendarErrorCode.InvalidInput, "OAuth redirect URI is not allowed.");

        var state = CreateRandomUrlToken(32);
        var verifier = CreateRandomUrlToken(32);
        var challenge = Base64UrlEncode(SHA256.HashData(Encoding.ASCII.GetBytes(verifier)));
        var now = DateTime.UtcNow;

        var expiredTransactions = await _dbContext.Set<ExternalCalendarOAuthTransaction>()
            .Where(transaction => transaction.ExpiresAtUtc <= now)
            .ToListAsync(cancellationToken);

        if (expiredTransactions.Count > 0)
        {
            _dbContext.RemoveRange(expiredTransactions);
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                foreach (var expired in expiredTransactions)
                    _dbContext.Entry(expired).State = EntityState.Detached;
            }
        }

        _dbContext.Set<ExternalCalendarOAuthTransaction>().Add(new ExternalCalendarOAuthTransaction(
            HashState(state),
            requestedByUserId,
            familyId,
            memberId,
            redirectUri,
            _protector.Protect(verifier),
            now.AddMinutes(10)));
        await _dbContext.SaveChangesAsync(cancellationToken);

        var tenantId = string.IsNullOrWhiteSpace(_options.TenantId) ? "common" : _options.TenantId;
        var authorizeUri = $"https://login.microsoftonline.com/{Uri.EscapeDataString(tenantId)}/oauth2/v2.0/authorize";
        return QueryHelpers.AddQueryString(authorizeUri, new Dictionary<string, string?>
        {
            ["client_id"] = _options.ClientId,
            ["response_type"] = "code",
            ["redirect_uri"] = redirectUri,
            ["scope"] = "https://graph.microsoft.com/Calendars.Read offline_access",
            ["state"] = state,
            ["response_mode"] = "query",
            ["code_challenge"] = challenge,
            ["code_challenge_method"] = "S256",
        });
    }

    public async Task<ExternalCalendarProviderAccount> ExchangeAuthorizationCodeAsync(
        string authorizationCode,
        string state,
        Guid familyId,
        Guid memberId,
        Guid requestedByUserId,
        string redirectUri,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(authorizationCode))
            throw new CalendarException(CalendarErrorCode.InvalidInput, "Authorization code is required.");

        if (!string.IsNullOrWhiteSpace(redirectUri) && !_options.IsRedirectUriAllowed(redirectUri))
            throw new CalendarException(CalendarErrorCode.InvalidInput, "OAuth redirect URI is not allowed.");

        var verifier = await ConsumeAuthorizationStateAsync(
            state,
            familyId,
            memberId,
            requestedByUserId,
            redirectUri,
            cancellationToken);

        if (verifier is null)
            throw new CalendarException(CalendarErrorCode.InvalidInput, "Outlook authorization request is invalid or has expired.");

        var tokenCacheEnvelope = new TokenCacheEnvelope();
        var app = BuildConfidentialClient(redirectUri, tokenCacheEnvelope);

        AuthenticationResult result;
        try
        {
            result = await app
                .AcquireTokenByAuthorizationCode(DelegatedScopes, authorizationCode)
                .WithPkceCodeVerifier(verifier)
                .ExecuteAsync(cancellationToken);
        }
        catch (MsalException ex)
        {
            _logger.LogWarning("Microsoft authorization code exchange failed with error code {ErrorCode}.", ex.ErrorCode);
            throw new InvalidOperationException("Authorization code exchange failed.", ex);
        }

        if (result.Account is null)
            throw new InvalidOperationException("Microsoft authorization succeeded but no account was returned.");

        if (!MicrosoftGraphScopePolicy.HasOnlyCalendarRead(result.Scopes))
            throw new CalendarException(CalendarErrorCode.ProviderAuthFailed, "Microsoft did not grant the required read-only calendar permission.");

        if (string.IsNullOrWhiteSpace(tokenCacheEnvelope.SerializedCache))
            throw new InvalidOperationException("Microsoft authorization succeeded but token cache serialization produced no data.");

        var providerAccountId = result.Account.HomeAccountId?.Identifier ?? result.UniqueId;
        var accountEmail = result.Account.Username ?? string.Empty;
        var grantedScopes = string.Join(" ", result.Scopes);

        return new ExternalCalendarProviderAccount(
            providerAccountId,
            accountEmail,
            result.TenantId,
            _protector.Protect(tokenCacheEnvelope.SerializedCache),
            grantedScopes);
    }

    internal async Task<string?> ConsumeAuthorizationStateAsync(
        string state,
        Guid familyId,
        Guid memberId,
        Guid requestedByUserId,
        string redirectUri,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(state) || string.IsNullOrWhiteSpace(redirectUri))
            return null;

        var stateHash = HashState(state);
        var now = DateTime.UtcNow;
        var transaction = await _dbContext.Set<ExternalCalendarOAuthTransaction>()
            .FirstOrDefaultAsync(candidate =>
                candidate.StateHash == stateHash &&
                candidate.FamilyId == familyId &&
                candidate.MemberId == memberId &&
                candidate.RequestedByUserId == requestedByUserId &&
                candidate.RedirectUri == redirectUri &&
                candidate.ExpiresAtUtc > now,
                cancellationToken);

        if (transaction is null || !_protector.TryUnprotect(transaction.ProtectedPkceVerifier, out var verifier))
            return null;

        _dbContext.Remove(transaction);
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return verifier;
        }
        catch (DbUpdateConcurrencyException)
        {
            return null;
        }
    }

    public async Task<string?> GetAccessTokenAsync(
        Guid connectionId,
        CancellationToken cancellationToken = default)
    {
        var connection = await _dbContext
            .Set<ExternalCalendarConnection>()
            .FirstOrDefaultAsync(
                c => c.Id == ExternalCalendarConnectionId.From(connectionId),
                cancellationToken);

        if (connection is null)
            return null;

        var entry = _dbContext.Entry(connection);
        var cachedToken = entry.Property<string?>(CachedAccessTokenProperty).CurrentValue;
        var expiresAtUtc = entry.Property<DateTime?>(AccessTokenExpiresAtUtcProperty).CurrentValue;

        if (!string.IsNullOrWhiteSpace(cachedToken) &&
            expiresAtUtc.HasValue &&
            expiresAtUtc.Value > DateTime.UtcNow.AddMinutes(5) &&
            _protector.TryUnprotect(cachedToken, out var accessToken))
        {
            return accessToken;
        }

        if (!string.IsNullOrWhiteSpace(cachedToken))
        {
            entry.Property<string?>(CachedAccessTokenProperty).CurrentValue = null;
            entry.Property<DateTime?>(AccessTokenExpiresAtUtcProperty).CurrentValue = null;
        }

        var protectedTokenCache = entry.Property<string?>(EncryptedTokenCacheProperty).CurrentValue;
        if (string.IsNullOrWhiteSpace(protectedTokenCache))
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("No Microsoft token cache is stored for connection {ConnectionId}. Reconnect is required.", connectionId);
            return null;
        }

        if (!_protector.TryUnprotect(protectedTokenCache, out var serializedTokenCache))
        {
            ClearTokenMaterial(entry);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogWarning("Microsoft token cache for connection {ConnectionId} is legacy, invalid, or encrypted with a different key. Reconnect is required.", connectionId);
            return null;
        }

        try
        {
            var tokenCacheEnvelope = new TokenCacheEnvelope(serializedTokenCache);
            var app = BuildConfidentialClient(redirectUri: null, tokenCacheEnvelope);
            var account = await ResolveAccountAsync(app, connection);
            if (account is null)
            {
                _logger.LogInformation("No cached MSAL account could be resolved for connection {ConnectionId}. Reconnect is required.", connectionId);
                return null;
            }

            AuthenticationResult result;
            try
            {
                result = await app
                    .AcquireTokenSilent(DelegatedScopes, account)
                    .ExecuteAsync(cancellationToken);
            }
            catch (MsalUiRequiredException ex)
            {
                _logger.LogInformation("Silent Microsoft token acquisition requires reauthentication for connection {ConnectionId}: {ErrorCode}.", connectionId, ex.ErrorCode);
                return null;
            }

            entry.Property<string?>(CachedAccessTokenProperty).CurrentValue = _protector.Protect(result.AccessToken);
            entry.Property<DateTime?>(AccessTokenExpiresAtUtcProperty).CurrentValue = result.ExpiresOn.UtcDateTime;
            entry.Property<string?>(EncryptedTokenCacheProperty).CurrentValue =
                _protector.Protect(tokenCacheEnvelope.SerializedCache ?? serializedTokenCache);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return result.AccessToken;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning("Failed to acquire delegated Microsoft access token for connection {ConnectionId}: {ErrorType}.", connectionId, ex.GetType().Name);
            return null;
        }
    }

    public async Task RevokeAsync(Guid connectionId, CancellationToken cancellationToken = default)
    {
        var connection = await _dbContext
            .Set<ExternalCalendarConnection>()
            .FirstOrDefaultAsync(
                c => c.Id == ExternalCalendarConnectionId.From(connectionId),
                cancellationToken);

        if (connection is null)
            return;

        ClearTokenMaterial(_dbContext.Entry(connection));
        _dbContext.Entry(connection).Property<string?>(GrantedScopesProperty).CurrentValue = null;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private IConfidentialClientApplication BuildConfidentialClient(
        string? redirectUri,
        TokenCacheEnvelope tokenCacheEnvelope)
    {
        var tenantId = string.IsNullOrWhiteSpace(_options.TenantId) ? "common" : _options.TenantId;
        var builder = ConfidentialClientApplicationBuilder
            .Create(_options.ClientId!)
            .WithClientSecret(_options.ClientSecret!)
            .WithAuthority(new Uri($"https://login.microsoftonline.com/{Uri.EscapeDataString(tenantId)}"))
            .WithLegacyCacheCompatibility(false);

        if (!string.IsNullOrWhiteSpace(redirectUri))
            builder = builder.WithRedirectUri(redirectUri);

        var app = builder.Build();
        RegisterTokenCacheSerialization(app.UserTokenCache, tokenCacheEnvelope);
        return app;
    }

    private static void RegisterTokenCacheSerialization(ITokenCache tokenCache, TokenCacheEnvelope envelope)
    {
        tokenCache.SetBeforeAccess(args =>
        {
            if (!string.IsNullOrWhiteSpace(envelope.SerializedCache))
                args.TokenCache.DeserializeMsalV3(Encoding.UTF8.GetBytes(envelope.SerializedCache), shouldClearExistingCache: true);
        });

        tokenCache.SetAfterAccess(args =>
        {
            if (args.HasStateChanged)
                envelope.SerializedCache = Encoding.UTF8.GetString(args.TokenCache.SerializeMsalV3());
        });
    }

    private static async Task<IAccount?> ResolveAccountAsync(
        IConfidentialClientApplication app,
        ExternalCalendarConnection connection)
    {
        if (string.IsNullOrWhiteSpace(connection.ProviderAccountId))
            return null;

        return await app.GetAccountAsync(connection.ProviderAccountId).ConfigureAwait(false);
    }

    private static void ClearTokenMaterial(EntityEntry<ExternalCalendarConnection> entry)
    {
        entry.Property<string?>(EncryptedTokenCacheProperty).CurrentValue = null;
        entry.Property<string?>(CachedAccessTokenProperty).CurrentValue = null;
        entry.Property<DateTime?>(AccessTokenExpiresAtUtcProperty).CurrentValue = null;
        entry.Property<string?>(GrantedScopesProperty).CurrentValue = null;
    }

    private static string CreateRandomUrlToken(int byteCount) => Base64UrlEncode(RandomNumberGenerator.GetBytes(byteCount));

    private static string Base64UrlEncode(byte[] value) =>
        Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private static string HashState(string state) =>
        Convert.ToHexString(SHA256.HashData(Encoding.ASCII.GetBytes(state)));

    private sealed class TokenCacheEnvelope
    {
        public TokenCacheEnvelope()
        {
        }

        public TokenCacheEnvelope(string serializedCache) => SerializedCache = serializedCache;

        public string? SerializedCache { get; set; }
    }
}

internal static class MicrosoftGraphScopePolicy
{
    private static readonly HashSet<string> NonResourceScopes = new(StringComparer.OrdinalIgnoreCase)
    {
        "offline_access",
        "openid",
        "profile",
    };

    public static bool HasOnlyCalendarRead(IEnumerable<string> scopes)
    {
        var grantedGraphScopes = scopes
            .Select(scope => scope.StartsWith("https://graph.microsoft.com/", StringComparison.OrdinalIgnoreCase)
                ? scope["https://graph.microsoft.com/".Length..]
                : scope)
            .Where(scope => !NonResourceScopes.Contains(scope))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return grantedGraphScopes.SetEquals(["Calendars.Read"]);
    }
}
