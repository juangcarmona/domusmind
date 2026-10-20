using DomusMind.Application.Abstractions.Integrations.Mcp;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Mcp.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/mcp/write-approvals")]
public sealed class McpWriteApprovalsController : ControllerBase
{
    private readonly ICurrentUser _currentUser;
    private readonly IMcpRequestIdentityAccessor _mcpIdentity;
    private readonly IMcpConfirmedWriteCoordinator _coordinator;

    public McpWriteApprovalsController(
        ICurrentUser currentUser,
        IMcpRequestIdentityAccessor mcpIdentity,
        IMcpConfirmedWriteCoordinator coordinator)
    {
        _currentUser = currentUser;
        _mcpIdentity = mcpIdentity;
        _coordinator = coordinator;
    }

    [HttpGet("{approvalRequestId:guid}")]
    public async Task<IActionResult> Get(
        Guid approvalRequestId,
        CancellationToken cancellationToken)
    {
        if (_currentUser.UserId is not Guid userId)
            return Unauthorized();

        var approval = await _coordinator.GetApprovalAsync(approvalRequestId, userId, cancellationToken);
        return approval is null ? NotFound() : Ok(approval);
    }

    [HttpPost("{approvalRequestId:guid}/approve")]
    public async Task<IActionResult> Approve(
        Guid approvalRequestId,
        CancellationToken cancellationToken)
    {
        if (_currentUser.UserId is not Guid userId)
            return Unauthorized();
        if (!_mcpIdentity.TryGet(out var identity) || identity.UserId != userId)
            return Forbid();

        var outcome = await _coordinator.ApproveAsync(
            approvalRequestId,
            userId,
            new McpApprovalContext(identity.ClientId, identity.HouseholdId, identity.HasScope("mcp.write")),
            cancellationToken);
        return ToActionResult(outcome);
    }

    [HttpPost("{approvalRequestId:guid}/cancel")]
    public async Task<IActionResult> Cancel(
        Guid approvalRequestId,
        CancellationToken cancellationToken)
    {
        if (_currentUser.UserId is not Guid userId)
            return Unauthorized();
        if (!_mcpIdentity.TryGet(out var identity) || identity.UserId != userId)
            return Forbid();

        var outcome = await _coordinator.CancelAsync(
            approvalRequestId,
            userId,
            new McpApprovalContext(identity.ClientId, identity.HouseholdId, identity.HasScope("mcp.write")),
            cancellationToken);
        return ToActionResult(outcome);
    }

    private IActionResult ToActionResult(McpConfirmedWriteOutcome outcome)
    {
        if (outcome.Status != McpWriteStatuses.Error)
            return Ok(outcome);

        return outcome.Code switch
        {
            "CONFIRMATION_UNKNOWN" => NotFound(outcome),
            "CONFIRMATION_INVALID" or "CONFIRMATION_EXPIRED" => Conflict(outcome),
            "AUTHORIZATION_DENIED" => Forbid(),
            _ => BadRequest(outcome),
        };
    }
}
