using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Family;

namespace DomusMind.Application.Features.Family.ProvisionMemberAccess;

public sealed record ProvisionMemberAccessCommand(
    Guid FamilyId,
    Guid MemberId,
    string Email,
    string? DisplayName,
    Guid RequestedByUserId) : ICommand<ProvisionMemberAccessResponse>;
