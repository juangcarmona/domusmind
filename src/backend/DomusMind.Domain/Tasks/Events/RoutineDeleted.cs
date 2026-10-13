using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Tasks.Events;

public sealed record RoutineDeleted(
    Guid EventId,
    Guid RoutineId,
    Guid FamilyId,
    DateTime OccurredAtUtc) : IDomainEvent;
