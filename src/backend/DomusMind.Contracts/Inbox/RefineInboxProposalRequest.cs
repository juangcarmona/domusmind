namespace DomusMind.Contracts.Inbox;

public sealed record RefineInboxProposalRequest(string Title, string? Description,
    string? Date, string? Time, string? EndDate, string? EndTime, string? DueDate,
    Guid? TargetPlanId = null, int? ReminderMinutesBefore = null);
