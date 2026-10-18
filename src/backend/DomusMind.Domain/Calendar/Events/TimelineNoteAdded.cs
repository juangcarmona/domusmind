using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Calendar.Events;

public sealed record TimelineNoteAdded(Guid EventId, Guid TimelineNoteId, Guid FamilyId,
    DateOnly Date, string Text, DateTime OccurredAtUtc) : IDomainEvent;
