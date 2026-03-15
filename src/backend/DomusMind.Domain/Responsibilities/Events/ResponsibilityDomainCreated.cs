using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Responsibilities.Events;

public sealed record ResponsibilityDomainCreated(
    Guid EventId,
    Guid ResponsibilityDomainId,
    Guid FamilyId,
    DateTime OccurredAtUtc) : IDomainEvent;
