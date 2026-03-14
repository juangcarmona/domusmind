using DomusMind.Application.Abstractions.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DomusMind.Infrastructure.Messaging;

public sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Dispatch<TResponse>(
        IQuery<TResponse> query,
        CancellationToken cancellationToken = default)
    {
        var queryType = query.GetType();

        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(queryType, typeof(TResponse));

        var handler = _serviceProvider.GetRequiredService(handlerType);

        var method = handlerType.GetMethod(
            nameof(IQueryHandler<IQuery<TResponse>, TResponse>.Handle))!;

        var task = (Task<TResponse>)method.Invoke(
            handler,
            new object[] { query, cancellationToken })!;

        return await task;
    }
}