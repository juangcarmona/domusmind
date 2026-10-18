using System.Security.Cryptography;
using System.Text;
using DomusMind.Application.Features.Calendar.CalendarSubscriptions;

namespace DomusMind.Api.Calendar;

internal static class CalendarSubscriptionIcsFormatter
{
    public static string Format(CalendarSubscriptionFeed feed)
    {
        var lines = new List<string>
        {
            "BEGIN:VCALENDAR", "VERSION:2.0", "PRODID:-//DomusMind//Calendar Subscriptions//EN",
            "CALSCALE:GREGORIAN", "METHOD:PUBLISH", $"X-WR-CALNAME:{Escape(feed.Label)}"
        };
        foreach (var plan in feed.Plans)
        {
            var opaqueId = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(
                $"{feed.SubscriptionId:N}:{plan.PlanId:N}"))).ToLowerInvariant();
            lines.AddRange(["BEGIN:VEVENT", $"UID:{opaqueId}@domusmind",
                $"DTSTAMP:{plan.CreatedAtUtc.ToUniversalTime():yyyyMMdd'T'HHmmss'Z'}",
                $"SUMMARY:{Escape(plan.Title)}"]);
            if (plan.IsAllDay)
            {
                var inclusiveEnd = plan.EndDate ?? plan.StartDate;
                lines.Add($"DTSTART;VALUE=DATE:{plan.StartDate:yyyyMMdd}");
                lines.Add($"DTEND;VALUE=DATE:{inclusiveEnd.AddDays(1):yyyyMMdd}");
            }
            else
            {
                lines.Add($"DTSTART:{plan.StartDate:yyyyMMdd}T{plan.StartTime:HHmmss}");
                if (plan.EndDate.HasValue && plan.EndTime.HasValue)
                    lines.Add($"DTEND:{plan.EndDate:yyyyMMdd}T{plan.EndTime:HHmmss}");
            }
            if (plan.IsCancelled) lines.Add("STATUS:CANCELLED");
            lines.Add("END:VEVENT");
        }
        lines.Add("END:VCALENDAR");
        return string.Join("\r\n", lines.SelectMany(FoldLine)) + "\r\n";
    }

    private static IEnumerable<string> FoldLine(string line)
    {
        var segment = new StringBuilder();
        var byteCount = 0;
        foreach (var rune in line.EnumerateRunes())
        {
            if (byteCount + rune.Utf8SequenceLength > 75)
            {
                yield return segment.ToString();
                segment.Clear().Append(' ');
                byteCount = 1;
            }
            segment.Append(rune.ToString());
            byteCount += rune.Utf8SequenceLength;
        }
        yield return segment.ToString();
    }

    private static string Escape(string value) => value.Replace("\\", "\\\\")
        .Replace(";", "\\;").Replace(",", "\\,").Replace("\r\n", "\\n")
        .Replace("\n", "\\n").Replace("\r", "\\n");
}
