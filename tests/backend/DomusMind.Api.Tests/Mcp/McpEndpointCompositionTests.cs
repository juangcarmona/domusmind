using System.Net;
using System.Net.Http.Json;
using DomusMind.Application.Abstractions.Integrations.Mcp;
using DomusMind.Api.Controllers;
using DomusMind.Api.Mcp;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Mcp.Authorization;
using DomusMind.Mcp.DependencyInjection;
using DomusMind.Mcp.Tools;
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

        using var getRequest = new HttpRequestMessage(HttpMethod.Get, "/mcp");
        getRequest.Headers.TryAddWithoutValidation("MCP-Protocol-Version", "2026-07-28");
        getRequest.Headers.Accept.ParseAdd("application/json, text/event-stream");
        using var mcpGetResponse = await client.SendAsync(getRequest);
        Assert.Equal(HttpStatusCode.MethodNotAllowed, mcpGetResponse.StatusCode);
        Assert.NotEqual("text/html", mcpGetResponse.Content.Headers.ContentType?.MediaType);
        Assert.DoesNotContain("spa-marker", await mcpGetResponse.Content.ReadAsStringAsync(), StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("https://untrusted.example")]
    [InlineData("https://trusted.example/")]
    [InlineData("https://trusted.example/a/..")]
    [InlineData("https://trusted.example?unexpected=query")]
    public async Task Present_invalid_origin_is_rejected_with_forbidden(string origin)
    {
        await using var host = await ApiTestHost.StartAsync();
        using var request = new HttpRequestMessage(HttpMethod.Post, "/mcp")
        {
            Content = JsonContent.Create(new { jsonrpc = "2.0", id = 1, method = "tools/list" }),
        };
        request.Headers.TryAddWithoutValidation("Origin", origin);

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
            ProtocolVersion = "2026-07-28",
        });

        var tools = await mcpClient.ListToolsAsync();
        Assert.Contains(tools, tool => tool.Name == "test_query_probe");
        Assert.Equal("2026-07-28", mcpClient.NegotiatedProtocolVersion);

        var dispatchCount = host.QueryDispatcher.DispatchCount;
        var blankInput = await mcpClient.CallToolAsync("test_query_probe", new Dictionary<string, object?>
        {
            ["value"] = " ",
        });
        Assert.True(blankInput.IsError);
        Assert.Equal(dispatchCount, host.QueryDispatcher.DispatchCount);

        var invalidInput = await mcpClient.CallToolAsync("test_query_probe", new Dictionary<string, object?>
        {
            ["value"] = new string('x', TestOnlyMcpTools.MaximumValueLength + 1),
        });
        Assert.True(invalidInput.IsError);
        Assert.DoesNotContain(
            "sensitive-internal-detail",
            string.Join(" ", invalidInput.Content.OfType<TextContentBlock>().Select(block => block.Text)),
            StringComparison.Ordinal);

        var result = await mcpClient.CallToolAsync("test_query_probe", new Dictionary<string, object?>
        {
            ["value"] = "bounded-input",
        });

        Assert.NotEqual(true, result.IsError);
        Assert.Contains(result.Content, block => block is TextContentBlock text && text.Text == "safe:bounded-input");
        Assert.True(host.QueryDispatcher.LastCancellationToken.CanBeCanceled);

        using var cancellation = new CancellationTokenSource();
        var pendingCall = mcpClient.CallToolAsync("test_query_probe", new Dictionary<string, object?>
        {
            ["value"] = "wait-for-cancellation",
        }, cancellationToken: cancellation.Token);
        await host.QueryDispatcher.WaitUntilStartedAsync();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await pendingCall);
        Assert.True(host.QueryDispatcher.LastCancellationToken.IsCancellationRequested);

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
            ProtocolVersion = "2025-11-25",
        });

        Assert.Empty(await mcpClient.ListToolsAsync());
        Assert.Equal("2025-11-25", mcpClient.NegotiatedProtocolVersion);
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

    [Fact]
    public async Task Confirmed_write_production_tool_rejects_unknown_and_wrong_typed_arguments_before_dispatch()
    {
        await using var host = await ApiTestHost.StartAsync(registerConfirmedWriteTool: true);
        var transport = new HttpClientTransport(new HttpClientTransportOptions
        {
            Endpoint = new Uri("http://localhost/mcp"),
            TransportMode = HttpTransportMode.StreamableHttp,
            EnableStandaloneGetStream = false,
        }, host.Client, ownsHttpClient: false);
        await using var mcpClient = await McpClient.CreateAsync(transport, new McpClientOptions
        {
            ClientInfo = new Implementation { Name = "DomusMind.Api.Tests", Version = "1.0" },
            ProtocolVersion = "2026-07-28",
        });

        foreach (var arguments in new Dictionary<string, object?>[]
        {
            new()
            {
                ["listId"] = Guid.NewGuid().ToString(),
                ["name"] = "Milk",
                ["idempotencyKey"] = "key",
                ["unexpected"] = "must be rejected",
            },
            new()
            {
                ["listId"] = 42,
                ["name"] = "Milk",
                ["idempotencyKey"] = "key",
            },
        })
        {
            var result = await mcpClient.CallToolAsync("list_item_add", arguments);

            Assert.True(result.IsError);
            Assert.Equal("ERROR", result.StructuredContent!.Value.GetProperty("status").GetString());
            Assert.Equal("INVALID_INPUT", result.StructuredContent.Value.GetProperty("error").GetProperty("code").GetString());
        }

        Assert.Equal(0, host.ConfirmedWriteCoordinator.Calls);
    }

    private sealed class ApiTestHost : IAsyncDisposable
    {
        private readonly WebApplication _app;
        private readonly string _webRoot;

        private ApiTestHost(
            WebApplication app,
            HttpClient client,
            TestQueryDispatcher queryDispatcher,
            TestConfirmedWriteCoordinator? confirmedWriteCoordinator)
        {
            _app = app;
            _webRoot = app.Environment.WebRootPath!;
            Client = client;
            QueryDispatcher = queryDispatcher;
            ConfirmedWriteCoordinator = confirmedWriteCoordinator ?? new TestConfirmedWriteCoordinator();
        }

        public HttpClient Client { get; }
        public TestQueryDispatcher QueryDispatcher { get; }
        public TestConfirmedWriteCoordinator ConfirmedWriteCoordinator { get; }

        public static async Task<ApiTestHost> StartAsync(
            bool registerTestProbe = false,
            bool registerConfirmedWriteTool = false)
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

            TestConfirmedWriteCoordinator? confirmedWriteCoordinator = null;
            if (registerConfirmedWriteTool)
            {
                confirmedWriteCoordinator = new TestConfirmedWriteCoordinator();
                builder.Services.AddSingleton<IMcpRequestIdentityAccessor, TestMcpRequestIdentityAccessor>();
                builder.Services.AddSingleton<IMcpConfirmedWriteCoordinator>(confirmedWriteCoordinator);
                builder.Services.AddSingleton<McpServerTool>(sp => DomusMindConfirmedWriteTools.CreateTool(sp));
            }

            var app = builder.Build();
            app.UseStaticFiles();
            app.UseDomusMindMcpOriginValidation();
            app.MapDomusMindEndpoints(webRoot);
            await app.StartAsync();
            return new ApiTestHost(app, app.GetTestClient(), queryDispatcher, confirmedWriteCoordinator);
        }

        public async ValueTask DisposeAsync()
        {
            Client.Dispose();
            await _app.DisposeAsync();
            Directory.Delete(_webRoot, recursive: true);
        }
    }

    private sealed class TestMcpRequestIdentityAccessor : IMcpRequestIdentityAccessor
    {
        public bool TryGet(out McpRequestIdentity identity)
        {
            identity = new McpRequestIdentity(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "test-client",
                "test-trace",
                new HashSet<string>(["mcp.write"]));
            return true;
        }
    }

    private sealed class TestConfirmedWriteCoordinator : IMcpConfirmedWriteCoordinator
    {
        public int Calls { get; private set; }

        public Task<McpConfirmedWriteOutcome> ExecuteListItemAddAsync(
            McpListItemAddRequest request,
            CancellationToken cancellationToken = default)
        {
            Calls++;
            return Task.FromResult(new McpConfirmedWriteOutcome(
                McpWriteStatuses.Error,
                Code: "TEST_SHOULD_NOT_BE_CALLED",
                Message: "The production boundary test should reject the request first."));
        }

        public Task<McpWriteApprovalView?> GetApprovalAsync(
            Guid approvalRequestId,
            Guid userId,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<McpConfirmedWriteOutcome> ApproveAsync(
            Guid approvalRequestId,
            Guid userId,
            McpApprovalContext context,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<McpConfirmedWriteOutcome> CancelAsync(
            Guid approvalRequestId,
            Guid userId,
            McpApprovalContext context,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();
    }

    [McpServerToolType]
    private static class TestOnlyMcpTools
    {
        public const int MaximumValueLength = 32;

        [McpServerTool(Name = "test_query_probe", ReadOnly = true)]
        public static Task<string> QueryProbe(
            string value,
            IQueryDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            if (value.Length > MaximumValueLength)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"Value must not exceed {MaximumValueLength} characters.");
            }

            return dispatcher.Dispatch(new ProbeQuery(value), cancellationToken);
        }
    }

    private sealed record ProbeQuery(string Value) : IQuery<string>;

    public sealed class TestQueryDispatcher : IQueryDispatcher
    {
        private readonly TaskCompletionSource _started = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private int _dispatchCount;

        public CancellationToken LastCancellationToken { get; private set; }
        public int DispatchCount => Volatile.Read(ref _dispatchCount);

        public Task WaitUntilStartedAsync() => _started.Task.WaitAsync(TimeSpan.FromSeconds(5));

        public async Task<TResponse> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            Interlocked.Increment(ref _dispatchCount);
            LastCancellationToken = cancellationToken;
            if (query is ProbeQuery { Value: "wait-for-cancellation" })
            {
                _started.TrySetResult();
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            }

            if (query is ProbeQuery { Value: "trigger-secret" })
            {
                throw new InvalidOperationException("sensitive-internal-detail");
            }

            var result = query is ProbeQuery probe
                ? $"safe:{probe.Value}"
                : throw new InvalidOperationException("Unexpected test query type.");
            return (TResponse)(object)result;
        }
    }
}
