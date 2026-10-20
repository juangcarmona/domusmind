using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DomusMind.Application.Abstractions.Integrations.Mcp;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Notifications;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Lists.AddItemToList;
using DomusMind.Application.Features.Lists;
using DomusMind.Contracts.Lists;
using DomusMind.Domain.Lists;
using DomusMind.Infrastructure.Persistence;
using DomusMind.Infrastructure.Persistence.Mcp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DomusMind.Infrastructure.Mcp;

public sealed class ConfirmedWriteCoordinator : IMcpConfirmedWriteCoordinator, IMcpConfirmedWriteMaintenance
{
    private const string OperationNamespace = "mcp:list_item_add:v1";
    private readonly DomusMindDbContext _db;
    private readonly ICommandDispatcher _commands;
    private readonly IFamilyAuthorizationService _familyAuthorization;
    private readonly IAuthUserRepository _users;
    private readonly IListChangePublisher _listChanges;
    private readonly TimeProvider _clock;
    private readonly McpConfirmedWriteOptions _options;
    private readonly ILogger<ConfirmedWriteCoordinator> _logger;

    public ConfirmedWriteCoordinator(
        DomusMindDbContext db,
        ICommandDispatcher commands,
        IFamilyAuthorizationService familyAuthorization,
        IAuthUserRepository users,
        IListChangePublisher listChanges,
        TimeProvider clock,
        IOptions<McpConfirmedWriteOptions> options,
        ILogger<ConfirmedWriteCoordinator> logger)
    {
        _db = db;
        _commands = commands;
        _familyAuthorization = familyAuthorization;
        _users = users;
        _listChanges = listChanges;
        _clock = clock;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<McpConfirmedWriteOutcome> ExecuteListItemAddAsync(
        McpListItemAddRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await ExecuteListItemAddCoreAsync(request, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (DbUpdateException)
        {
            return Error("OPERATION_STATUS_UNKNOWN", "The operation outcome is unknown. Retry with the same idempotency key.");
        }
        catch (Exception)
        {
            _logger.LogWarning(
                "MCP confirmed write coordinator failed operation={Operation} user={UserId} household={HouseholdId} trace={TraceId}",
                OperationNamespace,
                request.UserId,
                request.HouseholdId,
                request.TraceId);
            return Error("INTERNAL_ERROR", "The operation could not be completed.");
        }
    }

    private async Task<McpConfirmedWriteOutcome> ExecuteListItemAddCoreAsync(
        McpListItemAddRequest request,
        CancellationToken cancellationToken)
    {
        var validation = Validate(request);
        if (validation is not null)
            return validation;

        var now = UtcNow();
        await PurgeExpiredAsync(cancellationToken);
        var hash = ComputePayloadHash(request);
        await using var transaction = await BeginTransactionAsync(cancellationToken);

        var idempotency = await _db.McpIdempotency
            .SingleOrDefaultAsync(x =>
                x.OperationNamespace == OperationNamespace &&
                x.UserId == request.UserId &&
                x.ClientId == request.ClientId &&
                x.FamilyId == request.HouseholdId &&
                x.IdempotencyKey == request.IdempotencyKey,
                cancellationToken);

        if (idempotency is not null)
        {
            if (!string.Equals(idempotency.PayloadHash, hash, StringComparison.Ordinal))
                return Error("IDEMPOTENCY_KEY_REUSED", "The idempotency key was already used with a different request.");

            var existing = await ContinueExistingAsync(idempotency, request, now, cancellationToken);
            if (existing is not null)
            {
                await CommitAsync(transaction, cancellationToken);
                if (existing.Result is not null && existing.Status == McpWriteStatuses.Completed)
                    return existing;
                return existing;
            }
        }
        else
        {
            if (request.ApprovalRequestId.HasValue)
                return Error("CONFIRMATION_UNKNOWN", "The approval request was not found.");

            var approvalId = Guid.NewGuid();
            var expiresAt = now.AddSeconds(_options.ApprovalLifetimeSeconds);
            _db.McpWriteApprovals.Add(new McpWriteApprovalRecord
            {
                Id = approvalId,
                OperationNamespace = OperationNamespace,
                UserId = request.UserId,
                ClientId = request.ClientId,
                FamilyId = request.HouseholdId,
                ListId = request.ListId,
                IdempotencyKey = request.IdempotencyKey,
                PayloadHash = hash,
                Name = request.Name.Trim(),
                Quantity = NormalizeOptional(request.Quantity),
                Note = NormalizeOptional(request.Note),
                Status = McpWriteApprovalStatuses.Pending,
                CreatedAtUtc = now,
                ExpiresAtUtc = expiresAt,
            });
            idempotency = new McpIdempotencyRecord
            {
                Id = Guid.NewGuid(),
                OperationNamespace = OperationNamespace,
                UserId = request.UserId,
                ClientId = request.ClientId,
                FamilyId = request.HouseholdId,
                IdempotencyKey = request.IdempotencyKey,
                PayloadHash = hash,
                Status = McpIdempotencyStatuses.PendingApproval,
                ListId = request.ListId,
                ApprovalRequestId = approvalId,
                CreatedAtUtc = now,
            };
            _db.McpIdempotency.Add(idempotency);
            await _db.SaveChangesAsync(cancellationToken);
            await CommitAsync(transaction, cancellationToken);
            return new McpConfirmedWriteOutcome(
                McpWriteStatuses.ConfirmationRequired,
                ApprovalRequestId: approvalId,
                ExpiresAtUtc: expiresAt,
                Action: ToAction(request));
        }

        if (idempotency.Status != McpIdempotencyStatuses.Executing)
            return Error("OPERATION_STATUS_UNKNOWN", "The write operation is not executable in its current state.");

        AddItemToListResponse result;
        try
        {
            result = await _commands.Dispatch(
                new AddItemToListCommand(
                    request.ListId,
                    request.Name,
                    request.Quantity,
                    request.Note,
                    null,
                    request.UserId,
                    request.HouseholdId),
                cancellationToken);
        }
        catch (ListException exception)
        {
            var code = exception.Code == ListErrorCode.AccessDenied
                ? "ACCESS_DENIED"
                : "INVALID_INPUT";
            return Error(code, code == "ACCESS_DENIED"
                ? "Access to this household is denied."
                : "The list item input is invalid.");
        }

        idempotency.Status = McpIdempotencyStatuses.Completed;
        idempotency.ItemId = result.ItemId;
        idempotency.ResultJson = JsonSerializer.Serialize(result);
        idempotency.CompletedAtUtc = now;
        idempotency.ResultExpiresAtUtc = now.AddDays(_options.ResultRetentionDays);
        idempotency.TombstoneUntilUtc = now.AddDays(_options.TombstoneRetentionDays);

        if (idempotency.ApprovalRequestId is Guid approvalRequestId)
        {
            var approval = await _db.McpWriteApprovals
                .SingleAsync(x => x.Id == approvalRequestId, cancellationToken);
            approval.Status = McpWriteApprovalStatuses.Consumed;
            approval.ConsumedAtUtc = now;
        }

        await _db.SaveChangesAsync(cancellationToken);
        await CommitAsync(transaction, cancellationToken);
        _logger.LogInformation(
            "MCP confirmed write completed operation={Operation} user={UserId} household={HouseholdId} list={ListId} item={ItemId} trace={TraceId}",
            OperationNamespace,
            request.UserId,
            request.HouseholdId,
            request.ListId,
            result.ItemId,
            request.TraceId);
        _listChanges.Publish(result.ListId);
        return new McpConfirmedWriteOutcome(McpWriteStatuses.Completed, Result: result);
    }

    public async Task<McpWriteApprovalView?> GetApprovalAsync(
        Guid approvalRequestId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var approval = await _db.McpWriteApprovals
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == approvalRequestId && x.UserId == userId, cancellationToken);
        if (approval is null ||
            !await _familyAuthorization.CanAccessFamilyAsync(userId, approval.FamilyId, cancellationToken))
            return null;

        return ToView(approval);
    }

    public async Task<McpConfirmedWriteOutcome> ApproveAsync(
        Guid approvalRequestId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _users.FindByIdAsync(userId, cancellationToken);
        if (user is null || user.IsDisabled)
            return Error("AUTHORIZATION_DENIED", "The approving member is not active.");

        await using var transaction = await BeginTransactionAsync(cancellationToken);
        var approval = await _db.McpWriteApprovals
            .SingleOrDefaultAsync(x => x.Id == approvalRequestId && x.UserId == userId, cancellationToken);
        if (approval is null ||
            !await _familyAuthorization.CanAccessFamilyAsync(userId, approval.FamilyId, cancellationToken))
            return Error("CONFIRMATION_UNKNOWN", "The approval request was not found.");

        var now = UtcNow();
        if (approval.Status != McpWriteApprovalStatuses.Pending)
            return Error("CONFIRMATION_INVALID", "The approval request is no longer pending.");
        if (approval.ExpiresAtUtc <= now)
        {
            approval.Status = McpWriteApprovalStatuses.Expired;
            await MarkIdempotencyCancelledAsync(approvalRequestId, now, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            await CommitAsync(transaction, cancellationToken);
            return Error("CONFIRMATION_EXPIRED", "The approval request has expired.");
        }

        approval.Status = McpWriteApprovalStatuses.Approved;
        approval.ApprovedByUserId = userId;
        approval.ApprovedAtUtc = now;
        await _db.SaveChangesAsync(cancellationToken);
        await CommitAsync(transaction, cancellationToken);
        _logger.LogInformation(
            "MCP confirmed write approved operation={Operation} user={UserId} household={HouseholdId}",
            OperationNamespace,
            userId,
            approval.FamilyId);
        return new McpConfirmedWriteOutcome(McpWriteStatuses.ApprovalPending, Code: "APPROVED");
    }

    public async Task<McpConfirmedWriteOutcome> CancelAsync(
        Guid approvalRequestId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _users.FindByIdAsync(userId, cancellationToken);
        if (user is null || user.IsDisabled)
            return Error("AUTHORIZATION_DENIED", "The cancelling member is not active.");

        await using var transaction = await BeginTransactionAsync(cancellationToken);
        var approval = await _db.McpWriteApprovals
            .SingleOrDefaultAsync(x => x.Id == approvalRequestId && x.UserId == userId, cancellationToken);
        if (approval is null ||
            !await _familyAuthorization.CanAccessFamilyAsync(userId, approval.FamilyId, cancellationToken))
            return Error("CONFIRMATION_UNKNOWN", "The approval request was not found.");
        if (approval.Status != McpWriteApprovalStatuses.Pending)
            return Error("CONFIRMATION_INVALID", "The approval request is no longer pending.");

        approval.Status = McpWriteApprovalStatuses.Cancelled;
        var idempotency = await _db.McpIdempotency.SingleAsync(
            x => x.ApprovalRequestId == approvalRequestId,
            cancellationToken);
        idempotency.Status = McpIdempotencyStatuses.Cancelled;
        idempotency.TombstoneUntilUtc = UtcNow().AddDays(_options.TombstoneRetentionDays);
        await _db.SaveChangesAsync(cancellationToken);
        await CommitAsync(transaction, cancellationToken);
        _logger.LogInformation(
            "MCP confirmed write cancelled operation={Operation} user={UserId} household={HouseholdId}",
            OperationNamespace,
            userId,
            approval.FamilyId);
        return new McpConfirmedWriteOutcome(McpWriteStatuses.Error, Code: "CANCELLED", Message: "The approval request was cancelled.");
    }

    private async Task<McpConfirmedWriteOutcome?> ContinueExistingAsync(
        McpIdempotencyRecord idempotency,
        McpListItemAddRequest request,
        DateTime now,
        CancellationToken cancellationToken)
    {
        if (idempotency.Status == McpIdempotencyStatuses.Completed &&
            idempotency.ResultExpiresAtUtc is DateTime resultExpiresAt &&
            resultExpiresAt <= now)
        {
            return Error("IDEMPOTENCY_RESULT_EXPIRED", "The completed operation result is no longer retained.");
        }

        if (idempotency.Status == McpIdempotencyStatuses.Completed)
        {
            var result = idempotency.ResultJson is null
                ? null
                : JsonSerializer.Deserialize<DomusMind.Contracts.Lists.AddItemToListResponse>(idempotency.ResultJson);
            return result is null
                ? Error("OPERATION_STATUS_UNKNOWN", "The completed operation result is unavailable.")
                : new McpConfirmedWriteOutcome(McpWriteStatuses.Completed, Result: result);
        }

        if (idempotency.Status is McpIdempotencyStatuses.Cancelled or McpIdempotencyStatuses.Tombstone)
            return Error("IDEMPOTENCY_REPLAYED", "The operation was already cancelled or completed and cannot be replayed.");

        if (idempotency.Status == McpIdempotencyStatuses.Executing)
            return Error("OPERATION_IN_PROGRESS", "The operation is already being executed.");

        if (idempotency.ApprovalRequestId is not Guid approvalId)
            return Error("OPERATION_STATUS_UNKNOWN", "The pending operation has no approval request.");

        var approval = await _db.McpWriteApprovals
            .SingleOrDefaultAsync(x => x.Id == approvalId, cancellationToken);
        if (approval is null)
            return Error("CONFIRMATION_UNKNOWN", "The approval request was not found.");
        if (request.ApprovalRequestId is null)
            return new McpConfirmedWriteOutcome(
                McpWriteStatuses.ConfirmationRequired,
                ApprovalRequestId: approvalId,
                ExpiresAtUtc: approval.ExpiresAtUtc,
                Action: ToAction(request));
        if (request.ApprovalRequestId != approvalId)
            return Error("CONFIRMATION_INVALID", "The approval request does not match the pending operation.");
        if (approval.ExpiresAtUtc <= now)
        {
            approval.Status = McpWriteApprovalStatuses.Expired;
            idempotency.Status = McpIdempotencyStatuses.Cancelled;
            idempotency.TombstoneUntilUtc = now.AddDays(_options.TombstoneRetentionDays);
            await _db.SaveChangesAsync(cancellationToken);
            return Error("CONFIRMATION_INVALID", "The approval request is no longer valid.");
        }

        if (approval.Status == McpWriteApprovalStatuses.Pending)
            return new McpConfirmedWriteOutcome(
                McpWriteStatuses.ApprovalPending,
                ApprovalRequestId: approvalId,
                ExpiresAtUtc: approval.ExpiresAtUtc,
                Action: ToAction(request));
        if (approval.Status != McpWriteApprovalStatuses.Approved)
            return Error("CONFIRMATION_INVALID", "The approval request is no longer valid.");

        idempotency.Status = McpIdempotencyStatuses.Executing;
        await _db.SaveChangesAsync(cancellationToken);
        return null;
    }

    private static McpConfirmedWriteOutcome? Validate(McpListItemAddRequest request)
    {
        if (request.ListId == Guid.Empty)
            return Error("INVALID_INPUT", "listId is required.");
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Trim().Length > 200)
            return Error("INVALID_INPUT", "name must be non-empty and at most 200 characters.");
        if (request.Quantity?.Length > 100)
            return Error("INVALID_INPUT", "quantity must be at most 100 characters.");
        if (request.Note?.Length > 500)
            return Error("INVALID_INPUT", "note must be at most 500 characters.");
        if (string.IsNullOrWhiteSpace(request.IdempotencyKey) ||
            request.IdempotencyKey.Trim().Length > 128 ||
            request.IdempotencyKey.Any(c => c > 127 || char.IsWhiteSpace(c)))
            return Error("INVALID_INPUT", "idempotencyKey must be non-empty ASCII without whitespace and at most 128 characters.");
        if (request.ClientId.Length > 200)
            return Error("INVALID_INPUT", "clientId is too long.");
        return null;
    }

    private static string ComputePayloadHash(McpListItemAddRequest request)
    {
        var canonical = JsonSerializer.Serialize(
        new[]
        {
            request.ListId.ToString("D"),
            request.Name.Trim(),
            NormalizeOptional(request.Quantity) ?? string.Empty,
            NormalizeOptional(request.Note) ?? string.Empty });
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(canonical))).ToLowerInvariant();
    }

    public async Task PurgeExpiredAsync(CancellationToken cancellationToken = default)
    {
        var now = UtcNow();
        var expiredApprovals = await _db.McpWriteApprovals
            .Where(x => x.Status == McpWriteApprovalStatuses.Pending && x.ExpiresAtUtc <= now)
            .ToListAsync(cancellationToken);
        foreach (var approval in expiredApprovals)
        {
            approval.Status = McpWriteApprovalStatuses.Expired;
            await MarkIdempotencyCancelledAsync(approval.Id, now, cancellationToken);
        }

        var expiredResults = await _db.McpIdempotency
            .Where(x => x.Status == McpIdempotencyStatuses.Completed &&
                        x.ResultJson != null &&
                        x.ResultExpiresAtUtc != null &&
                        x.ResultExpiresAtUtc <= now)
            .ToListAsync(cancellationToken);
        foreach (var result in expiredResults)
            result.ResultJson = null;

        var expiredTombstones = await _db.McpIdempotency
            .Where(x => x.TombstoneUntilUtc != null && x.TombstoneUntilUtc <= now)
            .ToListAsync(cancellationToken);
        if (expiredTombstones.Count > 0)
        {
            var approvalIds = expiredTombstones
                .Where(x => x.ApprovalRequestId.HasValue)
                .Select(x => x.ApprovalRequestId!.Value)
                .ToArray();
            _db.McpIdempotency.RemoveRange(expiredTombstones);
            if (approvalIds.Length > 0)
            {
                var approvals = await _db.McpWriteApprovals
                    .Where(x => approvalIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);
                _db.McpWriteApprovals.RemoveRange(approvals);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task MarkIdempotencyCancelledAsync(
        Guid approvalRequestId,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var idempotency = await _db.McpIdempotency
            .SingleOrDefaultAsync(x => x.ApprovalRequestId == approvalRequestId, cancellationToken);
        if (idempotency is not null &&
            idempotency.Status == McpIdempotencyStatuses.PendingApproval)
        {
            idempotency.Status = McpIdempotencyStatuses.Cancelled;
            idempotency.TombstoneUntilUtc = now.AddDays(_options.TombstoneRetentionDays);
        }
    }

    private static McpListItemAddAction ToAction(McpListItemAddRequest request) =>
        new("list_item_add", request.ListId, request.Name.Trim(), NormalizeOptional(request.Quantity), NormalizeOptional(request.Note));

    private static McpWriteApprovalView ToView(McpWriteApprovalRecord approval) =>
        new(approval.Id, approval.FamilyId, approval.ListId, approval.Name, approval.Quantity, approval.Note,
            approval.Status, approval.CreatedAtUtc, approval.ExpiresAtUtc);

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private DateTime UtcNow() => _clock.GetUtcNow().UtcDateTime;

    private async Task<IDbContextTransaction?> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return _db.Database.IsRelational()
            ? await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken)
            : null;
    }

    private static Task CommitAsync(
        IDbContextTransaction? transaction,
        CancellationToken cancellationToken) =>
        transaction is null ? Task.CompletedTask : transaction.CommitAsync(cancellationToken);

    private static McpConfirmedWriteOutcome Error(string code, string message) =>
        new(McpWriteStatuses.Error, code, message);
}
