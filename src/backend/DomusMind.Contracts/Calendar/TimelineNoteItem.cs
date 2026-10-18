namespace DomusMind.Contracts.Calendar;

public sealed record TimelineNoteItem(Guid TimelineNoteId, string Text, string Date, DateTime CreatedAtUtc);
