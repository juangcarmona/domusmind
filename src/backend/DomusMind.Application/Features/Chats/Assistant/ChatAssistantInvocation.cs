namespace DomusMind.Application.Features.Chats.Assistant;

public static class ChatAssistantInvocation
{
    public static bool IsExplicit(string text)
    {
        var value = text.TrimStart();
        return value.StartsWith("@dm", StringComparison.OrdinalIgnoreCase)
            && (value.Length == 3 || char.IsWhiteSpace(value[3]) || value[3] is ',' or ':');
    }

    public static string GetQuestion(string text)
        => text.TrimStart()[3..].TrimStart(' ', ',', ':');
}
