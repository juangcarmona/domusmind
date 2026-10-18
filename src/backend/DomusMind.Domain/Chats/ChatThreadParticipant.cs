namespace DomusMind.Domain.Chats;

public sealed class ChatThreadParticipant
{
    public Guid MemberId { get; private set; }

    private ChatThreadParticipant(Guid memberId) => MemberId = memberId;
    private ChatThreadParticipant() { }

    internal static ChatThreadParticipant Create(Guid memberId) => new(memberId);
}
