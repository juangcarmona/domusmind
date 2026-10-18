using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Inbox;

namespace DomusMind.Application.Features.Inbox.Interpret;

public sealed record InterpretInboxItemCommand(Guid InboxItemId, Guid UserId) : ICommand<InboxItemModel>;
