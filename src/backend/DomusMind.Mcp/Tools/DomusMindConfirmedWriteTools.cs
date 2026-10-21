using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Reflection;
using DomusMind.Application.Abstractions.Integrations.Mcp;
using DomusMind.Mcp.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace DomusMind.Mcp.Tools;

[McpServerToolType]
public sealed class DomusMindConfirmedWriteTools
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static McpServerTool CreateTool(IServiceProvider services)
    {
        var method = typeof(DomusMindConfirmedWriteTools).GetMethod(
            nameof(ListItemAdd), BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("The list_item_add MCP tool method was not found.");

        var innerTool = McpServerTool.Create(method, (object?)null, new McpServerToolCreateOptions
        {
            Services = services,
            Name = "list_item_add",
            Description = "Request or execute a confirmed addition to an authorized household list.",
            ReadOnly = false,
            Destructive = false,
            Idempotent = true,
            UseStructuredContent = true,
            OutputSchema = JsonDocument.Parse(OutputSchemaJson).RootElement.Clone(),
            SerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            },
            SchemaCreateOptions = new Microsoft.Extensions.AI.AIJsonSchemaCreateOptions
            {
                TransformOptions = new Microsoft.Extensions.AI.AIJsonSchemaTransformOptions
                {
                    DisallowAdditionalProperties = true,
                },
            },
        });

        return new ValidatingTool(innerTool, services);
    }

    private const string OutputSchemaJson = """
        {
          "oneOf": [
            {
              "type": "object",
              "properties": {
                "status": { "const": "CONFIRMATION_REQUIRED" },
                "approvalRequestId": { "type": "string", "format": "uuid" },
                "expiresAt": { "type": "string", "format": "date-time" },
                "action": {
                  "type": "object",
                  "properties": {
                    "operation": { "const": "list_item_add" },
                    "listId": { "type": "string", "format": "uuid" },
                    "itemName": { "type": "string" },
                    "quantity": { "type": ["string", "null"] },
                    "note": { "type": ["string", "null"] }
                  },
                  "required": ["operation", "listId", "itemName", "quantity", "note"],
                  "additionalProperties": false
                },
                "idempotencyKey": { "type": "string" }
              },
              "required": ["status", "approvalRequestId", "expiresAt", "action", "idempotencyKey"],
              "additionalProperties": false
            },
            {
              "type": "object",
              "properties": {
                "status": { "const": "COMPLETED" },
                "itemId": { "type": "string", "format": "uuid" },
                "listId": { "type": "string", "format": "uuid" },
                "name": { "type": "string" },
                "checked": { "type": "boolean" },
                "quantity": { "type": ["string", "null"] },
                "note": { "type": ["string", "null"] }
              },
              "required": ["status", "itemId", "listId", "name", "checked", "quantity", "note"],
              "additionalProperties": false
            },
            {
              "type": "object",
              "properties": {
                "status": { "const": "ERROR" },
                "error": {
                  "type": "object",
                  "properties": {
                    "code": { "type": "string" },
                    "message": { "type": "string" }
                  },
                  "required": ["code", "message"],
                  "additionalProperties": false
                }
              },
              "required": ["status", "error"],
              "additionalProperties": false
            }
          ]
        }
        """;

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
        string idempotencyKey,
        IMcpRequestIdentityAccessor identityAccessor,
        IMcpConfirmedWriteCoordinator coordinator,
        ILogger<DomusMindConfirmedWriteTools> logger,
        CancellationToken cancellationToken,
        string? quantity = null,
        string? note = null,
        string? approvalRequestId = null)
    {
        if (!identityAccessor.TryGet(out var identity))
            return Audited(logger, Error("ACCESS_DENIED", "Access to this household is denied."), identityAccessor);
        if (!identity.HasScope("mcp.write"))
            return Audited(logger, Error("ACCESS_DENIED", "The mcp.write scope is required for this operation."), identityAccessor, identity);
        if (!Guid.TryParse(listId, out var parsedListId) || parsedListId == Guid.Empty)
            return Audited(logger, Error("INVALID_INPUT", "listId must be a valid identifier."), identityAccessor, identity);

        var request = new McpListItemAddRequest(
            identity.UserId,
            identity.HouseholdId,
            identity.ClientId,
            parsedListId,
            name,
            quantity,
            note,
            idempotencyKey,
            string.IsNullOrWhiteSpace(approvalRequestId) ? null : approvalRequestId.Trim(),
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

            return Audited(logger, outcome.Status switch
            {
                McpWriteStatuses.ConfirmationRequired => Confirmation(outcome, idempotencyKey),
                McpWriteStatuses.Completed => Completed(outcome),
                McpWriteStatuses.ApprovalPending => Error("APPROVAL_PENDING", "The requested operation is awaiting approval."),
                _ => Error(MapErrorCode(outcome.Code), outcome.Message ?? "The operation could not be completed."),
            }, identityAccessor, identity);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return Audited(logger, Error("CANCELLED", "The operation was cancelled before commit."), identityAccessor, identity);
        }
        catch (Exception)
        {
            return Audited(logger, Error("INTERNAL_ERROR", "The operation could not be completed."), identityAccessor, identity);
        }
    }

    private static CallToolResult Audited(
        ILogger logger,
        CallToolResult result,
        IMcpRequestIdentityAccessor identityAccessor,
        McpRequestIdentity? identity = null)
    {
        identity ??= identityAccessor.TryGet(out var current) ? current : null;
        var code = result.StructuredContent is { } structured &&
                   structured.TryGetProperty("error", out var error) &&
                   error.TryGetProperty("code", out var errorCode)
            ? errorCode.GetString()
            : null;
        logger.LogInformation(
            "MCP capability audit {Outcome} capability={Capability} code={Code} user={UserId} household={HouseholdId} client={ClientId} trace={TraceId}",
            result.IsError == true ? "tool_error" : "success",
            "list_item_add",
            code ?? "none",
            identity?.UserId.ToString() ?? "unknown",
            identity?.HouseholdId.ToString() ?? "unknown",
            identity?.ClientId ?? "unknown",
            identity?.TraceId ?? "unknown");
        return result;
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

    private sealed class ValidatingTool(McpServerTool innerTool, IServiceProvider services)
        : DelegatingMcpServerTool(innerTool)
    {
        private static readonly HashSet<string> AllowedArguments =
        ["listId", "name", "idempotencyKey", "quantity", "note", "approvalRequestId"];

        public override async ValueTask<CallToolResult> InvokeAsync(
            RequestContext<CallToolRequestParams> request,
            CancellationToken cancellationToken)
        {
            var arguments = request.Params.Arguments;
            var validationError = ValidateArguments(arguments);
            if (validationError is not null)
            {
                using var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var identityAccessor = scope.ServiceProvider.GetRequiredService<IMcpRequestIdentityAccessor>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<DomusMindConfirmedWriteTools>>();
                return Audited(logger, validationError, identityAccessor);
            }

            return await base.InvokeAsync(request, cancellationToken);
        }

        private static CallToolResult? ValidateArguments(
            IDictionary<string, JsonElement>? arguments)
        {
            if (arguments is null)
                return Error("INVALID_INPUT", "Tool arguments are required.");

            var unknown = arguments.Keys.FirstOrDefault(key => !AllowedArguments.Contains(key));
            if (unknown is not null)
                return Error("INVALID_INPUT", $"The argument '{unknown}' is not supported.");

            foreach (var required in new[] { "listId", "name", "idempotencyKey" })
            {
                if (!arguments.TryGetValue(required, out var value) || !IsString(value))
                    return Error("INVALID_INPUT", $"{required} must be a string.");
            }

            foreach (var optional in new[] { "quantity", "note", "approvalRequestId" })
            {
                if (arguments.TryGetValue(optional, out var value) && !IsString(value, allowNull: true))
                    return Error("INVALID_INPUT", $"{optional} must be a string or null.");
            }

            return null;
        }

        private static bool IsString(object? value, bool allowNull = false) =>
            value switch
            {
                string => true,
                JsonElement { ValueKind: JsonValueKind.String } => true,
                null when allowNull => true,
                JsonElement { ValueKind: JsonValueKind.Null } when allowNull => true,
                _ => false,
            };
    }
}

public sealed record ListItemAddOutput(
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("approvalRequestId")] Guid? ApprovalRequestId = null,
    [property: JsonPropertyName("expiresAt")] DateTime? ExpiresAt = null,
    [property: JsonPropertyName("action")] McpListItemAddAction? Action = null,
    [property: JsonPropertyName("itemId")] Guid? ItemId = null,
    [property: JsonPropertyName("listId")] Guid? ListId = null,
    [property: JsonPropertyName("name")] string? Name = null,
    [property: JsonPropertyName("checked")] bool? Checked = null,
    [property: JsonPropertyName("quantity")] string? Quantity = null,
    [property: JsonPropertyName("note")] string? Note = null,
    [property: JsonPropertyName("idempotencyKey")] string? IdempotencyKey = null,
    [property: JsonPropertyName("error")] McpErrorOutput? Error = null);
