using Microsoft.AspNetCore.Antiforgery;

namespace DomusMind.Api.Auth;

public sealed class BffRequestBoundary
{
    private readonly RequestDelegate _next;

    public BffRequestBoundary(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, IAntiforgery antiforgery)
    {
        if (context.Request.Path.StartsWithSegments("/api/auth/login") ||
            context.Request.Path.StartsWithSegments("/api/auth/refresh") ||
            context.Request.Path.StartsWithSegments("/api/auth/logout") ||
            context.Request.Path.StartsWithSegments("/api/auth/change-password"))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        if (context.Request.Path.StartsWithSegments("/api") &&
            !HttpMethods.IsGet(context.Request.Method) &&
            !HttpMethods.IsHead(context.Request.Method) &&
            !HttpMethods.IsOptions(context.Request.Method))
        {
            var origin = context.Request.Headers.Origin.ToString();
            var expectedOrigin = $"{context.Request.Scheme}://{context.Request.Host}";
            if (!string.Equals(origin, expectedOrigin, StringComparison.OrdinalIgnoreCase) ||
                !await antiforgery.IsRequestValidAsync(context))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new
                {
                    code = "csrf_rejected",
                    message = "A valid same-origin request token is required.",
                    traceId = context.TraceIdentifier,
                });
                return;
            }
        }

        await _next(context);
    }
}
