using System.Text.Json;
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
            return Error("INSUFFICIENT_SCOPE", "The mcp.write scope is required for this operation.");
        if (!Guid.TryParse(listId, out var parsedListId) || parsedListId == Guid.Empty)
            return Error("INVALID_INPUT", "listId must be a valid identifier.");
        if (!string.IsNullOrWhiteSpace(approvalRequestId) &&
            (!Guid.TryParse(approvalRequestId, out var parsedApprovalId) || parsedApprovalId == Guid.Empty))
            return Error("INVALID_INPUT", "approvalRequestId must be a valid identifier when supplied.");

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

        return outcome.Status == McpWriteStatuses.Error
            ? Error(outcome.Code ?? "INTERNAL_ERROR", outcome.Message ?? "The list item could not be added.")
            : Success(new ListItemAddOutput(
                outcome.Status,
                outcome.ApprovalRequestId,
                outcome.ExpiresAtUtc,
                outcome.Action,
                outcome.Result,
                outcome.Code));
    }

    private static CallToolResult Success(ListItemAddOutput output) =>
        new()
        {
            IsError = false,
            StructuredContent = JsonSerializer.SerializeToElement(output, JsonOptions),
            Content = [],
        };

    private static CallToolResult Error(string code, string message) =>
        new()
        {
            IsError = true,
            StructuredContent = JsonSerializer.SerializeToElement(new McpErrorOutput(code, message), JsonOptions),
            Content = [new TextContentBlock { Text = message }],
        };
}

public sealed record ListItemAddOutput(
    string Status,
    Guid? ApprovalRequestId,
    DateTime? ExpiresAtUtc,
    McpListItemAddAction? Action,
    DomusMind.Contracts.Lists.AddItemToListResponse? Result,
    string? Code);
