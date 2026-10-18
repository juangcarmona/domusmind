using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Chats;

namespace DomusMind.Application.Features.Chats.ConfigureAssistantProviders;

public sealed record GetAssistantProviderRouteQuery(Guid FamilyId, Guid UserId)
    : IQuery<AssistantProviderRouteResponse>;
