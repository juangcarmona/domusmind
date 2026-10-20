using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace DomusMind.Mcp.DependencyInjection;

internal sealed class McpOAuthResourceAuthorizationMiddleware(
    RequestDelegate next,
    IOptions<DomusMindMcpOptions> options,
    ILogger<McpOAuthResourceAuthorizationMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/mcp", StringComparison.OrdinalIgnoreCase) ||
            !options.Value.RequireOAuth)
        {
            await next(context);
            return;
        }

        var result = await context.AuthenticateAsync("OpenIddict.Validation.AspNetCore");
        if (!result.Succeeded || result.Principal is null)
        {
            logger.LogInformation("Rejected unauthenticated MCP request");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.Headers.WWWAuthenticate =
                $"Bearer resource_metadata=\"{options.Value.ProtectedResourceMetadataPath}\", scope=\"mcp.read\"";
            return;
        }

        var scopes = result.Principal.FindAll("scope")
            .SelectMany(claim => claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .ToHashSet(StringComparer.Ordinal);
        if (!scopes.Contains("mcp.read"))
        {
            logger.LogInformation("Rejected MCP request without read scope");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        context.User = result.Principal;
        await next(context);
    }
}
