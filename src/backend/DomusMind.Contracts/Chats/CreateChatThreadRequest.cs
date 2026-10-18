namespace DomusMind.Contracts.Chats;

public sealed record CreateChatThreadRequest(Guid FamilyId, string Title, string Kind,
    IReadOnlyList<Guid> ParticipantMemberIds);
