using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Responsibilities;

namespace DomusMind.Application.Features.Responsibilities.RemoveSecondaryOwner;

public sealed record RemoveSecondaryOwnerCommand(
    Guid ResponsibilityDomainId,
    Guid MemberId,
    Guid RequestedByUserId) : ICommand<RemoveSecondaryOwnerResponse>;
