namespace DomusMind.Domain.Abstractions;

public interface IAggregateRoot : IHasDomainEvents
{
    Guid AggregateId { get; }
}
