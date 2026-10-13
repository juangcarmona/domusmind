using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Tasks.Events;

public sealed record TaskAreaCleared(
    Guid EventId,
    Guid TaskId,
    Guid FamilyId,
    Guid PreviousAreaId,
    DateTime OccurredAtUtc) : IDomainEvent;
