namespace DomusMind.Contracts.Calendar;

public sealed record CreateCalendarSubscriptionResponse(Guid Id, string Label, Guid? MemberId,
    DateTime ExpiresAtUtc, string Url);
