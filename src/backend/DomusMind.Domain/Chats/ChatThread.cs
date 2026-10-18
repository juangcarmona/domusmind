using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Chats.Events;
using DomusMind.Domain.Chats.ValueObjects;
using DomusMind.Domain.Family;

namespace DomusMind.Domain.Chats;

public sealed class ChatThread : AggregateRoot<ChatThreadId>
{
    private readonly List<ChatThreadParticipant> _participants = [];

    public FamilyId FamilyId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Kind { get; private set; } = string.Empty;
    public Guid CreatedByMemberId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public IReadOnlyCollection<ChatThreadParticipant> Participants => _participants.AsReadOnly();

    private ChatThread(ChatThreadId id, FamilyId familyId, string title, string kind,
        Guid createdByMemberId, DateTime createdAtUtc) : base(id)
    {
        FamilyId = familyId;
        Title = title;
        Kind = kind;
        CreatedByMemberId = createdByMemberId;
        CreatedAtUtc = createdAtUtc;
    }

    private ChatThread() : base(default) { }

    public static ChatThread Create(ChatThreadId id, FamilyId familyId, string title, string kind,
        Guid createdByMemberId, IEnumerable<Guid> participantMemberIds, DateTime createdAtUtc)
    {
        if (kind is not ("Household" or "Private"))
            throw new ArgumentException("Chat thread kind must be Household or Private.", nameof(kind));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Chat thread title is required.", nameof(title));
        if (title.Trim().Length > 120)
            throw new ArgumentException("Chat thread title cannot exceed 120 characters.", nameof(title));

        var participants = participantMemberIds.Where(x => x != Guid.Empty).Distinct().ToArray();
        if (participants.Length < 2 || !participants.Contains(createdByMemberId))
            throw new ArgumentException("A conversation requires at least two members, including its creator.", nameof(participantMemberIds));

        var thread = new ChatThread(id, familyId, title.Trim(), kind, createdByMemberId, createdAtUtc);
        thread._participants.AddRange(participants.Select(ChatThreadParticipant.Create));
        thread.RaiseDomainEvent(new ChatThreadCreated(Guid.NewGuid(), id.Value, familyId.Value,
            createdByMemberId, createdAtUtc));
        return thread;
    }
}
