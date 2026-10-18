using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Calendar;

public readonly record struct CalendarSubscriptionId(Guid Value) : IAggregateIdentifier
{
    public static CalendarSubscriptionId New() => new(Guid.NewGuid());
    public static CalendarSubscriptionId From(Guid value) => new(value);
    public override string ToString() => Value.ToString();
}
