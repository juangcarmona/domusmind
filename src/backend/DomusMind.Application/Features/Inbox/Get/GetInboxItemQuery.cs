using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Inbox;

namespace DomusMind.Application.Features.Inbox.Get;

public sealed record GetInboxItemQuery(Guid InboxItemId, Guid UserId) : IQuery<InboxItemModel?>;
