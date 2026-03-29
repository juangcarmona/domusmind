namespace DomusMind.Application.Abstractions.Admin;

public sealed record OperatorInvitationProjection(
    Guid Id,
    string Email,
    string? Note,
    string Status,
    DateTime CreatedAtUtc,
    DateTime ExpiresAtUtc,
    Guid CreatedByUserId);

public sealed record OperatorInvitationCreatedResult(
    OperatorInvitationProjection Invitation,
    string Token);

public interface IOperatorInvitationRepository
{
    Task<int> CountPendingAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OperatorInvitationProjection>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<OperatorInvitationProjection?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<OperatorInvitationCreatedResult> CreateAsync(
        string email,
        string? note,
        Guid createdByUserId,
        CancellationToken cancellationToken = default);

    Task RevokeAsync(Guid id, CancellationToken cancellationToken = default);
}
