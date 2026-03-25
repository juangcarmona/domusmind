using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.SharedLists;
using DomusMind.Domain.SharedLists;
using DomusMind.Domain.SharedLists.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.SharedLists.RenameSharedList;

public sealed class RenameSharedListCommandHandler
    : ICommandHandler<RenameSharedListCommand, RenameSharedListResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IEventLogWriter _eventLogWriter;
    private readonly IFamilyAuthorizationService _authorizationService;

    public RenameSharedListCommandHandler(
        IDomusMindDbContext dbContext,
        IEventLogWriter eventLogWriter,
        IFamilyAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _eventLogWriter = eventLogWriter;
        _authorizationService = authorizationService;
    }

    public async Task<RenameSharedListResponse> Handle(
        RenameSharedListCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.NewName))
            throw new SharedListException(SharedListErrorCode.InvalidInput, "List name is required.");

        var listId = SharedListId.From(command.SharedListId);

        var list = await _dbContext.Set<SharedList>()
            .SingleOrDefaultAsync(l => l.Id == listId, cancellationToken);

        if (list is null)
            throw new SharedListException(SharedListErrorCode.ListNotFound, "Shared list not found.");

        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            command.RequestedByUserId, list.FamilyId.Value, cancellationToken);

        if (!canAccess)
            throw new SharedListException(SharedListErrorCode.AccessDenied, "Access to this family is denied.");

        SharedListName newName;
        try
        {
            newName = SharedListName.Create(command.NewName);
        }
        catch (ArgumentException ex)
        {
            throw new SharedListException(SharedListErrorCode.InvalidInput, ex.Message);
        }

        list.Rename(newName, DateTime.UtcNow);

        await _eventLogWriter.WriteAsync(list.DomainEvents, cancellationToken);
        list.ClearDomainEvents();
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new RenameSharedListResponse(list.Id.Value, list.Name.Value);
    }
}
