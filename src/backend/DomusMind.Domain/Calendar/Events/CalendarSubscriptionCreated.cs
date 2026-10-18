using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Calendar.Events;

public sealed record CalendarSubscriptionCreated(Guid EventId, Guid SubscriptionId, Guid FamilyId, Guid OwnerUserId,
    DateTime OccurredAtUtc) : IDomainEvent;
