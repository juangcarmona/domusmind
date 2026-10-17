using System.Diagnostics;
using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;
using DomusMind.Domain.Family.ValueObjects;
using DomusMind.Domain.Lists;
using DomusMind.Domain.Lists.ValueObjects;
using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.ValueObjects;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.EventLog;

public sealed class EventLogWriterTests
{
    [Fact]
    public async Task WriteAsync_WithNoDomainEvents_DoesNotAddRows()
    {
        await using var dbContext = CreateContext();
        var family = CreateFamily();
        family.ClearDomainEvents();
        var writer = new EventLogWriter(dbContext);

        await writer.WriteAsync([family]);

        Assert.Empty(dbContext.EventLog);
    }

    [Fact]
    public async Task WriteAsync_WithOneAggregate_WritesAggregateAndEventMetadata()
    {
        var occurredAt = DateTime.UtcNow;
        await using var dbContext = CreateContext();
        var family = CreateFamily(occurredAt);
        var writer = new EventLogWriter(dbContext);

        await writer.WriteAsync([family]);

        var entry = await dbContext.EventLog.SingleAsync();
        Assert.NotEqual(Guid.Empty, entry.EventId);
        Assert.Equal("FamilyCreated", entry.EventType);
        Assert.Equal(nameof(Family), entry.AggregateType);
        Assert.Equal(family.Id.Value.ToString("D"), entry.AggregateId);
        Assert.Equal("Family", entry.Module);
        Assert.Equal(occurredAt, entry.OccurredAtUtc);
        Assert.Equal(1, entry.Version);
        Assert.Null(entry.CorrelationId);
        Assert.Null(entry.CausationId);
        Assert.False(string.IsNullOrWhiteSpace(entry.PayloadJson));
    }

    [Fact]
    public async Task WriteAsync_WithCurrentActivity_WritesTraceAndCommandIds()
    {
        await using var dbContext = CreateContext();
        var family = CreateFamily();
        var writer = new EventLogWriter(dbContext);
        using var activity = new Activity("command").SetIdFormat(ActivityIdFormat.W3C).Start();

        await writer.WriteAsync([family]);

        var entry = await dbContext.EventLog.SingleAsync();
        Assert.Equal(activity.TraceId.ToString(), entry.CorrelationId);
        Assert.Equal(activity.SpanId.ToString(), entry.CausationId);
    }

    [Fact]
    public async Task WriteAsync_WithListsAndMealPlanningAggregates_AssignsTheirModules()
    {
        await using var dbContext = CreateContext();
        var familyId = FamilyId.New();
        var now = DateTime.UtcNow;
        var list = SharedList.Create(
            ListId.New(), familyId, ListName.Create("Shopping"), ListKind.Create("shopping"),
            areaId: null, linkedEntityType: null, linkedEntityId: null, now);
        var recipe = Recipe.Create(
            RecipeId.New(), familyId, "Pasta", null, null, null, null, false,
            allowedMealTypes: null, tags: null, now);
        var writer = new EventLogWriter(dbContext);

        await writer.WriteAsync([list, recipe]);

        var modules = await dbContext.EventLog
            .OrderBy(entry => entry.AggregateType)
            .Select(entry => entry.Module)
            .ToListAsync();
        Assert.Equal(["MealPlanning", "Lists"], modules);
    }

    [Fact]
    public async Task WriteAsync_WithVersionedEvent_UsesTheEventSchemaVersion()
    {
        await using var dbContext = CreateContext();
        var aggregate = new VersionedAggregate();
        var writer = new EventLogWriter(dbContext);

        await writer.WriteAsync([aggregate]);

        Assert.Equal(2, (await dbContext.EventLog.SingleAsync()).Version);
    }

    private static DomusMindDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new DomusMindDbContext(options);
    }

    private static DomusMind.Domain.Family.Family CreateFamily(DateTime? occurredAtUtc = null)
        => DomusMind.Domain.Family.Family.Create(
            FamilyId.New(),
            FamilyName.Create("Test family"),
            "en",
            occurredAtUtc ?? DateTime.UtcNow);

    private sealed class VersionedAggregate : AggregateRoot<TestAggregateId>
    {
        public VersionedAggregate() : base(new TestAggregateId(Guid.NewGuid()))
            => RaiseDomainEvent(new VersionedTestEvent(DateTime.UtcNow));
    }

    private readonly record struct TestAggregateId(Guid Value) : IAggregateIdentifier;

    private sealed record VersionedTestEvent(DateTime OccurredAtUtc) : IDomainEvent
    {
        public int Version => 2;
    }
}
