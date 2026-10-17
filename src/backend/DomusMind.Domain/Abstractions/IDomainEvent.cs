namespace DomusMind.Domain.Abstractions;

public interface IDomainEvent
{
    DateTime OccurredAtUtc { get; }

    int Version => 1;
}
