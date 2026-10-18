using DomusMind.Application.Features.Chats.Assistant;

namespace DomusMind.Application.Features.Inbox;

public sealed record InterpretedProposal(string Type, string Title, string? Description,
    string? Date, string? Time, string? EndDate, string? EndTime, string? DueDate, string? UnresolvedDetails,
    string? TargetPlanTitle = null, int? ReminderMinutesBefore = null);

public sealed record InboxInterpretationResult(IReadOnlyList<InterpretedProposal> Proposals,
    string ProviderName, string Hosting, bool FallbackOccurred);

public interface IInboxInterpreter
{
    Task<InboxInterpretationResult> InterpretAsync(Guid familyId, string text,
        CancellationToken cancellationToken);
}

public interface IInboxInterpreterProvider
{
    string Protocol { get; }
    Task<IReadOnlyList<InterpretedProposal>> InterpretAsync(AssistantProviderTarget target,
        string text, CancellationToken cancellationToken);
}
