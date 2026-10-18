using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Chats.ValueObjects;

public readonly record struct ChatThreadId(Guid Value) : IAggregateIdentifier
{
    public static ChatThreadId New() => new(Guid.NewGuid());
    public static ChatThreadId From(Guid value) => new(value);
}
