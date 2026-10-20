using System.Security.Claims;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace DomusMind.Api.Controllers;

[ApiController]
public sealed class McpOAuthController : ControllerBase
{
    private readonly IAuthUserRepository _users;
    private readonly IPasswordHasher _passwords;
    private readonly IFamilyAuthorizationService _familyAuthorization;
    private readonly IOptions<McpOAuthOptions> _options;
    private readonly IConfiguration _configuration;
    private readonly IOpenIddictApplicationManager _applications;
    private readonly IOpenIddictAuthorizationManager _authorizations;
    private readonly IOpenIddictTokenManager _tokens;

    public McpOAuthController(
        IAuthUserRepository users,
        IPasswordHasher passwords,
        IFamilyAuthorizationService familyAuthorization,
        IOptions<McpOAuthOptions> options,
        IConfiguration configuration,
        IOpenIddictApplicationManager applications,
        IOpenIddictAuthorizationManager authorizations,
        IOpenIddictTokenManager tokens)
    {
        _users = users;
        _passwords = passwords;
        _familyAuthorization = familyAuthorization;
        _options = options;
        _configuration = configuration;
        _applications = applications;
        _authorizations = authorizations;
        _tokens = tokens;
    }

    [HttpGet("/.well-known/oauth-protected-resource/mcp")]
    [AllowAnonymous]
    public IActionResult ProtectedResourceMetadata()
    {
        if (!_options.Value.Enabled)
            return NotFound();

        return Ok(new
        {
            resource = _options.Value.Resource,
            authorization_servers = new[] { _options.Value.Issuer },
            scopes_supported = _configuration.GetValue<bool>("Mcp:Capabilities:HouseholdWrites")
                ? new[] { McpOAuthConstants.ReadScope, McpOAuthConstants.WriteScope }
                : new[] { McpOAuthConstants.ReadScope },
            bearer_methods_supported = new[] { "header" },
        });
    }

    [HttpGet("/oauth/authorize")]
    [HttpPost("/oauth/authorize")]
    [AllowAnonymous]
    public async Task<IActionResult> Authorize(CancellationToken cancellationToken)
    {
        if (!_options.Value.Enabled)
            return NotFound();

        if (HttpMethods.IsPost(Request.Method))
        {
            await HttpContext.RequestServices
                .GetRequiredService<Microsoft.AspNetCore.Antiforgery.IAntiforgery>()
                .ValidateRequestAsync(HttpContext);
        }

        var request = HttpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenIddict request cannot be resolved.");

        var principal = await AuthenticateAuthorizationUserAsync(cancellationToken);
        if (principal is null)
        {
            var returnUrl = Request.PathBase + Request.Path + Request.QueryString;
            return Challenge(
                new AuthenticationProperties { RedirectUri = returnUrl },
                McpOAuthConstants.CookieScheme);
        }

        if (!Guid.TryParse(
                principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? principal.FindFirstValue(OpenIddictConstants.Claims.Subject),
                out var userId))
        {
            return Unauthorized(new { error = "oauth_user_mapping_failed" });
        }

        if (!Guid.TryParse(request.GetParameter("household_id")?.ToString(), out var householdId))
            return BadRequest(new { error = "household_selection_required" });

        var resourceValues = request.GetResources().ToArray();
        if (resourceValues.Length != 1 || !string.Equals(resourceValues[0], _options.Value.Resource, StringComparison.Ordinal))
            return BadRequest(new { error = "invalid_resource" });

        var requestedScopes = request.GetScopes().ToHashSet(StringComparer.Ordinal);
        if (!requestedScopes.Contains(McpOAuthConstants.ReadScope))
            return BadRequest(new { error = "mcp_read_scope_required" });
        if (requestedScopes.Contains(McpOAuthConstants.WriteScope) &&
            !_configuration.GetValue<bool>("Mcp:Capabilities:HouseholdWrites"))
            return BadRequest(new { error = "mcp_write_scope_unavailable" });

        var user = await _users.FindByIdAsync(userId, cancellationToken);
        if (user is null || user.IsDisabled)
            return Unauthorized(new { error = "oauth_user_unavailable" });

        if (!await _familyAuthorization.CanAccessFamilyAsync(userId, householdId, cancellationToken))
            return Forbid();

        if (!HttpMethods.IsPost(Request.Method))
            return ConsentPage(
                request.ClientId ?? "unknown-client",
                householdId,
                requestedScopes.Contains(McpOAuthConstants.WriteScope));

        if (!string.Equals(Request.Form["consent"], "allow", StringComparison.Ordinal))
            return BadRequest(new { error = "consent_required" });

        var identity = new ClaimsIdentity(
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            OpenIddictConstants.Claims.Name,
            OpenIddictConstants.Claims.Role);
        identity.SetClaim(OpenIddictConstants.Claims.Subject, user.UserId.ToString());
        identity.SetClaim(OpenIddictConstants.Claims.Email, user.Email);
        identity.SetClaim(OpenIddictConstants.Claims.Name, user.DisplayName ?? user.Email);
        identity.SetClaim(McpOAuthConstants.HouseholdClaim, householdId.ToString());

        var tokenPrincipal = new ClaimsPrincipal(identity);
        tokenPrincipal.SetScopes(requestedScopes);
        tokenPrincipal.SetResources(_options.Value.Resource);
        tokenPrincipal.SetDestinations(static claim => claim.Type switch
        {
            OpenIddictConstants.Claims.Email or OpenIddictConstants.Claims.Name =>
                [OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken],
            McpOAuthConstants.HouseholdClaim => [OpenIddictConstants.Destinations.AccessToken],
            _ => [OpenIddictConstants.Destinations.AccessToken],
        });

        return SignIn(tokenPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [HttpPost("/oauth/token")]
    [AllowAnonymous]
    public async Task<IActionResult> Token(CancellationToken cancellationToken)
    {
        if (!_options.Value.Enabled)
            return NotFound();

        var request = HttpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenIddict request cannot be resolved.");

        if (!HasExactResource(request))
            return BadRequest(new { error = "invalid_resource" });

        if (!request.IsAuthorizationCodeGrantType() && !request.IsRefreshTokenGrantType())
            return BadRequest(new { error = "unsupported_grant_type" });

        var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        if (!result.Succeeded || result.Principal is null)
            return Forbid();

        var subject = result.Principal.FindFirstValue(OpenIddictConstants.Claims.Subject);
        var household = result.Principal.FindFirstValue(McpOAuthConstants.HouseholdClaim);
        if (!Guid.TryParse(subject, out var userId) || !Guid.TryParse(household, out var householdId))
            return Forbid();

        var user = await _users.FindByIdAsync(userId, cancellationToken);
        if (user is null || user.IsDisabled ||
            !await _familyAuthorization.CanAccessFamilyAsync(userId, householdId, cancellationToken))
        {
            return Forbid();
        }

        result.Principal.SetResources(_options.Value.Resource);
        return SignIn(result.Principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [HttpPost("/oauth/revoke-consent")]
    [AllowAnonymous]
    public async Task<IActionResult> RevokeConsent(
        [FromForm] string authorizationId,
        CancellationToken cancellationToken)
    {
        await HttpContext.RequestServices
            .GetRequiredService<Microsoft.AspNetCore.Antiforgery.IAntiforgery>()
            .ValidateRequestAsync(HttpContext);

        var cookie = await HttpContext.AuthenticateAsync(McpOAuthConstants.CookieScheme);
        if (!cookie.Succeeded || cookie.Principal is null ||
            !Guid.TryParse(cookie.Principal.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            return Unauthorized();
        }

        if (!_options.Value.Enabled || string.IsNullOrWhiteSpace(authorizationId))
            return BadRequest(new { error = "authorization_required" });

        var application = await _applications.FindByClientIdAsync(_options.Value.ClientId, cancellationToken);
        var authorization = await _authorizations.FindByIdAsync(authorizationId.Trim(), cancellationToken);
        if (application is null || authorization is null ||
            !string.Equals(
                await _authorizations.GetApplicationIdAsync(authorization, cancellationToken),
                await _applications.GetIdAsync(application, cancellationToken),
                StringComparison.Ordinal) ||
            !string.Equals(
                await _authorizations.GetSubjectAsync(authorization, cancellationToken),
                userId.ToString(),
                StringComparison.Ordinal) ||
            !string.Equals(
                await _authorizations.GetStatusAsync(authorization, cancellationToken),
                OpenIddictConstants.Statuses.Valid,
                StringComparison.Ordinal) ||
            !string.Equals(
                await _authorizations.GetTypeAsync(authorization, cancellationToken),
                OpenIddictConstants.AuthorizationTypes.AdHoc,
                StringComparison.Ordinal) ||
            !await _authorizations.HasScopesAsync(
                authorization,
                [McpOAuthConstants.ReadScope],
                cancellationToken))
        {
            return NotFound();
        }

        await foreach (var token in _tokens.FindByAuthorizationIdAsync(authorizationId.Trim(), cancellationToken))
        {
            if (await _tokens.HasStatusAsync(token, OpenIddictConstants.Statuses.Valid, cancellationToken))
                await _tokens.TryRevokeAsync(token, cancellationToken);
        }
        await _authorizations.TryRevokeAsync(authorization, cancellationToken);

        return NoContent();
    }

    [HttpGet("/oauth/login")]
    [AllowAnonymous]
    public IActionResult LoginPage([FromQuery] string? returnUrl)
    {
        if (!Url.IsLocalUrl(returnUrl))
            return BadRequest(new { error = "invalid_return_url" });

        var token = HttpContext.RequestServices
            .GetRequiredService<Microsoft.AspNetCore.Antiforgery.IAntiforgery>()
            .GetAndStoreTokens(HttpContext)
            .RequestToken;
        var encodedReturnUrl = System.Net.WebUtility.HtmlEncode(returnUrl);
        var encodedToken = System.Net.WebUtility.HtmlEncode(token);

        return Content($"""
            <!doctype html>
            <html lang="en">
            <head><meta charset="utf-8"><title>DomusMind authorization</title></head>
            <body>
              <main>
                <h1>Sign in to DomusMind</h1>
                <form method="post" action="/oauth/login">
                  <input type="hidden" name="returnUrl" value="{encodedReturnUrl}">
                  <input type="hidden" name="__RequestVerificationToken" value="{encodedToken}">
                  <label>Email <input name="email" type="email" autocomplete="username" required></label>
                  <label>Password <input name="password" type="password" autocomplete="current-password" required></label>
                  <button type="submit">Continue</button>
                </form>
              </main>
            </body>
            </html>
            """, "text/html");
    }

    [HttpPost("/oauth/login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        [FromForm] string email,
        [FromForm] string password,
        [FromForm] string returnUrl,
        CancellationToken cancellationToken)
    {
        await HttpContext.RequestServices
            .GetRequiredService<Microsoft.AspNetCore.Antiforgery.IAntiforgery>()
            .ValidateRequestAsync(HttpContext);

        if (!Url.IsLocalUrl(returnUrl))
            return BadRequest(new { error = "invalid_return_url" });

        var user = await _users.FindByEmailAsync(email.Trim(), cancellationToken);
        if (user is null || user.IsDisabled || !_passwords.Verify(password, user.PasswordHash))
            return Unauthorized(new { error = "invalid_credentials" });

        var identity = new ClaimsIdentity(McpOAuthConstants.CookieScheme);
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
        identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        identity.AddClaim(new Claim(ClaimTypes.Name, user.DisplayName ?? user.Email));
        await HttpContext.SignInAsync(
            McpOAuthConstants.CookieScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = false, AllowRefresh = false });

        return LocalRedirect(returnUrl);
    }

    private async Task<ClaimsPrincipal?> AuthenticateAuthorizationUserAsync(CancellationToken cancellationToken)
    {
        if (User.Identity?.IsAuthenticated == true)
            return User;

        var cookie = await HttpContext.AuthenticateAsync(McpOAuthConstants.CookieScheme);
        return cookie.Succeeded ? cookie.Principal : null;
    }

    private IActionResult ConsentPage(string clientId, Guid householdId, bool writeRequested)
    {
        var encodedClientId = System.Net.WebUtility.HtmlEncode(clientId);
        var encodedHouseholdId = System.Net.WebUtility.HtmlEncode(householdId.ToString());
        var token = HttpContext.RequestServices
            .GetRequiredService<Microsoft.AspNetCore.Antiforgery.IAntiforgery>()
            .GetAndStoreTokens(HttpContext)
            .RequestToken;
        var encodedToken = System.Net.WebUtility.HtmlEncode(token);
        var action = System.Net.WebUtility.HtmlEncode(Request.PathBase + Request.Path + Request.QueryString);
        var request = HttpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenIddict request cannot be resolved.");
        var hiddenFields = string.Join("\n                  ", new[]
        {
            HiddenInput("client_id", request.GetParameter("client_id")),
            HiddenInput("response_type", request.GetParameter("response_type")),
            HiddenInput("redirect_uri", request.GetParameter("redirect_uri")),
            HiddenInput("scope", request.GetParameter("scope")),
            HiddenInput("resource", request.GetParameter("resource")),
            HiddenInput("code_challenge", request.GetParameter("code_challenge")),
            HiddenInput("code_challenge_method", request.GetParameter("code_challenge_method")),
            HiddenInput("nonce", request.GetParameter("nonce")),
            HiddenInput("state", request.GetParameter("state")),
            HiddenInput("household_id", request.GetParameter("household_id")),
        }.Where(value => value is not null)!);

        return Content($"""
            <!doctype html>
            <html lang="en">
            <head><meta charset="utf-8"><title>DomusMind authorization</title></head>
            <body>
              <main>
                <h1>Allow MCP access to DomusMind?</h1>
                <p>Client <strong>{encodedClientId}</strong> requests {(writeRequested ? "read and confirmed-write" : "read-only")} access to household <strong>{encodedHouseholdId}</strong>.</p>
                <form method="post" action="{action}">
                  <input type="hidden" name="__RequestVerificationToken" value="{encodedToken}">
                  {hiddenFields}
                  <button type="submit" name="consent" value="allow">Allow</button>
                  <button type="submit" name="consent" value="deny">Deny</button>
                </form>
              </main>
            </body>
            </html>
            """, "text/html");
    }

    private bool HasExactResource(OpenIddictRequest request) =>
        request.GetResources().Count() == 1 &&
        string.Equals(request.GetResources().Single(), _options.Value.Resource, StringComparison.Ordinal);

    private static string? HiddenInput(string name, OpenIddictParameter? value)
    {
        var text = value?.ToString();
        return string.IsNullOrEmpty(text)
            ? null
            : $"<input type=\"hidden\" name=\"{name}\" value=\"{System.Net.WebUtility.HtmlEncode(text)}\">";
    }
}
