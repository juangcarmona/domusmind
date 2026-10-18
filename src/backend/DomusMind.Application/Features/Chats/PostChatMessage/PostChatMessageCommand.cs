using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Chats;

namespace DomusMind.Application.Features.Chats.PostChatMessage;

public sealed record PostChatMessageCommand(Guid ChatThreadId, string Text, Guid UserId)
    : ICommand<PostChatMessageResult>;

public sealed record PostChatMessageResult(ChatMessageModel Message, Guid FamilyId,
    bool AssistantRequested);
