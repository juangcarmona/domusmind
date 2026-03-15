using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Responsibilities.Events;

public sealed record PrimaryOwnerAssigned(
    Guid EventId,
    Guid ResponsibilityDomainId,
    Guid MemberId,
    DateTime OccurredAtUtc) : IDomainEvent;
