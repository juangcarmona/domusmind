using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Admin;
using DomusMind.Application.Features.Admin.CreateOperatorInvitation;
using DomusMind.Application.Features.Admin.DisableUser;
using DomusMind.Application.Features.Admin.EnableUser;
using DomusMind.Application.Features.Admin.GetAdminHouseholds;
using DomusMind.Application.Features.Admin.GetAdminSummary;
using DomusMind.Application.Features.Admin.GetAdminUsers;
using DomusMind.Application.Features.Admin.ListOperatorInvitations;
using DomusMind.Application.Features.Admin.RevokeOperatorInvitation;
using DomusMind.Contracts.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Policy = "Operator")]
public sealed class AdminController : ControllerBase
{
    private readonly ICurrentUser _currentUser;

    public AdminController(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    // ── Diagnostics ────────────────────────────────────────────────────────────

    [HttpGet("summary")]
    [ProducesResponseType(typeof(AdminSummaryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary(
        [FromServices] IQueryDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.Dispatch(new GetAdminSummaryQuery(), cancellationToken);
        return Ok(result);
    }

    // ── Households ─────────────────────────────────────────────────────────────

    [HttpGet("households")]
    [ProducesResponseType(typeof(AdminHouseholdListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHouseholds(
        [FromQuery] string? search,
        [FromServices] IQueryDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.Dispatch(new GetAdminHouseholdsQuery(search), cancellationToken);
        return Ok(result);
    }

    // ── Users ──────────────────────────────────────────────────────────────────

    [HttpGet("users")]
    [ProducesResponseType(typeof(AdminUserListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(
        [FromQuery] string? search,
        [FromServices] IQueryDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.Dispatch(new GetAdminUsersQuery(search), cancellationToken);
        return Ok(result);
    }

    [HttpPost("users/{userId:guid}/disable")]
    [ProducesResponseType(typeof(DisableUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> DisableUser(
        Guid userId,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await dispatcher.Dispatch(new DisableUserCommand(userId), cancellationToken);
            return Ok(result);
        }
        catch (AdminException ex) when (ex.Code == AdminErrorCode.UserNotFound)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (AdminException ex) when (ex.Code == AdminErrorCode.CannotDisableOperator)
        {
            return UnprocessableEntity(new { error = ex.Message });
        }
    }

    [HttpPost("users/{userId:guid}/enable")]
    [ProducesResponseType(typeof(EnableUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EnableUser(
        Guid userId,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await dispatcher.Dispatch(new EnableUserCommand(userId), cancellationToken);
            return Ok(result);
        }
        catch (AdminException ex) when (ex.Code == AdminErrorCode.UserNotFound)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    // ── Operator Invitations ───────────────────────────────────────────────────

    [HttpGet("invitations")]
    [ProducesResponseType(typeof(OperatorInvitationListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInvitations(
        [FromServices] IQueryDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.Dispatch(new ListOperatorInvitationsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("invitations")]
    [ProducesResponseType(typeof(CreateOperatorInvitationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateInvitation(
        [FromBody] CreateOperatorInvitationRequest request,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await dispatcher.Dispatch(
                new CreateOperatorInvitationCommand(
                    request.Email,
                    request.Note,
                    _currentUser.UserId!.Value),
                cancellationToken);

            return Created($"api/admin/invitations", result);
        }
        catch (AdminException ex) when (ex.Code == AdminErrorCode.InvalidInput)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("invitations/{invitationId:guid}/revoke")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RevokeInvitation(
        Guid invitationId,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            await dispatcher.Dispatch(new RevokeOperatorInvitationCommand(invitationId), cancellationToken);
            return NoContent();
        }
        catch (AdminException ex) when (ex.Code == AdminErrorCode.InvitationNotFound)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (AdminException ex) when (ex.Code == AdminErrorCode.InvitationNotRevocable)
        {
            return UnprocessableEntity(new { error = ex.Message });
        }
    }
}
