using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Chats.Events;

public sealed record ChatThreadCreated(Guid EventId, Guid ChatThreadId, Guid FamilyId,
    Guid CreatedByMemberId, DateTime OccurredAtUtc) : IDomainEvent;
