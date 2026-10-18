using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Inbox;
using DomusMind.Domain.Inbox;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Inbox.Decide;

public sealed class ApproveInboxProposalHandler(IDomusMindDbContext db, IEventLogWriter events,
    IFamilyAuthorizationService auth) : ICommandHandler<ApproveInboxProposalCommand, InboxItemModel>
{
    public async Task<InboxItemModel> Handle(ApproveInboxProposalCommand c, CancellationToken ct)
    {
        var item = await db.Set<InboxItem>().Include(x => x.Proposals)
            .SingleOrDefaultAsync(x => x.Id == InboxItemId.From(c.InboxItemId), ct)
            ?? throw new KeyNotFoundException("Inbox item was not found.");
        if (!await auth.CanAccessFamilyAsync(c.UserId, item.FamilyId.Value, ct)) throw new UnauthorizedAccessException();
        item.BeginApproval(c.ProposalId, c.UserId);
        await events.WriteAsync([item], ct);
        item.ClearDomainEvents();
        return InboxMapping.Map(item);
    }
}
