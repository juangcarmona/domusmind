using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Chats;

namespace DomusMind.Application.Features.Chats.CreateChatThread;

public sealed record CreateChatThreadCommand(Guid FamilyId, string Title, string Kind,
    IReadOnlyList<Guid> ParticipantMemberIds, Guid UserId) : ICommand<ChatThreadModel>;
