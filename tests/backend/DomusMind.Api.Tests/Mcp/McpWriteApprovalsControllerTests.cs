using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using DomusMind.Api.Controllers;
using DomusMind.Application.Abstractions.Integrations.Mcp;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DomusMind.Api.Tests.Mcp;

public sealed class McpWriteApprovalsControllerTests
{
    [Fact]
    public async Task Mcp_bearer_credentials_cannot_call_cookie_authenticated_approval_endpoint()
    {
        await using var host = await ApprovalHost.StartAsync();
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"/api/mcp/write-approvals/{Guid.NewGuid()}/approve");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "mcp-token");

        using var response = await host.Client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal(0, host.Coordinator.ApproveCalls);
    }

    [Fact]
    public async Task Cookie_authenticated_approval_requires_antiforgery_token_before_coordinator()
    {
        await using var host = await ApprovalHost.StartAsync();
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"/api/mcp/write-approvals/{Guid.NewGuid()}/approve");
        request.Headers.TryAddWithoutValidation("X-Test-Cookie", "valid");

        using var response = await host.Client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(0, host.Coordinator.ApproveCalls);
    }

    private sealed class ApprovalHost : IAsyncDisposable
    {
        private readonly WebApplication _app;

        private ApprovalHost(WebApplication app, HttpClient client, RecordingCoordinator coordinator)
        {
            _app = app;
            Client = client;
            Coordinator = coordinator;
        }

        public HttpClient Client { get; }
        public RecordingCoordinator Coordinator { get; }

        public static async Task<ApprovalHost> StartAsync()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                EnvironmentName = "Testing",
            });
            builder.WebHost.UseTestServer();
            builder.Logging.ClearProviders();
            builder.Services.AddControllersWithViews()
                .AddApplicationPart(typeof(McpWriteApprovalsController).Assembly);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAntiforgery();
            builder.Services.AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, TestCookieAuthenticationHandler>(
                    McpOAuthConstants.CookieScheme,
                    _ => { });
            builder.Services.AddAuthorization();
            builder.Services.AddSingleton<ICurrentUser, CurrentUserAccessor>();
            var coordinator = new RecordingCoordinator();
            builder.Services.AddSingleton<IMcpConfirmedWriteCoordinator>(coordinator);

            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            await app.StartAsync();
            return new ApprovalHost(app, app.GetTestClient(), coordinator);
        }

        public async ValueTask DisposeAsync()
        {
            Client.Dispose();
            await _app.DisposeAsync();
        }
    }

    private sealed class TestCookieAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        System.Text.Encodings.Web.UrlEncoder encoder)
        : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("X-Test-Cookie", out var value) || value != "valid")
                return Task.FromResult(AuthenticateResult.NoResult());

            var identity = new ClaimsIdentity(
                [new Claim(ClaimTypes.NameIdentifier, Guid.Empty.ToString())],
                Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                Scheme.Name)));
        }
    }

    private sealed class RecordingCoordinator : IMcpConfirmedWriteCoordinator
    {
        public int ApproveCalls { get; private set; }

        public Task<McpConfirmedWriteOutcome> ExecuteListItemAddAsync(
            McpListItemAddRequest request,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(new McpConfirmedWriteOutcome(McpWriteStatuses.Completed));

        public Task<McpWriteApprovalView?> GetApprovalAsync(
            Guid approvalRequestId,
            Guid userId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<McpWriteApprovalView?>(null);

        public Task<McpConfirmedWriteOutcome> ApproveAsync(
            Guid approvalRequestId,
            Guid userId,
            McpApprovalContext context,
            CancellationToken cancellationToken = default)
        {
            ApproveCalls++;
            return Task.FromResult(new McpConfirmedWriteOutcome(McpWriteStatuses.Completed));
        }

        public Task<McpConfirmedWriteOutcome> CancelAsync(
            Guid approvalRequestId,
            Guid userId,
            McpApprovalContext context,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(new McpConfirmedWriteOutcome(McpWriteStatuses.Completed));
    }
}
