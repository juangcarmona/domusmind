using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DomusMind.Mcp.DependencyInjection;

internal sealed class McpOriginValidationMiddleware(
    RequestDelegate next,
    IOptions<DomusMindMcpOptions> options,
    ILogger<McpOriginValidationMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/mcp", StringComparison.OrdinalIgnoreCase))
        {
            await next(context);
            return;
        }

        var originHeaders = context.Request.Headers.Origin;
        if (originHeaders.Count == 0)
        {
            await next(context);
            return;
        }

        var origin = originHeaders.Count == 1 ? originHeaders[0] : null;
        var allowed = origin is not null &&
            Uri.TryCreate(origin, UriKind.Absolute, out var uri) &&
            (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) &&
            string.Equals(origin, uri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase) &&
            options.Value.AllowedOrigins.Any(allowedOrigin =>
                string.Equals(allowedOrigin, origin, StringComparison.OrdinalIgnoreCase));

        if (allowed)
        {
            await next(context);
            return;
        }

        logger.LogWarning("Rejected MCP request with an untrusted Origin");
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
    }
}
