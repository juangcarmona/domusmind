using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Family.Events;

public sealed record MemberAccountLinked(
    Guid EventId,
    Guid FamilyId,
    Guid MemberId,
    Guid AuthUserId,
    DateTime OccurredAtUtc) : IDomainEvent;
