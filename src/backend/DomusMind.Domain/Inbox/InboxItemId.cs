using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Inbox;

public readonly record struct InboxItemId(Guid Value) : IAggregateIdentifier
{
    public static InboxItemId New() => new(Guid.NewGuid());
    public static InboxItemId From(Guid value) => new(value);
    public override string ToString() => Value.ToString();
}
