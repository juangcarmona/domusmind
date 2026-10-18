using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Features.Calendar.ScheduleEvent;
using DomusMind.Application.Features.Calendar.AddReminder;
using DomusMind.Application.Features.Calendar.AddTimelineNote;
using DomusMind.Application.Features.Inbox.Decide;
using DomusMind.Application.Features.Tasks.CreateTask;
using DomusMind.Application.Features.Lists.AddItemToList;
using DomusMind.Application.Features.Lists.UpdateListItem;
using DomusMind.Application.Features.Calendar.RescheduleEvent;
using DomusMind.Application.Temporal;
using DomusMind.Domain.Inbox.Events;
using Microsoft.EntityFrameworkCore;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Domain.Inbox;
using DomusMind.Domain.Family;

namespace DomusMind.Application.Features.Inbox.Execute;

public sealed class ExecuteApprovedInboxProposalHandler(ICommandDispatcher commands, IDomusMindDbContext db)
    : IDomainEventHandler<InboxProposalApproved>
{
    public async Task Handle(InboxProposalApproved e, CancellationToken ct)
    {
        var item = await db.Set<InboxItem>().Include(x => x.Proposals)
            .SingleOrDefaultAsync(x => x.Id == InboxItemId.From(e.InboxItemId), ct)
            ?? throw new InvalidOperationException("Approved Inbox item was not found.");
        var proposal = item.Proposals.SingleOrDefault(x => x.Id == e.ProposalId)
            ?? throw new InvalidOperationException("Approved Inbox proposal was not found.");
        if (proposal.Status != "Executing")
            throw new InvalidOperationException($"Approved Inbox proposal has status '{proposal.Status}' instead of 'Executing'.");

        Guid resultId;
        if (proposal.Type == "Task")
        {
            var result = await commands.Dispatch(new CreateTaskCommand(proposal.Title, e.FamilyId,
                proposal.Description, proposal.DueDate, null, null, null, e.UserId), ct);
            resultId = result.TaskId;
        }
        else if (proposal.Type == "Plan")
        {
            if (proposal.Date is null || proposal.EndDate is null)
                throw new InvalidOperationException("Refine this Plan with a start and end date before approving it.");
            var result = await commands.Dispatch(new ScheduleEventCommand(proposal.Title, e.FamilyId,
                proposal.Date, proposal.Time, proposal.EndDate, proposal.EndTime, proposal.Description,
                null, null, null, e.UserId), ct);
            resultId = result.CalendarEventId;
        }
        else if (proposal.Type == "PlanUpdate")
        {
            if (!proposal.TargetPlanId.HasValue)
                throw new InvalidOperationException("Refine this Plan update with a Plan before approving it.");
            var plan = await db.Set<Domain.Calendar.CalendarEvent>().AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id.Value == proposal.TargetPlanId.Value
                    && x.FamilyId == FamilyId.From(e.FamilyId) && x.Status != Domain.Calendar.EventStatus.Cancelled, ct)
                ?? throw new InvalidOperationException("The Plan selected for update no longer exists.");
            var (currentDate, currentTime, currentEndDate, currentEndTime) = TemporalParser.FormatEventTime(plan.Time);
            var result = await commands.Dispatch(new RescheduleEventCommand(proposal.TargetPlanId.Value,
                proposal.Date ?? currentDate, proposal.Time ?? currentTime,
                proposal.EndDate ?? currentEndDate ?? currentDate, proposal.EndTime ?? currentEndTime,
                proposal.Title, proposal.Description, null, e.UserId), ct);
            resultId = result.CalendarEventId;
        }
        else if (proposal.Type == "ListItemAdd")
        {
            if (!proposal.TargetListId.HasValue)
                throw new InvalidOperationException("Refine this List Item proposal with a List before approving it.");
            var listExists = await db.Set<Domain.Lists.SharedList>().AsNoTracking()
                .AnyAsync(x => x.Id.Value == proposal.TargetListId.Value
                    && x.FamilyId == FamilyId.From(e.FamilyId) && !x.IsArchived, ct);
            if (!listExists)
                throw new InvalidOperationException("The List selected for this proposal is no longer available.");
            var result = await commands.Dispatch(new AddItemToListCommand(proposal.TargetListId.Value,
                proposal.Title, proposal.Quantity, proposal.Description, null, e.UserId), ct);
            resultId = result.ItemId;
        }
        else if (proposal.Type == "ListItemUpdate")
        {
            if (!proposal.TargetListId.HasValue || !proposal.TargetItemId.HasValue)
                throw new InvalidOperationException("Refine this List Item update with a List Item before approving it.");
            var list = await db.Set<Domain.Lists.SharedList>().AsNoTracking().Include(x => x.Items)
                .SingleOrDefaultAsync(x => x.Id.Value == proposal.TargetListId.Value
                    && x.FamilyId == FamilyId.From(e.FamilyId) && !x.IsArchived, ct)
                ?? throw new InvalidOperationException("The List selected for update no longer exists.");
            var targetItem = list.Items.SingleOrDefault(x => x.Id.Value == proposal.TargetItemId.Value)
                ?? throw new InvalidOperationException("The List Item selected for update no longer exists.");
            var result = await commands.Dispatch(new UpdateListItemCommand(proposal.TargetListId.Value,
                proposal.TargetItemId.Value, proposal.Title, proposal.Quantity ?? targetItem.Quantity,
                proposal.Description ?? targetItem.Note, e.UserId), ct);
            resultId = result.ItemId;
        }
        else if (proposal.Type == "Reminder")
        {
            if (!proposal.TargetPlanId.HasValue || !proposal.ReminderMinutesBefore.HasValue)
                throw new InvalidOperationException("Refine this Reminder with a Plan and an offset before approving it.");
            var planExists = await db.Set<Domain.Calendar.CalendarEvent>().AsNoTracking()
                .AnyAsync(x => x.Id.Value == proposal.TargetPlanId.Value
                    && x.FamilyId == FamilyId.From(e.FamilyId) && x.Status != Domain.Calendar.EventStatus.Cancelled, ct);
            if (!planExists)
                throw new InvalidOperationException("The Plan selected for this Reminder is no longer available.");
            await commands.Dispatch(new AddReminderCommand(proposal.TargetPlanId.Value,
                proposal.ReminderMinutesBefore.Value, e.UserId), ct);
            resultId = proposal.TargetPlanId.Value;
        }
        else
        {
            if (!DateOnly.TryParseExact(proposal.Date, "yyyy-MM-dd", out var date))
                throw new InvalidOperationException("Refine this Timeline Note with a date before approving it.");
            var result = await commands.Dispatch(new AddTimelineNoteCommand(e.FamilyId,
                proposal.Title, date, e.UserId), ct);
            resultId = result.TimelineNoteId;
        }

        await commands.Dispatch(new MarkInboxProposalAppliedCommand(e.InboxItemId, e.ProposalId,
            resultId, e.UserId), ct);
    }
}
