using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.SharedLists.Events;

public sealed record SharedListRenamed(
    Guid EventId,
    Guid SharedListId,
    string NewName,
    DateTime OccurredAtUtc) : IDomainEvent;
