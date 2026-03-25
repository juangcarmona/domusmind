using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.SharedLists;

namespace DomusMind.Application.Features.SharedLists.UpdateSharedListItem;

public sealed record UpdateSharedListItemCommand(
    Guid SharedListId,
    Guid ItemId,
    string Name,
    string? Quantity,
    string? Note,
    Guid RequestedByUserId) : ICommand<UpdateSharedListItemResponse>;
