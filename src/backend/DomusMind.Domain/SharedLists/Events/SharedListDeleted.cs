using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.SharedLists.Events;

public sealed record SharedListDeleted(
    Guid EventId,
    Guid SharedListId,
    DateTime OccurredAtUtc) : IDomainEvent;
