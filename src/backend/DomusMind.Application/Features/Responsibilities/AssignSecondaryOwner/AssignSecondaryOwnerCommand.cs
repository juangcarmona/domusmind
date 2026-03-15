using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Responsibilities;

namespace DomusMind.Application.Features.Responsibilities.AssignSecondaryOwner;

public sealed record AssignSecondaryOwnerCommand(
    Guid ResponsibilityDomainId,
    Guid MemberId,
    Guid RequestedByUserId)
    : ICommand<AssignSecondaryOwnerResponse>;
