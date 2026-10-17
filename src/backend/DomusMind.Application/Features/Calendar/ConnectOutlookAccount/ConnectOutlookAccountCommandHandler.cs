using DomusMind.Application.Abstractions.Integrations.Calendar;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Calendar;
using DomusMind.Domain.Calendar.ExternalConnections;
using DomusMind.Domain.Family;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DomusMind.Application.Features.Calendar.ConnectOutlookAccount;

public sealed class ConnectOutlookAccountCommandHandler
    : ICommandHandler<ConnectOutlookAccountCommand, ExternalCalendarConnectionDetailResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IEventLogWriter _eventLogWriter;
    private readonly IFamilyAuthorizationService _authorizationService;
    private readonly IExternalCalendarAuthService _authService;
    private readonly IExternalCalendarProviderClient _providerClient;
    private readonly ILogger<ConnectOutlookAccountCommandHandler> _logger;

    public ConnectOutlookAccountCommandHandler(
        IDomusMindDbContext dbContext,
        IEventLogWriter eventLogWriter,
        IFamilyAuthorizationService authorizationService,
        IExternalCalendarAuthService authService,
        IExternalCalendarProviderClient providerClient,
        ILogger<ConnectOutlookAccountCommandHandler> logger)
    {
        _dbContext = dbContext;
        _eventLogWriter = eventLogWriter;
        _authorizationService = authorizationService;
        _authService = authService;
        _providerClient = providerClient;
        _logger = logger;
    }

    public async Task<ExternalCalendarConnectionDetailResponse> Handle(
        ConnectOutlookAccountCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.AuthorizationCode))
            throw new CalendarException(CalendarErrorCode.InvalidInput, "Authorization code is required.");

        if (string.IsNullOrWhiteSpace(command.State))
            throw new CalendarException(CalendarErrorCode.InvalidInput, "OAuth state is required.");

        if (string.IsNullOrWhiteSpace(command.RedirectUri))
            throw new CalendarException(CalendarErrorCode.InvalidInput, "Redirect URI is required.");

        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            command.RequestedByUserId, command.FamilyId, cancellationToken);

        if (!canAccess)
            throw new CalendarException(CalendarErrorCode.AccessDenied, "Access to this family is denied.");

        ExternalCalendarProviderAccount providerAccount;
        try
        {
            providerAccount = await _authService.ExchangeAuthorizationCodeAsync(
                command.AuthorizationCode,
                command.State,
                command.FamilyId,
                command.MemberId,
                command.RequestedByUserId,
                command.RedirectUri,
                cancellationToken);
        }
        catch (Exception ex) when (ex is not CalendarException)
        {
            _logger.LogWarning(
                "OAuth code exchange failed for member {MemberId} with error type {ErrorType}",
                command.MemberId,
                ex.GetType().Name);
            throw new CalendarException(CalendarErrorCode.ProviderAuthFailed, "Authorization code exchange failed.");
        }

        // Reusing the same provider account for the same member is a reconnect.
        // It replaces credentials while preserving the connection, feeds and imported entries.
        var memberId = MemberId.From(command.MemberId);
        var existing = await _dbContext
            .Set<ExternalCalendarConnection>()
            .FirstOrDefaultAsync(c =>
                c.Provider == ExternalCalendarProvider.Microsoft &&
                c.ProviderAccountId == providerAccount.ProviderAccountId &&
                c.Status != ExternalCalendarConnectionStatus.Disconnected,
                cancellationToken);

        if (existing is not null &&
            (existing.FamilyId != FamilyId.From(command.FamilyId) || existing.MemberId != memberId))
            throw new CalendarException(CalendarErrorCode.ConnectionAlreadyExists,
                "An active connection for this provider account already exists.");

        string? accessToken;
        try
        {
            var familyId = FamilyId.From(command.FamilyId);
            var now = DateTime.UtcNow;

            var isReconnect = existing is not null;
            var connection = existing ?? ExternalCalendarConnection.Connect(
                ExternalCalendarConnectionId.New(),
                familyId,
                memberId,
                ExternalCalendarProvider.Microsoft,
                providerAccount.ProviderAccountId,
                providerAccount.AccountEmail,
                command.AccountDisplayLabel,
                providerAccount.TenantId,
                now);

            if (!isReconnect)
                _dbContext.Set<ExternalCalendarConnection>().Add(connection);

            // Write auth material into shadow properties before the initial persistence call.
            // Without this the token cache is not persisted and every subsequent
            // GetAccessTokenAsync call immediately returns null.
            _dbContext.SetExternalCalendarConnectionAuthMaterial(
                connection,
                providerAccount.EncryptedRefreshToken,
                providerAccount.GrantedScopes);

            if (!isReconnect)
            {
                await _eventLogWriter.WriteAsync([connection], cancellationToken);
                connection.ClearDomainEvents();
            }
            else
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation(
                "External calendar connection {ConnectionId} {Operation} for member {MemberId}.",
                connection.Id.Value, isReconnect ? "reconnected" : "created", command.MemberId);

            // Discover available calendars using a fresh access token
            accessToken = await _authService.GetAccessTokenAsync(connection.Id.Value, cancellationToken);
            IReadOnlyCollection<ExternalCalendarProviderCalendar> availableCalendars = [];

            if (accessToken is not null)
            {
                try
                {
                    availableCalendars = await _providerClient.GetCalendarsAsync(accessToken, cancellationToken);
                    foreach (var cal in availableCalendars)
                    {
                        connection.AddOrUpdateFeed(cal.CalendarId, cal.CalendarName, cal.IsDefault, false, now);
                    }
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Discovered {CalendarCount} provider calendar(s) for connection {ConnectionId}",
                        availableCalendars.Count, connection.Id.Value);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Calendar discovery failed for connection {ConnectionId}, continuing without feed list", connection.Id.Value);
                }
            }
            else
            {
                _logger.LogWarning(
                    "Could not acquire access token immediately after connection {ConnectionId} was created — calendar discovery skipped",
                    connection.Id.Value);
            }

            var feedResponses = connection.Feeds.Select(f => new ExternalCalendarFeedResponse(
                f.ProviderCalendarId, f.CalendarName, f.IsSelected,
                f.LastSuccessfulSyncUtc, f.WindowStartUtc, f.WindowEndUtc)).ToList();

            var availableResponses = availableCalendars.Select(c => new AvailableExternalCalendarResponse(
                c.CalendarId, c.CalendarName, c.IsDefault, false)).ToList();

            return new ExternalCalendarConnectionDetailResponse(
                connection.Id.Value,
                connection.MemberId.Value,
                ExternalCalendarProviderNames.ToProviderString(connection.Provider),
                ExternalCalendarProviderNames.ToProviderLabel(connection.Provider),
                connection.AccountEmail,
                connection.AccountDisplayLabel,
                connection.TenantId,
                connection.Horizon.ForwardHorizonDays,
                connection.ScheduledRefreshEnabled,
                connection.ScheduledRefreshIntervalMinutes,
                connection.LastSuccessfulSyncUtc,
                connection.LastSyncAttemptUtc,
                connection.LastSyncFailureUtc,
                ExternalCalendarConnectionStatusNames.ToStatusString(connection.Status),
                false,
                0,
                feedResponses,
                availableResponses,
                connection.LastErrorCode,
                connection.LastErrorMessage);
        }
        catch (CalendarException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist external calendar connection for member {MemberId}", command.MemberId);
            throw new CalendarException(CalendarErrorCode.ProviderApiError, "Failed to complete connection setup.");
        }
    }
}
