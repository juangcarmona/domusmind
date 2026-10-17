using DomusMind.Domain.Family;
using DomusMind.Domain.Family.ValueObjects;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Xunit.Sdk;

namespace DomusMind.Infrastructure.Tests.Events;

public sealed class EventLogAtomicityPostgreSqlTests
{
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
                await db.Database.EnsureCreatedAsync();
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
}
