using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Inbox.Events;

public sealed record InboxProposalApproved(Guid EventId, Guid InboxItemId, Guid ProposalId,
    Guid FamilyId, Guid UserId, DateTime OccurredAtUtc) : IDomainEvent;
