using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Family.Events;

public sealed record FamilyCreatedEvent(
    Guid EventId,
    Guid FamilyId,
    DateTime OccurredAtUtc) : IDomainEvent;
