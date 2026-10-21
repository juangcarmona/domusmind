using System.Security.Claims;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace DomusMind.Mcp.Authorization;

public sealed record McpRequestIdentity(
    Guid UserId,
    Guid HouseholdId,
    string ClientId,
    string TraceId,
    IReadOnlySet<string>? Scopes = null)
{
    public bool HasScope(string scope) => Scopes?.Contains(scope) == true;
}

public interface IMcpRequestIdentityAccessor
{
    bool TryGet(out McpRequestIdentity identity);
}

internal sealed class HttpContextMcpRequestIdentityAccessor(IHttpContextAccessor httpContextAccessor)
    : IMcpRequestIdentityAccessor
{
    public bool TryGet(out McpRequestIdentity identity)
    {
        var principal = httpContextAccessor.HttpContext?.User;
        if (principal?.Identity?.IsAuthenticated != true ||
            !TryGetGuid(principal, "sub", ClaimTypes.NameIdentifier, out var userId) ||
            !TryGetGuid(principal, "dm_household_id", out var householdId))
        {
            identity = default!;
            return false;
        }

        identity = new McpRequestIdentity(
            userId,
            householdId,
            principal.FindFirst("client_id")?.Value ?? "unknown",
            Activity.Current?.TraceId.ToString() ?? httpContextAccessor.HttpContext?.TraceIdentifier ?? "unknown",
            principal.FindAll("scope")
                .SelectMany(claim => claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .ToHashSet(StringComparer.Ordinal));
        return true;
    }

    private static bool TryGetGuid(ClaimsPrincipal principal, string claimType, out Guid value)
        => TryGetGuid(principal, claimType, null, out value);

    private static bool TryGetGuid(ClaimsPrincipal principal, string firstClaimType, string? secondClaimType,
        out Guid value)
    {
        var first = principal.FindFirst(firstClaimType)?.Value;
        var second = secondClaimType is null ? null : principal.FindFirst(secondClaimType)?.Value;
        return Guid.TryParse(first ?? second, out value) && value != Guid.Empty;
    }
}
