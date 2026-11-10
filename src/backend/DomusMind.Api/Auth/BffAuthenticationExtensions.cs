using System.Security.Claims;
using DomusMind.Application.Abstractions.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace DomusMind.Api.Auth;

public static class BffAuthenticationExtensions
{
    public const string CookieScheme = "DomusMindBff";
    public const string OidcScheme = "DomusMindOidc";

    public static bool IsEnabled(IHostEnvironment environment, IConfiguration configuration) =>
        environment.IsDevelopment() && configuration.GetValue<bool>("Bff:Enabled");

    public static IServiceCollection AddDevelopmentBff(this IServiceCollection services, IConfiguration configuration)
    {
        var authority = configuration["Bff:Authority"];
        var clientId = configuration["Bff:ClientId"];
        if (string.IsNullOrWhiteSpace(authority) || string.IsNullOrWhiteSpace(clientId))
            throw new InvalidOperationException("Bff:Authority and Bff:ClientId are required for the development BFF.");

        var localAuthority = authority.StartsWith("http://localhost:", StringComparison.OrdinalIgnoreCase)
            || authority.StartsWith("http://127.0.0.1:", StringComparison.OrdinalIgnoreCase);
        if (!authority.StartsWith("https://", StringComparison.OrdinalIgnoreCase) && !localAuthority)
            throw new InvalidOperationException("The development BFF requires an HTTPS or loopback OIDC authority.");

        var sessionLifetimeSeconds = configuration.GetValue("Bff:SessionLifetimeSeconds", 1800);
        if (sessionLifetimeSeconds is < 30 or > 1800)
            throw new InvalidOperationException("Bff:SessionLifetimeSeconds must be between 30 and 1800 seconds.");

        services.AddSingleton<DevelopmentBffTicketStore>();
        services.AddOptions<CookieAuthenticationOptions>(CookieScheme)
            .Configure<DevelopmentBffTicketStore>((options, ticketStore) => options.SessionStore = ticketStore);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieScheme;
            options.DefaultChallengeScheme = CookieScheme;
            options.DefaultSignInScheme = CookieScheme;
        })
        .AddCookie(CookieScheme, options =>
        {
            options.Cookie.Name = "__Host-DomusMind-Dev";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.Path = "/";
            options.ExpireTimeSpan = TimeSpan.FromSeconds(sessionLifetimeSeconds);
            options.SlidingExpiration = false;
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };
            options.Events.OnValidatePrincipal = async context =>
            {
                var id = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                var users = context.HttpContext.RequestServices.GetRequiredService<IAuthUserRepository>();
                var user = Guid.TryParse(id, out var userId)
                    ? await users.FindByIdAsync(userId, context.HttpContext.RequestAborted)
                    : null;
                if (user is null || user.IsDisabled || user.MustChangePassword)
                {
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync(CookieScheme);
                }
            };
        })
        .AddOpenIdConnect(OidcScheme, options =>
        {
            options.Authority = authority;
            options.ClientId = clientId;
            options.ClientSecret = configuration["Bff:ClientSecret"];
            options.RequireHttpsMetadata = !localAuthority;
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.UsePkce = true;
            options.SaveTokens = false;
            options.MapInboundClaims = false;
            options.SignInScheme = CookieScheme;
            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("email");
            options.Events.OnTokenValidated = async context =>
            {
                var users = context.HttpContext.RequestServices.GetRequiredService<IAuthUserRepository>();
                var mappedPrincipal = context.Principal is null
                    ? null
                    : await DevelopmentBffIdentityMapper.MapAsync(
                        context.Principal,
                        configuration,
                        users,
                        context.HttpContext.RequestAborted);
                if (mappedPrincipal is null)
                    context.Fail("OIDC identity is not an approved development user with an active local account.");
                else
                    context.Principal = mappedPrincipal;
            };
            options.Events.OnRedirectToIdentityProviderForSignOut = context =>
            {
                context.ProtocolMessage.ClientId = clientId;
                return Task.CompletedTask;
            };
        });

        services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");
        return services;
    }
}
