using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Inbox;
using DomusMind.Domain.Inbox;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Inbox.Get;

public sealed class GetInboxItemHandler(IDomusMindDbContext db, IFamilyAuthorizationService auth)
    : IQueryHandler<GetInboxItemQuery, InboxItemModel?>
{
    public async Task<InboxItemModel?> Handle(GetInboxItemQuery q, CancellationToken ct)
    {
        var itemId = InboxItemId.From(q.InboxItemId);
        var item = await db.Set<InboxItem>().AsNoTracking().SingleOrDefaultAsync(x => x.Id == itemId, ct);
        if (item is null) return null;
        if (!await auth.CanAccessFamilyAsync(q.UserId, item.FamilyId.Value, ct)) throw new UnauthorizedAccessException();
        return InboxMapping.Map(item);
    }
}
