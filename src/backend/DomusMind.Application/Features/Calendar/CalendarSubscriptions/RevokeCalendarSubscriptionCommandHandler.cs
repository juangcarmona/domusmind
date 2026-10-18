using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Domain.Calendar;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Calendar.CalendarSubscriptions;

public sealed class RevokeCalendarSubscriptionCommandHandler(IDomusMindDbContext db, IEventLogWriter eventLog, TimeProvider clock)
    : ICommandHandler<RevokeCalendarSubscriptionCommand, bool>
{
    public async Task<bool> Handle(RevokeCalendarSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var subscription = await db.Set<CalendarSubscription>().SingleOrDefaultAsync(
            x => x.Id == CalendarSubscriptionId.From(command.SubscriptionId) && x.OwnerUserId == command.RequestedByUserId,
            cancellationToken);
        if (subscription is null) return false;
        subscription.Revoke(clock.GetUtcNow().UtcDateTime);
        await eventLog.WriteAsync([subscription], cancellationToken);
        return true;
    }
}
