using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Responsibilities;

namespace DomusMind.Application.Features.Responsibilities.RenameResponsibilityDomain;

public sealed record RenameResponsibilityDomainCommand(
    Guid ResponsibilityDomainId,
    string Name,
    Guid RequestedByUserId)
    : ICommand<RenameResponsibilityDomainResponse>;
