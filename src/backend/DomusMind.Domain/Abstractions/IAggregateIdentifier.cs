namespace DomusMind.Domain.Abstractions;

public interface IAggregateIdentifier
{
    Guid Value { get; }
}
