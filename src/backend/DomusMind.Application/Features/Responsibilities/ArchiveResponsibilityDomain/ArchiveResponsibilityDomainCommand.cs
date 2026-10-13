using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Responsibilities;

namespace DomusMind.Application.Features.Responsibilities.ArchiveResponsibilityDomain;

public sealed record ArchiveResponsibilityDomainCommand(Guid ResponsibilityDomainId, Guid RequestedByUserId)
    : ICommand<ArchiveResponsibilityDomainResponse>;
