using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Calendar;

namespace DomusMind.Application.Features.Calendar.CalendarSubscriptions;

public sealed record ListCalendarSubscriptionsQuery(Guid FamilyId, Guid RequestedByUserId)
    : IQuery<IReadOnlyCollection<CalendarSubscriptionSummary>>;
