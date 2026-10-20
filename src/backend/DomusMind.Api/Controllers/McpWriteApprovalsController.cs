using DomusMind.Application.Abstractions.Integrations.Mcp;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = McpOAuthConstants.CookieScheme)]
[Route("api/mcp/write-approvals")]
public sealed class McpWriteApprovalsController : ControllerBase
{
    private readonly ICurrentUser _currentUser;
    private readonly IMcpConfirmedWriteCoordinator _coordinator;

    public McpWriteApprovalsController(
        ICurrentUser currentUser,
        IMcpConfirmedWriteCoordinator coordinator)
    {
        _currentUser = currentUser;
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(
        Guid approvalRequestId,
        CancellationToken cancellationToken)
    {
        if (_currentUser.UserId is not Guid userId)
            return Unauthorized();
        var outcome = await _coordinator.ApproveAsync(
            approvalRequestId,
            userId,
            McpApprovalContext.TrustedFirstParty,
            cancellationToken);
        return ToActionResult(outcome);
    }

    [HttpPost("{approvalRequestId:guid}/cancel")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(
        Guid approvalRequestId,
        CancellationToken cancellationToken)
    {
        if (_currentUser.UserId is not Guid userId)
            return Unauthorized();
        var outcome = await _coordinator.CancelAsync(
            approvalRequestId,
            userId,
            McpApprovalContext.TrustedFirstParty,
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
