namespace DomusMind.Contracts.Calendar;

public sealed record CalendarSubscriptionSummary(Guid Id, string Label, Guid? MemberId,
    DateTime CreatedAtUtc, DateTime ExpiresAtUtc, DateTime? RevokedAtUtc,
    DateTime? LastAccessedAtUtc, int AccessCount);
