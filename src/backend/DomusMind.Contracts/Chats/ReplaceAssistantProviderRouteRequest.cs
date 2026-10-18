namespace DomusMind.Contracts.Chats;

public sealed record ReplaceAssistantProviderRouteRequest(
    IReadOnlyList<AssistantProviderTargetRequest> Targets);
