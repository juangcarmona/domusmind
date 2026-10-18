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
    public IReadOnlyCollection<InboxProposal> Proposals => _proposals.AsReadOnly();

    private InboxItem(InboxItemId id, FamilyId familyId, Guid createdByUserId, string rawText, DateTime createdAtUtc)
        : base(id)
    { FamilyId = familyId; CreatedByUserId = createdByUserId; RawText = rawText; CreatedAtUtc = createdAtUtc; Version = Guid.NewGuid(); }

    private InboxItem() : base(default) { }

    public static InboxItem Capture(InboxItemId id, FamilyId familyId, Guid userId, string text, DateTime now)
    {
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Inbox note is required.", nameof(text));
        if (text.Length > 4000) throw new ArgumentException("Inbox note cannot exceed 4000 characters.", nameof(text));
        var item = new InboxItem(id, familyId, userId, text.Trim(), now);
        item.RaiseDomainEvent(new InboxItemCaptured(Guid.NewGuid(), id.Value, familyId.Value, userId, now));
        return item;
    }

    public void SetProposals(IEnumerable<InboxProposal> proposals)
    {
        if (_proposals.Count > 0) throw new InvalidOperationException("This Inbox item has already been interpreted.");
        _proposals.AddRange(proposals.Take(8));
        Version = Guid.NewGuid();
        RaiseDomainEvent(new InboxItemInterpreted(Guid.NewGuid(), Id.Value, DateTime.UtcNow));
    }

    public void BeginApproval(Guid proposalId, Guid userId)
    {
        Find(proposalId).BeginApproval();
        Version = Guid.NewGuid();
        RaiseDomainEvent(new InboxProposalApproved(Guid.NewGuid(), Id.Value, proposalId, FamilyId.Value, userId, DateTime.UtcNow));
    }

    public void Refine(Guid proposalId, string title, string? description, string? date, string? time,
        string? endDate, string? endTime, string? dueDate, Guid? targetPlanId = null,
        int? reminderMinutesBefore = null)
    {
        Find(proposalId).Refine(title, description, date, time, endDate, endTime, dueDate,
            targetPlanId, reminderMinutesBefore);
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
