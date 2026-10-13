using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Tasks;

namespace DomusMind.Application.Features.Tasks.UnassignTask;

public sealed record UnassignTaskCommand(
    Guid TaskId,
    Guid RequestedByUserId) : ICommand<UnassignTaskResponse>;
