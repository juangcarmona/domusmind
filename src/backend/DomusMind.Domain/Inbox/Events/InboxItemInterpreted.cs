using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Inbox.Events;

public sealed record InboxItemInterpreted(Guid EventId, Guid InboxItemId, DateTime OccurredAtUtc,
    string? ProviderName = null, string? ProviderHosting = null, bool FallbackOccurred = false) : IDomainEvent;
