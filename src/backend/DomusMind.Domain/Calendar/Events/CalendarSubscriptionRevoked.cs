using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Calendar.Events;

public sealed record CalendarSubscriptionRevoked(Guid EventId, Guid SubscriptionId, DateTime OccurredAtUtc) : IDomainEvent;
