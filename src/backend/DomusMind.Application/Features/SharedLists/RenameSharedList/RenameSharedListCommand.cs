using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.SharedLists;

namespace DomusMind.Application.Features.SharedLists.RenameSharedList;

public sealed record RenameSharedListCommand(
    Guid SharedListId,
    string NewName,
    Guid RequestedByUserId) : ICommand<RenameSharedListResponse>;
