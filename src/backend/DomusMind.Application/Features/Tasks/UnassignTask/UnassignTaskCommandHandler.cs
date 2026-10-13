using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Tasks;
using DomusMind.Domain.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Tasks.UnassignTask;

public sealed class UnassignTaskCommandHandler
    : ICommandHandler<UnassignTaskCommand, UnassignTaskResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IEventLogWriter _eventLogWriter;
    private readonly IFamilyAuthorizationService _authorizationService;

    public UnassignTaskCommandHandler(
        IDomusMindDbContext dbContext,
        IEventLogWriter eventLogWriter,
        IFamilyAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _eventLogWriter = eventLogWriter;
        _authorizationService = authorizationService;
    }

    public async Task<UnassignTaskResponse> Handle(
        UnassignTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = await _dbContext.Set<HouseholdTask>()
            .SingleOrDefaultAsync(t => t.Id == TaskId.From(command.TaskId), cancellationToken);

        if (task is null)
            throw new TasksException(TasksErrorCode.TaskNotFound, "Task not found.");

        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            command.RequestedByUserId, task.FamilyId.Value, cancellationToken);
        if (!canAccess)
            throw new TasksException(TasksErrorCode.AccessDenied, "Access to this family is denied.");

        var previousAssigneeId = task.AssigneeId?.Value;
        try
        {
            task.Unassign();
        }
        catch (InvalidOperationException ex)
        {
            throw new TasksException(TasksErrorCode.InvalidInput, ex.Message);
        }

        await _eventLogWriter.WriteAsync(task.DomainEvents, cancellationToken);
        task.ClearDomainEvents();

        return new UnassignTaskResponse(command.TaskId, previousAssigneeId!.Value);
    }
}
