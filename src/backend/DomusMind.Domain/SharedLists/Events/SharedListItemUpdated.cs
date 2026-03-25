using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.SharedLists.Events;

public sealed record SharedListItemUpdated(
    Guid EventId,
    Guid SharedListId,
    Guid ItemId,
    string NewName,
    DateTime OccurredAtUtc) : IDomainEvent;
