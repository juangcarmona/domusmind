namespace DomusMind.Contracts.Responsibilities;

public sealed record UpdateResponsibilityDomainColorResponse(
    Guid ResponsibilityDomainId,
    string Color);
