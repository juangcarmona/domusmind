namespace DomusMind.Contracts.Responsibilities;

public sealed record CreateResponsibilityDomainResponse(
    Guid ResponsibilityDomainId,
    Guid FamilyId,
    string Name,
    DateTime CreatedAtUtc);
