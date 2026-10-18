using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Chats.Events;
using DomusMind.Domain.Chats.ValueObjects;
using DomusMind.Domain.Family;

namespace DomusMind.Domain.Chats;

public sealed class ChatMessage : AggregateRoot<ChatMessageId>
{
    public Guid ChatThreadId { get; private set; }
    public FamilyId FamilyId { get; private set; }
    public Guid? AuthorMemberId { get; private set; }
    public string AuthorType { get; private set; } = string.Empty;
    public string Text { get; private set; } = string.Empty;
    public Guid? ReplyToMessageId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private ChatMessage(ChatMessageId id, Guid chatThreadId, FamilyId familyId, Guid? authorMemberId,
        string authorType, string text, Guid? replyToMessageId, DateTime createdAtUtc) : base(id)
    {
        ChatThreadId = chatThreadId;
        FamilyId = familyId;
        AuthorMemberId = authorMemberId;
        AuthorType = authorType;
        Text = text;
        ReplyToMessageId = replyToMessageId;
        CreatedAtUtc = createdAtUtc;
    }

    private ChatMessage() : base(default) { }

    public static ChatMessage PostMember(ChatMessageId id, Guid threadId, FamilyId familyId,
        Guid memberId, string text, DateTime createdAtUtc)
        => Create(id, threadId, familyId, memberId, "Member", text, null, createdAtUtc);

    public static ChatMessage PostAssistant(ChatMessageId id, Guid threadId, FamilyId familyId,
        string text, Guid replyToMessageId, DateTime createdAtUtc)
        => Create(id, threadId, familyId, null, "Assistant", text, replyToMessageId, createdAtUtc);

    private static ChatMessage Create(ChatMessageId id, Guid threadId, FamilyId familyId,
        Guid? memberId, string authorType, string text, Guid? replyToMessageId, DateTime createdAtUtc)
    {
        if (threadId == Guid.Empty) throw new ArgumentException("Chat thread id is required.", nameof(threadId));
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Message text is required.", nameof(text));
        if (text.Trim().Length > 4000) throw new ArgumentException("Message text cannot exceed 4000 characters.", nameof(text));
        if (authorType == "Member" && (!memberId.HasValue || replyToMessageId.HasValue))
            throw new ArgumentException("A member message requires its author and cannot reply to another message.", nameof(memberId));
        if (authorType == "Assistant" && (memberId.HasValue || !replyToMessageId.HasValue))
            throw new ArgumentException("An assistant response must reference the member message it answers.", nameof(replyToMessageId));

        var message = new ChatMessage(id, threadId, familyId, memberId, authorType,
            text.Trim(), replyToMessageId, createdAtUtc);
        message.RaiseDomainEvent(new ChatMessagePosted(Guid.NewGuid(), id.Value, threadId,
            familyId.Value, authorType, memberId, createdAtUtc));
        return message;
    }
}
