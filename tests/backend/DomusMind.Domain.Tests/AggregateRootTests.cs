using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Tests;

public class AggregateRootTests
{
    [Fact]
    public void RaiseDomainEvent_AddsEventToCollection()
    {
        var aggregate = new TestAggregate(new TestAggregateId(Guid.NewGuid()));
        var domainEvent = new TestDomainEvent(DateTime.UtcNow);

        aggregate.Publish(domainEvent);

        Assert.Single(aggregate.DomainEvents);
        Assert.Contains(domainEvent, aggregate.DomainEvents);
    }

    private sealed class TestAggregate : AggregateRoot<TestAggregateId>
    {
        public TestAggregate(TestAggregateId id) : base(id)
        {
        }

        public void Publish(IDomainEvent domainEvent)
        {
            RaiseDomainEvent(domainEvent);
        }
    }

    private readonly record struct TestAggregateId(Guid Value) : IAggregateIdentifier;

    private sealed record TestDomainEvent(DateTime OccurredAtUtc) : IDomainEvent;
}
