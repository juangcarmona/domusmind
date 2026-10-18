using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Inbox;

namespace DomusMind.Application.Features.Inbox.Decide;

public sealed record RefineInboxProposalCommand(Guid InboxItemId, Guid ProposalId, Guid UserId,
    RefineInboxProposalRequest Changes) : ICommand<InboxItemModel>;
