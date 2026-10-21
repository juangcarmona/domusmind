using System.Text;
using System.Security.Cryptography.X509Certificates;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Abstractions.System;
using DomusMind.Infrastructure.Initialization;
using DomusMind.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;

namespace DomusMind.Infrastructure.Auth;

public static class AuthInfrastructureExtensions
{
    public static IServiceCollection AddDomusMindAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .Validate(
                o => !string.IsNullOrWhiteSpace(o.SigningKey),
                "JWT signing key is required. Set Jwt:SigningKey in configuration.")
            .Validate(
                o => string.IsNullOrWhiteSpace(o.SigningKey) || o.SigningKey.Length >= 32,
                "JWT signing key must be at least 32 characters.")
            .ValidateOnStart();

        services
            .AddOptions<BootstrapAdminOptions>()
            .Bind(configuration.GetSection(BootstrapAdminOptions.SectionName))
            .Validate(
                o => !o.Enabled || !string.IsNullOrWhiteSpace(o.Email),
                "BootstrapAdmin:Email is required when BootstrapAdmin:Enabled is true.")
            .Validate(
                o => !o.Enabled || !string.IsNullOrWhiteSpace(o.Password),
                "BootstrapAdmin:Password is required when BootstrapAdmin:Enabled is true.")
            .ValidateOnStart();

        var jwtSection = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
            ?? throw new InvalidOperationException(
                "Jwt configuration section is missing. Ensure Jwt:SigningKey is set.");

        if (string.IsNullOrWhiteSpace(jwtSection.SigningKey))
            throw new InvalidOperationException(
                "JWT signing key is missing. Set Jwt:SigningKey in configuration.");

        if (jwtSection.SigningKey.Length < 32)
            throw new InvalidOperationException(
                "JWT signing key is too weak. It must be at least 32 characters.");

        var keyBytes = Encoding.UTF8.GetBytes(jwtSection.SigningKey);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSection.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSection.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ClockSkew = TimeSpan.Zero,
                };
            })
            .AddCookie(McpOAuthConstants.CookieScheme, options =>
            {
                options.Cookie.Name = "domusmind.mcp.oauth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.SlidingExpiration = false;
                options.LoginPath = "/oauth/login";
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
            });

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUserAccessor>();
        services.AddScoped<IAccessTokenGenerator, AccessTokenGenerator>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IRefreshTokenService, RefreshTokenStore>();
        services.AddScoped<IAuthUserRepository, AuthUserRepository>();
        services.AddScoped<ITemporaryPasswordGenerator, TemporaryPasswordGenerator>();
        services.AddScoped<IFamilyAuthorizationService, FamilyAuthorizationService>();
        services.AddScoped<IFamilyAccessGranter, FamilyAccessGranter>();
        services.AddScoped<IUserFamilyAccessReader, UserFamilyAccessReader>();
        services.AddScoped<ISystemInitializationState, SystemInitializationRepository>();

        return services;
    }

    public static IServiceCollection AddDomusMindMcpOAuth(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services
            .AddOptions<McpOAuthOptions>()
            .Bind(configuration.GetSection(McpOAuthOptions.SectionName))
            .Validate(
                options => !options.Enabled || Uri.TryCreate(options.Issuer, UriKind.Absolute, out _),
                "Mcp:OAuth:Issuer must be an absolute URI when MCP OAuth is enabled.")
            .Validate(
                options => !options.Enabled || Uri.TryCreate(options.Resource, UriKind.Absolute, out _),
                "Mcp:OAuth:Resource must be an absolute URI when MCP OAuth is enabled.")
            .Validate(
                options => !options.Enabled || options.RedirectUris.Length > 0,
                "Mcp:OAuth:RedirectUris must contain at least one URI when MCP OAuth is enabled.")
            .Validate(
                options => options.AccessTokenLifetimeSeconds is >= 60 and <= 900,
                "Mcp:OAuth:AccessTokenLifetimeSeconds must be between 60 and 900 seconds.")
            .Validate(
                options => options.RefreshTokenLifetimeDays is >= 1 and <= 30,
                "Mcp:OAuth:RefreshTokenLifetimeDays must be between 1 and 30 days.")
            .Validate(
                options => options.AuthorizationCodeLifetimeSeconds is >= 30 and <= 300,
                "Mcp:OAuth:AuthorizationCodeLifetimeSeconds must be between 30 and 300 seconds.")
            .Validate(
                options => !options.Enabled || environment.IsDevelopment() ||
                    (!string.IsNullOrWhiteSpace(options.SigningCertificatePath) &&
                     !string.IsNullOrWhiteSpace(options.EncryptionCertificatePath)),
                "Mcp:OAuth signing and encryption certificate paths are required outside Development.")
            .ValidateOnStart();

        var options = configuration.GetSection(McpOAuthOptions.SectionName).Get<McpOAuthOptions>()
            ?? new McpOAuthOptions();

        var openIddict = services.AddOpenIddict()
            .AddCore(builder =>
            {
                builder.UseEntityFrameworkCore()
                    .UseDbContext<DomusMindDbContext>();
            });

        if (!options.Enabled)
            return services;

        openIddict.AddServer(builder =>
            {
                builder.SetAuthorizationEndpointUris("/oauth/authorize")
                    .SetTokenEndpointUris("/oauth/token")
                    .SetRevocationEndpointUris("/oauth/revoke")
                    .AllowAuthorizationCodeFlow()
                    .AllowRefreshTokenFlow()
                    .RequireProofKeyForCodeExchange()
                    .RegisterScopes(OpenIddictConstants.Scopes.OpenId, OpenIddictConstants.Scopes.Profile, OpenIddictConstants.Scopes.OfflineAccess, McpOAuthConstants.ReadScope, McpOAuthConstants.WriteScope)
                    .RegisterResources(options.Resource)
                    .RegisterAudiences(options.Resource)
                    .SetAccessTokenLifetime(TimeSpan.FromSeconds(options.AccessTokenLifetimeSeconds))
                    .SetRefreshTokenLifetime(TimeSpan.FromDays(options.RefreshTokenLifetimeDays))
                    .SetRefreshTokenReuseLeeway(TimeSpan.Zero)
                    .SetAuthorizationCodeLifetime(TimeSpan.FromSeconds(options.AuthorizationCodeLifetimeSeconds))
                    .UseReferenceAccessTokens()
                    .UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableTokenEndpointPassthrough();

                if (Uri.TryCreate(options.Issuer, UriKind.Absolute, out var issuer))
                    builder.SetIssuer(issuer);

                if (environment.IsDevelopment())
                {
                    builder.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();
                }
                else
                {
                    builder.AddEncryptionCertificate(X509CertificateLoader.LoadPkcs12FromFile(
                        options.EncryptionCertificatePath!,
                        options.EncryptionCertificatePassword,
                        X509KeyStorageFlags.EphemeralKeySet,
                        loaderLimits: null));
                    builder.AddSigningCertificate(X509CertificateLoader.LoadPkcs12FromFile(
                        options.SigningCertificatePath!,
                        options.SigningCertificatePassword,
                        X509KeyStorageFlags.EphemeralKeySet,
                        loaderLimits: null));
                }
            });

        openIddict.AddValidation(builder =>
            {
                builder.SetIssuer(options.Issuer);
                builder.UseLocalServer();
                builder.UseAspNetCore();
                builder.EnableAuthorizationEntryValidation();
                builder.EnableTokenEntryValidation();
            });

        return services;
    }

    public static IServiceCollection AddDomusMindAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();
        return services;
    }
}
