using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Inbox.Events;

public sealed record InboxItemInterpreted(Guid EventId, Guid InboxItemId, DateTime OccurredAtUtc) : IDomainEvent;
