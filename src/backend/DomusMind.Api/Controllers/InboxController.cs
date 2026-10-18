using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Features.Calendar;
using DomusMind.Application.Features.Inbox.Capture;
using DomusMind.Application.Features.Inbox.Decide;
using DomusMind.Application.Features.Inbox.Get;
using DomusMind.Application.Features.Inbox.Interpret;
using DomusMind.Application.Features.Chats.ConfigureAssistantProviders;
using DomusMind.Application.Features.Chats.Assistant;
using DomusMind.Application.Features.Tasks;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Inbox;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers;

[ApiController]
[Route("api/inbox")]
[Authorize]
public sealed class InboxController(ICurrentUser currentUser) : ControllerBase
{
    [HttpGet("interpreter")]
    public async Task<IActionResult> Interpreter(Guid familyId,
        [FromServices] IQueryDispatcher queries, CancellationToken ct)
    {
        try { return Ok(await queries.Dispatch(new GetAssistantProviderRouteQuery(familyId, currentUser.UserId!.Value), ct)); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost]
    public async Task<IActionResult> Capture(CaptureInboxItemRequest request,
        [FromServices] ICommandDispatcher commands, CancellationToken cancellationToken)
    {
        try
        {
            var item = await commands.Dispatch(new CaptureInboxItemCommand(request.FamilyId, request.Text,
                currentUser.UserId!.Value), cancellationToken);
            return Created($"/api/inbox/{item.InboxItemId}", item);
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpGet]
    public async Task<IActionResult> List(Guid familyId, [FromServices] IQueryDispatcher queries,
        CancellationToken cancellationToken)
    {
        try { return Ok(await queries.Dispatch(new GetInboxItemsQuery(familyId, currentUser.UserId!.Value), cancellationToken)); }
        catch (UnauthorizedAccessException) { return Forbid(); }
    }

    [HttpPost("{id:guid}/interpret")]
    public async Task<IActionResult> Interpret(Guid id, [FromServices] ICommandDispatcher commands,
        CancellationToken cancellationToken)
    {
        try { return Ok(await commands.Dispatch(new InterpretInboxItemCommand(id, currentUser.UserId!.Value), cancellationToken)); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (ChatAssistantUnavailableException) { return StatusCode(503, new { error = "No configured provider could interpret this Inbox note. The captured note is still saved." }); }
    }

    [HttpPost("{id:guid}/proposals/{proposalId:guid}/refine")]
    public async Task<IActionResult> Refine(Guid id, Guid proposalId, RefineInboxProposalRequest request,
        [FromServices] ICommandDispatcher commands, CancellationToken cancellationToken)
        => await Decide(new RefineInboxProposalCommand(id, proposalId, currentUser.UserId!.Value, request), commands, cancellationToken);

    [HttpPost("{id:guid}/proposals/{proposalId:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id, Guid proposalId,
        [FromServices] ICommandDispatcher commands, CancellationToken cancellationToken)
        => await Decide(new RejectInboxProposalCommand(id, proposalId, currentUser.UserId!.Value), commands, cancellationToken);

    [HttpPost("{id:guid}/proposals/{proposalId:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id, Guid proposalId,
        [FromServices] IQueryDispatcher queries, [FromServices] ICommandDispatcher commands,
        CancellationToken cancellationToken)
    {
        try
        {
            var item = await queries.Dispatch(new GetInboxItemQuery(id, currentUser.UserId!.Value), cancellationToken);
            var proposal = item?.Proposals.SingleOrDefault(x => x.ProposalId == proposalId && x.Status == "Pending");
            if (item is null || proposal is null) return NotFound();
            if (proposal.Type == "Plan" && (proposal.Date is null || proposal.EndDate is null))
                return BadRequest(new { error = "Refine this Plan with a start and end date before approving it." });
            if (proposal.Type == "Reminder" && (!proposal.TargetPlanId.HasValue || !proposal.ReminderMinutesBefore.HasValue))
                return BadRequest(new { error = "Refine this Reminder with a Plan and an offset before approving it." });
            if (proposal.Type == "TimelineNote" && proposal.Date is null)
                return BadRequest(new { error = "Refine this Timeline Note with a date before approving it." });
            if (proposal.Type == "PlanUpdate" && !proposal.TargetPlanId.HasValue)
                return BadRequest(new { error = "Refine this Plan update with a Plan before approving it." });
            if (proposal.Type == "ListItemAdd" && !proposal.TargetListId.HasValue)
                return BadRequest(new { error = "Refine this List Item proposal with a List before approving it." });
            if (proposal.Type == "ListItemUpdate" && (!proposal.TargetListId.HasValue || !proposal.TargetItemId.HasValue))
                return BadRequest(new { error = "Refine this List Item update with a List and item before approving it." });
            return Ok(await commands.Dispatch(new ApproveInboxProposalCommand(id, proposalId,
                currentUser.UserId!.Value), cancellationToken));
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (CalendarException ex) { return MapCalendarException(ex); }
        catch (TasksException ex) { return MapTasksException(ex); }
    }

    private IActionResult MapCalendarException(CalendarException ex) => ex.Code switch
    {
        CalendarErrorCode.EventNotFound or CalendarErrorCode.ParticipantNotFound or CalendarErrorCode.ReminderOffsetNotFound
            => NotFound(new { error = ex.Message }),
        CalendarErrorCode.AccessDenied => Forbid(),
        CalendarErrorCode.InvalidInput => BadRequest(new { error = ex.Message }),
        _ => Conflict(new { error = ex.Message }),
    };

    private IActionResult MapTasksException(TasksException ex) => ex.Code switch
    {
        TasksErrorCode.TaskNotFound or TasksErrorCode.RoutineNotFound => NotFound(new { error = ex.Message }),
        TasksErrorCode.AccessDenied => Forbid(),
        TasksErrorCode.InvalidInput => BadRequest(new { error = ex.Message }),
        _ => Conflict(new { error = ex.Message }),
    };

    private async Task<IActionResult> Decide<TCommand>(TCommand command, ICommandDispatcher commands, CancellationToken ct)
        where TCommand : DomusMind.Application.Abstractions.Messaging.ICommand<InboxItemModel>
    {
        try { return Ok(await commands.Dispatch(command, ct)); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }
    }
}
