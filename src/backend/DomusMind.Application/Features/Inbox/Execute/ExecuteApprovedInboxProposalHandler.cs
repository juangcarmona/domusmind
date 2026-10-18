using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Features.Calendar.ScheduleEvent;
using DomusMind.Application.Features.Calendar.AddReminder;
using DomusMind.Application.Features.Calendar.AddTimelineNote;
using DomusMind.Application.Features.Inbox.Decide;
using DomusMind.Application.Features.Tasks.CreateTask;
using DomusMind.Domain.Inbox.Events;
using Microsoft.EntityFrameworkCore;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Domain.Inbox;

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
        else if (proposal.Type == "Reminder")
        {
            if (!proposal.TargetPlanId.HasValue || !proposal.ReminderMinutesBefore.HasValue)
                throw new InvalidOperationException("Refine this Reminder with a Plan and an offset before approving it.");
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
