using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Inbox;

namespace DomusMind.Application.Features.Inbox.Decide;

public sealed record MarkInboxProposalAppliedCommand(Guid InboxItemId, Guid ProposalId, Guid ResultId, Guid UserId) : ICommand<InboxItemModel>;
