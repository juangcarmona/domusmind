using DomusMind.Api.Calendar;
using DomusMind.Application.Features.Calendar.CalendarSubscriptions;
using FluentAssertions;

namespace DomusMind.Api.Tests.Controllers;

public sealed class CalendarSubscriptionIcsFormatterTests
{
    [Fact]
    public void Format_UsesExclusiveEndForAllDayPlanAndEscapesTitle()
    {
        var feed = new CalendarSubscriptionFeed(Guid.NewGuid(), "Family, Calendar", [
            new CalendarSubscriptionFeedPlan(Guid.NewGuid(), "School;\nTrip", new DateOnly(2026, 10, 18), null,
                null, null, false, true, new DateTime(2026, 10, 1, 10, 5, 0, DateTimeKind.Utc))
        ]);

        var result = CalendarSubscriptionIcsFormatter.Format(feed);

        result.Should().Contain("X-WR-CALNAME:Family\\, Calendar");
        result.Should().Contain("SUMMARY:School\\;\\nTrip");
        result.Should().Contain("DTSTAMP:20261001T100500Z");
        result.Should().Contain("DTSTART;VALUE=DATE:20261018");
        result.Should().Contain("DTEND;VALUE=DATE:20261019");
        result.Should().NotContain(feed.Plans.Single().PlanId.ToString());
        result.Should().EndWith("END:VCALENDAR\r\n");
    }

    [Fact]
    public void Format_FoldsLongUtf8LinesWithoutSplittingCharacters()
    {
        var feed = new CalendarSubscriptionFeed(Guid.NewGuid(), new string('家', 40), [
            new CalendarSubscriptionFeedPlan(Guid.NewGuid(), new string('é', 60), new DateOnly(2026, 10, 18),
                null, null, null, false, true, DateTime.UtcNow)
        ]);

        var result = CalendarSubscriptionIcsFormatter.Format(feed);

        result.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
            .Should().OnlyContain(line => System.Text.Encoding.UTF8.GetByteCount(line) <= 75);
        result.Should().Contain("\r\n ");
    }
}
