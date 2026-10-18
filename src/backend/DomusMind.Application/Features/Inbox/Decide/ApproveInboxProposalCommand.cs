using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Inbox;

namespace DomusMind.Application.Features.Inbox.Decide;

public sealed record ApproveInboxProposalCommand(Guid InboxItemId, Guid ProposalId, Guid UserId) : ICommand<InboxItemModel>;
