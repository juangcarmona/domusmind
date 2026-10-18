using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Inbox;
using DomusMind.Domain.Family;
using DomusMind.Domain.Inbox;

namespace DomusMind.Application.Features.Inbox.Capture;

public sealed class CaptureInboxItemHandler(IDomusMindDbContext db, IEventLogWriter events,
    IFamilyAuthorizationService auth) : ICommandHandler<CaptureInboxItemCommand, InboxItemModel>
{
    public async Task<InboxItemModel> Handle(CaptureInboxItemCommand c, CancellationToken ct)
    {
        if (!await auth.CanAccessFamilyAsync(c.UserId, c.FamilyId, ct)) throw new UnauthorizedAccessException();
        var item = InboxItem.Capture(InboxItemId.New(), FamilyId.From(c.FamilyId), c.UserId, c.Text, DateTime.UtcNow);
        db.Set<InboxItem>().Add(item);
        await events.WriteAsync([item], ct);
        item.ClearDomainEvents();
        return InboxMapping.Map(item);
    }
}
