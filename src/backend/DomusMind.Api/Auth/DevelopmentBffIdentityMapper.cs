using System.Security.Claims;
using DomusMind.Application.Abstractions.Security;

namespace DomusMind.Api.Auth;

public static class DevelopmentBffIdentityMapper
{
    public static async Task<ClaimsPrincipal?> MapAsync(
        ClaimsPrincipal oidcPrincipal,
        IConfiguration configuration,
        IAuthUserRepository users,
        CancellationToken cancellationToken)
    {
        var subject = oidcPrincipal.FindFirstValue("sub");
        var configuredEmail = subject is null ? null : configuration[$"Bff:TestUsers:{subject}"];
        var assertedEmail = oidcPrincipal.FindFirstValue("email");
        var emailVerified = oidcPrincipal.FindFirstValue("email_verified");
        if (string.IsNullOrWhiteSpace(configuredEmail) || emailVerified != "true" ||
            !string.Equals(configuredEmail, assertedEmail, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var user = await users.FindByEmailAsync(configuredEmail, cancellationToken);
        if (user is null || user.IsDisabled || user.MustChangePassword)
            return null;

        return new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
             new Claim(ClaimTypes.Email, user.Email)], BffAuthenticationExtensions.CookieScheme));
    }
}
