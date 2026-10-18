using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Inbox;

namespace DomusMind.Application.Features.Inbox.Capture;

public sealed record CaptureInboxItemCommand(Guid FamilyId, string Text, Guid UserId) : ICommand<InboxItemModel>;
