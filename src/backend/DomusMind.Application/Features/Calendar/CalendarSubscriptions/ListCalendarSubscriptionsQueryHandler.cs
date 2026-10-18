using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Calendar;
using DomusMind.Domain.Calendar;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Calendar.CalendarSubscriptions;

public sealed class ListCalendarSubscriptionsQueryHandler(IDomusMindDbContext db, IFamilyAuthorizationService authorization)
    : IQueryHandler<ListCalendarSubscriptionsQuery, IReadOnlyCollection<CalendarSubscriptionSummary>>
{
    public async Task<IReadOnlyCollection<CalendarSubscriptionSummary>> Handle(ListCalendarSubscriptionsQuery query,
        CancellationToken cancellationToken)
    {
        if (!await authorization.CanAccessFamilyAsync(query.RequestedByUserId, query.FamilyId, cancellationToken))
            throw new UnauthorizedAccessException("Access to this family is denied.");
        return await db.Set<CalendarSubscription>().AsNoTracking()
            .Where(x => x.FamilyId == query.FamilyId && x.OwnerUserId == query.RequestedByUserId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new CalendarSubscriptionSummary(x.Id.Value, x.Label, x.MemberId,
                x.CreatedAtUtc, x.ExpiresAtUtc, x.RevokedAtUtc, x.LastAccessedAtUtc, x.AccessCount))
            .ToListAsync(cancellationToken);
    }
}
