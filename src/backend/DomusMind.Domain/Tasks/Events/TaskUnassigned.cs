using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Tasks.Events;

public sealed record TaskUnassigned(
    Guid EventId,
    Guid TaskId,
    Guid FamilyId,
    Guid PreviousAssigneeId,
    DateTime OccurredAtUtc) : IDomainEvent;
