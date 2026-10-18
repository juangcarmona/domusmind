using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Chats.Assistant;
using DomusMind.Contracts.Chats;
using DomusMind.Domain.Chats;
using DomusMind.Domain.Chats.ValueObjects;
using DomusMind.Domain.Family;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Chats.PostChatMessage;

public sealed class PostChatMessageHandler(IDomusMindDbContext db, IEventLogWriter events,
    IFamilyAuthorizationService auth)
    : ICommandHandler<PostChatMessageCommand, PostChatMessageResult>
{
    public async Task<PostChatMessageResult> Handle(PostChatMessageCommand command, CancellationToken ct)
    {
        var thread = await db.Set<ChatThread>().Include(x => x.Participants)
            .SingleOrDefaultAsync(x => x.Id == ChatThreadId.From(command.ChatThreadId), ct)
            ?? throw new KeyNotFoundException("Chat thread was not found.");
        if (!await auth.CanAccessFamilyAsync(command.UserId, thread.FamilyId.Value, ct))
            throw new UnauthorizedAccessException();
        var family = await db.Set<Domain.Family.Family>().AsNoTracking().Include(x => x.Members)
            .SingleAsync(x => x.Id == thread.FamilyId, ct);
        var member = family.Members.SingleOrDefault(x => x.AuthUserId == command.UserId);
        if (member is null || !thread.Participants.Any(x => x.MemberId == member.Id.Value))
            throw new UnauthorizedAccessException();

        var message = ChatMessage.PostMember(ChatMessageId.New(), thread.Id.Value, thread.FamilyId,
            member.Id.Value, command.Text ?? string.Empty, DateTime.UtcNow);
        db.Set<ChatMessage>().Add(message);
        await events.WriteAsync([message], ct);
        message.ClearDomainEvents();
        var model = new ChatMessageModel(message.Id.Value, thread.Id.Value, member.Id.Value,
            "Member", member.DisplayName, message.Text, null, message.CreatedAtUtc);
        var invokesAssistant = ChatAssistantInvocation.IsExplicit(command.Text ?? string.Empty);
        return new PostChatMessageResult(model, thread.FamilyId.Value, invokesAssistant);
    }
}
