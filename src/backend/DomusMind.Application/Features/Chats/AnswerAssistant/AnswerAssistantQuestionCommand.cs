using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Chats;

namespace DomusMind.Application.Features.Chats.AnswerAssistant;

public sealed record AnswerAssistantQuestionCommand(Guid ChatMessageId, Guid UserId)
    : ICommand<ChatMessageModel>;
