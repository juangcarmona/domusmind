using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Calendar;
using DomusMind.Application.Features.Calendar.ConfigureExternalCalendarConnection;
using DomusMind.Application.Features.Calendar.ConnectOutlookAccount;
using DomusMind.Application.Features.Calendar.DisconnectExternalCalendarConnection;
using DomusMind.Application.Features.Calendar.GetExternalCalendarConnectionDetail;
using DomusMind.Application.Features.Calendar.GetMemberExternalCalendarConnections;
using DomusMind.Application.Features.Calendar.SyncExternalCalendarConnection;
using DomusMind.Application.Features.Calendar.SyncMemberExternalCalendarConnections;
using DomusMind.Application.Features.Calendar.StartOutlookAuthorization;
using DomusMind.Contracts.Calendar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers;

[ApiController]
[Route("api/families/{familyId:guid}/members/{memberId:guid}/external-calendar-connections")]
[Authorize]
public sealed class ExternalCalendarConnectionsController : ControllerBase
{
    private readonly ICurrentUser _currentUser;

    public ExternalCalendarConnectionsController(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    /// <summary>Returns the Microsoft OAuth authorization URL for initiating and Outlook connection.</summary>
    [HttpGet("outlook/auth-url")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetOutlookAuthUrl(
        Guid familyId,
        Guid memberId,
        [FromQuery] string redirectUri,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await dispatcher.Dispatch(
                new StartOutlookAuthorizationCommand(
                    familyId,
                    memberId,
                    redirectUri,
                    _currentUser.UserId!.Value),
                cancellationToken);

            return Ok(result);
        }
        catch (CalendarException ex)
        {
            return MapCalendarException(ex);
        }
    }

    /// <summary>Returns all active external calendar connections for a member.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ExternalCalendarConnectionSummaryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetConnections(
        Guid familyId,
        Guid memberId,
        [FromServices] IQueryDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await dispatcher.Dispatch(
                new GetMemberExternalCalendarConnectionsQuery(familyId, memberId, _currentUser.UserId!.Value),
                cancellationToken);

            return Ok(result);
        }
        catch (CalendarException ex)
        {
            return MapCalendarException(ex);
        }
    }

    /// <summary>Returns full detail for a single external calendar connection.</summary>
    [HttpGet("{connectionId:guid}")]
    [ProducesResponseType(typeof(ExternalCalendarConnectionDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetConnectionDetail(
        Guid familyId,
        Guid memberId,
        Guid connectionId,
        [FromServices] IQueryDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await dispatcher.Dispatch(
                new GetExternalCalendarConnectionDetailQuery(familyId, memberId, connectionId, _currentUser.UserId!.Value),
                cancellationToken);

            return Ok(result);
        }
        catch (CalendarException ex)
        {
            return MapCalendarException(ex);
        }
    }

    /// <summary>Connects an Outlook account by exchanging an OAuth authorization code.</summary>
    [HttpPost("outlook")]
    [ProducesResponseType(typeof(ExternalCalendarConnectionDetailResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ConnectOutlook(
        Guid familyId,
        Guid memberId,
        [FromBody] ConnectOutlookAccountRequest request,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await dispatcher.Dispatch(
                new ConnectOutlookAccountCommand(
                    familyId,
                    memberId,
                    request.AuthorizationCode,
                    request.State,
                    request.RedirectUri,
                    request.AccountDisplayLabel,
                    _currentUser.UserId!.Value),
                cancellationToken);

            return Created(
                $"api/families/{familyId}/members/{memberId}/external-calendar-connections/{result.ConnectionId}",
                result);
        }
        catch (CalendarException ex)
        {
            return MapCalendarException(ex);
        }
    }

    /// <summary>Updates calendar selection and sync horizon for a connection.</summary>
    [HttpPut("{connectionId:guid}")]
    [ProducesResponseType(typeof(ConfigureExternalCalendarConnectionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfigureConnection(
        Guid familyId,
        Guid memberId,
        Guid connectionId,
        [FromBody] ConfigureExternalCalendarConnectionRequest request,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var selections = request.SelectedCalendars
                .Select(c => (c.CalendarId, c.CalendarName, c.IsSelected, c.ColorHex))
                .ToList()
                .AsReadOnly();

            var result = await dispatcher.Dispatch(
                new ConfigureExternalCalendarConnectionCommand(
                    familyId,
                    memberId,
                    connectionId,
                    selections,
                    request.ForwardHorizonDays,
                    request.ScheduledRefreshEnabled,
                    request.ScheduledRefreshIntervalMinutes,
                    _currentUser.UserId!.Value),
                cancellationToken);

            return Ok(result);
        }
        catch (CalendarException ex)
        {
            return MapCalendarException(ex);
        }
    }

    /// <summary>Triggers a manual sync for a single connection.</summary>
    [HttpPost("{connectionId:guid}/sync")]
    [ProducesResponseType(typeof(SyncExternalCalendarConnectionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SyncConnection(
        Guid familyId,
        Guid memberId,
        Guid connectionId,
        [FromBody] SyncExternalCalendarConnectionRequest request,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await dispatcher.Dispatch(
                new SyncExternalCalendarConnectionCommand(
                    familyId,
                    memberId,
                    connectionId,
                    request.Reason,
                    _currentUser.UserId!.Value),
                cancellationToken);

            return Ok(result);
        }
        catch (CalendarException ex)
        {
            return MapCalendarException(ex);
        }
    }

    /// <summary>Triggers a bulk sync for all connections belonging to a member.</summary>
    [HttpPost("sync")]
    [ProducesResponseType(typeof(SyncMemberExternalCalendarConnectionsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SyncAllConnections(
        Guid familyId,
        Guid memberId,
        [FromBody] SyncMemberExternalCalendarConnectionsRequest request,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await dispatcher.Dispatch(
                new SyncMemberExternalCalendarConnectionsCommand(
                    familyId,
                    memberId,
                    request.Reason,
                    _currentUser.UserId!.Value),
                cancellationToken);

            return Ok(result);
        }
        catch (CalendarException ex)
        {
            return MapCalendarException(ex);
        }
    }

    /// <summary>Disconnects and removes an external calendar connection.</summary>
    [HttpDelete("{connectionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DisconnectConnection(
        Guid familyId,
        Guid memberId,
        Guid connectionId,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            await dispatcher.Dispatch(
                new DisconnectExternalCalendarConnectionCommand(
                    familyId,
                    memberId,
                    connectionId,
                    _currentUser.UserId!.Value),
                cancellationToken);

            return NoContent();
        }
        catch (CalendarException ex)
        {
            return MapCalendarException(ex);
        }
    }

    private IActionResult MapCalendarException(CalendarException ex) => ex.Code switch
    {
        CalendarErrorCode.ProviderNotConfigured =>
            StatusCode(StatusCodes.Status503ServiceUnavailable, new { error = ex.Message }),
        CalendarErrorCode.AccessDenied =>
            StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message }),
        CalendarErrorCode.ConnectionNotFound =>
            NotFound(new { error = ex.Message }),
        CalendarErrorCode.ConnectionAlreadyExists =>
            Conflict(new { error = ex.Message }),
        CalendarErrorCode.ConnectionSyncInProgress =>
            Conflict(new { error = ex.Message }),
        CalendarErrorCode.ConnectionAuthExpired =>
            StatusCode(StatusCodes.Status422UnprocessableEntity, new { error = ex.Message }),
        CalendarErrorCode.ProviderAuthFailed =>
            BadRequest(new { error = ex.Message }),
        CalendarErrorCode.ProviderApiError =>
            StatusCode(StatusCodes.Status502BadGateway, new { error = ex.Message }),
        CalendarErrorCode.InvalidInput =>
            BadRequest(new { error = ex.Message }),
        _ =>
            StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message }),
    };
}
