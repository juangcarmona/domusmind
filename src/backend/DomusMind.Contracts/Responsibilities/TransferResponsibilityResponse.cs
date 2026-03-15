namespace DomusMind.Contracts.Responsibilities;

public sealed record TransferResponsibilityResponse(
    Guid ResponsibilityDomainId,
    Guid NewPrimaryOwnerId);
