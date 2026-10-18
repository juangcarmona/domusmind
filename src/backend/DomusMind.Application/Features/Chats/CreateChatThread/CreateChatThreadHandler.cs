using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Chats;
using DomusMind.Domain.Chats;
using DomusMind.Domain.Chats.ValueObjects;
using DomusMind.Domain.Family;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Chats.CreateChatThread;

public sealed class CreateChatThreadHandler(IDomusMindDbContext db, IEventLogWriter events,
    IFamilyAuthorizationService auth) : ICommandHandler<CreateChatThreadCommand, ChatThreadModel>
{
    public async Task<ChatThreadModel> Handle(CreateChatThreadCommand command, CancellationToken ct)
    {
        if (!await auth.CanAccessFamilyAsync(command.UserId, command.FamilyId, ct))
            throw new UnauthorizedAccessException();

        var familyId = FamilyId.From(command.FamilyId);
        var family = await db.Set<Domain.Family.Family>().Include(x => x.Members)
            .SingleOrDefaultAsync(x => x.Id == familyId, ct) ?? throw new KeyNotFoundException("Household was not found.");
        var creator = family.Members.SingleOrDefault(x => x.AuthUserId == command.UserId)
            ?? throw new UnauthorizedAccessException();
        var members = family.Members.Select(x => x.Id.Value).ToHashSet();
        var requestedParticipants = command.ParticipantMemberIds.Append(creator.Id.Value).Distinct().ToArray();
        if (requestedParticipants.Any(x => !members.Contains(x)))
            throw new ArgumentException("Every chat participant must belong to this household.");
        var participants = command.Kind == "Household" ? members.ToArray() : requestedParticipants;

        var thread = ChatThread.Create(ChatThreadId.New(), familyId, command.Title, command.Kind,
            creator.Id.Value, participants, DateTime.UtcNow);
        db.Set<ChatThread>().Add(thread);
        await events.WriteAsync([thread], ct);
        thread.ClearDomainEvents();
        return new ChatThreadModel(thread.Id.Value, familyId.Value, thread.Title, thread.Kind,
            thread.CreatedByMemberId, participants, thread.CreatedAtUtc);
    }
}
