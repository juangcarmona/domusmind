namespace DomusMind.Domain.Inbox;

public sealed class InboxProposal
{
    public Guid Id { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Date { get; private set; }
    public string? Time { get; private set; }
    public string? EndDate { get; private set; }
    public string? EndTime { get; private set; }
    public string? DueDate { get; private set; }
    public string Status { get; private set; } = "Pending";
    public string? UnresolvedDetails { get; private set; }
    public Guid? ResultId { get; private set; }
    public string? TargetPlanTitle { get; private set; }
    public Guid? TargetPlanId { get; private set; }
    public int? ReminderMinutesBefore { get; private set; }

    private InboxProposal() { }

    private static void Validate(string title, string? description, string? unresolvedDetails)
    {
        if (title.Trim().Length > 200) throw new ArgumentException("Proposal title cannot exceed 200 characters.", nameof(title));
        if (description?.Length > 2000) throw new ArgumentException("Proposal description cannot exceed 2000 characters.", nameof(description));
        if (unresolvedDetails?.Length > 500) throw new ArgumentException("Unresolved details cannot exceed 500 characters.", nameof(unresolvedDetails));
    }

    public static InboxProposal Create(string type, string title, string? description,
        string? date, string? time, string? endDate, string? endTime, string? dueDate,
        string? unresolvedDetails, string? targetPlanTitle = null, int? reminderMinutesBefore = null)
    {
        if (type is not ("Plan" or "Task" or "Reminder" or "TimelineNote"))
            throw new ArgumentException("Unsupported proposal type.", nameof(type));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("A proposal title is required.", nameof(title));
        Validate(title, description, unresolvedDetails);
        return new InboxProposal
        {
            Id = Guid.NewGuid(), Type = type, Title = title.Trim(), Description = description,
            Date = date, Time = time, EndDate = endDate, EndTime = endTime, DueDate = dueDate,
            UnresolvedDetails = unresolvedDetails, TargetPlanTitle = targetPlanTitle,
            ReminderMinutesBefore = reminderMinutesBefore
        };
    }

    public void Refine(string title, string? description, string? date, string? time,
        string? endDate, string? endTime, string? dueDate, Guid? targetPlanId = null,
        int? reminderMinutesBefore = null)
    {
        if (Status != "Pending") throw new InvalidOperationException("Only pending proposals can be refined.");
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("A proposal title is required.", nameof(title));
        Validate(title, description, null);
        Title = title.Trim(); Description = description; Date = date; Time = time;
        EndDate = endDate; EndTime = endTime; DueDate = dueDate; UnresolvedDetails = null;
        if (Type == "Reminder")
        {
            TargetPlanId = targetPlanId;
            ReminderMinutesBefore = reminderMinutesBefore;
        }
    }

    public void Reject()
    {
        if (Status != "Pending") throw new InvalidOperationException("Only pending proposals can be rejected.");
        Status = "Rejected";
    }

    public void BeginApproval()
    {
        if (Status != "Pending") throw new InvalidOperationException("Only pending proposals can be approved.");
        Status = "Executing";
    }

    public void MarkApplied(Guid resultId)
    {
        if (Status != "Executing") throw new InvalidOperationException("Only an approved proposal can be applied.");
        Status = "Applied"; ResultId = resultId;
    }
}
