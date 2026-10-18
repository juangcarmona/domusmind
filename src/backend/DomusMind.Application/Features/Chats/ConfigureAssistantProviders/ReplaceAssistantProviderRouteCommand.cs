using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Chats;

namespace DomusMind.Application.Features.Chats.ConfigureAssistantProviders;

public sealed record ReplaceAssistantProviderRouteCommand(Guid FamilyId,
    IReadOnlyList<AssistantProviderTargetRequest> Targets, Guid UserId)
    : ICommand<AssistantProviderRouteResponse>;
