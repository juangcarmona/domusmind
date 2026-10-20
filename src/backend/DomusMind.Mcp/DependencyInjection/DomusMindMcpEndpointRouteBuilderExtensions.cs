using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using ModelContextProtocol.AspNetCore;

namespace DomusMind.Mcp.DependencyInjection;

public static class DomusMindMcpEndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapDomusMindMcp(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapMcp("/mcp").RequireRateLimiting("mcp");
}
