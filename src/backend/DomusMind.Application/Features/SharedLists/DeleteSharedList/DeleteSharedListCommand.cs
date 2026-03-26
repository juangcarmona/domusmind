using DomusMind.Application.Abstractions.Messaging;

namespace DomusMind.Application.Features.SharedLists.DeleteSharedList;

public sealed record DeleteSharedListCommand(
    Guid SharedListId,
    Guid RequestedByUserId) : ICommand<bool>;
