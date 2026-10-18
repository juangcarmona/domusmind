using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Inbox;
using DomusMind.Domain.Inbox;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Inbox.Decide;

public sealed class DecideInboxProposalHandler(IDomusMindDbContext db, IEventLogWriter events,
    IFamilyAuthorizationService auth) :
    ICommandHandler<RefineInboxProposalCommand, InboxItemModel>,
    ICommandHandler<RejectInboxProposalCommand, InboxItemModel>,
    ICommandHandler<MarkInboxProposalAppliedCommand, InboxItemModel>
{
    public Task<InboxItemModel> Handle(RefineInboxProposalCommand c, CancellationToken ct) =>
        Update(c.InboxItemId, c.UserId, (x) => x.Refine(c.ProposalId, c.Changes.Title, c.Changes.Description,
            c.Changes.Date, c.Changes.Time, c.Changes.EndDate, c.Changes.EndTime, c.Changes.DueDate,
            c.Changes.TargetPlanId, c.Changes.ReminderMinutesBefore,
            c.Changes.TargetListId, c.Changes.TargetItemId, c.Changes.Quantity), ct);

    public Task<InboxItemModel> Handle(RejectInboxProposalCommand c, CancellationToken ct) =>
        Update(c.InboxItemId, c.UserId, x => x.Reject(c.ProposalId), ct);

    public Task<InboxItemModel> Handle(MarkInboxProposalAppliedCommand c, CancellationToken ct) =>
        Update(c.InboxItemId, c.UserId, x => x.MarkApplied(c.ProposalId, c.ResultId), ct);

    private async Task<InboxItemModel> Update(Guid id, Guid userId, Action<InboxItem> mutate, CancellationToken ct)
    {
        var itemId = InboxItemId.From(id);
        var item = await db.Set<InboxItem>().Include(x => x.Proposals).SingleOrDefaultAsync(x => x.Id == itemId, ct)
            ?? throw new KeyNotFoundException("Inbox item was not found.");
        if (!await auth.CanAccessFamilyAsync(userId, item.FamilyId.Value, ct)) throw new UnauthorizedAccessException();
        mutate(item);
        await events.WriteAsync([item], ct); item.ClearDomainEvents();
        return InboxMapping.Map(item);
    }
}
