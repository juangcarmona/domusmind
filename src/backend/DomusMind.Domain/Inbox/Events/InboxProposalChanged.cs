using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Inbox.Events;

public sealed record InboxProposalChanged(Guid EventId, Guid InboxItemId, Guid ProposalId,
    string Decision, DateTime OccurredAtUtc) : IDomainEvent;
