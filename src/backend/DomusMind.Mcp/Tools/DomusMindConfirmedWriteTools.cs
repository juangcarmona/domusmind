using System.Text.Json;
using System.Text.Json.Serialization;
using DomusMind.Application.Abstractions.Integrations.Mcp;
using DomusMind.Mcp.Authorization;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace DomusMind.Mcp.Tools;

[McpServerToolType]
public sealed class DomusMindConfirmedWriteTools
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [McpServerTool(
        Name = "list_item_add",
        ReadOnly = false,
        UseStructuredContent = true,
        OutputSchemaType = typeof(ListItemAddOutput))]
    [System.ComponentModel.Description(
        "Request or execute a confirmed addition to an authorized household list. " +
        "The first call creates an approval request; a trusted DomusMind surface must approve it before retrying with approvalRequestId.")]
    public static async Task<CallToolResult> ListItemAdd(
        string listId,
        string name,
        string? quantity,
        string? note,
        string idempotencyKey,
        string? approvalRequestId,
        IMcpRequestIdentityAccessor identityAccessor,
        IMcpConfirmedWriteCoordinator coordinator,
        ILogger<DomusMindConfirmedWriteTools> logger,
        CancellationToken cancellationToken)
    {
        if (!identityAccessor.TryGet(out var identity))
            return Error("ACCESS_DENIED", "Access to this household is denied.");
        if (!identity.HasScope("mcp.write"))
            return Error("ACCESS_DENIED", "The mcp.write scope is required for this operation.");
        if (!Guid.TryParse(listId, out var parsedListId) || parsedListId == Guid.Empty)
            return Error("INVALID_INPUT", "listId must be a valid identifier.");
        if (!string.IsNullOrWhiteSpace(approvalRequestId) &&
            (!Guid.TryParse(approvalRequestId, out var parsedApprovalId) || parsedApprovalId == Guid.Empty))
            return Error("CONFIRMATION_INVALID", "The approval request is not valid.");

        var request = new McpListItemAddRequest(
            identity.UserId,
            identity.HouseholdId,
            identity.ClientId,
            parsedListId,
            name,
            quantity,
            note,
            idempotencyKey,
            string.IsNullOrWhiteSpace(approvalRequestId) ? null : Guid.Parse(approvalRequestId),
            identity.TraceId);

        try
        {
            var outcome = await coordinator.ExecuteListItemAddAsync(request, cancellationToken);
            logger.LogInformation(
                "MCP capability audit outcome={Outcome} capability={Capability} user={UserId} household={HouseholdId} client={ClientId} trace={TraceId} code={Code}",
                outcome.Status,
                "list_item_add",
                identity.UserId,
                identity.HouseholdId,
                identity.ClientId,
                identity.TraceId,
                outcome.Code ?? "none");

            return outcome.Status switch
            {
                McpWriteStatuses.ConfirmationRequired => Confirmation(outcome, idempotencyKey),
                McpWriteStatuses.Completed => Completed(outcome),
                McpWriteStatuses.ApprovalPending => Error("APPROVAL_PENDING", "The requested operation is awaiting approval."),
                _ => Error(MapErrorCode(outcome.Code), outcome.Message ?? "The operation could not be completed."),
            };
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return Error("CANCELLED", "The operation was cancelled before commit.");
        }
        catch (Exception)
        {
            logger.LogWarning(
                "MCP capability audit outcome=internal_error capability={Capability} user={UserId} household={HouseholdId} client={ClientId} trace={TraceId}",
                "list_item_add",
                identity.UserId,
                identity.HouseholdId,
                identity.ClientId,
                identity.TraceId);
            return Error("INTERNAL_ERROR", "The operation could not be completed.");
        }
    }

    private static CallToolResult Confirmation(
        McpConfirmedWriteOutcome outcome,
        string idempotencyKey) =>
        new()
        {
            IsError = false,
            StructuredContent = JsonSerializer.SerializeToElement(new
            {
                status = McpWriteStatuses.ConfirmationRequired,
                approvalRequestId = outcome.ApprovalRequestId,
                expiresAt = outcome.ExpiresAtUtc,
                action = outcome.Action,
                idempotencyKey,
            }, JsonOptions),
            Content = [],
        };

    private static CallToolResult Completed(McpConfirmedWriteOutcome outcome) =>
        new()
        {
            IsError = false,
            StructuredContent = JsonSerializer.SerializeToElement(new
            {
                status = McpWriteStatuses.Completed,
                itemId = outcome.Result!.ItemId,
                listId = outcome.Result.ListId,
                name = outcome.Result.Name,
                @checked = outcome.Result.Checked,
                quantity = outcome.Result.Quantity,
                note = outcome.Result.Note,
            }, JsonOptions),
            Content = [],
        };

    private static CallToolResult Error(string code, string message) =>
        new()
        {
            IsError = true,
            StructuredContent = JsonSerializer.SerializeToElement(new
            {
                status = McpWriteStatuses.Error,
                error = new { code, message },
            }, JsonOptions),
            Content = [new TextContentBlock { Text = message }],
        };

    private static string MapErrorCode(string? code) => code switch
    {
        "AUTHORIZATION_DENIED" or "INSUFFICIENT_SCOPE" => "ACCESS_DENIED",
        "IDEMPOTENCY_KEY_REUSED" => "IDEMPOTENCY_CONFLICT",
        "IDEMPOTENCY_REPLAYED" or "IDEMPOTENCY_RESULT_EXPIRED" => "DUPLICATE_OPERATION",
        "CONFIRMATION_UNKNOWN" or "CONFIRMATION_EXPIRED" => "CONFIRMATION_INVALID",
        _ => code ?? "INTERNAL_ERROR",
    };
}

public sealed record ListItemAddOutput(
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("approvalRequestId")] Guid? ApprovalRequestId,
    [property: JsonPropertyName("expiresAt")] DateTime? ExpiresAt,
    [property: JsonPropertyName("action")] McpListItemAddAction? Action,
    [property: JsonPropertyName("itemId")] Guid? ItemId,
    [property: JsonPropertyName("listId")] Guid? ListId,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("checked")] bool? Checked,
    [property: JsonPropertyName("quantity")] string? Quantity,
    [property: JsonPropertyName("note")] string? Note,
    [property: JsonPropertyName("idempotencyKey")] string? IdempotencyKey,
    [property: JsonPropertyName("error")] McpErrorOutput? Error);
