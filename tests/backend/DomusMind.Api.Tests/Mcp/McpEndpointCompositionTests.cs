using System.Net;
using System.Net.Http.Json;
using DomusMind.Api.Controllers;
using DomusMind.Api.Mcp;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Mcp.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace DomusMind.Api.Tests.Mcp;

public sealed class McpEndpointCompositionTests
{
    [Fact]
    public async Task Actual_api_mapping_keeps_rest_routes_and_does_not_send_spa_for_unsupported_mcp_method()
    {
        await using var host = await ApiTestHost.StartAsync();
        var client = host.Client;

        using var restResponse = await client.GetAsync("/api/system/ping");
        Assert.Equal(HttpStatusCode.OK, restResponse.StatusCode);

        using var mcpGetResponse = await client.GetAsync("/mcp");
        Assert.Equal(HttpStatusCode.NotFound, mcpGetResponse.StatusCode);
        Assert.DoesNotContain("spa-marker", await mcpGetResponse.Content.ReadAsStringAsync(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task Present_invalid_origin_is_rejected_with_forbidden()
    {
        await using var host = await ApiTestHost.StartAsync();
        using var request = new HttpRequestMessage(HttpMethod.Post, "/mcp")
        {
            Content = JsonContent.Create(new { jsonrpc = "2.0", id = 1, method = "tools/list" }),
        };
        request.Headers.TryAddWithoutValidation("Origin", "https://untrusted.example");

        using var response = await host.Client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Official_client_can_call_a_test_only_probe_through_application_query_dispatch()
    {
        await using var host = await ApiTestHost.StartAsync(registerTestProbe: true);
        var transport = new HttpClientTransport(new HttpClientTransportOptions
        {
            Endpoint = new Uri("http://localhost/mcp"),
            TransportMode = HttpTransportMode.StreamableHttp,
            EnableStandaloneGetStream = false,
        }, host.Client, ownsHttpClient: false);

        await using var mcpClient = await McpClient.CreateAsync(transport, new McpClientOptions
        {
            ClientInfo = new Implementation { Name = "DomusMind.Api.Tests", Version = "1.0" },
        });

        var tools = await mcpClient.ListToolsAsync();
        Assert.Contains(tools, tool => tool.Name == "test_query_probe");

        var result = await mcpClient.CallToolAsync("test_query_probe", new Dictionary<string, object?>
        {
            ["value"] = "bounded-input",
        });

        Assert.NotEqual(true, result.IsError);
        Assert.Contains(result.Content, block => block is TextContentBlock text && text.Text == "safe:bounded-input");
        Assert.True(host.QueryDispatcher.LastCancellationToken.CanBeCanceled);

        var error = await mcpClient.CallToolAsync("test_query_probe", new Dictionary<string, object?>
        {
            ["value"] = "trigger-secret",
        });
        Assert.Equal(true, error.IsError);
        Assert.DoesNotContain(
            "sensitive-internal-detail",
            string.Join(" ", error.Content.OfType<TextContentBlock>().Select(block => block.Text)),
            StringComparison.Ordinal);
    }

    [Fact]
    public async Task Production_registration_exposes_no_tools()
    {
        await using var host = await ApiTestHost.StartAsync();
        var transport = new HttpClientTransport(new HttpClientTransportOptions
        {
            Endpoint = new Uri("http://localhost/mcp"),
            TransportMode = HttpTransportMode.StreamableHttp,
            EnableStandaloneGetStream = false,
        }, host.Client, ownsHttpClient: false);
        await using var mcpClient = await McpClient.CreateAsync(transport, new McpClientOptions
        {
            ClientInfo = new Implementation { Name = "DomusMind.Api.Tests", Version = "1.0" },
        });

        Assert.Empty(await mcpClient.ListToolsAsync());
    }

    [Fact]
    public async Task Allowed_origin_reaches_protocol_endpoint_and_is_not_rejected_by_origin_guard()
    {
        await using var host = await ApiTestHost.StartAsync();
        using var request = new HttpRequestMessage(HttpMethod.Post, "/mcp")
        {
            Content = JsonContent.Create(new { jsonrpc = "2.0", id = 1, method = "tools/list" }),
        };
        request.Headers.TryAddWithoutValidation("Origin", "https://trusted.example");
        request.Headers.Accept.ParseAdd("application/json, text/event-stream");

        using var response = await host.Client.SendAsync(request);

        Assert.NotEqual(HttpStatusCode.Forbidden, response.StatusCode);
    }

    private sealed class ApiTestHost : IAsyncDisposable
    {
        private readonly WebApplication _app;
        private readonly string _webRoot;

        private ApiTestHost(WebApplication app, HttpClient client, TestQueryDispatcher queryDispatcher)
        {
            _app = app;
            _webRoot = app.Environment.WebRootPath!;
            Client = client;
            QueryDispatcher = queryDispatcher;
        }

        public HttpClient Client { get; }
        public TestQueryDispatcher QueryDispatcher { get; }

        public static async Task<ApiTestHost> StartAsync(bool registerTestProbe = false)
        {
            var webRoot = Path.Combine(Path.GetTempPath(), $"domusmind-mcp-test-{Guid.NewGuid():N}");
            Directory.CreateDirectory(webRoot);
            await File.WriteAllTextAsync(Path.Combine(webRoot, "index.html"), "spa-marker");

            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                EnvironmentName = "Testing",
                ContentRootPath = webRoot,
                WebRootPath = webRoot,
            });
            builder.WebHost.UseTestServer();
            builder.Logging.ClearProviders();
            builder.Services.AddControllers().AddApplicationPart(typeof(SystemController).Assembly);
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Mcp:AllowedOrigins:0"] = "https://trusted.example",
            }).Build();
            var queryDispatcher = new TestQueryDispatcher();
            builder.Services.AddSingleton<IQueryDispatcher>(queryDispatcher);
            var mcpBuilder = builder.Services.AddDomusMindMcp(config);
            if (registerTestProbe)
            {
                mcpBuilder.WithTools([typeof(TestOnlyMcpTools)]);
            }

            var app = builder.Build();
            app.UseStaticFiles();
            app.UseDomusMindMcpOriginValidation();
            app.MapDomusMindEndpoints(webRoot);
            await app.StartAsync();
            return new ApiTestHost(app, app.GetTestClient(), queryDispatcher);
        }

        public async ValueTask DisposeAsync()
        {
            Client.Dispose();
            await _app.DisposeAsync();
            Directory.Delete(_webRoot, recursive: true);
        }
    }

    [McpServerToolType]
    private static class TestOnlyMcpTools
    {
        [McpServerTool(Name = "test_query_probe", ReadOnly = true)]
        public static Task<string> QueryProbe(
            string value,
            IQueryDispatcher dispatcher,
            CancellationToken cancellationToken) =>
            dispatcher.Dispatch(new ProbeQuery(value), cancellationToken);
    }

    private sealed record ProbeQuery(string Value) : IQuery<string>;

    public sealed class TestQueryDispatcher : IQueryDispatcher
    {
        public CancellationToken LastCancellationToken { get; private set; }

        public Task<TResponse> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            LastCancellationToken = cancellationToken;
            if (query is ProbeQuery { Value: "trigger-secret" })
            {
                throw new InvalidOperationException("sensitive-internal-detail");
            }

            var result = query is ProbeQuery probe
                ? $"safe:{probe.Value}"
                : throw new InvalidOperationException("Unexpected test query type.");
            return Task.FromResult((TResponse)(object)result);
        }
    }
}
