using System.Text.Json;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Calendar.GetFamilyPlans;
using DomusMind.Application.Features.Family.GetHouseholdTimeline;
using DomusMind.Application.Features.Responsibilities.GetResponsibilityVisibility;
using DomusMind.Contracts.Calendar;
using DomusMind.Contracts.Family;
using DomusMind.Domain.Calendar;
using DomusMind.Domain.Family;
using DomusMind.Domain.Lists;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Chats.Assistant;

public sealed class AssistantToolRouter(IQueryDispatcher queries, IDomusMindDbContext db,
    IFamilyAuthorizationService authorization) : IAssistantToolRouter
{
    public async Task<string> GetAuthorizedContextAsync(AssistantIntent intent, Guid familyId, Guid userId,
        CancellationToken cancellationToken)
    {
        if (!await authorization.CanAccessFamilyAsync(userId, familyId, cancellationToken))
            throw new UnauthorizedAccessException();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        object result = intent switch
        {
            AssistantIntent.Responsibilities => await queries.Dispatch(
                new GetResponsibilityVisibilityQuery(familyId, userId), cancellationToken),
            AssistantIntent.HouseholdTimeline or AssistantIntent.TasksDueSoon => await queries.Dispatch(
                new GetHouseholdTimelineQuery(familyId, userId), cancellationToken),
            AssistantIntent.Lists => await GetListsAsync(familyId, cancellationToken),
            _ => await queries.Dispatch(new GetFamilyPlansQuery(familyId, null, today, today.AddDays(90), userId),
                cancellationToken)
        };

        if (intent == AssistantIntent.TasksDueSoon && result is HouseholdTimelineResponse timeline)
            result = timeline.Entries.Where(x => x.EntryType == "Task").ToArray();
        return JsonSerializer.Serialize(result);
    }

    public async Task<bool> IsProposalTargetAuthorizedAsync(AssistantActionProposal proposal, Guid familyId,
        Guid userId, CancellationToken cancellationToken)
    {
        if (!await authorization.CanAccessFamilyAsync(userId, familyId, cancellationToken)) return false;
        if (proposal.Type == "CreatePlan") return true;
        if (!Guid.TryParse(proposal.TargetId, out var targetId)) return false;

        if (proposal.Type == "UpdatePlan")
            return await db.Set<CalendarEvent>().AsNoTracking().AnyAsync(x => x.Id.Value == targetId
                && x.FamilyId == FamilyId.From(familyId) && x.Status != EventStatus.Cancelled, cancellationToken);

        if (proposal.Type is "AddListItem" or "UpdateListItem")
        {
            var list = await db.Set<SharedList>().AsNoTracking().Include(x => x.Items)
                .SingleOrDefaultAsync(x => x.Id.Value == targetId && x.FamilyId == FamilyId.From(familyId)
                    && !x.IsArchived, cancellationToken);
            if (list is null) return false;
            return proposal.Type == "AddListItem"
                || Guid.TryParse(proposal.ItemId, out var itemId) && list.Items.Any(x => x.Id.Value == itemId);
        }

        return false;
    }

    private async Task<object> GetListsAsync(Guid familyId, CancellationToken ct)
    {
        var lists = await db.Set<SharedList>().AsNoTracking().Include(x => x.Items)
            .Where(x => x.FamilyId == FamilyId.From(familyId) && !x.IsArchived)
            .OrderBy(x => x.CreatedAtUtc).ToListAsync(ct);
        return lists.Select(x => new
        {
            listId = x.Id.Value,
            name = x.Name.Value,
            items = x.Items.OrderBy(i => i.Order).Select(i => new
            {
                itemId = i.Id.Value,
                name = i.Name.Value,
                quantity = i.Quantity,
                note = i.Note,
                checkedState = i.Checked
            })
        }).ToArray();
    }
}
