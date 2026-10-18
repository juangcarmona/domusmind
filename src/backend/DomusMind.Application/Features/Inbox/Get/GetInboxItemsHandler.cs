using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Inbox;
using DomusMind.Domain.Family;
using DomusMind.Domain.Inbox;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Inbox.Get;

public sealed class GetInboxItemsHandler(IDomusMindDbContext db, IFamilyAuthorizationService auth)
    : IQueryHandler<GetInboxItemsQuery, IReadOnlyList<InboxItemModel>>
{
    public async Task<IReadOnlyList<InboxItemModel>> Handle(GetInboxItemsQuery q, CancellationToken ct)
    {
        if (!await auth.CanAccessFamilyAsync(q.UserId, q.FamilyId, ct)) throw new UnauthorizedAccessException();
        var familyId = FamilyId.From(q.FamilyId);
        var items = await db.Set<InboxItem>().AsNoTracking().Where(x => x.FamilyId == familyId)
            .OrderByDescending(x => x.CreatedAtUtc).ToListAsync(ct);
        return items.Select(InboxMapping.Map).ToArray();
    }
}
