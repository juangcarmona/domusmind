namespace DomusMind.Application.Abstractions.Security;

/// <summary>
/// Projection used by operator admin views to inspect auth user state.
/// </summary>
public sealed record AdminAuthUserProjection(
    Guid UserId,
    string Email,
    string? DisplayName,
    bool IsDisabled,
    bool IsOperator,
    DateTime CreatedAtUtc,
    DateTime? LastLoginAtUtc);
