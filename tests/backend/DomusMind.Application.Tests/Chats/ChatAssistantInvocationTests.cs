using DomusMind.Application.Features.Chats.Assistant;
using FluentAssertions;

namespace DomusMind.Application.Tests.Chats;

public sealed class ChatAssistantInvocationTests
{
    [Theory]
    [InlineData("@dm what is next?", true, "what is next?")]
    [InlineData("  @DM: what is next?", true, "what is next?")]
    [InlineData("@dm, what is next?", true, "what is next?")]
    [InlineData("@dmitri hello", false, "")]
    [InlineData("hello @dm", false, "")]
    public void ExplicitInvocationRequiresALeadingMention(string text, bool expected, string question)
    {
        ChatAssistantInvocation.IsExplicit(text).Should().Be(expected);
        if (expected)
            ChatAssistantInvocation.GetQuestion(text).Should().Be(question);
    }
}
