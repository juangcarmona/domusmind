namespace DomusMind.Contracts.Admin;

public sealed record AdminHouseholdSummary(
    Guid FamilyId,
    string Name,
    DateTime CreatedAtUtc,
    int MemberCount);

public sealed record AdminHouseholdListResponse(
    IReadOnlyList<AdminHouseholdSummary> Items);
