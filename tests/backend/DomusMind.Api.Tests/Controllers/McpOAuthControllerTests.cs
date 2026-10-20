using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
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
using OpenIddict.Abstractions;
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
            "scope=openid%20offline_access%20mcp.read",
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

    [Fact]
    public async Task Authorization_code_exchange_and_mcp_request_use_the_bound_household()
    {
        await using var host = await TestHost.CreateAsync(enabled: true);
        using var client = host.CreateClient();
        const string verifier = "test-code-verifier-012345678901234567890123456789";
        var challenge = Convert.ToBase64String(SHA256.HashData(Encoding.ASCII.GetBytes(verifier)))
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
        var authorizeUrl = "/oauth/authorize?" + string.Join('&',
            "client_id=test-client",
            "response_type=code",
            "redirect_uri=https%3A%2F%2Fclient.test%2Fcallback",
            "scope=openid%20mcp.read",
            "resource=https%3A%2F%2Fdomusmind.test%2Fmcp",
            $"code_challenge={Uri.EscapeDataString(challenge)}",
            "code_challenge_method=S256",
            $"household_id={TestHost.TestHouseholdId}",
            "state=test-state");

        using var loginRedirect = await client.GetAsync(authorizeUrl);
        Assert.Equal(HttpStatusCode.Redirect, loginRedirect.StatusCode);
        var loginUrl = loginRedirect.Headers.Location!;

        using var loginPage = await client.GetAsync(loginUrl);
        var loginPageBody = await loginPage.Content.ReadAsStringAsync();
        var loginCookies = GetCookieHeader(loginPage.Headers);
        using var loginRequest = new HttpRequestMessage(HttpMethod.Post, loginUrl)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["email"] = TestHost.TestEmail,
                ["password"] = TestHost.TestPassword,
                ["returnUrl"] = authorizeUrl,
                ["__RequestVerificationToken"] = ExtractAntiforgeryToken(loginPageBody),
            }),
        };
        loginRequest.Headers.TryAddWithoutValidation("Cookie", loginCookies);
        using var loginResponse = await client.SendAsync(loginRequest);
        Assert.Equal(HttpStatusCode.Redirect, loginResponse.StatusCode);
        var authCookies = CombineCookies(loginCookies, GetCookieHeader(loginResponse.Headers));

        using var consentPage = await SendWithCookiesAsync(client, HttpMethod.Get, authorizeUrl, authCookies);
        var consentBody = await consentPage.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, consentPage.StatusCode);
        Assert.Contains("Allow MCP access", consentBody, StringComparison.Ordinal);
        authCookies = CombineCookies(authCookies, GetCookieHeader(consentPage.Headers));

        using var consentRequest = new HttpRequestMessage(HttpMethod.Post, authorizeUrl)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = "test-client",
                ["response_type"] = "code",
                ["redirect_uri"] = "https://client.test/callback",
                ["scope"] = "openid offline_access mcp.read",
                ["resource"] = "https://domusmind.test/mcp",
                ["code_challenge"] = challenge,
                ["code_challenge_method"] = "S256",
                ["household_id"] = TestHost.TestHouseholdId.ToString(),
                ["state"] = "test-state",
                ["consent"] = "allow",
                ["__RequestVerificationToken"] = ExtractAntiforgeryToken(consentBody),
            }),
        };
        consentRequest.Headers.TryAddWithoutValidation("Cookie", authCookies);
        using var consentResponse = await client.SendAsync(consentRequest);
        Assert.Equal(HttpStatusCode.Redirect, consentResponse.StatusCode);
        var callback = consentResponse.Headers.Location!;
        var callbackQuery = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(callback.Query);
        var code = callbackQuery["code"].ToString();
        Assert.False(string.IsNullOrWhiteSpace(code));
        Assert.Equal("test-state", callbackQuery["state"].ToString());

        using var wrongResourceResponse = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = "test-client",
            ["code"] = code,
            ["redirect_uri"] = "https://client.test/callback",
            ["code_verifier"] = verifier,
            ["resource"] = "https://other.test/mcp",
        }));
        Assert.Equal(HttpStatusCode.BadRequest, wrongResourceResponse.StatusCode);

        using var tokenResponse = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = "test-client",
            ["code"] = code,
            ["redirect_uri"] = "https://client.test/callback",
            ["code_verifier"] = verifier,
            ["resource"] = "https://domusmind.test/mcp",
        }));
        var tokenBody = await tokenResponse.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, tokenResponse.StatusCode);
        using var tokenJson = JsonDocument.Parse(tokenBody);
        var accessToken = tokenJson.RootElement.GetProperty("access_token").GetString();
        Assert.False(string.IsNullOrWhiteSpace(accessToken));
        var refreshToken = tokenJson.RootElement.GetProperty("refresh_token").GetString();
        Assert.False(string.IsNullOrWhiteSpace(refreshToken));

        using var replayedCodeResponse = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = "test-client",
            ["code"] = code,
            ["redirect_uri"] = "https://client.test/callback",
            ["code_verifier"] = verifier,
            ["resource"] = "https://domusmind.test/mcp",
        }));
        Assert.Equal(HttpStatusCode.BadRequest, replayedCodeResponse.StatusCode);

        using var refreshResponse = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["client_id"] = "test-client",
            ["refresh_token"] = refreshToken!,
            ["resource"] = "https://domusmind.test/mcp",
        }));
        Assert.Equal(HttpStatusCode.OK, refreshResponse.StatusCode);

        using var mcpRequest = new HttpRequestMessage(HttpMethod.Get, "/mcp");
        mcpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        using var mcpResponse = await client.SendAsync(mcpRequest);
        Assert.Equal(HttpStatusCode.OK, mcpResponse.StatusCode);

        await host.DisableTestUserAsync();
        using var disabledUserRequest = new HttpRequestMessage(HttpMethod.Get, "/mcp");
        disabledUserRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        using var disabledUserResponse = await client.SendAsync(disabledUserRequest);
        Assert.Equal(HttpStatusCode.Forbidden, disabledUserResponse.StatusCode);

        await host.EnableTestUserAndRemoveHouseholdAccessAsync();
        using var removedMembershipRequest = new HttpRequestMessage(HttpMethod.Get, "/mcp");
        removedMembershipRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        using var removedMembershipResponse = await client.SendAsync(removedMembershipRequest);
        Assert.Equal(HttpStatusCode.Forbidden, removedMembershipResponse.StatusCode);

        using var revokeRequest = new HttpRequestMessage(HttpMethod.Post, "/oauth/revoke-consent")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["authorizationId"] = await host.GetAuthorizationIdAsync(),
                ["__RequestVerificationToken"] = ExtractAntiforgeryToken(consentBody),
            }),
        };
        revokeRequest.Headers.TryAddWithoutValidation("Cookie", authCookies);
        using var revokeResponse = await client.SendAsync(revokeRequest);
        var revokeBody = await revokeResponse.Content.ReadAsStringAsync();
        Assert.True(revokeResponse.StatusCode == HttpStatusCode.NoContent,
            $"Expected revocation to succeed, got {(int)revokeResponse.StatusCode}: {revokeBody}");

        using var revokedMcpRequest = new HttpRequestMessage(HttpMethod.Get, "/mcp");
        revokedMcpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        using var revokedMcpResponse = await client.SendAsync(revokedMcpRequest);
        Assert.Equal(HttpStatusCode.Unauthorized, revokedMcpResponse.StatusCode);

        using var reusedRefreshResponse = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["client_id"] = "test-client",
            ["refresh_token"] = refreshToken!,
            ["resource"] = "https://domusmind.test/mcp",
        }));
        Assert.Equal(HttpStatusCode.BadRequest, reusedRefreshResponse.StatusCode);
    }

    [Fact]
    public async Task Invalid_bearer_tokens_fail_closed_without_disclosing_data()
    {
        await using var host = await TestHost.CreateAsync(enabled: true);
        using var client = host.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/mcp");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "not-a-domusmind-token");

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.DoesNotContain("household", await response.Content.ReadAsStringAsync(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Mcp_requests_are_rate_limited()
    {
        await using var host = await TestHost.CreateAsync(enabled: true, requestsPerMinute: 1);
        using var client = host.CreateClient();

        using var first = await client.GetAsync("/mcp");
        using var second = await client.GetAsync("/mcp");

        Assert.Equal(HttpStatusCode.Unauthorized, first.StatusCode);
        Assert.Equal((HttpStatusCode)429, second.StatusCode);
    }

    private static async Task<HttpResponseMessage> SendWithCookiesAsync(HttpClient client, HttpMethod method, string url, string cookies)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.TryAddWithoutValidation("Cookie", cookies);
        return await client.SendAsync(request);
    }

    private static string ExtractAntiforgeryToken(string body)
    {
        const string marker = "name=\"__RequestVerificationToken\" value=\"";
        var start = body.IndexOf(marker, StringComparison.Ordinal);
        Assert.True(start >= 0, "The response did not contain an antiforgery token.");
        start += marker.Length;
        var end = body.IndexOf('"', start);
        Assert.True(end > start, "The antiforgery token was malformed.");
        return System.Net.WebUtility.HtmlDecode(body[start..end]);
    }

    private static string GetCookieHeader(HttpResponseHeaders headers) =>
        headers.TryGetValues("Set-Cookie", out var values)
            ? string.Join("; ", values.Select(value => value.Split(';', 2)[0]))
            : string.Empty;

    private static string CombineCookies(string first, string second) =>
        string.Join("; ", new[] { first, second }
            .SelectMany(value => value.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Distinct(StringComparer.Ordinal));

    private sealed class TestHost : IAsyncDisposable
    {
        private readonly WebApplication _app;
        private static readonly InMemoryDatabaseRoot DatabaseRoot = new();

        public static readonly Guid TestHouseholdId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public static readonly Guid TestUserId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        public const string TestEmail = "mcp-test@domusmind.test";
        public const string TestPassword = "Mcp-Test-Password-123!";

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

        public static async Task<TestHost> CreateAsync(bool enabled, int requestsPerMinute = 60)
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
                ["Mcp:RequestsPerMinute"] = requestsPerMinute.ToString(),
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
            app.UseRateLimiter();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseDomusMindMcpOAuthResourceAuthorization();
            app.MapControllers();
            app.MapGet("/mcp", () => Results.Ok()).RequireRateLimiting("mcp");
            using (var scope = app.Services.CreateScope())
            {
                var database = scope.ServiceProvider.GetRequiredService<DomusMindDbContext>();
                database.Database.EnsureCreated();
                if (!await database.AuthUsers.AnyAsync(user => user.UserId == TestUserId))
                {
                    database.AuthUsers.Add(new AuthUser
                    {
                        UserId = TestUserId,
                        Email = TestEmail,
                        PasswordHash = new PasswordHasher().Hash(TestPassword),
                        CreatedAtUtc = DateTime.UtcNow,
                        DisplayName = "MCP Test User",
                    });
                }

                if (!await database.UserFamilyAccesses.AnyAsync(access =>
                        access.UserId == TestUserId && access.FamilyId == TestHouseholdId))
                {
                    database.UserFamilyAccesses.Add(new UserFamilyAccess
                    {
                        UserId = TestUserId,
                        FamilyId = TestHouseholdId,
                        GrantedAtUtc = DateTime.UtcNow,
                    });
                }
                await database.SaveChangesAsync();
                await McpOAuthSeedService.SeedClientAsync(scope.ServiceProvider, CancellationToken.None);
            }
            await app.StartAsync();
            return new TestHost(app);
        }

        public async Task<string> GetAuthorizationIdAsync()
        {
            using var scope = _app.Services.CreateScope();
            var tokens = scope.ServiceProvider.GetRequiredService<IOpenIddictTokenManager>();
            await foreach (var token in tokens.FindBySubjectAsync(TestUserId.ToString(), CancellationToken.None))
            {
                var authorizationId = await tokens.GetAuthorizationIdAsync(token, CancellationToken.None);
                if (!string.IsNullOrWhiteSpace(authorizationId))
                    return authorizationId;
            }

            throw new InvalidOperationException("The test flow did not create an OAuth authorization.");
        }

        public async Task DisableTestUserAsync()
        {
            using var scope = _app.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<DomusMindDbContext>();
            var user = await database.AuthUsers.SingleAsync(user => user.UserId == TestUserId);
            user.IsDisabled = true;
            await database.SaveChangesAsync();
        }

        public async Task EnableTestUserAndRemoveHouseholdAccessAsync()
        {
            using var scope = _app.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<DomusMindDbContext>();
            var user = await database.AuthUsers.SingleAsync(user => user.UserId == TestUserId);
            user.IsDisabled = false;
            var access = await database.UserFamilyAccesses.SingleAsync(access =>
                access.UserId == TestUserId && access.FamilyId == TestHouseholdId);
            database.UserFamilyAccesses.Remove(access);
            await database.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }
    }
}
