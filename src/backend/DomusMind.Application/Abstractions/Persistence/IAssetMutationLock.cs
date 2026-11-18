namespace DomusMind.Application.Abstractions.Persistence;

/// <summary>Serializes household asset writes across processes, before any hierarchy reads.</summary>
public interface IAssetMutationLock
{
    Task<IAssetMutation> BeginAsync(Guid familyId, CancellationToken cancellationToken);
}

public interface IAssetMutation : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken);
}
