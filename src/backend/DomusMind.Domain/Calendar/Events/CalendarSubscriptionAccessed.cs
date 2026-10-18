using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Calendar.Events;

public sealed record CalendarSubscriptionAccessed(Guid EventId, Guid SubscriptionId, DateTime OccurredAtUtc,
    int AccessCount) : IDomainEvent;
