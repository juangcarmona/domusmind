using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Calendar;

public readonly record struct CalendarEventId(Guid Value) : IAggregateIdentifier
{
    public static CalendarEventId New() => new(Guid.NewGuid());
    public static CalendarEventId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
