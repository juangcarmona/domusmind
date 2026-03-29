namespace DomusMind.Infrastructure.Auth;

/// <summary>
/// Records an operator-issued invitation for a new user to register in the deployment.
/// Separate from family-member invitations (InviteMember); this is account-scoped.
/// </summary>
public sealed class OperatorInvitation
{
    public Guid Id { get; init; }

    /// <summary>Email the invitation was issued to.</summary>
    public string Email { get; init; } = default!;

    /// <summary>Optional operator-supplied note (not shown to invitee).</summary>
    public string? Note { get; set; }

    /// <summary>Cryptographically random token for future acceptance flow.</summary>
    public string Token { get; init; } = default!;

    /// <summary>Status: Pending | Accepted | Revoked</summary>
    public string Status { get; set; } = "Pending";

    public DateTime CreatedAtUtc { get; init; }

    public DateTime ExpiresAtUtc { get; init; }

    public Guid CreatedByUserId { get; init; }
}
