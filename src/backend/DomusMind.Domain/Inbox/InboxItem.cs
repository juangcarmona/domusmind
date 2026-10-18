using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;
using DomusMind.Domain.Inbox.Events;

namespace DomusMind.Domain.Inbox;

public sealed class InboxItem : AggregateRoot<InboxItemId>
{
    private readonly List<InboxProposal> _proposals = [];
    public FamilyId FamilyId { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public Guid Version { get; private set; }
    public string RawText { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }
    public Guid? SourceChatMessageId { get; private set; }
    public string? ProviderName { get; private set; }
    public string? ProviderHosting { get; private set; }
    public bool ProviderFallbackOccurred { get; private set; }
    public IReadOnlyCollection<InboxProposal> Proposals => _proposals.AsReadOnly();

    private InboxItem(InboxItemId id, FamilyId familyId, Guid createdByUserId, string rawText,
        DateTime createdAtUtc, Guid? sourceChatMessageId = null, string? providerName = null,
        string? providerHosting = null, bool providerFallbackOccurred = false)
        : base(id)
    {
        FamilyId = familyId; CreatedByUserId = createdByUserId; RawText = rawText;
        CreatedAtUtc = createdAtUtc; SourceChatMessageId = sourceChatMessageId;
        ProviderName = providerName; ProviderHosting = providerHosting;
        ProviderFallbackOccurred = providerFallbackOccurred; Version = Guid.NewGuid();
    }

    private InboxItem() : base(default) { }

    public static InboxItem Capture(InboxItemId id, FamilyId familyId, Guid userId, string text, DateTime now)
    {
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Inbox note is required.", nameof(text));
        if (text.Length > 4000) throw new ArgumentException("Inbox note cannot exceed 4000 characters.", nameof(text));
        var item = new InboxItem(id, familyId, userId, text.Trim(), now);
        item.RaiseDomainEvent(new InboxItemCaptured(Guid.NewGuid(), id.Value, familyId.Value, userId, now));
        return item;
    }

    public static InboxItem CaptureAssistantProposal(InboxItemId id, FamilyId familyId, Guid userId,
        string text, DateTime now, Guid sourceChatMessageId, string providerName,
        string providerHosting, bool fallbackOccurred)
    {
        if (sourceChatMessageId == Guid.Empty) throw new ArgumentException("Source chat message is required.", nameof(sourceChatMessageId));
        if (string.IsNullOrWhiteSpace(providerName) || string.IsNullOrWhiteSpace(providerHosting))
            throw new ArgumentException("Provider disclosure is required.", nameof(providerName));
        if (string.IsNullOrWhiteSpace(text) || text.Trim().Length > 4000)
            throw new ArgumentException("Inbox note must contain at most 4000 characters.", nameof(text));
        var item = new InboxItem(id, familyId, userId, text.Trim(), now, sourceChatMessageId,
            providerName, providerHosting, fallbackOccurred);
        item.RaiseDomainEvent(new InboxItemCaptured(Guid.NewGuid(), id.Value, familyId.Value,
            userId, now, sourceChatMessageId));
        return item;
    }

    public void SetProposals(IEnumerable<InboxProposal> proposals, string? providerName = null,
        string? providerHosting = null, bool? fallbackOccurred = null)
    {
        if (_proposals.Count > 0) throw new InvalidOperationException("This Inbox item has already been interpreted.");
        _proposals.AddRange(proposals.Take(8));
        if (!string.IsNullOrWhiteSpace(providerName)) ProviderName = providerName;
        if (!string.IsNullOrWhiteSpace(providerHosting)) ProviderHosting = providerHosting;
        if (fallbackOccurred.HasValue) ProviderFallbackOccurred = fallbackOccurred.Value;
        Version = Guid.NewGuid();
        RaiseDomainEvent(new InboxItemInterpreted(Guid.NewGuid(), Id.Value, DateTime.UtcNow,
            ProviderName, ProviderHosting, ProviderFallbackOccurred));
    }

    public void BeginApproval(Guid proposalId, Guid userId)
    {
        Find(proposalId).BeginApproval();
        Version = Guid.NewGuid();
        RaiseDomainEvent(new InboxProposalApproved(Guid.NewGuid(), Id.Value, proposalId, FamilyId.Value, userId, DateTime.UtcNow));
    }

    public void Refine(Guid proposalId, string title, string? description, string? date, string? time,
        string? endDate, string? endTime, string? dueDate, Guid? targetPlanId = null,
        int? reminderMinutesBefore = null, Guid? targetListId = null, Guid? targetItemId = null,
        string? quantity = null)
    {
        Find(proposalId).Refine(title, description, date, time, endDate, endTime, dueDate,
            targetPlanId, reminderMinutesBefore, targetListId, targetItemId, quantity);
        Version = Guid.NewGuid();
        RaiseDomainEvent(new InboxProposalChanged(Guid.NewGuid(), Id.Value, proposalId, "Refined", DateTime.UtcNow));
    }

    public void Reject(Guid proposalId)
    {
        Find(proposalId).Reject();
        Version = Guid.NewGuid();
        RaiseDomainEvent(new InboxProposalChanged(Guid.NewGuid(), Id.Value, proposalId, "Rejected", DateTime.UtcNow));
    }
    public void MarkApplied(Guid proposalId, Guid resultId)
    {
        Find(proposalId).MarkApplied(resultId);
        Version = Guid.NewGuid();
        RaiseDomainEvent(new InboxProposalApplied(Guid.NewGuid(), Id.Value, proposalId, resultId, DateTime.UtcNow));
    }

    private InboxProposal Find(Guid id) => _proposals.SingleOrDefault(x => x.Id == id)
        ?? throw new KeyNotFoundException("Inbox proposal was not found.");
}
