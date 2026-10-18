using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Domain.Chats;
using DomusMind.Domain.Chats.Events;
using DomusMind.Domain.Chats.ValueObjects;
using DomusMind.Domain.Family;
using DomusMind.Domain.Inbox;
using DomusMind.Domain.Lists;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Inbox.FromAssistant;

public sealed class StageAssistantProposalHandler(IDomusMindDbContext db, IEventLogWriter events,
    IFamilyAuthorizationService authorization) : IDomainEventHandler<AssistantProposalRequested>
{
    public async Task Handle(AssistantProposalRequested request, CancellationToken ct)
    {
        var thread = await db.Set<ChatThread>().AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == ChatThreadId.From(request.ChatThreadId), ct);
        if (thread is null || thread.Kind != "Household") return;
        if (!await authorization.CanAccessFamilyAsync(request.RequestingUserId, request.FamilyId, ct))
            throw new UnauthorizedAccessException();

        var source = await db.Set<ChatMessage>().AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == ChatMessageId.From(request.SourceMessageId), ct)
            ?? throw new InvalidOperationException("The source Chat message for an assistant proposal was not found.");
        var proposal = await BuildProposalAsync(request, ct);
        var item = InboxItem.CaptureAssistantProposal(InboxItemId.New(), FamilyId.From(request.FamilyId),
            request.RequestingUserId, source.Text, request.OccurredAtUtc, source.Id.Value,
            request.ProviderName, request.ProviderHosting, request.FallbackOccurred);
        item.SetProposals([proposal]);
        db.Set<InboxItem>().Add(item);
        await events.WriteAsync([item], ct);
        item.ClearDomainEvents();
    }

    private async Task<InboxProposal> BuildProposalAsync(AssistantProposalRequested request, CancellationToken ct)
    {
        var p = request.Proposal;
        var type = p.Type switch
        {
            "CreatePlan" => "Plan",
            "UpdatePlan" => "PlanUpdate",
            "AddListItem" => "ListItemAdd",
            "UpdateListItem" => "ListItemUpdate",
            _ => throw new InvalidOperationException("Unsupported assistant proposal type.")
        };

        var title = p.Title;
        var description = p.Description;
        string? targetPlanTitle = null;
        Guid? targetPlanId = null;
        Guid? targetListId = null;
        Guid? targetItemId = null;
        if (type == "PlanUpdate")
        {
            var planId = p.TargetId ?? throw new InvalidOperationException("A Plan update proposal needs a target Plan.");
            targetPlanId = planId;
            var plan = await db.Set<Domain.Calendar.CalendarEvent>().AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == Domain.Calendar.CalendarEventId.From(planId)
                    && x.FamilyId == FamilyId.From(request.FamilyId), ct)
                ?? throw new InvalidOperationException("The Plan selected for an assistant proposal no longer exists.");
            title ??= plan.Title.Value;
            description ??= plan.Description;
            targetPlanTitle = plan.Title.Value;
        }
        else if (type is "ListItemAdd" or "ListItemUpdate")
        {
            var listId = p.TargetId ?? throw new InvalidOperationException("A List Item proposal needs a target List.");
            targetListId = listId;
            description = p.Note ?? p.Description;
            if (type == "ListItemUpdate")
            {
                targetItemId = p.ItemId;
                var list = await db.Set<SharedList>().AsNoTracking().Include(x => x.Items)
                    .SingleOrDefaultAsync(x => x.Id == ListId.From(listId)
                        && x.FamilyId == FamilyId.From(request.FamilyId), ct)
                    ?? throw new InvalidOperationException("The List selected for an assistant proposal no longer exists.");
                var current = list.Items.SingleOrDefault(x => x.Id.Value == targetItemId)
                    ?? throw new InvalidOperationException("The List Item selected for an assistant proposal no longer exists.");
                title ??= current.Name.Value;
                description ??= current.Note;
            }
        }

        return InboxProposal.Create(type, title ?? throw new InvalidOperationException("A complete assistant proposal needs a title."),
            description, p.Date, p.Time, p.EndDate, p.EndTime, null, null,
            targetPlanTitle, null, targetPlanId, targetListId, targetItemId, p.Quantity);
    }
}
