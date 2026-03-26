using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.SharedLists.Events;

public sealed record SharedListItemRemoved(
    Guid EventId,
    Guid SharedListId,
    Guid ItemId,
    DateTime OccurredAtUtc) : IDomainEvent;
