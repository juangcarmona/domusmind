namespace DomusMind.Contracts.Chats;

public sealed record ChatMessageModel(Guid ChatMessageId, Guid ChatThreadId, Guid? AuthorMemberId,
    string AuthorType, string AuthorName, string Text, Guid? ReplyToMessageId, DateTime CreatedAtUtc);
