using DomusMind.Contracts.Lists;
using System.Text.Json.Serialization;

namespace DomusMind.Application.Abstractions.Integrations.Mcp;

public static class McpWriteStatuses
{
    public const string ConfirmationRequired = "CONFIRMATION_REQUIRED";
    public const string ApprovalPending = "APPROVAL_PENDING";
    public const string Completed = "COMPLETED";
    public const string Error = "ERROR";
}

public sealed record McpListItemAddRequest(
    Guid UserId,
    Guid HouseholdId,
    string ClientId,
    Guid ListId,
    string Name,
    string? Quantity,
    string? Note,
    string IdempotencyKey,
    string? ApprovalRequestId,
    string TraceId);

public sealed record McpListItemAddAction(
    [property: JsonPropertyName("operation")] string Operation,
    [property: JsonPropertyName("listId")] Guid ListId,
    [property: JsonPropertyName("itemName")] string ItemName,
    [property: JsonPropertyName("quantity")] string? Quantity,
    [property: JsonPropertyName("note")] string? Note);

public sealed record McpConfirmedWriteOutcome(
    string Status,
    string? Code = null,
    string? Message = null,
    Guid? ApprovalRequestId = null,
    DateTime? ExpiresAtUtc = null,
    McpListItemAddAction? Action = null,
    AddItemToListResponse? Result = null);

public sealed record McpWriteApprovalView(
    Guid ApprovalRequestId,
    Guid HouseholdId,
    Guid ListId,
    string Name,
    string? Quantity,
    string? Note,
    string Status,
    DateTime CreatedAtUtc,
    DateTime ExpiresAtUtc);

public sealed record McpApprovalContext(
    string? ClientId,
    Guid? HouseholdId,
    bool HasWriteScope,
    bool IsTrustedFirstParty = false,
    string? TraceId = null)
{
    public static McpApprovalContext TrustedFirstParty { get; } = new(null, null, false, true);
}

public interface IMcpConfirmedWriteCoordinator
{
    Task<McpConfirmedWriteOutcome> ExecuteListItemAddAsync(
        McpListItemAddRequest request,
        CancellationToken cancellationToken = default);

    Task<McpWriteApprovalView?> GetApprovalAsync(
        Guid approvalRequestId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<McpConfirmedWriteOutcome> ApproveAsync(
        Guid approvalRequestId,
        Guid userId,
        McpApprovalContext context,
        CancellationToken cancellationToken = default);

    Task<McpConfirmedWriteOutcome> CancelAsync(
        Guid approvalRequestId,
        Guid userId,
        McpApprovalContext context,
        CancellationToken cancellationToken = default);
}

public interface IMcpConfirmedWriteMaintenance
{
    Task PurgeExpiredAsync(CancellationToken cancellationToken = default);
}
