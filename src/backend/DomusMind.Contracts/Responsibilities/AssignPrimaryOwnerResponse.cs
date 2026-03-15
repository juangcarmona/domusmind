namespace DomusMind.Contracts.Responsibilities;

public sealed record AssignPrimaryOwnerResponse(
    Guid ResponsibilityDomainId,
    Guid MemberId);
