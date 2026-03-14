using DomusMind.Application.Abstractions.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DomusMind.Infrastructure.Messaging;

public sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Dispatch<TResponse>(
        ICommand<TResponse> command,
        CancellationToken cancellationToken = default)
    {
        var commandType = command.GetType();

        var handlerType = typeof(ICommandHandler<,>)
            .MakeGenericType(commandType, typeof(TResponse));

        var handler = _serviceProvider.GetRequiredService(handlerType);

        var method = handlerType.GetMethod(
            nameof(ICommandHandler<ICommand<TResponse>, TResponse>.Handle))!;

        var task = (Task<TResponse>)method.Invoke(
            handler,
            new object[] { command, cancellationToken })!;

        return await task;
    }
}