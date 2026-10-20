using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModelContextProtocol.AspNetCore;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace DomusMind.Mcp.DependencyInjection;

public static class DomusMindMcpServiceCollectionExtensions
{
    public static IMcpServerBuilder AddDomusMindMcp(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<DomusMindMcpOptions>()
            .Bind(configuration.GetSection(DomusMindMcpOptions.SectionName))
            .ValidateOnStart();
        services.AddSingleton<IValidateOptions<DomusMindMcpOptions>, DomusMindMcpOptionsValidator>();
        return services.AddMcpServer()
            .WithHttpTransport(options =>
                options.SessionMode = HttpServerSessionMode.StatefulForInitializeClients)
            .WithListToolsHandler((_, _) => new ValueTask<ListToolsResult>(new ListToolsResult { Tools = [] }));
    }

    public static IApplicationBuilder UseDomusMindMcpOriginValidation(this IApplicationBuilder app) =>
        app.UseMiddleware<McpOriginValidationMiddleware>();

    public static IApplicationBuilder UseDomusMindMcpOAuthResourceAuthorization(this IApplicationBuilder app) =>
        app.UseMiddleware<McpOAuthResourceAuthorizationMiddleware>();
}
