namespace DomusMind.Contracts.Tasks;

public sealed record UnassignTaskResponse(
    Guid TaskId,
    Guid PreviousAssigneeId);
