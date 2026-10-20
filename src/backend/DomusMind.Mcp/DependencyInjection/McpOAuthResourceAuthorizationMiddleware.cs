using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using DomusMind.Application.Abstractions.Security;
using System.Diagnostics;
using System.Security.Claims;

namespace DomusMind.Mcp.DependencyInjection;

internal sealed class McpOAuthResourceAuthorizationMiddleware(
    RequestDelegate next,
    IOptions<DomusMindMcpOptions> options,
    ILogger<McpOAuthResourceAuthorizationMiddleware> logger)
{
    private const string McpOperation = "MCP /mcp";

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
            await RejectAsync(context, logger, StatusCodes.Status401Unauthorized, "unauthenticated");
            context.Response.Headers.WWWAuthenticate =
                $"Bearer resource_metadata=\"{options.Value.ProtectedResourceMetadataPath}\", scope=\"mcp.read\"";
            return;
        }

        var scopes = result.Principal.FindAll("scope")
            .SelectMany(claim => claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .ToHashSet(StringComparer.Ordinal);
        if (!scopes.Contains("mcp.read"))
        {
            await RejectAsync(context, logger, StatusCodes.Status403Forbidden, "insufficient_scope", result.Principal);
            return;
        }

        var audiences = result.Principal.FindAll("aud")
            .SelectMany(claim => claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .ToHashSet(StringComparer.Ordinal);
        if (string.IsNullOrWhiteSpace(options.Value.ExpectedAudience) ||
            !audiences.Contains(options.Value.ExpectedAudience))
        {
            await RejectAsync(context, logger, StatusCodes.Status401Unauthorized, "invalid_audience", result.Principal);
            context.Response.Headers.WWWAuthenticate =
                $"Bearer resource_metadata=\"{options.Value.ProtectedResourceMetadataPath}\"";
            return;
        }

        if (!Guid.TryParse(result.Principal.FindFirst("sub")?.Value, out var userId) ||
            !Guid.TryParse(result.Principal.FindFirst("dm_household_id")?.Value, out var householdId))
        {
            await RejectAsync(context, logger, StatusCodes.Status403Forbidden, "invalid_subject_binding", result.Principal);
            return;
        }

        var user = await users.FindByIdAsync(userId, context.RequestAborted);
        if (user is null || user.IsDisabled ||
            !await familyAuthorization.CanAccessFamilyAsync(userId, householdId, context.RequestAborted))
        {
            await RejectAsync(context, logger, StatusCodes.Status403Forbidden, "current_authorization_denied", result.Principal, userId, householdId);
            return;
        }

        context.User = result.Principal;
        try
        {
            await next(context);
            logger.LogInformation(
                "MCP security audit {Outcome} operation={Operation} user={UserId} household={HouseholdId} client={ClientId} scope={Scope} status={StatusCode} trace={TraceId}",
                "completed",
                McpOperation,
                userId,
                householdId,
                result.Principal.FindFirst("client_id")?.Value ?? "unknown",
                "mcp.read",
                context.Response.StatusCode,
                Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier);
        }
        catch
        {
            logger.LogWarning(
                "MCP security audit {Outcome} operation={Operation} user={UserId} household={HouseholdId} client={ClientId} scope={Scope} status={StatusCode} trace={TraceId}",
                "failed",
                McpOperation,
                userId,
                householdId,
                result.Principal.FindFirst("client_id")?.Value ?? "unknown",
                "mcp.read",
                StatusCodes.Status500InternalServerError,
                Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier);
            throw;
        }
    }

    private static Task RejectAsync(
        HttpContext context,
        ILogger logger,
        int statusCode,
        string reason,
        ClaimsPrincipal? principal = null,
        Guid? userId = null,
        Guid? householdId = null)
    {
        context.Response.StatusCode = statusCode;
        logger.LogWarning(
            "MCP security audit {Outcome} reason={Reason} operation={Operation} user={UserId} household={HouseholdId} client={ClientId} scope={Scope} status={StatusCode} trace={TraceId}",
            "denied",
            reason,
            McpOperation,
            userId?.ToString() ?? principal?.FindFirst("sub")?.Value ?? "unknown",
            householdId?.ToString() ?? principal?.FindFirst("dm_household_id")?.Value ?? "unknown",
            principal?.FindFirst("client_id")?.Value ?? "unknown",
            principal?.FindFirst("scope")?.Value ?? "unknown",
            statusCode,
            Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier);
        return Task.CompletedTask;
    }
}
