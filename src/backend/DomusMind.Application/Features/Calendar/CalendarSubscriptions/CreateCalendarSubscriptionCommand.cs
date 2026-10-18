using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Calendar;

namespace DomusMind.Application.Features.Calendar.CalendarSubscriptions;

public sealed record CreateCalendarSubscriptionCommand(Guid FamilyId, string Label, Guid? MemberId,
    Guid RequestedByUserId) : ICommand<CreatedCalendarSubscription>;

public sealed record CreatedCalendarSubscription(Guid Id, string Label, Guid? MemberId,
    DateTime ExpiresAtUtc, string Token);
