using DomusMind.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DomusMind.Infrastructure.Persistence;

public sealed class AssetMutationLock(DomusMindDbContext db) : IAssetMutationLock
{
    public async Task<IAssetMutation> BeginAsync(Guid familyId, CancellationToken cancellationToken)
    {
        var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // Transaction-scoped PostgreSQL lock: released on commit, rollback, cancellation or disconnect.
            // A namespace separates this key from other application locks; collisions only serialize extra work.
            var key = $"household-assets:{familyId:D}";
            await db.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock(hashtextextended({key}, 0))", cancellationToken);
            return new Mutation(transaction);
        }
        catch
        {
            await transaction.DisposeAsync();
            throw;
        }
    }

    private sealed class Mutation(IDbContextTransaction transaction) : IAssetMutation
    {
        public Task CommitAsync(CancellationToken cancellationToken) => transaction.CommitAsync(cancellationToken);
        public ValueTask DisposeAsync() => transaction.DisposeAsync();
    }
}
