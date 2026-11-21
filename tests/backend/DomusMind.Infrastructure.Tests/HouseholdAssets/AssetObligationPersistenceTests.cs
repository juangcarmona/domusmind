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

public sealed class AssetObligationPersistenceTests(AssetPostgreSqlFixture fixture) : IClassFixture<AssetPostgreSqlFixture>
{
    private static readonly DateOnly Due = new(2025, 1, 31);
    private async Task<ObligationDefinedResponse> Define()
    {
        await using var db = fixture.CreateContext();
        var family = await db.Families.SingleAsync(x => x.Id == fixture.FamilyId);
        family.ConfigureInitialTimeZone("Pacific/Honolulu");
        var asset = Asset.Create(AssetId.New(), fixture.FamilyId, "Boiler", "Appliance");
        db.Set<Asset>().Add(asset); await db.SaveChangesAsync();
        return await new DefineAssetObligationCommandHandler(db, new FamilyAuthorizationService(db), new AssetMutationLock(db), new EventLogWriter(db))
            .Handle(new(fixture.FamilyId.Value, asset.Id.Value, fixture.UserId, new(Guid.NewGuid(), "Service", Due, "MonthlyLastDay")), default);
    }
    private Task<ObligationOperationResponse> Act(DomusMindDbContext db, ObligationDefinedResponse o, Guid cycle,
        string action, Guid? operation = null, DateOnly? date = null, IAssetMutationLock? gate = null)
        => new ActOnAssetObligationCommandHandler(db, new FamilyAuthorizationService(db), gate ?? new AssetMutationLock(db), new EventLogWriter(db))
            .Handle(new(fixture.FamilyId.Value, o.AssetId, o.Id, fixture.UserId, new(operation ?? Guid.NewGuid(), cycle, action, date)), default);
    private async Task<AssetObligationResponse> Read(ObligationDefinedResponse o, TimeProvider? clock = null)
    {
        await using var db = fixture.CreateContext();
        return Assert.Single(await new GetAssetObligationsQueryHandler(db, new FamilyAuthorizationService(db), clock)
            .Handle(new(fixture.FamilyId.Value, o.AssetId, fixture.UserId), default));
    }

    [AssetPostgreSqlFact]
    public async Task ReloadPreservesRescheduleOutcomesAndExactOriginalRetryAcrossLaterCycles()
    {
        var o = await Define(); var op = Guid.NewGuid();
        ObligationOperationResponse original;
        await using (var db = fixture.CreateContext())
            await Act(db, o, o.InitialCycleId, "Reschedule", date: new DateOnly(2025, 2, 5));
        await using (var db = fixture.CreateContext())
            original = await Act(db, o, o.InitialCycleId, "Complete", op, new DateOnly(2025, 2, 7));
        Assert.Equal(new DateOnly(2025, 3, 31), original.NextDueDate);
        await using (var db = fixture.CreateContext())
            await Act(db, o, original.NextCycleId!.Value, "Skip");
        await using (var db = fixture.CreateContext())
            Assert.Equal(original, await Act(db, o, o.InitialCycleId, "Complete", op, new DateOnly(2025, 2, 7)));
        var saved = await Read(o);
        Assert.Equal(3, saved.Occurrences.Count); Assert.Equal(3, saved.History.Count);
        Assert.Single(saved.Occurrences, x => x.State == "Pending");
        Assert.Single(saved.Occurrences, x => x.State == "Completed");
        Assert.Single(saved.Occurrences, x => x.State == "Skipped");
        Assert.Equal(Due, saved.History[0].PreviousDueDate);
        await using (var db = fixture.CreateContext())
        {
            var error = await Assert.ThrowsAsync<AssetsException>(() => Act(db, o, o.InitialCycleId, "Skip"));
            Assert.Equal(AssetsErrorCode.Conflict, error.Code);
            Assert.Equal(3, await db.Set<ObligationOperation>().CountAsync(x => x.ObligationId == AssetObligationId.From(o.Id)));
            Assert.Equal(3, await db.EventLog.CountAsync(x => x.AggregateId == o.Id.ToString("D") && x.EventType == "AssetObligationActedOn"));
        }
        // Definition retries retain the initial response even after the obligation advances.
        await using (var db = fixture.CreateContext())
            Assert.Equal(o, await new DefineAssetObligationCommandHandler(db, new FamilyAuthorizationService(db), new AssetMutationLock(db), new EventLogWriter(db))
                .Handle(new(fixture.FamilyId.Value, o.AssetId, fixture.UserId, new(o.OperationId, o.Name, Due, "MonthlyLastDay")), default));
    }

    [AssetPostgreSqlFact]
    public async Task HouseholdTimezoneDeterminesDueAndOverdueWithoutGeneratingMissedCycles()
    {
        var o = await Define();
        var localToday = await Read(o, new FixedClock(new DateTimeOffset(2025, 2, 1, 5, 0, 0, TimeSpan.Zero)));
        Assert.Equal("Due", localToday.DueState); Assert.Equal("Pacific/Honolulu", localToday.TimeZoneId);
        var overdue = await Read(o, new FixedClock(new DateTimeOffset(2025, 7, 1, 5, 0, 0, TimeSpan.Zero)));
        Assert.Equal("Overdue", overdue.DueState); Assert.Single(overdue.Occurrences);
        Assert.Equal(o.InitialCycleId, overdue.Occurrences[0].Id); Assert.Empty(overdue.History);
    }

    [AssetPostgreSqlFact]
    public async Task CompleteVersusSkipWaitsOnRealLockAndOnlyOneOutcomeWins()
    {
        var o = await Define();
        await using var first = fixture.CreateContext(); await using var second = fixture.CreateContext();
        var gate = new HeldLock(new AssetMutationLock(first));
        await second.Database.OpenConnectionAsync(); var pid = ((NpgsqlConnection)second.Database.GetDbConnection()).ProcessID;
        var complete = Act(first, o, o.InitialCycleId, "Complete", date: Due, gate: gate);
        await gate.Acquired.Task.WaitAsync(TimeSpan.FromSeconds(10));
        var skip = Act(second, o, o.InitialCycleId, "Skip");
        try { await AssertWaiting(pid); } finally { gate.Release.TrySetResult(); }
        await complete;
        Assert.Equal(AssetsErrorCode.Conflict, (await Assert.ThrowsAsync<AssetsException>(() => skip)).Code);
        var saved = await Read(o); Assert.Equal(2, saved.Occurrences.Count); Assert.Single(saved.History);
        Assert.Single(saved.Occurrences, x => x.State == "Completed");
        Assert.Equal(new DateOnly(2025, 2, 28), saved.Occurrences.Single(x => x.State == "Pending").DueDate);
    }

    [AssetPostgreSqlFact]
    public async Task ArchiveWinsRaceWithoutResolvingCycleAndRestoreRetainsOverdueOrSuspendedState()
    {
        var o = await Define();
        await using var first = fixture.CreateContext(); await using var second = fixture.CreateContext();
        var gate = new HeldLock(new AssetMutationLock(first));
        await second.Database.OpenConnectionAsync(); var pid = ((NpgsqlConnection)second.Database.GetDbConnection()).ProcessID;
        var archive = new ArchiveAssetCommandHandler(first, new FamilyAuthorizationService(first), gate, new EventLogWriter(first))
            .Handle(new(fixture.FamilyId.Value, o.AssetId, fixture.UserId), default);
        await gate.Acquired.Task.WaitAsync(TimeSpan.FromSeconds(10));
        var complete = Act(second, o, o.InitialCycleId, "Complete", date: Due);
        try { await AssertWaiting(pid); } finally { gate.Release.TrySetResult(); }
        await archive;
        Assert.Equal(AssetsErrorCode.Conflict, (await Assert.ThrowsAsync<AssetsException>(() => complete)).Code);
        var archived = await Read(o); Assert.Null(archived.DueState); Assert.Empty(archived.History);
        Assert.Equal("Pending", Assert.Single(archived.Occurrences).State);
        await Restore(o);
        var restored = await Read(o); Assert.Equal("Overdue", restored.DueState); Assert.Equal(Due, restored.Occurrences[0].DueDate);
        await using (var db = fixture.CreateContext()) await Act(db, o, o.InitialCycleId, "Suspend");
        await using (var db = fixture.CreateContext())
            await new ArchiveAssetCommandHandler(db, new FamilyAuthorizationService(db), new AssetMutationLock(db), new EventLogWriter(db))
                .Handle(new(fixture.FamilyId.Value, o.AssetId, fixture.UserId), default);
        await Restore(o);
        var suspended = await Read(o); Assert.Equal("Suspended", suspended.State); Assert.Null(suspended.DueState);
        Assert.Equal(Due, suspended.Occurrences[0].DueDate);
    }

    [AssetPostgreSqlFact]
    public async Task StaleTrackedObligationCannotOverwriteWinningOutcomeOrAppendHistory()
    {
        var o = await Define(); await using var stale = fixture.CreateContext();
        await stale.Set<AssetObligation>().Include(x => x.Occurrences).Include(x => x.Operations)
            .SingleAsync(x => x.Id == AssetObligationId.From(o.Id));
        await using (var db = fixture.CreateContext()) await Act(db, o, o.InitialCycleId, "Complete", date: Due);
        Assert.Equal(AssetsErrorCode.Conflict, (await Assert.ThrowsAsync<AssetsException>(() => Act(stale, o, o.InitialCycleId, "Skip"))).Code);
        var saved = await Read(o); Assert.Single(saved.History); Assert.Single(saved.Occurrences, x => x.State == "Completed");
    }

    [AssetPostgreSqlFact]
    public async Task MigrationRollbackRefusesToDiscardObligationsAndTheirHistory()
    {
        var isolated = new AssetPostgreSqlFixture(); await isolated.InitializeAsync();
        try
        {
            await using var db = isolated.CreateContext();
            var asset = Asset.Create(AssetId.New(), isolated.FamilyId, "Keep", "Other");
            var o = AssetObligation.Create(AssetObligationId.New(), isolated.FamilyId, asset.Id, Guid.NewGuid(), "Keep history", Due, ObligationRecurrence.OneTime());
            o.Apply(Guid.NewGuid(), o.Pending!.Id, ObligationAction.Complete, Due, false);
            db.Set<Asset>().Add(asset); db.Set<AssetObligation>().Add(o); await db.SaveChangesAsync();
            var previous = db.Database.GetMigrations().ToArray()[^2];
            var error = await Assert.ThrowsAsync<PostgresException>(() => db.GetService<IMigrator>().MigrateAsync(previous));
            Assert.Equal("P0001", error.SqlState); Assert.Equal(1, await db.Set<ObligationOperation>().CountAsync());
            Assert.Equal("Keep history", (await db.Set<AssetObligation>().AsNoTracking().SingleAsync()).Name);
        }
        finally { await isolated.DisposeAsync(); }
    }

    private async Task Restore(ObligationDefinedResponse o)
    {
        await using var db = fixture.CreateContext();
        await new RestoreAssetCommandHandler(db, new FamilyAuthorizationService(db), new AssetMutationLock(db), new EventLogWriter(db))
            .Handle(new(fixture.FamilyId.Value, o.AssetId, fixture.UserId), default);
    }
    private async Task AssertWaiting(int pid)
    {
        await using var observer = fixture.CreateContext();
        var deadline = DateTime.UtcNow.AddSeconds(10);
        while (DateTime.UtcNow < deadline)
        {
            if (await observer.Database.SqlQuery<int>($"SELECT count(*)::integer AS \"Value\" FROM pg_locks WHERE pid = {pid} AND locktype = 'advisory' AND NOT granted").SingleAsync() > 0) return;
            await Task.Delay(20);
        }
        Assert.Fail("Competing mutation did not wait on the PostgreSQL transaction lock.");
    }
    private sealed class FixedClock(DateTimeOffset now) : TimeProvider { public override DateTimeOffset GetUtcNow() => now; }
    private sealed class HeldLock(IAssetMutationLock inner) : IAssetMutationLock
    {
        public TaskCompletionSource Acquired { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource Release { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public async Task<IAssetMutation> BeginAsync(Guid family, CancellationToken ct)
        {
            var mutation = await inner.BeginAsync(family, ct); Acquired.TrySetResult();
            await Release.Task.WaitAsync(TimeSpan.FromSeconds(15), ct); return mutation;
        }
    }
}
