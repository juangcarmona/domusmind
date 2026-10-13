using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Tasks;

namespace DomusMind.Application.Features.Tasks.DeleteRoutine;

public sealed record DeleteRoutineCommand(Guid RoutineId, Guid RequestedByUserId)
    : ICommand<DeleteRoutineResponse>;
