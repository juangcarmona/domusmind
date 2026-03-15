using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Responsibilities;

namespace DomusMind.Application.Features.Responsibilities.CreateResponsibilityDomain;

public sealed record CreateResponsibilityDomainCommand(
    string Name,
    Guid FamilyId,
    Guid RequestedByUserId)
    : ICommand<CreateResponsibilityDomainResponse>;
