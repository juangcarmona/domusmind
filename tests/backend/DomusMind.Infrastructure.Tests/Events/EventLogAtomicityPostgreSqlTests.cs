using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;
using DomusMind.Domain.Family.ValueObjects;
using DomusMind.Domain.Lists;
using DomusMind.Domain.Lists.ValueObjects;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Xunit.Sdk;

namespace DomusMind.Infrastructure.Tests.Events;

public sealed class EventLogAtomicityPostgreSqlTests
{
    [PostgreSqlFact]
    public async Task SharedList_WhenTwoContextsEditTheSameVersion_RejectsTheStaleWrite()
    {
        var baseConnectionString = Environment.GetEnvironmentVariable("DOMUSMIND_TEST_POSTGRES")!;
        var schema = $"list_concurrency_test_{Guid.NewGuid():N}";
        var adminConnectionString = new NpgsqlConnectionStringBuilder(baseConnectionString).ConnectionString;
        var testConnectionString = new NpgsqlConnectionStringBuilder(baseConnectionString)
        {
            SearchPath = schema
        }.ConnectionString;

        await using (var admin = new NpgsqlConnection(adminConnectionString))
        {
            await admin.OpenAsync();
            await using var createSchema = admin.CreateCommand();
            createSchema.CommandText = $"CREATE SCHEMA \"{schema}\"";
            await createSchema.ExecuteNonQueryAsync();
        }

        try
        {
            var options = new DbContextOptionsBuilder<DomusMindDbContext>()
                .UseNpgsql(testConnectionString)
                .Options;
            var familyId = FamilyId.New();
            var listId = ListId.New();

            await using (var setup = new DomusMindDbContext(options))
            {
                await setup.Database.MigrateAsync();
                setup.Families.Add(Family.Create(
                    familyId,
                    FamilyName.Create("Concurrent list test"),
                    "en",
                    DateTime.UtcNow));
                setup.Lists.Add(SharedList.Create(
                    listId,
                    familyId,
                    ListName.Create("Concurrent list"),
                    ListKind.Create("Shopping"),
                    null,
                    null,
                    null,
                    DateTime.UtcNow));
                await setup.SaveChangesAsync();
            }

            await using var first = new DomusMindDbContext(options);
            await using var second = new DomusMindDbContext(options);
            var firstList = await first.Lists.Include(list => list.Items)
                .SingleAsync(list => list.Id == listId);
            var secondList = await second.Lists.Include(list => list.Items)
                .SingleAsync(list => list.Id == listId);

            firstList.AddItem(ListItemId.New(), ListItemName.Create("Milk"), null, null, DateTime.UtcNow);
            secondList.AddItem(ListItemId.New(), ListItemName.Create("Bread"), null, null, DateTime.UtcNow);

            await first.SaveChangesAsync();
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => second.SaveChangesAsync());

            await using var verification = new DomusMindDbContext(options);
            var savedList = await verification.Lists.Include(list => list.Items)
                .SingleAsync(list => list.Id == listId);
            Assert.Equal(2, savedList.Version);
            Assert.Single(savedList.Items);
            Assert.Equal("Milk", savedList.Items.Single().Name.Value);
        }
        finally
        {
            await using var admin = new NpgsqlConnection(adminConnectionString);
            await admin.OpenAsync();
            await using var dropSchema = admin.CreateCommand();
            dropSchema.CommandText = $"DROP SCHEMA IF EXISTS \"{schema}\" CASCADE";
            await dropSchema.ExecuteNonQueryAsync();
        }
    }

    [PostgreSqlFact]
    public async Task WriteAsync_WhenReactionFails_RollsBackAggregateAndEventLog()
    {
        var baseConnectionString = Environment.GetEnvironmentVariable("DOMUSMIND_TEST_POSTGRES")!;
        var schema = $"event_reaction_test_{Guid.NewGuid():N}";
        var adminConnectionString = new NpgsqlConnectionStringBuilder(baseConnectionString).ConnectionString;
        var testConnectionString = new NpgsqlConnectionStringBuilder(baseConnectionString)
        {
            SearchPath = schema
        }.ConnectionString;

        await using (var admin = new NpgsqlConnection(adminConnectionString))
        {
            await admin.OpenAsync();
            await using var createSchema = admin.CreateCommand();
            createSchema.CommandText = $"CREATE SCHEMA \"{schema}\"";
            await createSchema.ExecuteNonQueryAsync();
        }

        try
        {
            var options = new DbContextOptionsBuilder<DomusMindDbContext>()
                .UseNpgsql(testConnectionString)
                .Options;

            await using (var db = new DomusMindDbContext(options))
            {
                await db.Database.MigrateAsync();
                var family = Family.Create(
                    FamilyId.New(),
                    FamilyName.Create("Reaction rollback test"),
                    "en",
                    DateTime.UtcNow);
                db.Families.Add(family);

                var writer = new EventLogWriter(db, new FailingDomainEventDispatcher(db, family.Id));
                var error = await Assert.ThrowsAsync<InvalidOperationException>(
                    () => writer.WriteAsync([family]));
                Assert.Equal("simulated event reaction failure", error.Message);
            }

            await using (var verification = new DomusMindDbContext(options))
            {
                Assert.Equal(0, await verification.Families.CountAsync());
                Assert.Equal(0, await verification.Set<SharedList>().CountAsync());
                Assert.Equal(0, await verification.EventLog.CountAsync());
            }
        }
        finally
        {
            await using var admin = new NpgsqlConnection(adminConnectionString);
            await admin.OpenAsync();
            await using var dropSchema = admin.CreateCommand();
            dropSchema.CommandText = $"DROP SCHEMA IF EXISTS \"{schema}\" CASCADE";
            await dropSchema.ExecuteNonQueryAsync();
        }
    }

    [PostgreSqlFact]
    public async Task WriteAsync_WhenEventInsertFails_RollsBackAggregateAndEventLog()
    {
        var baseConnectionString = Environment.GetEnvironmentVariable("DOMUSMIND_TEST_POSTGRES")!;
        var schema = $"event_log_test_{Guid.NewGuid():N}";
        var adminConnectionString = new NpgsqlConnectionStringBuilder(baseConnectionString).ConnectionString;
        var testConnectionString = new NpgsqlConnectionStringBuilder(baseConnectionString)
        {
            SearchPath = schema
        }.ConnectionString;

        await using (var admin = new NpgsqlConnection(adminConnectionString))
        {
            await admin.OpenAsync();
            await using var createSchema = admin.CreateCommand();
            createSchema.CommandText = $"CREATE SCHEMA \"{schema}\"";
            await createSchema.ExecuteNonQueryAsync();
        }

        try
        {
            var options = new DbContextOptionsBuilder<DomusMindDbContext>()
                .UseNpgsql(testConnectionString)
                .Options;

            await using (var db = new DomusMindDbContext(options))
            {
                await db.Database.MigrateAsync();
                await db.Database.ExecuteSqlRawAsync("""
                    CREATE FUNCTION reject_event_log_insert() RETURNS trigger
                    LANGUAGE plpgsql AS $$
                    BEGIN
                        RAISE EXCEPTION 'simulated event log persistence failure';
                    END;
                    $$;

                    CREATE CONSTRAINT TRIGGER reject_event_log_insert
                    AFTER INSERT ON event_log
                    DEFERRABLE INITIALLY DEFERRED
                    FOR EACH ROW EXECUTE FUNCTION reject_event_log_insert();
                    """);

                var family = Family.Create(
                    FamilyId.New(),
                    FamilyName.Create("Atomicity test"),
                    "en",
                    DateTime.UtcNow);
                db.Families.Add(family);

                var writer = new EventLogWriter(db);
                await Assert.ThrowsAnyAsync<Exception>(() => writer.WriteAsync([family]));
            }

            await using (var verification = new DomusMindDbContext(options))
            {
                Assert.Equal(0, await verification.Families.CountAsync());
                Assert.Equal(0, await verification.EventLog.CountAsync());
            }
        }
        finally
        {
            await using var admin = new NpgsqlConnection(adminConnectionString);
            await admin.OpenAsync();
            await using var dropSchema = admin.CreateCommand();
            dropSchema.CommandText = $"DROP SCHEMA IF EXISTS \"{schema}\" CASCADE";
            await dropSchema.ExecuteNonQueryAsync();
        }
    }

    private sealed class PostgreSqlFactAttribute : FactAttribute
    {
        public PostgreSqlFactAttribute()
        {
            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DOMUSMIND_TEST_POSTGRES")))
                Skip = "Set DOMUSMIND_TEST_POSTGRES to run PostgreSQL-backed persistence tests.";
        }
    }

    private sealed class FailingDomainEventDispatcher(DomusMindDbContext dbContext, FamilyId familyId)
        : IDomainEventDispatcher
    {
        public async Task Dispatch(
            IReadOnlyCollection<IDomainEvent> domainEvents,
            CancellationToken cancellationToken = default)
        {
            var list = SharedList.Create(
                ListId.New(),
                familyId,
                ListName.Create("Reaction rollback"),
                ListKind.Create("shopping"),
                areaId: null,
                linkedEntityType: null,
                linkedEntityId: null,
                DateTime.UtcNow);
            dbContext.Set<SharedList>().Add(list);
            await new EventLogWriter(dbContext).WriteAsync([list], cancellationToken);
            throw new InvalidOperationException("simulated event reaction failure");
        }
    }
}
