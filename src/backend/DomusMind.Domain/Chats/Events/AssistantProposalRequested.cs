using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Chats.Events;

public sealed record AssistantProposalRequested(
    Guid EventId,
    Guid AssistantMessageId,
    Guid SourceMessageId,
    Guid ChatThreadId,
    Guid FamilyId,
    Guid RequestingUserId,
    string ProviderName,
    string ProviderHosting,
    bool FallbackOccurred,
    AssistantProposalDraft Proposal,
    DateTime OccurredAtUtc) : IDomainEvent;
