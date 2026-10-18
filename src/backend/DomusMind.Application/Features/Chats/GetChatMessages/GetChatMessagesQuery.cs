using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Chats;

namespace DomusMind.Application.Features.Chats.GetChatMessages;

public sealed record GetChatMessagesQuery(Guid ChatThreadId, Guid UserId) : IQuery<IReadOnlyList<ChatMessageModel>>;
