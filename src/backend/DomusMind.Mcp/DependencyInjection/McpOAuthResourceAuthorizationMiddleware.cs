using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using DomusMind.Application.Abstractions.Security;

namespace DomusMind.Mcp.DependencyInjection;

internal sealed class McpOAuthResourceAuthorizationMiddleware(
    RequestDelegate next,
    IOptions<DomusMindMcpOptions> options,
    ILogger<McpOAuthResourceAuthorizationMiddleware> logger)
{
    public async Task InvokeAsync(
        HttpContext context,
        IAuthUserRepository users,
        IFamilyAuthorizationService familyAuthorization)
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

        var audiences = result.Principal.FindAll("aud")
            .SelectMany(claim => claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .ToHashSet(StringComparer.Ordinal);
        if (string.IsNullOrWhiteSpace(options.Value.ExpectedAudience) ||
            !audiences.Contains(options.Value.ExpectedAudience))
        {
            logger.LogInformation("Rejected MCP request with an invalid resource audience");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.Headers.WWWAuthenticate =
                $"Bearer resource_metadata=\"{options.Value.ProtectedResourceMetadataPath}\"";
            return;
        }

        if (!Guid.TryParse(result.Principal.FindFirst("sub")?.Value, out var userId) ||
            !Guid.TryParse(result.Principal.FindFirst("dm_household_id")?.Value, out var householdId))
        {
            logger.LogInformation("Rejected MCP request without a valid user and household binding");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        var user = await users.FindByIdAsync(userId, context.RequestAborted);
        if (user is null || user.IsDisabled ||
            !await familyAuthorization.CanAccessFamilyAsync(userId, householdId, context.RequestAborted))
        {
            logger.LogInformation("Rejected MCP request after current household authorization check for user {UserId}", userId);
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        logger.LogInformation(
            "Authorized MCP request for user {UserId}, household {HouseholdId}, client {ClientId}, path {Path}",
            userId,
            householdId,
            result.Principal.FindFirst("client_id")?.Value ?? "unknown",
            context.Request.Path);
        context.User = result.Principal;
        await next(context);
    }
}
