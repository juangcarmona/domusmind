using DomusMind.Application.Abstractions.Messaging;

namespace DomusMind.Application.Features.Calendar.CalendarSubscriptions;

public sealed record RevokeCalendarSubscriptionCommand(Guid SubscriptionId, Guid RequestedByUserId) : ICommand<bool>;
