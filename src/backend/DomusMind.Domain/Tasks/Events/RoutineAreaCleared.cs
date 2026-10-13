using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Tasks.Events;

public sealed record RoutineAreaCleared(
    Guid EventId,
    Guid RoutineId,
    Guid FamilyId,
    Guid PreviousAreaId,
    DateTime OccurredAtUtc) : IDomainEvent;
