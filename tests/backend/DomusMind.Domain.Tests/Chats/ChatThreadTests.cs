using DomusMind.Domain.Chats;
using DomusMind.Domain.Chats.Events;
using DomusMind.Domain.Chats.ValueObjects;
using DomusMind.Domain.Family;
using FluentAssertions;

namespace DomusMind.Domain.Tests.Chats;

public sealed class ChatThreadTests
{
    [Fact]
    public void Create_IncludesDistinctParticipantsAndEmitsEvent()
    {
        var creator = Guid.NewGuid();
        var other = Guid.NewGuid();

        var thread = ChatThread.Create(ChatThreadId.New(), FamilyId.New(), "  Planning  ", "Private",
            creator, [creator, other, other], DateTime.UtcNow);

        thread.Title.Should().Be("Planning");
        thread.Participants.Select(x => x.MemberId).Should().BeEquivalentTo([creator, other]);
        thread.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<ChatThreadCreated>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("Direct")]
    public void Create_RejectsInvalidTitleOrKind(string invalid)
    {
        var act = () => ChatThread.Create(ChatThreadId.New(), FamilyId.New(), invalid,
            invalid == "Direct" ? invalid : "Household", Guid.NewGuid(), [Guid.NewGuid(), Guid.NewGuid()], DateTime.UtcNow);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_RequiresCreatorAndAnotherParticipant()
    {
        var creator = Guid.NewGuid();

        var act = () => ChatThread.Create(ChatThreadId.New(), FamilyId.New(), "Family", "Household",
            creator, [creator], DateTime.UtcNow);

        act.Should().Throw<ArgumentException>();
    }
}
