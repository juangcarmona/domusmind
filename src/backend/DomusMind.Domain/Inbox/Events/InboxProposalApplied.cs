using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Inbox.Events;

public sealed record InboxProposalApplied(Guid EventId, Guid InboxItemId, Guid ProposalId, Guid ResultId, DateTime OccurredAtUtc) : IDomainEvent;
