using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Inbox;
using DomusMind.Domain.Inbox;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Inbox.Interpret;

public sealed class InterpretInboxItemHandler(IDomusMindDbContext db, IEventLogWriter events,
    IFamilyAuthorizationService auth, IInboxInterpreter interpreter)
    : ICommandHandler<InterpretInboxItemCommand, InboxItemModel>
{
    public async Task<InboxItemModel> Handle(InterpretInboxItemCommand c, CancellationToken ct)
    {
        var itemId = InboxItemId.From(c.InboxItemId);
        var item = await db.Set<InboxItem>().Include(x => x.Proposals).SingleOrDefaultAsync(x => x.Id == itemId, ct)
            ?? throw new KeyNotFoundException("Inbox item was not found.");
        if (!await auth.CanAccessFamilyAsync(c.UserId, item.FamilyId.Value, ct)) throw new UnauthorizedAccessException();
        var proposals = (await interpreter.InterpretAsync(item.RawText, ct)).Select(p => InboxProposal.Create(
            p.Type, p.Title, p.Description, p.Date, p.Time, p.EndDate, p.EndTime, p.DueDate,
            p.UnresolvedDetails, p.TargetPlanTitle, p.ReminderMinutesBefore));
        item.SetProposals(proposals);
        await events.WriteAsync([item], ct); item.ClearDomainEvents();
        return InboxMapping.Map(item);
    }
}
