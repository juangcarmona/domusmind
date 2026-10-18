using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Calendar.Events;
using DomusMind.Domain.Family;

namespace DomusMind.Domain.Calendar;

public sealed class TimelineNote : AggregateRoot<TimelineNoteId>
{
    public FamilyId FamilyId { get; private set; }
    public DateOnly Date { get; private set; }
    public string Text { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }

    private TimelineNote(TimelineNoteId id, FamilyId familyId, DateOnly date, string text, DateTime createdAtUtc)
        : base(id)
    {
        FamilyId = familyId;
        Date = date;
        Text = text;
        CreatedAtUtc = createdAtUtc;
    }

    public static TimelineNote Create(TimelineNoteId id, FamilyId familyId, DateOnly date,
        string text, DateTime? createdAtUtc = null)
    {
        var normalized = text.Trim();
        if (normalized.Length is 0 or > 4000)
            throw new ArgumentException("A timeline note must contain 1 to 4000 characters.", nameof(text));
        var created = createdAtUtc ?? DateTime.UtcNow;
        var note = new TimelineNote(id, familyId, date, normalized, created);
        note.RaiseDomainEvent(new TimelineNoteAdded(Guid.NewGuid(), id.Value, familyId.Value, date,
            normalized, created));
        return note;
    }
}
