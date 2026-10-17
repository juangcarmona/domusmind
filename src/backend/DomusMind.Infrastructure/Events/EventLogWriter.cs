using System.Diagnostics;
using System.Text.Json;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Domain.Abstractions;
using DomusMind.Infrastructure.Persistence;
using EventLogRecord = DomusMind.Infrastructure.Persistence.EventLogEntry;

namespace DomusMind.Infrastructure.Events;

public sealed class EventLogWriter : IEventLogWriter
{
    private readonly DomusMindDbContext _dbContext;

    public EventLogWriter(DomusMindDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task WriteAsync(
        IReadOnlyCollection<IAggregateRoot> aggregates,
        CancellationToken cancellationToken = default)
    {
        if (aggregates.Count == 0 || aggregates.All(aggregate => aggregate.DomainEvents.Count == 0))
        {
            return;
        }

        var entries = aggregates
            .SelectMany(aggregate => aggregate.DomainEvents.Select(domainEvent => ToEntry(aggregate, domainEvent)))
            .ToList();

        _dbContext.EventLog.AddRange(entries);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static EventLogRecord ToEntry(IAggregateRoot aggregate, IDomainEvent domainEvent)
    {
        var activity = Activity.Current;
        var aggregateType = aggregate.GetType();

        return new EventLogRecord
        {
            EventId = Guid.NewGuid(),
            EventType = domainEvent.GetType().Name,
            AggregateType = aggregateType.Name,
            AggregateId = aggregate.AggregateId.ToString("D"),
            Module = InferModule(aggregateType.Namespace),
            OccurredAtUtc = domainEvent.OccurredAtUtc,
            Version = domainEvent.Version,
            PayloadJson = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
            CorrelationId = activity?.IdFormat == ActivityIdFormat.W3C ? activity.TraceId.ToString() : null,
            CausationId = activity?.IdFormat == ActivityIdFormat.W3C ? activity.SpanId.ToString() : null
        };
    }

    private static string InferModule(string? aggregateNamespace)
    {
        if (string.IsNullOrWhiteSpace(aggregateNamespace))
        {
            return "unknown";
        }

        var segments = aggregateNamespace.Split('.');
        return segments.Length > 2 && segments[0] == "DomusMind" && segments[1] == "Domain"
            ? segments[2]
            : "unknown";
    }
}
