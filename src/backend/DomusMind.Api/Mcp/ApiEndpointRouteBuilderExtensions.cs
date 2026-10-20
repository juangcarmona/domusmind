using DomusMind.Mcp.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DomusMind.Api.Mcp;

public static class ApiEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapDomusMindEndpoints(
        this WebApplication app,
        string webRootPath)
    {
        app.MapControllers();
        app.MapDomusMindMcp();

        var spaIndexPath = Path.Combine(webRootPath, "index.html");
        if (File.Exists(spaIndexPath))
        {
            app.MapFallback(async context =>
            {
                if (context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase) ||
                    context.Request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase) ||
                    context.Request.Path.StartsWithSegments("/mcp", StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    return;
                }

                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync(spaIndexPath);
            });
        }
        else
        {
            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
        }

        return app;
    }
}
