namespace DomusMind.Domain.Chats;

public sealed record AssistantProposalDraft(
    string Type,
    Guid? TargetId,
    Guid? ItemId,
    string? Title,
    string? Description,
    string? Date,
    string? Time,
    string? EndDate,
    string? EndTime,
    string? Quantity,
    string? Note);
