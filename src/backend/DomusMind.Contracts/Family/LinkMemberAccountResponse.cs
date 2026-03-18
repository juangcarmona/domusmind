namespace DomusMind.Contracts.Family;

public sealed record LinkMemberAccountResponse(
    Guid MemberId,
    Guid FamilyId,
    string Name,
    string Role,
    bool IsManager,
    DateOnly? BirthDate,
    string Username,
    Guid AuthUserId,
    DateTime LinkedAtUtc);
