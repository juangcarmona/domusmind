using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.RateLimiting;
using ModelContextProtocol.AspNetCore;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using DomusMind.Mcp.Authorization;
using DomusMind.Mcp.Tools;
using DomusMind.Application.Abstractions.Integrations.Mcp;

namespace DomusMind.Mcp.DependencyInjection;

public static class DomusMindMcpServiceCollectionExtensions
{
    private const string McpOperation = "MCP /mcp";

    public static IMcpServerBuilder AddDomusMindMcp(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<DomusMindMcpOptions>()
            .Bind(configuration.GetSection(DomusMindMcpOptions.SectionName))
            .PostConfigure(options =>
            {
                options.ExpectedAudience ??= configuration["Mcp:OAuth:Resource"];
            })
            .ValidateOnStart();
        services.AddSingleton<IValidateOptions<DomusMindMcpOptions>, DomusMindMcpOptionsValidator>();
        services.AddHttpContextAccessor();
        services.AddScoped<IMcpRequestIdentityAccessor, HttpContextMcpRequestIdentityAccessor>();
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = (context, _) =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("DomusMind.Mcp.SecurityAudit");
                logger.LogWarning(
                    "MCP security audit {Outcome} reason={Reason} operation={Operation} trace={TraceId}",
                    "denied",
                    "rate_limited",
                    McpOperation,
                    System.Diagnostics.Activity.Current?.TraceId.ToString() ?? context.HttpContext.TraceIdentifier);
                return ValueTask.CompletedTask;
            };
            options.AddPolicy("mcp", context =>
            {
                var configured = context.RequestServices
                    .GetRequiredService<IOptions<DomusMindMcpOptions>>()
                    .Value;
                var partition = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                return RateLimitPartition.GetFixedWindowLimiter(
                    partition,
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = configured.RequestsPerMinute,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        AutoReplenishment = true,
                    });
            });
        });
        var mcpBuilder = services.AddMcpServer()
            .WithHttpTransport(options =>
                options.SessionMode = HttpServerSessionMode.StatefulForInitializeClients);

        if (configuration.GetValue<bool>("Mcp:Capabilities:HouseholdReadOnly") &&
            configuration.GetValue<bool>("Mcp:OAuth:Enabled") &&
            configuration.GetValue<bool>("Mcp:RequireOAuth"))
        {
            mcpBuilder.WithTools([typeof(DomusMindReadOnlyTools)]);
        }

        if (configuration.GetValue<bool>("Mcp:Capabilities:HouseholdWrites") &&
            configuration.GetValue<bool>("Mcp:OAuth:Enabled") &&
            configuration.GetValue<bool>("Mcp:RequireOAuth"))
        {
            mcpBuilder.WithTools([typeof(DomusMindConfirmedWriteTools)]);
        }

        return mcpBuilder.WithListToolsHandler((_, _) => new ValueTask<ListToolsResult>(new ListToolsResult { Tools = [] }));
    }

    public static IApplicationBuilder UseDomusMindMcpOriginValidation(this IApplicationBuilder app) =>
        app.UseMiddleware<McpOriginValidationMiddleware>();

    public static IApplicationBuilder UseDomusMindMcpOAuthResourceAuthorization(this IApplicationBuilder app) =>
        app.UseMiddleware<McpOAuthResourceAuthorizationMiddleware>();
}
