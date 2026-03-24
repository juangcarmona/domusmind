namespace DomusMind.Contracts.Responsibilities;

public sealed record RenameResponsibilityDomainResponse(
    Guid ResponsibilityDomainId,
    string Name);
