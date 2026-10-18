using DomusMind.Domain.Calendar;
using DomusMind.Domain.Calendar.Events;
using DomusMind.Domain.Family;
using FluentAssertions;

namespace DomusMind.Domain.Tests.Calendar;

public sealed class TimelineNoteTests
{
    [Fact]
    public void Create_StoresTrimmedTextAndDate()
    {
        var date = new DateOnly(2030, 6, 23);
        var note = TimelineNote.Create(TimelineNoteId.New(), FamilyId.New(), date, "  First day at home.  ");

        note.Text.Should().Be("First day at home.");
        note.Date.Should().Be(date);
    }

    [Fact]
    public void Create_EmitsTimelineNoteAddedEvent()
    {
        var note = TimelineNote.Create(TimelineNoteId.New(), FamilyId.New(), new DateOnly(2030, 6, 23), "First day at home.");

        note.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<TimelineNoteAdded>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_RejectsEmptyText(string text)
    {
        var act = () => TimelineNote.Create(TimelineNoteId.New(), FamilyId.New(), new DateOnly(2030, 6, 23), text);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_RejectsTextOver4000Characters()
    {
        var act = () => TimelineNote.Create(TimelineNoteId.New(), FamilyId.New(), new DateOnly(2030, 6, 23), new string('x', 4001));

        act.Should().Throw<ArgumentException>();
    }
}
