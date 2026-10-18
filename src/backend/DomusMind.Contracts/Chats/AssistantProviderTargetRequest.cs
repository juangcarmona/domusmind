namespace DomusMind.Contracts.Chats;

public sealed record AssistantProviderTargetRequest(Guid Id, string Name, string Hosting,
    string Protocol, string Endpoint, string Model, string? ApiKey);
