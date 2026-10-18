using System.Text.Json;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Features.Calendar.GetFamilyPlans;
using DomusMind.Application.Features.Family.GetHouseholdTimeline;
using DomusMind.Application.Features.Responsibilities.GetResponsibilityVisibility;

namespace DomusMind.Application.Features.Chats.Assistant;

public sealed class AssistantToolRouter(IQueryDispatcher queries)
    : IAssistantToolRouter
{
    public async Task<string> GetAuthorizedContextAsync(AssistantIntent intent, Guid familyId, Guid userId,
        CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        object? result = intent switch
        {
            AssistantIntent.Responsibilities => await queries.Dispatch(
                new GetResponsibilityVisibilityQuery(familyId, userId), cancellationToken),
            AssistantIntent.HouseholdTimeline or AssistantIntent.TasksDueSoon => await queries.Dispatch(
                new GetHouseholdTimelineQuery(familyId, userId), cancellationToken),
            _ => await queries.Dispatch(new GetFamilyPlansQuery(familyId, null, today, today.AddDays(90), userId),
                cancellationToken)
        };

        if (intent == AssistantIntent.TasksDueSoon && result is DomusMind.Contracts.Family.HouseholdTimelineResponse timeline)
            result = timeline.Entries.Where(x => x.EntryType == "Task").ToArray();

        return JsonSerializer.Serialize(result);
    }
}
