using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Responsibilities.Events;

public sealed record SecondaryOwnerAssigned(
    Guid EventId,
    Guid ResponsibilityDomainId,
    Guid MemberId,
    DateTime OccurredAtUtc) : IDomainEvent;
