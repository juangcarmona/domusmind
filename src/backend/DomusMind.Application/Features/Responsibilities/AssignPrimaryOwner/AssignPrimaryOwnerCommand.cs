using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Responsibilities;

namespace DomusMind.Application.Features.Responsibilities.AssignPrimaryOwner;

public sealed record AssignPrimaryOwnerCommand(
    Guid ResponsibilityDomainId,
    Guid MemberId,
    Guid RequestedByUserId)
    : ICommand<AssignPrimaryOwnerResponse>;
