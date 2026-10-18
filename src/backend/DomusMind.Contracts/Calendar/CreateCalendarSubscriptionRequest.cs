namespace DomusMind.Contracts.Calendar;

public sealed record CreateCalendarSubscriptionRequest(Guid FamilyId, string Label, Guid? MemberId);
