using DomusMind.Api.Calendar;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Calendar.CalendarSubscriptions;
using DomusMind.Contracts.Calendar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers;

[ApiController]
[Route("api/calendar/subscriptions")]
public sealed class CalendarSubscriptionsController(ICurrentUser currentUser) : ControllerBase
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateCalendarSubscriptionResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateCalendarSubscriptionRequest request,
        [FromServices] ICommandDispatcher commands, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId) return Unauthorized();
        try
        {
            var created = await commands.Dispatch(new CreateCalendarSubscriptionCommand(
                request.FamilyId, request.Label, request.MemberId, userId), cancellationToken);
            var url = $"{Request.Scheme}://{Request.Host}/calendar/subscriptions/{created.Token}.ics";
            return Created($"/api/calendar/subscriptions/{created.Id}",
                new CreateCalendarSubscriptionResponse(created.Id, created.Label, created.MemberId,
                    created.ExpiresAtUtc, url));
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IReadOnlyCollection<CalendarSubscriptionSummary>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromQuery] Guid familyId,
        [FromServices] IQueryDispatcher queries, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId) return Unauthorized();
        try
        {
            var result = await queries.Dispatch(new ListCalendarSubscriptionsQuery(familyId, userId), cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
    }

    [HttpDelete("{subscriptionId:guid}")]
    [Authorize]
    public async Task<IActionResult> Revoke(Guid subscriptionId,
        [FromServices] ICommandDispatcher commands, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId) return Unauthorized();
        return await commands.Dispatch(new RevokeCalendarSubscriptionCommand(subscriptionId, userId), cancellationToken)
            ? NoContent()
            : NotFound();
    }

    [HttpGet("/calendar/subscriptions/{token}.ics")]
    [AllowAnonymous]
    [Produces("text/calendar")]
    public async Task<IActionResult> ReadFeed(string token,
        [FromServices] IQueryDispatcher queries, CancellationToken cancellationToken)
    {
        if (token.Length is < 20 or > 100) return NotFound();
        var feed = await queries.Dispatch(new ReadCalendarSubscriptionFeedQuery(token), cancellationToken);
        if (feed is null) return NotFound();
        Response.Headers.CacheControl = "no-store, no-cache";
        Response.Headers.Pragma = "no-cache";
        return Content(CalendarSubscriptionIcsFormatter.Format(feed), "text/calendar; charset=utf-8");
    }
}
