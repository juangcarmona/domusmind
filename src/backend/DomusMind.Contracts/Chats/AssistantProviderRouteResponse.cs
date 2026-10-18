namespace DomusMind.Contracts.Chats;

public sealed record AssistantProviderRouteResponse(Guid FamilyId, bool CanManage,
    bool IsDeploymentDefault, IReadOnlyList<AssistantProviderTargetResponse> Targets);
