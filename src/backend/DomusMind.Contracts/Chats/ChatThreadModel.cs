namespace DomusMind.Contracts.Chats;

public sealed record ChatThreadModel(Guid ChatThreadId, Guid FamilyId, string Title, string Kind,
    Guid CreatedByMemberId, IReadOnlyList<Guid> ParticipantMemberIds, DateTime CreatedAtUtc);
