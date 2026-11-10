using DomusMind.Api.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace DomusMind.Api.Tests.Auth;

public sealed class BffAuthenticationConfigurationTests
{
    [Fact]
    public void OidcUsesAuthorizationCodePkceAndDoesNotSaveProviderTokens()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Bff:Authority"] = "http://localhost:8180/realms/domusmind-dev",
                ["Bff:ClientId"] = "domusmind-dev",
            })
            .Build();
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDataProtection();
        services.AddDevelopmentBff(configuration);
        using var provider = services.BuildServiceProvider();

        var oidc = provider.GetRequiredService<IOptionsMonitor<OpenIdConnectOptions>>()
            .Get(BffAuthenticationExtensions.OidcScheme);
        var cookie = provider.GetRequiredService<IOptionsMonitor<CookieAuthenticationOptions>>()
            .Get(BffAuthenticationExtensions.CookieScheme);

        Assert.Equal(OpenIdConnectResponseType.Code, oidc.ResponseType);
        Assert.True(oidc.UsePkce);
        Assert.False(oidc.SaveTokens);
        Assert.Equal("__Host-DomusMind-Dev", cookie.Cookie.Name);
        Assert.True(cookie.Cookie.HttpOnly);
        Assert.Equal(SameSiteMode.Lax, cookie.Cookie.SameSite);
        Assert.NotNull(cookie.SessionStore);
    }
}
