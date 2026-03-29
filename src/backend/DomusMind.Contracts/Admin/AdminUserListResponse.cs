namespace DomusMind.Contracts.Admin;

public sealed record AdminUserSummary(
    Guid UserId,
    string Email,
    string? DisplayName,
    bool IsDisabled,
    bool IsOperator,
    DateTime CreatedAtUtc,
    DateTime? LastLoginAtUtc,
    Guid? LinkedFamilyId);

public sealed record AdminUserListResponse(
    IReadOnlyList<AdminUserSummary> Items);
