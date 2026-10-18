using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Calendar;

public readonly record struct TimelineNoteId(Guid Value) : IAggregateIdentifier
{
    public static TimelineNoteId New() => new(Guid.NewGuid());
    public static TimelineNoteId From(Guid value) => new(value);
    public override string ToString() => Value.ToString();
}
