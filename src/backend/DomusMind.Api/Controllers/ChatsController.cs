using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Chats.AnswerAssistant;
using DomusMind.Application.Features.Chats.Assistant;
using DomusMind.Application.Features.Chats.ConfigureAssistantProviders;
using DomusMind.Application.Features.Chats.CreateChatThread;
using DomusMind.Application.Features.Chats.GetChatMessages;
using DomusMind.Application.Features.Chats.GetChatThreads;
using DomusMind.Application.Features.Chats.PostChatMessage;
using DomusMind.Contracts.Chats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers;

[ApiController]
[Route("api/chats")]
[Authorize]
public sealed class ChatsController(ICurrentUser currentUser) : ControllerBase
{
    [HttpGet("assistant/providers")]
    public async Task<IActionResult> AssistantProviders(Guid familyId,
        [FromServices] IQueryDispatcher queries, CancellationToken ct)
    {
        try
        {
            return Ok(await queries.Dispatch(new GetAssistantProviderRouteQuery(
                familyId, currentUser.UserId!.Value), ct));
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPut("assistant/providers")]
    public async Task<IActionResult> ReplaceAssistantProviders(Guid familyId,
        ReplaceAssistantProviderRouteRequest request, [FromServices] ICommandDispatcher commands,
        CancellationToken ct)
    {
        try
        {
            return Ok(await commands.Dispatch(new ReplaceAssistantProviderRouteCommand(
                familyId, request.Targets, currentUser.UserId!.Value), ct));
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpGet]
    public async Task<IActionResult> GetThreads(Guid familyId, [FromServices] IQueryDispatcher queries,
        CancellationToken ct)
    {
        try { return Ok(await queries.Dispatch(new GetChatThreadsQuery(familyId, currentUser.UserId!.Value), ct)); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost]
    public async Task<IActionResult> CreateThread(CreateChatThreadRequest request,
        [FromServices] ICommandDispatcher commands, CancellationToken ct)
    {
        try
        {
            var thread = await commands.Dispatch(new CreateChatThreadCommand(request.FamilyId,
                request.Title, request.Kind, request.ParticipantMemberIds, currentUser.UserId!.Value), ct);
            return Created($"/api/chats/{thread.ChatThreadId}", thread);
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpGet("{threadId:guid}/messages")]
    public async Task<IActionResult> GetMessages(Guid threadId, [FromServices] IQueryDispatcher queries,
        CancellationToken ct)
    {
        try { return Ok(await queries.Dispatch(new GetChatMessagesQuery(threadId, currentUser.UserId!.Value), ct)); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost("{threadId:guid}/messages")]
    public async Task<IActionResult> PostMessage(Guid threadId, PostChatMessageRequest request,
        [FromServices] ICommandDispatcher commands, CancellationToken ct)
    {
        try
        {
            var posted = await commands.Dispatch(new PostChatMessageCommand(threadId,
                request.Text, currentUser.UserId!.Value), ct);
            ChatMessageModel? assistantResponse = null;
            var assistantUnavailable = false;
            if (posted.AssistantRequested)
            {
                try
                {
                    assistantResponse = await commands.Dispatch(new AnswerAssistantQuestionCommand(
                        posted.Message.ChatMessageId, currentUser.UserId!.Value), ct);
                }
                catch (ChatAssistantUnavailableException) { assistantUnavailable = true; }
            }

            return StatusCode(StatusCodes.Status201Created,
                new PostChatMessageResponse(posted.Message, assistantResponse, assistantUnavailable));
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }
}
