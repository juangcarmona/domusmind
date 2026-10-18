using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Inbox.Events;

public sealed record InboxItemCaptured(Guid EventId, Guid InboxItemId, Guid FamilyId, Guid UserId, DateTime OccurredAtUtc) : IDomainEvent;
