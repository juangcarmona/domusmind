using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DomusMind.Application.Tests.Dispatching;

// Handlers are registered by hand in AddApplication, so a new slice whose handler is
// forgotten compiles and fails only at request time with handler_resolution_failed.
public class HandlerRegistrationTests
{
    [Fact]
    public void AddApplication_Should_Register_Every_Command_And_Query_Handler()
    {
        var services = new ServiceCollection();
        services.AddApplication();

        var registered = services
            .Where(d => d.ImplementationType is not null)
            .Select(d => (d.ServiceType, d.ImplementationType!))
            .ToHashSet();

        var handlerInterfaces = new[] { typeof(ICommandHandler<,>), typeof(IQueryHandler<,>) };

        var missing = typeof(ApplicationServices).Assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false })
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && handlerInterfaces.Contains(i.GetGenericTypeDefinition()))
                .Select(i => (Service: i, Implementation: t)))
            .Where(pair => !registered.Contains(pair))
            .Select(pair => pair.Implementation.Name)
            .OrderBy(name => name)
            .ToList();

        Assert.Empty(missing);
    }
}
