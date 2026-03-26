using DomusMind.Application.Abstractions.Messaging;

namespace DomusMind.Application.Features.SharedLists.RemoveSharedListItem;

public sealed record RemoveSharedListItemCommand(
    Guid SharedListId,
    Guid ItemId,
    Guid RequestedByUserId) : ICommand<bool>;
