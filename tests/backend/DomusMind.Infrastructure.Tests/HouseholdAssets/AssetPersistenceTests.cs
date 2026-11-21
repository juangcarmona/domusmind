using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Features.HouseholdAssets;
using DomusMind.Contracts.HouseholdAssets;
using DomusMind.Domain.HouseholdAssets;
using DomusMind.Infrastructure.Auth;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using DomusMind.Tests.HouseholdAssets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;

namespace DomusMind.Infrastructure.Tests.HouseholdAssets;

public sealed class AssetPersistenceTests(AssetPostgreSqlFixture fixture) : IClassFixture<AssetPostgreSqlFixture>
{
    [AssetPostgreSqlFact]
    public async Task ArchiveRestoreRoundTripPreservesReferencesHistoryAndOtherAssets()
    {
        var parent = AssetId.New();
        var id = AssetId.New();
        var location = AssetLocationId.New();
        await using (var db = fixture.CreateContext())
        {
            var parentAsset = Asset.Create(parent, fixture.FamilyId, "House", "Property");
            var asset = Asset.Create(id, fixture.FamilyId, "Boiler", "Appliance");
            asset.Update("Boiler", "Appliance", "Acme", "One", "S123", "Managed", parent, location, fixture.MemberId, fixture.AreaId);
            db.Set<AssetLocation>().Add(AssetLocation.Create(location, fixture.FamilyId, "Home"));
            db.Set<Asset>().AddRange(parentAsset, asset);
            await new EventLogWriter(db).WriteAsync([parentAsset, asset]);
        }
        AssetResponse before;
        Dictionary<Guid, string> history;
        await using (var db = fixture.CreateContext())
        {
            before = await new GetAssetQueryHandler(db, new FamilyAuthorizationService(db))
                .Handle(new(fixture.FamilyId.Value, id.Value, fixture.UserId), default);
            history = await db.EventLog.Where(x => x.AggregateId == id.Value.ToString("D"))
                .ToDictionaryAsync(x => x.EventId, x => x.PayloadJson);
            Assert.NotEmpty(history);
            await new ArchiveAssetCommandHandler(db, new FamilyAuthorizationService(db), new AssetMutationLock(db), new EventLogWriter(db))
                .Handle(new(fixture.FamilyId.Value, id.Value, fixture.UserId), default);
        }
        await using (var db = fixture.CreateContext())
        {
            var archived = await new GetAssetQueryHandler(db, new FamilyAuthorizationService(db))
                .Handle(new(fixture.FamilyId.Value, id.Value, fixture.UserId), default);
            Assert.Equal(before with { Status = "Archived" }, archived);
            Assert.False((await db.Set<Asset>().SingleAsync(x => x.Id == parent)).IsArchived);
            Assert.Null((await db.Set<AssetLocation>().SingleAsync(x => x.Id == location)).Address);
            var error = await Assert.ThrowsAsync<AssetsException>(() => new UpdateAssetCommandHandler(db,
                new FamilyAuthorizationService(db), new AssetMutationLock(db), new EventLogWriter(db))
                .Handle(new(fixture.FamilyId.Value, id.Value, fixture.UserId, new("Changed", "Other")), default));
            Assert.Equal(AssetsErrorCode.Conflict, error.Code);
            await new RestoreAssetCommandHandler(db, new FamilyAuthorizationService(db), new AssetMutationLock(db), new EventLogWriter(db))
                .Handle(new(fixture.FamilyId.Value, id.Value, fixture.UserId), default);
        }
        await using (var db = fixture.CreateContext())
        {
            var restored = await new GetAssetQueryHandler(db, new FamilyAuthorizationService(db))
                .Handle(new(fixture.FamilyId.Value, id.Value, fixture.UserId), default);
            Assert.Equal(before, restored);
            foreach (var entry in history)
                Assert.Equal(entry.Value, (await db.EventLog.SingleAsync(x => x.EventId == entry.Key)).PayloadJson);
            Assert.Equal(1, await db.EventLog.CountAsync(x => x.AggregateId == id.Value.ToString("D") && x.EventType == "AssetArchived"));
            Assert.Equal(1, await db.EventLog.CountAsync(x => x.AggregateId == id.Value.ToString("D") && x.EventType == "AssetRestored"));
        }
    }

    [AssetPostgreSqlFact]
    public async Task ConcurrentOppositeReparentingWaitsThenRejectsCycle()
    {
        var a = AssetId.New();
        var b = AssetId.New();
        await using (var setup = fixture.CreateContext())
        {
            setup.Set<Asset>().AddRange(Asset.Create(a, fixture.FamilyId, "A", "Other"), Asset.Create(b, fixture.FamilyId, "B", "Other"));
            await setup.SaveChangesAsync();
        }
        await using var first = fixture.CreateContext();
        await using var second = fixture.CreateContext();
        await second.Database.OpenConnectionAsync();
        var secondPid = ((NpgsqlConnection)second.Database.GetDbConnection()).ProcessID;
        var heldLock = new HeldLock(new AssetMutationLock(first));
        var firstWrite = new UpdateAssetCommandHandler(first, new FamilyAuthorizationService(first), heldLock, new EventLogWriter(first))
            .Handle(new(fixture.FamilyId.Value, a.Value, fixture.UserId, new("A", "Other", ParentAssetId: b.Value)), default);
        await heldLock.Acquired.Task.WaitAsync(TimeSpan.FromSeconds(10));
        var secondWrite = new UpdateAssetCommandHandler(second, new FamilyAuthorizationService(second), new AssetMutationLock(second), new EventLogWriter(second))
            .Handle(new(fixture.FamilyId.Value, b.Value, fixture.UserId, new("B", "Other", ParentAssetId: a.Value)), default);
        try
        {
            await using var observer = fixture.CreateContext();
            var waiting = false;
            var deadline = DateTime.UtcNow.AddSeconds(10);
            while (!waiting && DateTime.UtcNow < deadline)
            {
                waiting = await observer.Database.SqlQuery<int>($"SELECT count(*)::integer AS \"Value\" FROM pg_locks WHERE pid = {secondPid} AND locktype = 'advisory' AND NOT granted").SingleAsync() > 0;
                if (!waiting) await Task.Delay(20);
            }
            Assert.True(waiting, "The competing writer must wait on the real PostgreSQL advisory lock.");
        }
        finally { heldLock.Release.TrySetResult(); }
        await firstWrite;
        var error = await Assert.ThrowsAsync<AssetsException>(() => secondWrite);
        Assert.Equal(AssetsErrorCode.InvalidInput, error.Code);
        await using var verification = fixture.CreateContext();
        Assert.Equal(b, (await verification.Set<Asset>().SingleAsync(x => x.Id == a)).ParentAssetId);
        Assert.Null((await verification.Set<Asset>().SingleAsync(x => x.Id == b)).ParentAssetId);
    }

    [AssetPostgreSqlFact]
    public async Task StaleTrackedEditCannotOverwriteArchive()
    {
        var id = AssetId.New();
        await using (var setup = fixture.CreateContext())
        {
            setup.Set<Asset>().Add(Asset.Create(id, fixture.FamilyId, "Tool", "Other"));
            await setup.SaveChangesAsync();
        }
        await using var stale = fixture.CreateContext();
        await stale.Set<Asset>().SingleAsync(x => x.Id == id);
        await using (var current = fixture.CreateContext())
            await new ArchiveAssetCommandHandler(current, new FamilyAuthorizationService(current), new AssetMutationLock(current), new EventLogWriter(current))
                .Handle(new(fixture.FamilyId.Value, id.Value, fixture.UserId), default);
        var error = await Assert.ThrowsAsync<AssetsException>(() => new UpdateAssetCommandHandler(stale,
            new FamilyAuthorizationService(stale), new AssetMutationLock(stale), new EventLogWriter(stale))
            .Handle(new(fixture.FamilyId.Value, id.Value, fixture.UserId, new("Changed", "Other")), default));
        Assert.Equal(AssetsErrorCode.Conflict, error.Code);
        await using var verify = fixture.CreateContext();
        var saved = await verify.Set<Asset>().SingleAsync(x => x.Id == id);
        Assert.True(saved.IsArchived);
        Assert.Equal("Tool", saved.Name);
        Assert.False(await verify.EventLog.AnyAsync(x => x.AggregateId == id.Value.ToString("D") && x.EventType == "AssetUpdated"));
    }

    [AssetPostgreSqlFact]
    public async Task MigrationRollbackRequiresEmptyAssetTables()
    {
        var isolated = new AssetPostgreSqlFixture();
        await isolated.InitializeAsync();
        try
        {
            await using var db = isolated.CreateContext();
            var migrations = db.Database.GetMigrations().ToArray();
            var foundation = Array.FindIndex(migrations, x => x.EndsWith("_AddHouseholdAssetFoundation", StringComparison.Ordinal));
            var previous = migrations[foundation - 1];
            var migrator = db.GetService<IMigrator>();
            await migrator.MigrateAsync(previous);
            await db.Database.MigrateAsync();
            var id = AssetId.New();
            db.Set<Asset>().Add(Asset.Create(id, isolated.FamilyId, "Preserve this", "Other"));
            await db.SaveChangesAsync();
            var error = await Assert.ThrowsAsync<PostgresException>(() => migrator.MigrateAsync(previous));
            Assert.Equal("P0001", error.SqlState);
            Assert.Equal("Preserve this", (await db.Set<Asset>().AsNoTracking().SingleAsync(x => x.Id == id)).Name);
            Assert.Equal(migrations[foundation], (await db.Database.GetAppliedMigrationsAsync()).Last());
        }
        finally { await isolated.DisposeAsync(); }
    }

    private sealed class HeldLock(IAssetMutationLock inner) : IAssetMutationLock
    {
        public TaskCompletionSource Acquired { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource Release { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public async Task<IAssetMutation> BeginAsync(Guid familyId, CancellationToken cancellationToken)
        {
            var mutation = await inner.BeginAsync(familyId, cancellationToken);
            Acquired.TrySetResult();
            await Release.Task.WaitAsync(TimeSpan.FromSeconds(15), cancellationToken);
            return mutation;
        }
    }
}
