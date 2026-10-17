using DomusMind.Domain.Abstractions;

namespace DomusMind.Application.Abstractions.Persistence;

public interface IEventLogWriter
{
    Task WriteAsync(
        IReadOnlyCollection<IAggregateRoot> aggregates,
        CancellationToken cancellationToken = default);
}
