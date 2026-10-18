namespace DomusMind.Application.Features.Chats.Assistant;

public sealed record AssistantProviderTarget(
    Guid Id,
    string Name,
    string Hosting,
    string Protocol,
    string Endpoint,
    string Model,
    string? ApiKey);

public sealed record AssistantProviderTargetSummary(
    Guid Id,
    string Name,
    string Hosting,
    string Protocol,
    string Endpoint,
    string Model,
    bool HasCredential);

public sealed record AssistantProviderRoute(
    Guid FamilyId,
    IReadOnlyList<AssistantProviderTarget> Targets,
    bool IsDeploymentDefault);

public interface IAssistantProviderRouteStore
{
    Task<AssistantProviderRoute> GetAsync(Guid familyId, CancellationToken cancellationToken);
    Task<AssistantProviderRoute> ReplaceAsync(Guid familyId,
        IReadOnlyList<AssistantProviderTarget> targets, CancellationToken cancellationToken);
}

public sealed record AssistantConversationTurn(string Role, string Text);

public sealed record AssistantActionProposal(
    string Type,
    string? TargetId,
    string? ItemId,
    string? Title,
    string? Description,
    string? Date,
    string? Time,
    string? EndDate,
    string? EndTime,
    string? Quantity,
    string? Note,
    string? UnresolvedDetails);

public sealed record AssistantCompletion(string Answer, AssistantActionProposal? Proposal);

public sealed record AssistantProviderRequest(
    string Question,
    IReadOnlyList<AssistantConversationTurn> Conversation,
    string AuthorizedContext,
    bool CanStageProposal);

public sealed record AssistantProviderResult(
    AssistantCompletion Completion,
    string ProviderName,
    string Hosting,
    bool FallbackOccurred);

public interface IChatAssistantProvider
{
    string Protocol { get; }
    Task<AssistantCompletion> CompleteAsync(AssistantProviderTarget target,
        AssistantProviderRequest request, CancellationToken cancellationToken);
}

public interface IChatAssistant
{
    Task<AssistantProviderResult> CompleteAsync(Guid familyId, AssistantProviderRequest request,
        CancellationToken cancellationToken);
}

public sealed class ChatAssistantUnavailableException(string message, Exception? innerException = null)
    : Exception(message, innerException);

public sealed class ChatAssistantTargetException(string message, bool retryable, Exception? innerException = null)
    : Exception(message, innerException)
{
    public bool Retryable { get; } = retryable;
}
