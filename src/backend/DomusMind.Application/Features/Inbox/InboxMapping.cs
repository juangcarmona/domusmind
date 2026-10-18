using DomusMind.Contracts.Inbox;
using DomusMind.Domain.Inbox;

namespace DomusMind.Application.Features.Inbox;

internal static class InboxMapping
{
    public static InboxItemModel Map(InboxItem x) => new(x.Id.Value, x.FamilyId.Value, x.CreatedByUserId,
        x.RawText, x.CreatedAtUtc, x.Proposals.Select(p => new InboxProposalModel(p.Id, p.Type, p.Title,
            p.Description, p.Date, p.Time, p.EndDate, p.EndTime, p.DueDate, p.Status,
            p.UnresolvedDetails, p.ResultId, p.TargetPlanTitle, p.TargetPlanId,
            p.ReminderMinutesBefore, p.TargetListId, p.TargetItemId, p.Quantity)).ToArray(),
        x.SourceChatMessageId, x.ProviderName, x.ProviderHosting, x.ProviderFallbackOccurred);
}
