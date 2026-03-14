using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DomusMind.Infrastructure.Messaging;

public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Dispatch(
        IReadOnlyCollection<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var eventType = domainEvent.GetType();

            var handlerType = typeof(IDomainEventHandler<>)
                .MakeGenericType(eventType);

            var handlers = _serviceProvider.GetServices(handlerType);

            var method = handlerType.GetMethod(
                nameof(IDomainEventHandler<IDomainEvent>.Handle))!;

            foreach (var handler in handlers)
            {
                var task = (Task)method.Invoke(
                    handler,
                    new object[] { domainEvent, cancellationToken })!;

                await task;
            }
        }
    }
}