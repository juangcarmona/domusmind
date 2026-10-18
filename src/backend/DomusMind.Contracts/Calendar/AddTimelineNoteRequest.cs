namespace DomusMind.Contracts.Calendar;

public sealed record AddTimelineNoteRequest(Guid FamilyId, string Text, DateOnly Date);
