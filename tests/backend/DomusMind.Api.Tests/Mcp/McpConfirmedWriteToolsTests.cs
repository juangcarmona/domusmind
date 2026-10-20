using DomusMind.Application.Abstractions.Integrations.Mcp;
using DomusMind.Contracts.Lists;
using DomusMind.Mcp.Authorization;
using DomusMind.Mcp.Tools;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace DomusMind.Api.Tests.Mcp;

public sealed class McpConfirmedWriteToolsTests
{
    [Fact]
    public async Task Confirmation_response_uses_the_closed_success_contract()
    {
        var approvalId = Guid.NewGuid();
        var listId = Guid.NewGuid();
        var coordinator = new StubCoordinator(new McpConfirmedWriteOutcome(
            McpWriteStatuses.ConfirmationRequired,
            ApprovalRequestId: approvalId,
            ExpiresAtUtc: DateTime.UtcNow.AddMinutes(5),
            Action: new McpListItemAddAction("list_item_add", listId, "Milk", "2", "Whole")));

        var result = await DomusMindConfirmedWriteTools.ListItemAdd(
            listId.ToString(),
            "Milk",
            "key",
            new StubIdentityAccessor(),
            coordinator,
            NullLogger<DomusMindConfirmedWriteTools>.Instance,
            CancellationToken.None,
            "2",
            "Whole");

        result.IsError.Should().BeFalse();
        result.StructuredContent!.Value.GetProperty("status").GetString().Should().Be("CONFIRMATION_REQUIRED");
        result.StructuredContent.Value.GetProperty("approvalRequestId").GetGuid().Should().Be(approvalId);
        result.StructuredContent.Value.GetProperty("action").GetProperty("operation").GetString()
            .Should().Be("list_item_add");
        result.StructuredContent.Value.GetProperty("action").GetProperty("itemName").GetString().Should().Be("Milk");
        result.StructuredContent.Value.GetProperty("idempotencyKey").GetString().Should().Be("key");
    }

    [Fact]
    public async Task Pending_and_conflict_outcomes_are_error_envelopes()
    {
        var coordinator = new StubCoordinator(new McpConfirmedWriteOutcome(
            McpWriteStatuses.ApprovalPending,
            Code: "APPROVAL_PENDING",
            Message: "awaiting"));

        var result = await DomusMindConfirmedWriteTools.ListItemAdd(
            Guid.NewGuid().ToString(),
            "Milk",
            "key",
            new StubIdentityAccessor(),
            coordinator,
            NullLogger<DomusMindConfirmedWriteTools>.Instance,
            CancellationToken.None,
            approvalRequestId: Guid.NewGuid().ToString());

        result.IsError.Should().BeTrue();
        result.StructuredContent!.Value.GetProperty("status").GetString().Should().Be("ERROR");
        result.StructuredContent.Value.GetProperty("error").GetProperty("code").GetString()
            .Should().Be("APPROVAL_PENDING");
    }

    private sealed class StubIdentityAccessor : IMcpRequestIdentityAccessor
    {
        public bool TryGet(out McpRequestIdentity identity)
        {
            identity = new McpRequestIdentity(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "client",
                "trace",
                new HashSet<string>(["mcp.write"]));
            return true;
        }
    }

    private sealed class StubCoordinator(McpConfirmedWriteOutcome outcome) : IMcpConfirmedWriteCoordinator
    {
        public Task<McpConfirmedWriteOutcome> ExecuteListItemAddAsync(
            McpListItemAddRequest request,
            CancellationToken cancellationToken = default) => Task.FromResult(outcome);

        public Task<McpWriteApprovalView?> GetApprovalAsync(Guid approvalRequestId, Guid userId, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();

        public Task<McpConfirmedWriteOutcome> ApproveAsync(Guid approvalRequestId, Guid userId, McpApprovalContext context, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();

        public Task<McpConfirmedWriteOutcome> CancelAsync(Guid approvalRequestId, Guid userId, McpApprovalContext context, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();
    }
}
