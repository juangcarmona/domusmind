using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Responsibilities.Events;

public sealed record ResponsibilityTransferred(
    Guid EventId,
    Guid ResponsibilityDomainId,
    Guid? PreviousPrimaryOwnerId,
    Guid NewPrimaryOwnerId,
    DateTime OccurredAtUtc) : IDomainEvent;
