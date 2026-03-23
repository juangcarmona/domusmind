using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Family;

namespace DomusMind.Application.Features.Family.DisableMemberAccess;

public sealed record DisableMemberAccessCommand(
    Guid FamilyId,
    Guid MemberId,
    Guid RequestedByUserId) : ICommand<DisableMemberAccessResponse>;
