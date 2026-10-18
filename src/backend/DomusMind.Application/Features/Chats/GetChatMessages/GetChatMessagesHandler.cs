using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Chats;
using DomusMind.Domain.Chats;
using DomusMind.Domain.Chats.ValueObjects;
using DomusMind.Domain.Family;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Chats.GetChatMessages;

public sealed class GetChatMessagesHandler(IDomusMindDbContext db, IFamilyAuthorizationService auth)
    : IQueryHandler<GetChatMessagesQuery, IReadOnlyList<ChatMessageModel>>
{
    public async Task<IReadOnlyList<ChatMessageModel>> Handle(GetChatMessagesQuery query, CancellationToken ct)
    {
        var threadId = ChatThreadId.From(query.ChatThreadId);
        var thread = await db.Set<ChatThread>().AsNoTracking().Include(x => x.Participants)
            .SingleOrDefaultAsync(x => x.Id == threadId, ct) ?? throw new KeyNotFoundException("Chat thread was not found.");
        if (!await auth.CanAccessFamilyAsync(query.UserId, thread.FamilyId.Value, ct))
            throw new UnauthorizedAccessException();

        var family = await db.Set<Domain.Family.Family>().AsNoTracking().Include(x => x.Members)
            .SingleAsync(x => x.Id == thread.FamilyId, ct);
        var member = family.Members.SingleOrDefault(x => x.AuthUserId == query.UserId);
        if (member is null || !thread.Participants.Any(x => x.MemberId == member.Id.Value))
            throw new UnauthorizedAccessException();

        var recent = await db.Set<ChatMessage>().AsNoTracking().Where(x => x.ChatThreadId == query.ChatThreadId)
            .OrderByDescending(x => x.CreatedAtUtc).Take(100).ToListAsync(ct);
        var names = family.Members.ToDictionary(x => x.Id.Value, x => x.DisplayName);
        return recent.OrderBy(x => x.CreatedAtUtc).Select(x => new ChatMessageModel(x.Id.Value,
            x.ChatThreadId, x.AuthorMemberId, x.AuthorType,
            x.AuthorType == "Assistant" ? "DomusMind" : names.GetValueOrDefault(x.AuthorMemberId!.Value, "Household member"),
            x.Text, x.ReplyToMessageId, x.CreatedAtUtc, x.ProviderName, x.ProviderHosting,
            x.FallbackOccurred)).ToArray();
    }
}
