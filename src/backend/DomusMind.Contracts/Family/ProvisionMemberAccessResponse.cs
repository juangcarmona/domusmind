namespace DomusMind.Contracts.Family;

/// <summary>Returned once when admin provisions a member account. temporaryPassword is never retrievable again.</summary>
public sealed record ProvisionMemberAccessResponse(
    Guid UserId,
    Guid MemberId,
    string Email,
    string TemporaryPassword,
    bool MustChangePassword);
