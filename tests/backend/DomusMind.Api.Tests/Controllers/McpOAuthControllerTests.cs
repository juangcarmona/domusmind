using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Features.Calendar.GetFamilyTimeline;
using DomusMind.Application.Features.Family.GetFamilyMembers;
using DomusMind.Api.Controllers;
using DomusMind.Contracts.Calendar;
using DomusMind.Contracts.Family;
using DomusMind.Domain.Calendar;
using DomusMind.Domain.Calendar.ValueObjects;
using DomusMind.Domain.Family;
using DomusMind.Domain.Family.ValueObjects;
using DomusMind.Domain.Shared;
using DomusMind.Infrastructure.Auth;
using DomusMind.Infrastructure.Messaging;
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
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using Xunit;

namespace DomusMind.Api.Tests.Controllers;

public sealed class McpOAuthControllerTests
{
    [Fact]
    public async Task Authorized_read_only_capability_registers_only_the_release_tools()
    {
        await using var host = await TestHost.CreateAsync(
            enabled: true,
            householdReadOnlyEnabled: true,
            useKestrelHttps: true);
        using var client = host.CreateClient();

        using var metadataResponse = await client.GetAsync("/.well-known/oauth-protected-resource/mcp");
        Assert.Equal(HttpStatusCode.OK, metadataResponse.StatusCode);
        Assert.Contains("https://domusmind.test/mcp",
            await metadataResponse.Content.ReadAsStringAsync(), StringComparison.Ordinal);

        var (status, code, verifier) = await host.IssueAuthorizationCodeAsync(client);
        Assert.Equal(HttpStatusCode.Redirect, status);
        Assert.NotNull(code);

        using var tokenResponse = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = "test-client",
            ["code"] = code!,
            ["redirect_uri"] = "https://client.test/callback",
            ["code_verifier"] = verifier,
            ["resource"] = "https://domusmind.test/mcp",
        }));
        Assert.Equal(HttpStatusCode.OK, tokenResponse.StatusCode);
        using var tokenJson = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync());
        var accessToken = tokenJson.RootElement.GetProperty("access_token").GetString();
        Assert.False(string.IsNullOrWhiteSpace(accessToken));

        using var mcpHttpClient = host.CreateClient();
        mcpHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var transport = new HttpClientTransport(new HttpClientTransportOptions
        {
            Endpoint = new Uri(host.BaseAddress, "/mcp"),
            TransportMode = HttpTransportMode.StreamableHttp,
            EnableStandaloneGetStream = false,
        }, mcpHttpClient, ownsHttpClient: false);
        await using var mcpClient = await McpClient.CreateAsync(transport, new McpClientOptions
        {
            ClientInfo = new Implementation { Name = "DomusMind.Api.Tests", Version = "1.0" },
            ProtocolVersion = "2026-07-28",
        });

        var tools = await mcpClient.ListToolsAsync();
        Assert.Equal(["agenda_get", "areas_get", "household_members_list", "lists_get", "meal_plan_get", "routines_get"], tools.Select(tool => tool.Name).OrderBy(name => name));
        Assert.DoesNotContain(tools, tool => tool.Name is "tasks_list" or "plan_create" or "list_item_add");

        var agendaResult = await mcpClient.CallToolAsync("agenda_get", new Dictionary<string, object?>
        {
            ["from"] = "2026-10-20",
            ["to"] = "2026-10-31",
        });
        Assert.NotEqual(true, agendaResult.IsError);
        Assert.Contains(agendaResult.StructuredContent!.Value.GetProperty("events").EnumerateArray(),
            item => item.GetProperty("title").GetString() == "OAuth household appointment"
                && item.GetProperty("date").GetString() == "2026-10-20");

        var membersResult = await mcpClient.CallToolAsync("household_members_list", new Dictionary<string, object?>());
        Assert.NotEqual(true, membersResult.IsError);
        Assert.Contains(membersResult.StructuredContent!.Value.GetProperty("members").EnumerateArray(),
            member => member.GetProperty("name").GetString() == "OAuth Test Member");

        await host.DisableTestUserAsync();
        using var disabledUserResponse = await mcpHttpClient.GetAsync("/mcp");
        Assert.Equal(HttpStatusCode.Forbidden, disabledUserResponse.StatusCode);

        await host.EnableTestUserAndRemoveHouseholdAccessAsync();
        using var removedMembershipResponse = await mcpHttpClient.GetAsync("/mcp");
        Assert.Equal(HttpStatusCode.Forbidden, removedMembershipResponse.StatusCode);

        await host.RestoreTestHouseholdAccessAsync();
    }

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
        await using var host = await TestHost.CreateAsync(enabled: true, householdReadOnlyEnabled: true);
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
            ["code"] = code!,
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

        using var refreshResponse = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["client_id"] = "test-client",
            ["refresh_token"] = refreshToken!,
            ["resource"] = "https://domusmind.test/mcp",
        }));
        Assert.Equal(HttpStatusCode.OK, refreshResponse.StatusCode);
        using var refreshJson = JsonDocument.Parse(await refreshResponse.Content.ReadAsStringAsync());
        var rotatedRefreshToken = refreshJson.RootElement.GetProperty("refresh_token").GetString();
        Assert.False(string.IsNullOrWhiteSpace(rotatedRefreshToken));

        using var mcpHttpClient = host.CreateClient();
        mcpHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var transport = new HttpClientTransport(new HttpClientTransportOptions
        {
            Endpoint = new Uri("https://domusmind.test/mcp"),
            TransportMode = HttpTransportMode.StreamableHttp,
            EnableStandaloneGetStream = false,
        }, mcpHttpClient, ownsHttpClient: false);
        await using var mcpClient = await McpClient.CreateAsync(transport, new McpClientOptions
        {
            ClientInfo = new Implementation { Name = "DomusMind.Api.Tests", Version = "1.0" },
            ProtocolVersion = "2026-07-28",
        });
        Assert.Equal(["agenda_get", "areas_get", "household_members_list", "lists_get", "meal_plan_get", "routines_get"],
            (await mcpClient.ListToolsAsync()).Select(tool => tool.Name).OrderBy(name => name));

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

        await host.RestoreTestHouseholdAccessAsync();

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

        using var revokedMcpRequest = new HttpRequestMessage(HttpMethod.Post, "/mcp")
        {
            Content = new StringContent("{\"jsonrpc\":\"2.0\",\"id\":1,\"method\":\"tools/list\"}", Encoding.UTF8, "application/json"),
        };
        revokedMcpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        revokedMcpRequest.Headers.Accept.ParseAdd("application/json, text/event-stream");
        revokedMcpRequest.Headers.TryAddWithoutValidation("MCP-Protocol-Version", "2026-07-28");
        using var revokedMcpResponse = await client.SendAsync(revokedMcpRequest);
        Assert.Equal(HttpStatusCode.Unauthorized, revokedMcpResponse.StatusCode);

    }

    [Fact]
    public async Task Authorization_codes_are_single_use_without_relying_on_consent_revocation()
    {
        await using var host = await TestHost.CreateAsync(enabled: true);
        using var client = host.CreateClient();
        var (_, code, verifier) = await host.IssueAuthorizationCodeAsync(client);
        Assert.NotNull(code);

        using var firstExchange = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = "test-client",
            ["code"] = code!,
            ["redirect_uri"] = "https://client.test/callback",
            ["code_verifier"] = verifier,
            ["resource"] = "https://domusmind.test/mcp",
        }));
        Assert.Equal(HttpStatusCode.OK, firstExchange.StatusCode);

        using var replay = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = "test-client",
            ["code"] = code!,
            ["redirect_uri"] = "https://client.test/callback",
            ["code_verifier"] = verifier,
            ["resource"] = "https://domusmind.test/mcp",
        }));
        Assert.Equal(HttpStatusCode.BadRequest, replay.StatusCode);
    }

    [Fact]
    public async Task Refresh_tokens_are_single_use_in_an_isolated_flow()
    {
        await using var host = await TestHost.CreateAsync(enabled: true);
        using var client = host.CreateClient();
        var (_, code, verifier) = await host.IssueAuthorizationCodeAsync(client);
        Assert.NotNull(code);

        using var firstExchange = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = "test-client",
            ["code"] = code!,
            ["redirect_uri"] = "https://client.test/callback",
            ["code_verifier"] = verifier,
            ["resource"] = "https://domusmind.test/mcp",
        }));
        Assert.Equal(HttpStatusCode.OK, firstExchange.StatusCode);
        using var firstJson = JsonDocument.Parse(await firstExchange.Content.ReadAsStringAsync());
        var refreshToken = firstJson.RootElement.GetProperty("refresh_token").GetString();
        Assert.False(string.IsNullOrWhiteSpace(refreshToken));

        using var rotated = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["client_id"] = "test-client",
            ["refresh_token"] = refreshToken!,
            ["resource"] = "https://domusmind.test/mcp",
        }));
        Assert.Equal(HttpStatusCode.OK, rotated.StatusCode);

        using var reused = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["client_id"] = "test-client",
            ["refresh_token"] = refreshToken!,
            ["resource"] = "https://domusmind.test/mcp",
        }));
        Assert.Equal(HttpStatusCode.BadRequest, reused.StatusCode);
    }

    [Fact]
    public async Task Invalid_bearer_tokens_fail_closed_without_disclosing_data()
    {
        await using var host = await TestHost.CreateAsync(enabled: true);
        using var client = host.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/mcp");
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL3d4b25nLWlzc3Vlci50ZXN0IiwiYXVkIjoiaHR0cHM6Ly9kb211c21pbmQudGVzdC9tY3AifQ.invalid");

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.DoesNotContain("household", await response.Content.ReadAsStringAsync(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Wrong_resource_audience_is_rejected_at_the_mcp_boundary()
    {
        await using var host = await TestHost.CreateAsync(enabled: true, expectedAudience: "https://other.test/mcp");
        using var client = host.CreateClient();
        var (_, code, verifier) = await host.IssueAuthorizationCodeAsync(client);
        Assert.NotNull(code);
        using var tokenResponse = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = "test-client",
            ["code"] = code!,
            ["redirect_uri"] = "https://client.test/callback",
            ["code_verifier"] = verifier,
            ["resource"] = "https://domusmind.test/mcp",
        }));
        using var tokenJson = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync());

        using var request = new HttpRequestMessage(HttpMethod.Get, "/mcp");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenJson.RootElement.GetProperty("access_token").GetString());
        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Expired_access_tokens_are_rejected()
    {
        var clock = new TestTimeProvider(DateTimeOffset.UtcNow);
        await using var host = await TestHost.CreateAsync(enabled: true, timeProvider: clock);
        using var client = host.CreateClient();
        var (_, code, verifier) = await host.IssueAuthorizationCodeAsync(client);
        Assert.NotNull(code);
        using var tokenResponse = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = "test-client",
            ["code"] = code!,
            ["redirect_uri"] = "https://client.test/callback",
            ["code_verifier"] = verifier,
            ["resource"] = "https://domusmind.test/mcp",
        }));
        using var tokenJson = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync());
        clock.Advance(TimeSpan.FromMinutes(10));

        using var request = new HttpRequestMessage(HttpMethod.Get, "/mcp");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenJson.RootElement.GetProperty("access_token").GetString());
        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Authorization_without_read_scope_is_rejected()
    {
        await using var host = await TestHost.CreateAsync(enabled: true);
        using var client = host.CreateClient();

        var result = await host.IssueAuthorizationCodeAsync(client, "openid");

        Assert.Equal(HttpStatusCode.BadRequest, result.ConsentStatus);
        Assert.Null(result.Code);
    }

    private sealed class TestTimeProvider(DateTimeOffset initialUtcNow) : TimeProvider
    {
        private long _utcTicks = initialUtcNow.UtcTicks;

        public override DateTimeOffset GetUtcNow() =>
            new(Interlocked.Read(ref _utcTicks), TimeSpan.Zero);

        public void Advance(TimeSpan duration) => Interlocked.Add(ref _utcTicks, duration.Ticks);
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

    private static Uri ResolveForClient(HttpClient client, Uri location)
    {
        if (!location.IsAbsoluteUri || client.BaseAddress is null)
            return location;

        return new Uri(client.BaseAddress, location.PathAndQuery);
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
        private readonly bool _useKestrelHttps;
        private readonly X509Certificate2? _testCertificate;
        private readonly Uri _baseAddress;

        public static readonly Guid TestHouseholdId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public static readonly Guid TestUserId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        public const string TestEmail = "mcp-test@domusmind.test";
        public const string TestPassword = "Mcp-Test-Password-123!";

        private TestHost(
            WebApplication app,
            bool useKestrelHttps,
            X509Certificate2? testCertificate,
            Uri baseAddress)
        {
            _app = app;
            _useKestrelHttps = useKestrelHttps;
            _testCertificate = testCertificate;
            _baseAddress = baseAddress;
        }

        public HttpClient CreateClient()
        {
            if (_useKestrelHttps)
            {
                return new HttpClient(new HttpClientHandler
                {
                    AllowAutoRedirect = false,
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                })
                {
                    BaseAddress = _baseAddress,
                };
            }

            return new HttpClient(_app.GetTestServer().CreateHandler())
            {
                BaseAddress = new Uri("https://domusmind.test"),
            };
        }

        public Uri BaseAddress => _baseAddress;

        public static async Task<TestHost> CreateAsync(
            bool enabled,
            int requestsPerMinute = 60,
            string? expectedAudience = null,
            TimeProvider? timeProvider = null,
            bool householdReadOnlyEnabled = false,
            bool useKestrelHttps = false)
        {
            var databaseRoot = new InMemoryDatabaseRoot();
            var databaseName = $"mcp-oauth-{Guid.NewGuid():N}";
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                EnvironmentName = "Development",
                ApplicationName = typeof(McpOAuthController).Assembly.GetName().Name,
            });
            X509Certificate2? testCertificate = null;
            if (useKestrelHttps)
            {
                testCertificate = CreateTestCertificate();
                builder.WebHost.ConfigureKestrel(options =>
                    options.Listen(System.Net.IPAddress.Loopback, 0,
                        listenOptions => listenOptions.UseHttps(testCertificate)));
            }
            else
            {
                builder.WebHost.UseTestServer();
            }
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
                ["Mcp:Capabilities:HouseholdReadOnly"] = householdReadOnlyEnabled.ToString(),
                ["Mcp:RequestsPerMinute"] = requestsPerMinute.ToString(),
            });
            if (expectedAudience is not null)
                builder.Configuration["Mcp:ExpectedAudience"] = expectedAudience;
            if (timeProvider is not null)
                builder.Services.AddSingleton<TimeProvider>(timeProvider);

            builder.Services.AddControllers()
                .AddApplicationPart(typeof(McpOAuthController).Assembly);
            builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            builder.Services.AddScoped<
                IQueryHandler<GetFamilyTimelineQuery, FamilyTimelineResponse>,
                GetFamilyTimelineQueryHandler>();
            builder.Services.AddScoped<
                IQueryHandler<GetFamilyMembersQuery, IReadOnlyCollection<MemberDirectoryItemResponse>>,
                GetFamilyMembersQueryHandler>();
            builder.Services.AddAntiforgery();
            builder.Services.AddDbContext<DomusMindDbContext>(options =>
                options.UseInMemoryDatabase(databaseName, databaseRoot));
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
            app.MapDomusMindMcp();
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

                if (!await database.Set<Domain.Family.Family>().AnyAsync(family => family.Id == FamilyId.From(TestHouseholdId)))
                {
                    var now = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc);
                    var family = Domain.Family.Family.Create(
                        FamilyId.From(TestHouseholdId),
                        FamilyName.Create("OAuth Household"),
                        null,
                        now);
                    var member = family.AddMember(
                        MemberId.New(),
                        MemberName.Create("OAuth Test Member"),
                        MemberRole.Adult,
                        isManager: true,
                        birthDate: null,
                        now,
                        authUserId: TestUserId);
                    var calendarEvent = Domain.Calendar.CalendarEvent.Create(
                        CalendarEventId.New(),
                        family.Id,
                        EventTitle.Create("OAuth household appointment"),
                        null,
                        EventTime.Moment(new DateOnly(2026, 10, 20), new TimeOnly(10, 0)),
                        HexColor.From("#3B82F6"),
                        createdAtUtc: now);
                    calendarEvent.AddParticipant(member.Id);
                    database.Set<Domain.Family.Family>().Add(family);
                    database.Set<Domain.Calendar.CalendarEvent>().Add(calendarEvent);
                }
                await database.SaveChangesAsync();
                await McpOAuthSeedService.SeedClientAsync(scope.ServiceProvider, CancellationToken.None);
            }
            await app.StartAsync();
            var baseAddress = useKestrelHttps
                ? new Uri(app.Urls.Single(address => address.StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
                : new Uri("https://domusmind.test");
            return new TestHost(app, useKestrelHttps, testCertificate, baseAddress);
        }

        private static X509Certificate2 CreateTestCertificate()
        {
            using var rsa = RSA.Create(2048);
            var request = new CertificateRequest(
                "CN=localhost",
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);
            request.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));
            request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, false));
            request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));
            var san = new SubjectAlternativeNameBuilder();
            san.AddDnsName("localhost");
            san.AddIpAddress(System.Net.IPAddress.Loopback);
            request.CertificateExtensions.Add(san.Build());
            return request.CreateSelfSigned(DateTimeOffset.UtcNow.AddMinutes(-1), DateTimeOffset.UtcNow.AddHours(1));
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

        public async Task<(HttpStatusCode ConsentStatus, string? Code, string Verifier)> IssueAuthorizationCodeAsync(
            HttpClient client,
            string scope = "openid offline_access mcp.read")
        {
            const string verifier = "test-code-verifier-012345678901234567890123456789";
            var challenge = Convert.ToBase64String(SHA256.HashData(Encoding.ASCII.GetBytes(verifier)))
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');
            var authorizeUrl = "/oauth/authorize?" + string.Join('&',
                "client_id=test-client",
                "response_type=code",
                "redirect_uri=https%3A%2F%2Fclient.test%2Fcallback",
                $"scope={Uri.EscapeDataString(scope)}",
                "resource=https%3A%2F%2Fdomusmind.test%2Fmcp",
                $"code_challenge={Uri.EscapeDataString(challenge)}",
                "code_challenge_method=S256",
                $"household_id={TestHouseholdId}",
                "state=replay-test");

            using var loginRedirect = await client.GetAsync(authorizeUrl);
            var loginRedirectBody = await loginRedirect.Content.ReadAsStringAsync();
            Assert.True(loginRedirect.Headers.Location is not null,
                $"Authorization did not redirect: {(int)loginRedirect.StatusCode} {loginRedirect.ReasonPhrase}: {loginRedirectBody}");
            var loginUrl = ResolveForClient(client, loginRedirect.Headers.Location!);
            using var loginPage = await client.GetAsync(loginUrl);
            var loginPageBody = await loginPage.Content.ReadAsStringAsync();
            Assert.True(loginPage.IsSuccessStatusCode,
                $"Login page was {(int)loginPage.StatusCode} {loginPage.ReasonPhrase}: {loginPageBody}");
            var loginCookies = GetCookieHeader(loginPage.Headers);
            using var loginRequest = new HttpRequestMessage(HttpMethod.Post, loginUrl)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["email"] = TestEmail,
                    ["password"] = TestPassword,
                    ["returnUrl"] = authorizeUrl,
                    ["__RequestVerificationToken"] = ExtractAntiforgeryToken(loginPageBody),
                }),
            };
            loginRequest.Headers.TryAddWithoutValidation("Cookie", loginCookies);
            using var loginResponse = await client.SendAsync(loginRequest);
            var authCookies = CombineCookies(loginCookies, GetCookieHeader(loginResponse.Headers));
            using var consentPage = await SendWithCookiesAsync(client, HttpMethod.Get, authorizeUrl, authCookies);
            if (!consentPage.IsSuccessStatusCode)
                return (consentPage.StatusCode, null, verifier);
            var consentBody = await consentPage.Content.ReadAsStringAsync();
            authCookies = CombineCookies(authCookies, GetCookieHeader(consentPage.Headers));
            using var consentRequest = new HttpRequestMessage(HttpMethod.Post, authorizeUrl)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["client_id"] = "test-client",
                    ["response_type"] = "code",
                    ["redirect_uri"] = "https://client.test/callback",
                    ["scope"] = scope,
                    ["resource"] = "https://domusmind.test/mcp",
                    ["code_challenge"] = challenge,
                    ["code_challenge_method"] = "S256",
                    ["household_id"] = TestHouseholdId.ToString(),
                    ["state"] = "replay-test",
                    ["consent"] = "allow",
                    ["__RequestVerificationToken"] = ExtractAntiforgeryToken(consentBody),
                }),
            };
            consentRequest.Headers.TryAddWithoutValidation("Cookie", authCookies);
            using var consentResponse = await client.SendAsync(consentRequest);
            var callbackQuery = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(
                consentResponse.Headers.Location!.Query);
            return (consentResponse.StatusCode, callbackQuery["code"].ToString(), verifier);
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

        public async Task RestoreTestHouseholdAccessAsync()
        {
            using var scope = _app.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<DomusMindDbContext>();
            if (!await database.UserFamilyAccesses.AnyAsync(access =>
                    access.UserId == TestUserId && access.FamilyId == TestHouseholdId))
            {
                database.UserFamilyAccesses.Add(new UserFamilyAccess
                {
                    UserId = TestUserId,
                    FamilyId = TestHouseholdId,
                    GrantedAtUtc = DateTime.UtcNow,
                });
                await database.SaveChangesAsync();
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
            _testCertificate?.Dispose();
        }
    }
}
