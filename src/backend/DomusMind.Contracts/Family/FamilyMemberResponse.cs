namespace DomusMind.Contracts.Family;

/// <summary>Access status of a member's linked auth account.</summary>
public enum MemberAccessStatus
{
    /// <summary>No login account has been provisioned.</summary>
    None,
    /// <summary>Account exists and is active. Member must still change the temporary password.</summary>
    PasswordChangeRequired,
    /// <summary>Account exists, active and in normal use.</summary>
    Active,
    /// <summary>Account has been disabled by an admin.</summary>
    Disabled,
}

public sealed record FamilyMemberResponse(
    Guid MemberId,
    Guid FamilyId,
    string Name,
    string Role,
    bool IsManager,
    DateOnly? BirthDate,
    DateTime JoinedAtUtc,
    Guid? AuthUserId,
    MemberAccessStatus AccessStatus,
    string? LinkedEmail);
