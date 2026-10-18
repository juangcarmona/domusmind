using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Chats;

namespace DomusMind.Application.Features.Chats.GetChatThreads;

public sealed record GetChatThreadsQuery(Guid FamilyId, Guid UserId) : IQuery<IReadOnlyList<ChatThreadModel>>;
