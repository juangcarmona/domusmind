using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Responsibilities;

namespace DomusMind.Application.Features.Responsibilities.UpdateResponsibilityDomainColor;

public sealed record UpdateResponsibilityDomainColorCommand(
    Guid ResponsibilityDomainId,
    string Color,
    Guid RequestedByUserId)
    : ICommand<UpdateResponsibilityDomainColorResponse>;
