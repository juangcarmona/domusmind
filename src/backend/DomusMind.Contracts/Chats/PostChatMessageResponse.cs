namespace DomusMind.Contracts.Chats;

public sealed record PostChatMessageResponse(ChatMessageModel Message, ChatMessageModel? AssistantResponse,
    bool AssistantUnavailable);
