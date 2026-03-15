using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Responsibilities;

namespace DomusMind.Application.Features.Responsibilities.TransferResponsibility;

public sealed record TransferResponsibilityCommand(
    Guid ResponsibilityDomainId,
    Guid NewPrimaryOwnerId,
    Guid RequestedByUserId)
    : ICommand<TransferResponsibilityResponse>;
