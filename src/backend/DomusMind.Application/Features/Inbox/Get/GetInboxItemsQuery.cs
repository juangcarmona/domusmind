using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Inbox;

namespace DomusMind.Application.Features.Inbox.Get;

public sealed record GetInboxItemsQuery(Guid FamilyId, Guid UserId) : IQuery<IReadOnlyList<InboxItemModel>>;
