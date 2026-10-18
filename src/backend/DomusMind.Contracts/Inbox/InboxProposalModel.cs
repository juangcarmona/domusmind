namespace DomusMind.Contracts.Inbox;

public sealed record InboxProposalModel(Guid ProposalId, string Type, string Title, string? Description,
    string? Date, string? Time, string? EndDate, string? EndTime, string? DueDate,
    string Status, string? UnresolvedDetails, Guid? ResultId, string? TargetPlanTitle,
    Guid? TargetPlanId, int? ReminderMinutesBefore);
