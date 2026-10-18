using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Chats.Events;

public sealed record ChatMessagePosted(Guid EventId, Guid ChatMessageId, Guid ChatThreadId,
    Guid FamilyId, string AuthorType, Guid? AuthorMemberId, DateTime OccurredAtUtc) : IDomainEvent;
