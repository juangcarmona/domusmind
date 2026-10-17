using DomusMind.Domain.Calendar.ExternalConnections;
using DomusMind.Domain.Family;
using DomusMind.Infrastructure.BackgroundJobs.Calendar;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Infrastructure.Tests.Calendar;

public sealed class ExternalCalendarRefreshWorkerTests
{
    [Fact]
    public async Task SelectDueConnectionBatchPrioritizesMostOverdueAndUsesConnectionIdForTies()
    {
        var now = new DateTime(2026, 10, 17, 12, 0, 0, DateTimeKind.Utc);
        var nullDueId = Guid.Parse("00000000-0000-0000-0000-000000000004");
        var olderDueId = Guid.Parse("00000000-0000-0000-0000-000000000003");
        var tieFirstId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var tieSecondId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var futureId = Guid.Parse("00000000-0000-0000-0000-000000000005");

        var options = new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var db = new DomusMindDbContext(options);
        await db.Database.EnsureCreatedAsync();

        var nullDue = CreateConnection(nullDueId, now);
        var olderDue = CreateConnection(olderDueId, now);
        olderDue.RecordSyncSuccess(0, 0, 0, now.AddHours(-3));
        var tieFirst = CreateConnection(tieFirstId, now);
        tieFirst.RecordSyncSuccess(0, 0, 0, now.AddHours(-2));
        var tieSecond = CreateConnection(tieSecondId, now);
        tieSecond.RecordSyncSuccess(0, 0, 0, now.AddHours(-2));
        var future = CreateConnection(futureId, now);
        future.RecordSyncSuccess(0, 0, 0, now.AddMinutes(-59));

        db.AddRange(nullDue, olderDue, tieFirst, tieSecond, future);
        await db.SaveChangesAsync();

        var selectedIds = await ExternalCalendarRefreshWorker
            .SelectDueConnectionBatch(db, now, batchSize: 4)
            .Select(connection => connection.Id.Value)
            .ToListAsync();

        selectedIds.Should().Equal(nullDueId, olderDueId, tieFirstId, tieSecondId);
    }

    private static ExternalCalendarConnection CreateConnection(Guid id, DateTime createdAtUtc)
        => ExternalCalendarConnection.Connect(
            ExternalCalendarConnectionId.From(id),
            FamilyId.New(),
            MemberId.New(),
            ExternalCalendarProvider.Microsoft,
            $"provider-account-{id}",
            $"{id}@example.test",
            null,
            null,
            createdAtUtc);
}
