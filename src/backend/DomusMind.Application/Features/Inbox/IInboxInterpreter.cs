namespace DomusMind.Application.Features.Inbox;

public sealed record InterpretedProposal(string Type, string Title, string? Description,
    string? Date, string? Time, string? EndDate, string? EndTime, string? DueDate, string? UnresolvedDetails,
    string? TargetPlanTitle = null, int? ReminderMinutesBefore = null);

public interface IInboxInterpreter
{
    Task<IReadOnlyList<InterpretedProposal>> InterpretAsync(string text, CancellationToken cancellationToken);
}
