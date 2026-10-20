using DomusMind.Application.Abstractions.Integrations.Mcp;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Notifications;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Lists.AddItemToList;
using DomusMind.Contracts.Lists;
using DomusMind.Infrastructure.Mcp;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace DomusMind.Infrastructure.Tests.Mcp;

public sealed class ConfirmedWriteCoordinatorTests
{
    [Fact]
    public async Task Execute_requires_approval_then_executes_once_and_replays_completed_result()
    {
        var userId = Guid.NewGuid();
        var householdId = Guid.NewGuid();
        var listId = Guid.NewGuid();
        await using var db = new DomusMindDbContext(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
        var dispatcher = new RecordingDispatcher();
        var publisher = new RecordingPublisher();
        var coordinator = CreateCoordinator(db, dispatcher, publisher);
        var request = new McpListItemAddRequest(
            userId, householdId, "client", listId, " Milk ", "2L", "whole", "key-1", null, "trace");

        var challenge = await coordinator.ExecuteListItemAddAsync(request);

        challenge.Status.Should().Be(McpWriteStatuses.ConfirmationRequired);
        challenge.ApprovalRequestId.Should().NotBeNull();
        dispatcher.Calls.Should().Be(0);

        var approval = await coordinator.ApproveAsync(challenge.ApprovalRequestId!.Value, userId);
        approval.Code.Should().Be("APPROVED");

        var completed = await coordinator.ExecuteListItemAddAsync(
            request with { ApprovalRequestId = challenge.ApprovalRequestId });

        completed.Status.Should().Be(McpWriteStatuses.Completed);
        completed.Result!.Name.Should().Be("Milk");
        dispatcher.Calls.Should().Be(1);
        publisher.Published.Should().ContainSingle().Which.Should().Be(listId);

        var replay = await coordinator.ExecuteListItemAddAsync(
            request with { ApprovalRequestId = challenge.ApprovalRequestId });

        replay.Status.Should().Be(McpWriteStatuses.Completed);
        replay.Result!.ItemId.Should().Be(completed.Result.ItemId);
        dispatcher.Calls.Should().Be(1);
    }

    [Fact]
    public async Task Approval_is_bound_to_the_requesting_subject()
    {
        var userId = Guid.NewGuid();
        await using var db = new DomusMindDbContext(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
        var coordinator = CreateCoordinator(db, new RecordingDispatcher(), new RecordingPublisher());
        var challenge = await coordinator.ExecuteListItemAddAsync(new McpListItemAddRequest(
            userId, Guid.NewGuid(), "client", Guid.NewGuid(), "Milk", null, null, "key-2", null, "trace"));

        var result = await coordinator.ApproveAsync(challenge.ApprovalRequestId!.Value, Guid.NewGuid());

        result.Code.Should().Be("CONFIRMATION_UNKNOWN");
        (await db.McpWriteApprovals.SingleAsync()).Status.Should().Be("PENDING");
    }

    [Fact]
    public async Task Canonical_payload_binding_rejects_ambiguous_field_boundaries()
    {
        var userId = Guid.NewGuid();
        var householdId = Guid.NewGuid();
        var listId = Guid.NewGuid();
        await using var db = new DomusMindDbContext(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
        var coordinator = CreateCoordinator(db, new RecordingDispatcher(), new RecordingPublisher());

        await coordinator.ExecuteListItemAddAsync(new McpListItemAddRequest(
            userId, householdId, "client", listId, "Milk\n2", "3", null, "key-3", null, "trace"));
        var collision = await coordinator.ExecuteListItemAddAsync(new McpListItemAddRequest(
            userId, householdId, "client", listId, "Milk", "2\n3", null, "key-3", null, "trace"));

        collision.Code.Should().Be("IDEMPOTENCY_KEY_REUSED");
    }

    [Fact]
    public async Task Approved_request_expires_before_execution()
    {
        var clock = new TestTimeProvider(DateTimeOffset.UtcNow);
        var userId = Guid.NewGuid();
        await using var db = new DomusMindDbContext(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
        var dispatcher = new RecordingDispatcher();
        var coordinator = CreateCoordinator(db, dispatcher, new RecordingPublisher(), clock);
        var request = new McpListItemAddRequest(
            userId, Guid.NewGuid(), "client", Guid.NewGuid(), "Milk", null, null, "key-4", null, "trace");
        var challenge = await coordinator.ExecuteListItemAddAsync(request);
        await coordinator.ApproveAsync(challenge.ApprovalRequestId!.Value, userId);
        clock.Advance(TimeSpan.FromMinutes(11));

        var result = await coordinator.ExecuteListItemAddAsync(
            request with { ApprovalRequestId = challenge.ApprovalRequestId });

        result.Code.Should().Be("CONFIRMATION_INVALID");
        dispatcher.Calls.Should().Be(0);
    }

    private static ConfirmedWriteCoordinator CreateCoordinator(
        DomusMindDbContext db,
        RecordingDispatcher dispatcher,
        RecordingPublisher publisher,
        TimeProvider? clock = null)
    {
        return new ConfirmedWriteCoordinator(
            db,
            dispatcher,
            new AllowFamilyAuthorization(),
            new ActiveUserRepository(),
            publisher,
            clock ?? TimeProvider.System,
            Options.Create(new McpConfirmedWriteOptions()),
            NullLogger<ConfirmedWriteCoordinator>.Instance);
    }

    private sealed class RecordingDispatcher : ICommandDispatcher
    {
        public int Calls { get; private set; }

        public Task<TResponse> Dispatch<TResponse>(
            ICommand<TResponse> command,
            CancellationToken cancellationToken = default)
        {
            Calls++;
            var addCommand = (AddItemToListCommand)(object)command;
            var response = new AddItemToListResponse(
                Guid.NewGuid(),
                addCommand.ListId,
                addCommand.Name.Trim(),
                false,
                addCommand.Quantity,
                addCommand.Note,
                1,
                false,
                null,
                null,
                null);
            return Task.FromResult((TResponse)(object)response);
        }
    }

    private sealed class RecordingPublisher : IListChangePublisher
    {
        public List<Guid> Published { get; } = [];
        public void Publish(Guid listId) => Published.Add(listId);
    }

    private sealed class AllowFamilyAuthorization : IFamilyAuthorizationService
    {
        public Task<bool> CanAccessFamilyAsync(Guid userId, Guid familyId, CancellationToken cancellationToken = default)
            => Task.FromResult(true);
    }

    private sealed class ActiveUserRepository : IAuthUserRepository
    {
        public Task<AuthUserRecord?> FindByIdAsync(Guid userId, CancellationToken cancellationToken)
            => Task.FromResult<AuthUserRecord?>(new AuthUserRecord(userId, "user@example.test", "hash"));

        public Task<AuthUserRecord?> FindByEmailAsync(string email, CancellationToken cancellationToken)
            => throw new NotSupportedException();
        public Task<IReadOnlyDictionary<Guid, AuthUserStatusProjection>> GetStatusByIdsAsync(IReadOnlyCollection<Guid> userIds, CancellationToken cancellationToken)
            => throw new NotSupportedException();
        public Task AddAsync(AuthUserRecord user, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task UpdatePasswordHashAsync(Guid userId, string newPasswordHash, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task UpdateMustChangePasswordAsync(Guid userId, bool mustChangePassword, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task UpdatePasswordChangedAtAsync(Guid userId, DateTime changedAtUtc, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task DisableUserAsync(Guid userId, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task EnableUserAsync(Guid userId, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task UpdateLastLoginAtAsync(Guid userId, DateTime lastLoginAtUtc, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task<bool> AnyUsersAsync(CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task SaveChangesAsync(CancellationToken cancellationToken) => throw new NotSupportedException();
    }

    private sealed class TestTimeProvider(DateTimeOffset initial) : TimeProvider
    {
        private DateTimeOffset _now = initial;
        public override DateTimeOffset GetUtcNow() => _now;
        public void Advance(TimeSpan duration) => _now += duration;
    }
}
