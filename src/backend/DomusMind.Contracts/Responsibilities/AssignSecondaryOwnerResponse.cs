namespace DomusMind.Contracts.Responsibilities;

public sealed record AssignSecondaryOwnerResponse(
    Guid ResponsibilityDomainId,
    Guid MemberId);
