using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Chats.ValueObjects;

public readonly record struct ChatMessageId(Guid Value) : IAggregateIdentifier
{
    public static ChatMessageId New() => new(Guid.NewGuid());
    public static ChatMessageId From(Guid value) => new(value);
}
