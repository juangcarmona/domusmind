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
    public string? ProviderName { get; private set; }
    public string? ProviderHosting { get; private set; }
    public bool FallbackOccurred { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private ChatMessage(ChatMessageId id, Guid chatThreadId, FamilyId familyId, Guid? authorMemberId,
        string authorType, string text, Guid? replyToMessageId, DateTime createdAtUtc,
        string? providerName = null, string? providerHosting = null, bool fallbackOccurred = false) : base(id)
    {
        ChatThreadId = chatThreadId;
        FamilyId = familyId;
        AuthorMemberId = authorMemberId;
        AuthorType = authorType;
        Text = text;
        ReplyToMessageId = replyToMessageId;
        CreatedAtUtc = createdAtUtc;
        ProviderName = providerName;
        ProviderHosting = providerHosting;
        FallbackOccurred = fallbackOccurred;
    }

    private ChatMessage() : base(default) { }

    public static ChatMessage PostMember(ChatMessageId id, Guid threadId, FamilyId familyId,
        Guid memberId, string text, DateTime createdAtUtc)
        => Create(id, threadId, familyId, memberId, "Member", text, null, createdAtUtc);

    public static ChatMessage PostAssistant(ChatMessageId id, Guid threadId, FamilyId familyId,
        string text, Guid replyToMessageId, DateTime createdAtUtc, string? providerName = null,
        string? providerHosting = null, bool fallbackOccurred = false, Guid? requestingUserId = null,
        AssistantProposalDraft? proposal = null)
        => Create(id, threadId, familyId, null, "Assistant", text, replyToMessageId,
            createdAtUtc, providerName, providerHosting, fallbackOccurred, requestingUserId, proposal);

    private static ChatMessage Create(ChatMessageId id, Guid threadId, FamilyId familyId,
        Guid? memberId, string authorType, string text, Guid? replyToMessageId, DateTime createdAtUtc,
        string? providerName = null, string? providerHosting = null, bool fallbackOccurred = false,
        Guid? requestingUserId = null, AssistantProposalDraft? proposal = null)
    {
        if (threadId == Guid.Empty) throw new ArgumentException("Chat thread id is required.", nameof(threadId));
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Message text is required.", nameof(text));
        if (text.Trim().Length > 4000) throw new ArgumentException("Message text cannot exceed 4000 characters.", nameof(text));
        if (authorType == "Member" && (!memberId.HasValue || replyToMessageId.HasValue))
            throw new ArgumentException("A member message requires its author and cannot reply to another message.", nameof(memberId));
        if (authorType == "Assistant" && (memberId.HasValue || !replyToMessageId.HasValue))
            throw new ArgumentException("An assistant response must reference the member message it answers.", nameof(replyToMessageId));

        var message = new ChatMessage(id, threadId, familyId, memberId, authorType,
            text.Trim(), replyToMessageId, createdAtUtc, providerName, providerHosting, fallbackOccurred);
        message.RaiseDomainEvent(new ChatMessagePosted(Guid.NewGuid(), id.Value, threadId,
            familyId.Value, authorType, memberId, createdAtUtc, providerName, providerHosting, fallbackOccurred));
        if (proposal is not null)
        {
            if (authorType != "Assistant" || !replyToMessageId.HasValue || !requestingUserId.HasValue
                || string.IsNullOrWhiteSpace(providerName) || string.IsNullOrWhiteSpace(providerHosting))
                throw new ArgumentException("An assistant proposal requires its member request and provider disclosure.", nameof(proposal));
            message.RaiseDomainEvent(new AssistantProposalRequested(Guid.NewGuid(), id.Value,
                replyToMessageId.Value, threadId, familyId.Value, requestingUserId.Value,
                providerName, providerHosting, fallbackOccurred, proposal, createdAtUtc));
        }
        return message;
    }
}
