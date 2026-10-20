using System.Net;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Api.Controllers;
using DomusMind.Infrastructure.Auth;
using DomusMind.Infrastructure.Persistence;
using DomusMind.Mcp.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DomusMind.Api.Tests.Controllers;

public sealed class McpOAuthControllerTests
{
    [Fact]
    public async Task Protected_resource_metadata_advertises_the_configured_resource_and_authority()
    {
        await using var host = await TestHost.CreateAsync(enabled: true);
        using var client = host.CreateClient();

        using var response = await client.GetAsync("/.well-known/oauth-protected-resource/mcp");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("https://domusmind.test/mcp", body, StringComparison.Ordinal);
        Assert.Contains("https://domusmind.test", body, StringComparison.Ordinal);
        Assert.Contains("mcp.read", body, StringComparison.Ordinal);
        Assert.Contains("header", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Protected_resource_metadata_is_not_published_when_oauth_is_disabled()
    {
        await using var host = await TestHost.CreateAsync(enabled: false);
        using var client = host.CreateClient();

        using var response = await client.GetAsync("/.well-known/oauth-protected-resource/mcp");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Login_page_requires_a_local_authorization_return_url_and_emits_antiforgery_token()
    {
        await using var host = await TestHost.CreateAsync(enabled: true);
        using var client = host.CreateClient();

        using var response = await client.GetAsync("/oauth/login?returnUrl=%2Foauth%2Fauthorize%3Fstate%3Dabc");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("action=\"/oauth/login\"", body, StringComparison.Ordinal);
        Assert.Contains("name=\"__RequestVerificationToken\"", body, StringComparison.Ordinal);
        Assert.Contains("name=\"returnUrl\"", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Login_page_rejects_an_external_return_url()
    {
        await using var host = await TestHost.CreateAsync(enabled: true);
        using var client = host.CreateClient();

        using var response = await client.GetAsync("/oauth/login?returnUrl=https%3A%2F%2Fevil.test%2Fsteal");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Openid_configuration_is_published_by_the_enabled_authorization_server()
    {
        await using var host = await TestHost.CreateAsync(enabled: true);
        using var client = host.CreateClient();

        using var response = await client.GetAsync("/.well-known/openid-configuration");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("https://domusmind.test/oauth/authorize", body, StringComparison.Ordinal);
        Assert.Contains("https://domusmind.test/oauth/token", body, StringComparison.Ordinal);
        Assert.Contains("mcp.read", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Mcp_resource_requires_a_bearer_token_when_oauth_is_enabled()
    {
        await using var host = await TestHost.CreateAsync(enabled: true);
        using var client = host.CreateClient();

        using var response = await client.GetAsync("/mcp");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Contains("resource_metadata", response.Headers.WwwAuthenticate.ToString(), StringComparison.Ordinal);
        Assert.Contains("mcp.read", response.Headers.WwwAuthenticate.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task Authorization_request_requires_a_domusmind_login_before_consent()
    {
        await using var host = await TestHost.CreateAsync(enabled: true);
        using var client = host.CreateClient();
        var query = string.Join('&',
            "client_id=test-client",
            "response_type=code",
            "redirect_uri=https%3A%2F%2Fclient.test%2Fcallback",
            "scope=openid%20mcp.read",
            "resource=https%3A%2F%2Fdomusmind.test%2Fmcp",
            "code_challenge=aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            "code_challenge_method=S256",
            "household_id=00000000-0000-0000-0000-000000000001",
            "state=test-state");

        using var response = await client.GetAsync($"/oauth/authorize?{query}");

        var body = await response.Content.ReadAsStringAsync();
        Assert.True(response.StatusCode == HttpStatusCode.Redirect, $"Expected redirect, got {(int)response.StatusCode}: {body}");
        Assert.Equal("/oauth/login", response.Headers.Location?.AbsolutePath);
        Assert.Contains("ReturnUrl=", response.Headers.Location?.Query, StringComparison.Ordinal);
    }

    private sealed class TestHost : IAsyncDisposable
    {
        private readonly WebApplication _app;
        private static readonly InMemoryDatabaseRoot DatabaseRoot = new();

        private TestHost(WebApplication app)
        {
            _app = app;
        }

        public HttpClient CreateClient()
        {
            var client = new HttpClient(_app.GetTestServer().CreateHandler());
            client.BaseAddress = new Uri("https://domusmind.test");
            return client;
        }

        public static async Task<TestHost> CreateAsync(bool enabled)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                EnvironmentName = "Development",
                ApplicationName = typeof(McpOAuthController).Assembly.GetName().Name,
            });
            builder.WebHost.UseTestServer();
            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:SigningKey"] = "test-signing-key-that-is-at-least-32-characters",
                ["Jwt:Issuer"] = "domusmind-test",
                ["Jwt:Audience"] = "domusmind-test-api",
                ["Mcp:OAuth:Enabled"] = enabled.ToString(),
                ["Mcp:OAuth:Issuer"] = "https://domusmind.test",
                ["Mcp:OAuth:Resource"] = "https://domusmind.test/mcp",
                ["Mcp:OAuth:ClientId"] = "test-client",
                ["Mcp:OAuth:RedirectUris:0"] = "https://client.test/callback",
                ["Mcp:RequireOAuth"] = enabled.ToString(),
            });

            builder.Services.AddControllers()
                .AddApplicationPart(typeof(McpOAuthController).Assembly);
            builder.Services.AddAntiforgery();
            builder.Services.AddDbContext<DomusMindDbContext>(options =>
                options.UseInMemoryDatabase("mcp-oauth", DatabaseRoot));
            builder.Services.AddScoped<IDomusMindDbContext>(services =>
                services.GetRequiredService<DomusMindDbContext>());
            builder.Services.AddDomusMindAuthentication(builder.Configuration);
            builder.Services.AddDomusMindMcpOAuth(builder.Configuration, builder.Environment);
            builder.Services.AddDomusMindMcp(builder.Configuration);
            builder.Services.AddDomusMindAuthorization();

            var app = builder.Build();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseDomusMindMcpOAuthResourceAuthorization();
            app.MapControllers();
            app.MapGet("/mcp", () => Results.Ok());
            using (var scope = app.Services.CreateScope())
            {
                var database = scope.ServiceProvider.GetRequiredService<DomusMindDbContext>();
                database.Database.EnsureCreated();
                await McpOAuthSeedService.SeedClientAsync(scope.ServiceProvider, CancellationToken.None);
            }
            await app.StartAsync();
            return new TestHost(app);
        }

        public async ValueTask DisposeAsync()
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }
    }
}
