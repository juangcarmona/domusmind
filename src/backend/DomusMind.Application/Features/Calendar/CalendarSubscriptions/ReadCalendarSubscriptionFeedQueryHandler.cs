using System.Security.Cryptography;
using System.Text;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Domain.Calendar;
using DomusMind.Domain.Family;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Calendar.CalendarSubscriptions;

public sealed class ReadCalendarSubscriptionFeedQueryHandler(IDomusMindDbContext db, IEventLogWriter eventLog,
    ICalendarSubscriptionTokenGenerator tokens, TimeProvider clock)
    : IQueryHandler<ReadCalendarSubscriptionFeedQuery, CalendarSubscriptionFeed?>
{
    public async Task<CalendarSubscriptionFeed?> Handle(ReadCalendarSubscriptionFeedQuery query,
        CancellationToken cancellationToken)
    {
        var hash = tokens.Hash(query.Token);
        var subscription = await db.Set<CalendarSubscription>().SingleOrDefaultAsync(
            x => x.TokenHash == hash, cancellationToken);
        var now = clock.GetUtcNow().UtcDateTime;
        if (subscription is null || subscription.RevokedAtUtc.HasValue || now >= subscription.ExpiresAtUtc)
            return null;

        var plans = await db.Set<Domain.Calendar.CalendarEvent>().AsNoTracking()
            .Where(x => x.FamilyId == FamilyId.From(subscription.FamilyId))
            .OrderBy(x => x.Time.Date).ThenBy(x => x.Time.Time)
            .ToListAsync(cancellationToken);
        if (subscription.MemberId.HasValue)
            plans = plans.Where(x => x.ParticipantIds.Any(p => p.Value == subscription.MemberId.Value)).ToList();

        subscription.RecordAccess(now);
        await eventLog.WriteAsync([subscription], cancellationToken);

        return new(subscription.Id.Value, subscription.Label, plans.Select(x => new CalendarSubscriptionFeedPlan(
            x.Id.Value, x.Title.Value, x.Time.Date, x.Time.Time, x.Time.EndDate, x.Time.EndTime,
            x.Status == EventStatus.Cancelled, x.Time.Time is null, x.CreatedAtUtc)).ToArray());
    }
}
