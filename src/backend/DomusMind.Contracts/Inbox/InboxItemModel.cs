namespace DomusMind.Contracts.Inbox;

public sealed record InboxItemModel(Guid InboxItemId, Guid FamilyId, Guid CreatedByUserId,
    string RawText, DateTime CreatedAtUtc, IReadOnlyList<InboxProposalModel> Proposals,
    Guid? SourceChatMessageId = null, string? ProviderName = null, string? ProviderHosting = null,
    bool ProviderFallbackOccurred = false);
