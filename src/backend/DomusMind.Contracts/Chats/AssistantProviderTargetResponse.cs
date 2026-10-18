namespace DomusMind.Contracts.Chats;

public sealed record AssistantProviderTargetResponse(Guid Id, string Name, string Hosting,
    string Protocol, string Endpoint, string Model, bool HasCredential);
