namespace DomusMind.Contracts.Responsibilities;

public sealed record RemoveSecondaryOwnerResponse(
    Guid ResponsibilityDomainId,
    Guid MemberId);
