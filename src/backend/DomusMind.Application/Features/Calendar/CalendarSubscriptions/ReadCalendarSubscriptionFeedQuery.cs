using DomusMind.Application.Abstractions.Messaging;

namespace DomusMind.Application.Features.Calendar.CalendarSubscriptions;

public sealed record ReadCalendarSubscriptionFeedQuery(string Token) : IQuery<CalendarSubscriptionFeed?>;

public sealed record CalendarSubscriptionFeed(Guid SubscriptionId, string Label,
    IReadOnlyCollection<CalendarSubscriptionFeedPlan> Plans);

public sealed record CalendarSubscriptionFeedPlan(Guid PlanId, string Title, DateOnly StartDate,
    TimeOnly? StartTime, DateOnly? EndDate, TimeOnly? EndTime, bool IsCancelled, bool IsAllDay,
    DateTime CreatedAtUtc);
