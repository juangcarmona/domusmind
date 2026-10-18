using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Chats.Assistant;
using DomusMind.Contracts.Chats;
using DomusMind.Domain.Chats;
using DomusMind.Domain.Chats.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Chats.AnswerAssistant;

public sealed class AnswerAssistantQuestionHandler(IDomusMindDbContext db, IEventLogWriter events,
    IFamilyAuthorizationService auth, IAssistantIntentClassifier classifier,
    IAssistantToolRouter tools, IChatAssistant assistant)
    : ICommandHandler<AnswerAssistantQuestionCommand, ChatMessageModel>
{
    public async Task<ChatMessageModel> Handle(AnswerAssistantQuestionCommand command, CancellationToken ct)
    {
        var message = await db.Set<ChatMessage>().AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == ChatMessageId.From(command.ChatMessageId), ct)
            ?? throw new KeyNotFoundException("Chat message was not found.");
        if (message.AuthorType != "Member" || !ChatAssistantInvocation.IsExplicit(message.Text))
            throw new InvalidOperationException("Only a member message explicitly addressed to @dm can be answered.");
        if (!await auth.CanAccessFamilyAsync(command.UserId, message.FamilyId.Value, ct))
            throw new UnauthorizedAccessException();

        var thread = await db.Set<ChatThread>().AsNoTracking().Include(x => x.Participants)
            .SingleAsync(x => x.Id == ChatThreadId.From(message.ChatThreadId), ct);
        var family = await db.Set<Domain.Family.Family>().AsNoTracking().Include(x => x.Members)
            .SingleAsync(x => x.Id == message.FamilyId, ct);
        var member = family.Members.SingleOrDefault(x => x.AuthUserId == command.UserId);
        if (member is null || !thread.Participants.Any(x => x.MemberId == member.Id.Value))
            throw new UnauthorizedAccessException();

        var existing = await db.Set<ChatMessage>().AsNoTracking()
            .SingleOrDefaultAsync(x => x.ReplyToMessageId == message.Id.Value, ct);
        if (existing is not null)
            return Map(existing);

        var question = ChatAssistantInvocation.GetQuestion(message.Text);
        if (string.IsNullOrWhiteSpace(question))
            return await SaveAnswer(message, "What would you like to know about your household?", ct);

        var context = await tools.GetAuthorizedContextAsync(classifier.Classify(question),
            message.FamilyId.Value, command.UserId, ct);
        var answer = await assistant.AnswerAsync(question, context, ct);
        return await SaveAnswer(message, answer, ct);
    }

    private async Task<ChatMessageModel> SaveAnswer(ChatMessage source, string text, CancellationToken ct)
    {
        var response = ChatMessage.PostAssistant(ChatMessageId.New(), source.ChatThreadId,
            source.FamilyId, text, source.Id.Value, DateTime.UtcNow);
        db.Set<ChatMessage>().Add(response);
        await events.WriteAsync([response], ct);
        response.ClearDomainEvents();
        return Map(response);
    }

    private static ChatMessageModel Map(ChatMessage message) => new(message.Id.Value,
        message.ChatThreadId, null, "Assistant", "DomusMind", message.Text,
        message.ReplyToMessageId, message.CreatedAtUtc);

}
