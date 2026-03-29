namespace DomusMind.Contracts.Admin;

public sealed record OperatorInvitationItem(
    Guid Id,
    string Email,
    string? Note,
    string Status,
    DateTime CreatedAtUtc,
    DateTime ExpiresAtUtc,
    Guid CreatedByUserId);

public sealed record OperatorInvitationListResponse(
    IReadOnlyList<OperatorInvitationItem> Items);

public sealed record CreateOperatorInvitationRequest(
    string Email,
    string? Note);

public sealed record CreateOperatorInvitationResponse(
    Guid Id,
    string Email,
    string Token,
    DateTime ExpiresAtUtc);
