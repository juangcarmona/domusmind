using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Notifications;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Abstractions.Integrations.Mcp;
using DomusMind.Application.Features.Lists.AddItemToList;
using DomusMind.Contracts.Lists;
using DomusMind.Domain.Family;
using DomusMind.Domain.Family.ValueObjects;
using DomusMind.Domain.Lists;
using DomusMind.Domain.Lists.ValueObjects;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Messaging;
using DomusMind.Infrastructure.Mcp;
using DomusMind.Infrastructure.Persistence;
using DomusMind.Infrastructure.Persistence.Mcp;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DomusMind.Infrastructure.Tests.Mcp;

public sealed class ConfirmedWriteCoordinatorPostgreSqlTests
{
    [PostgreSqlFact]
    public async Task Confirmed_write_uses_the_real_list_handler_and_event_log_transaction()
    {
        await using var database = await PostgreSqlDatabase.CreateAsync();
        var userId = Guid.NewGuid();
        var familyId = FamilyId.New();
        var listId = ListId.New();

        await database.SeedAsync(familyId, listId);
        await using var db = new DomusMindDbContext(database.Options);
        await using var services = CreateServices(db);
        var coordinator = CreateCoordinator(db, services.GetRequiredService<ICommandDispatcher>());
        var request = new McpListItemAddRequest(
            userId,
            familyId.Value,
            "postgres-test-client",
            listId.Value,
            "Milk",
            "2L",
            "whole",
            "postgres-key",
            null,
            "postgres-trace");

        var challenge = await coordinator.ExecuteListItemAddAsync(request);
        await coordinator.ApproveAsync(
            challenge.ApprovalRequestId!.Value,
            userId,
            new McpApprovalContext("postgres-test-client", familyId.Value, true));
        var completed = await coordinator.ExecuteListItemAddAsync(
            request with { ApprovalRequestId = challenge.ApprovalRequestId.Value.ToString() });

        completed.Status.Should().Be(McpWriteStatuses.Completed);
        completed.Result.Should().NotBeNull();

        await using var verification = new DomusMindDbContext(database.Options);
        var savedList = await verification.Lists
            .Include(list => list.Items)
            .SingleAsync(list => list.Id == listId);
        savedList.Items.Should().ContainSingle(item => item.Name.Value == "Milk");
        (await verification.EventLog.CountAsync()).Should().Be(1);
        (await verification.McpIdempotency.SingleAsync()).Status.Should().Be(McpIdempotencyStatuses.Completed);
    }

    [PostgreSqlFact]
    public async Task Failed_confirmed_write_rolls_back_the_list_and_event_log_transaction()
    {
        await using var database = await PostgreSqlDatabase.CreateAsync();
        var userId = Guid.NewGuid();
        var familyId = FamilyId.New();
        var listId = ListId.New();

        await database.SeedAsync(familyId, listId);
        await using var db = new DomusMindDbContext(database.Options);
        await using var services = CreateServices(db);
        var dispatcher = services.GetRequiredService<ICommandDispatcher>();
        var coordinator = CreateCoordinator(db, new FailingDispatcher(dispatcher));
        var request = new McpListItemAddRequest(
            userId,
            familyId.Value,
            "postgres-test-client",
            listId.Value,
            "Eggs",
            null,
            null,
            "postgres-rollback-key",
            null,
            "postgres-rollback-trace");

        var challenge = await coordinator.ExecuteListItemAddAsync(request);
        await coordinator.ApproveAsync(
            challenge.ApprovalRequestId!.Value,
            userId,
            new McpApprovalContext("postgres-test-client", familyId.Value, true));
        var failed = await coordinator.ExecuteListItemAddAsync(
            request with { ApprovalRequestId = challenge.ApprovalRequestId.Value.ToString() });

        failed.Code.Should().Be("INTERNAL_ERROR");

        await using var verification = new DomusMindDbContext(database.Options);
        var savedList = await verification.Lists
            .Include(list => list.Items)
            .SingleAsync(list => list.Id == listId);
        savedList.Items.Should().BeEmpty();
        (await verification.EventLog.CountAsync()).Should().Be(0);
        (await verification.McpWriteApprovals.SingleAsync()).Status.Should().Be(McpWriteApprovalStatuses.Approved);
    }

    private static ServiceProvider CreateServices(DomusMindDbContext db)
    {
        var services = new ServiceCollection();
        services.AddSingleton<IDomusMindDbContext>(db);
        services.AddSingleton<IEventLogWriter>(new EventLogWriter(db));
        services.AddSingleton<IFamilyAuthorizationService, AllowFamilyAuthorization>();
        services.AddSingleton<ICommandHandler<AddItemToListCommand, AddItemToListResponse>, AddItemToListCommandHandler>();
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        return services.BuildServiceProvider();
    }

    private static ConfirmedWriteCoordinator CreateCoordinator(
        DomusMindDbContext db,
        ICommandDispatcher dispatcher)
    {
        return new ConfirmedWriteCoordinator(
            db,
            dispatcher,
            new AllowFamilyAuthorization(),
            new ActiveUserRepository(),
            new RecordingPublisher(),
            TimeProvider.System,
            Options.Create(new McpConfirmedWriteOptions()),
            NullLogger<ConfirmedWriteCoordinator>.Instance);
    }

    private sealed class FailingDispatcher(ICommandDispatcher inner) : ICommandDispatcher
    {
        public async Task<TResponse> Dispatch<TResponse>(
            ICommand<TResponse> command,
            CancellationToken cancellationToken = default)
        {
            _ = await inner.Dispatch(command, cancellationToken);
            throw new InvalidOperationException("simulated post-handler failure");
        }
    }

    private sealed class AllowFamilyAuthorization : IFamilyAuthorizationService
    {
        public Task<bool> CanAccessFamilyAsync(
            Guid userId,
            Guid familyId,
            CancellationToken cancellationToken = default) => Task.FromResult(true);
    }

    private sealed class ActiveUserRepository : IAuthUserRepository
    {
        public Task<AuthUserRecord?> FindByIdAsync(Guid userId, CancellationToken cancellationToken)
            => Task.FromResult<AuthUserRecord?>(new AuthUserRecord(userId, "postgres@example.test", "hash"));

        public Task<AuthUserRecord?> FindByEmailAsync(string email, CancellationToken cancellationToken)
            => throw new NotSupportedException();
        public Task<IReadOnlyDictionary<Guid, AuthUserStatusProjection>> GetStatusByIdsAsync(IReadOnlyCollection<Guid> userIds, CancellationToken cancellationToken)
            => throw new NotSupportedException();
        public Task AddAsync(AuthUserRecord user, CancellationToken cancellationToken)
            => throw new NotSupportedException();
        public Task UpdatePasswordHashAsync(Guid userId, string newPasswordHash, CancellationToken cancellationToken)
            => throw new NotSupportedException();
        public Task UpdateMustChangePasswordAsync(Guid userId, bool mustChangePassword, CancellationToken cancellationToken)
            => throw new NotSupportedException();
        public Task UpdatePasswordChangedAtAsync(Guid userId, DateTime changedAtUtc, CancellationToken cancellationToken)
            => throw new NotSupportedException();
        public Task DisableUserAsync(Guid userId, CancellationToken cancellationToken)
            => throw new NotSupportedException();
        public Task EnableUserAsync(Guid userId, CancellationToken cancellationToken)
            => throw new NotSupportedException();
        public Task UpdateLastLoginAtAsync(Guid userId, DateTime changedAtUtc, CancellationToken cancellationToken)
            => throw new NotSupportedException();
        public Task<bool> AnyUsersAsync(CancellationToken cancellationToken)
            => throw new NotSupportedException();
        public Task SaveChangesAsync(CancellationToken cancellationToken)
            => throw new NotSupportedException();
    }

    private sealed class RecordingPublisher : IListChangePublisher
    {
        public void Publish(Guid listId)
        {
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

    private sealed class PostgreSqlDatabase : IAsyncDisposable
    {
        private readonly string _adminConnectionString;
        private readonly string _schema;

        private PostgreSqlDatabase(DbContextOptions<DomusMindDbContext> options, string adminConnectionString, string schema)
        {
            Options = options;
            _adminConnectionString = adminConnectionString;
            _schema = schema;
        }

        public DbContextOptions<DomusMindDbContext> Options { get; }

        public static async Task<PostgreSqlDatabase> CreateAsync()
        {
            var baseConnectionString = Environment.GetEnvironmentVariable("DOMUSMIND_TEST_POSTGRES")!;
            var schema = $"mcp_confirmed_write_{Guid.NewGuid():N}";
            var adminConnectionString = new NpgsqlConnectionStringBuilder(baseConnectionString).ConnectionString;
            var testConnectionString = new NpgsqlConnectionStringBuilder(baseConnectionString)
            {
                SearchPath = schema,
            }.ConnectionString;

            await using var admin = new NpgsqlConnection(adminConnectionString);
            await admin.OpenAsync();
            await using var createSchema = admin.CreateCommand();
            createSchema.CommandText = $"CREATE SCHEMA \"{schema}\"";
            await createSchema.ExecuteNonQueryAsync();

            return new PostgreSqlDatabase(
                new DbContextOptionsBuilder<DomusMindDbContext>().UseNpgsql(testConnectionString).Options,
                adminConnectionString,
                schema);
        }

        public async Task SeedAsync(FamilyId familyId, ListId listId)
        {
            await using var db = new DomusMindDbContext(Options);
            await db.Database.EnsureCreatedAsync();
            db.Families.Add(Family.Create(
                familyId,
                FamilyName.Create("MCP confirmed writes"),
                "en",
                DateTime.UtcNow));
            db.Lists.Add(SharedList.Create(
                listId,
                familyId,
                ListName.Create("Shopping"),
                ListKind.Create("shopping"),
                null,
                null,
                null,
                DateTime.UtcNow));
            await db.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await using var admin = new NpgsqlConnection(_adminConnectionString);
            await admin.OpenAsync();
            await using var dropSchema = admin.CreateCommand();
            dropSchema.CommandText = $"DROP SCHEMA IF EXISTS \"{_schema}\" CASCADE";
            await dropSchema.ExecuteNonQueryAsync();
        }
    }
}
