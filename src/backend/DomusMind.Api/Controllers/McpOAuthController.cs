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

    public McpOAuthController(
        IAuthUserRepository users,
        IPasswordHasher passwords,
        IFamilyAuthorizationService familyAuthorization,
        IOptions<McpOAuthOptions> options)
    {
        _users = users;
        _passwords = passwords;
        _familyAuthorization = familyAuthorization;
        _options = options;
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
            scopes_supported = new[] { McpOAuthConstants.ReadScope },
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

        var user = await _users.FindByIdAsync(userId, cancellationToken);
        if (user is null || user.IsDisabled)
            return Unauthorized(new { error = "oauth_user_unavailable" });

        if (!await _familyAuthorization.CanAccessFamilyAsync(userId, householdId, cancellationToken))
            return Forbid();

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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        [FromForm] string email,
        [FromForm] string password,
        [FromForm] string returnUrl,
        CancellationToken cancellationToken)
    {
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
}
