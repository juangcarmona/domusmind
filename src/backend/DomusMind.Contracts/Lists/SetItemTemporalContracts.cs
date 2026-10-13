namespace DomusMind.Contracts.Lists;

public sealed record SetItemTemporalRequest(
    DateOnly? DueDate,
    DateTimeOffset? Reminder,
    string? Repeat,
    bool ClearDueDate = false,
    bool ClearReminder = false,
    bool ClearRepeat = false);

public sealed record SetItemTemporalResponse(
    Guid ItemId,
    DateOnly? DueDate,
    DateTimeOffset? Reminder,
    string? Repeat,
    DateTime UpdatedAtUtc);
