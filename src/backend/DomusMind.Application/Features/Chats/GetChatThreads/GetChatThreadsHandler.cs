using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Chats;
using DomusMind.Domain.Chats;
using DomusMind.Domain.Family;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Chats.GetChatThreads;

public sealed class GetChatThreadsHandler(IDomusMindDbContext db, IFamilyAuthorizationService auth)
    : IQueryHandler<GetChatThreadsQuery, IReadOnlyList<ChatThreadModel>>
{
    public async Task<IReadOnlyList<ChatThreadModel>> Handle(GetChatThreadsQuery query, CancellationToken ct)
    {
        if (!await auth.CanAccessFamilyAsync(query.UserId, query.FamilyId, ct))
            throw new UnauthorizedAccessException();

        var familyId = FamilyId.From(query.FamilyId);
        var family = await db.Set<Domain.Family.Family>().AsNoTracking().Include(x => x.Members)
            .SingleOrDefaultAsync(x => x.Id == familyId, ct) ?? throw new KeyNotFoundException("Household was not found.");
        var memberId = family.Members.SingleOrDefault(x => x.AuthUserId == query.UserId)?.Id.Value
            ?? throw new UnauthorizedAccessException();

        var threads = await db.Set<ChatThread>().AsNoTracking().Include(x => x.Participants)
            .Where(x => x.FamilyId == familyId).OrderByDescending(x => x.CreatedAtUtc).ToListAsync(ct);
        return threads.Where(x => x.Participants.Any(p => p.MemberId == memberId))
            .Select(x => new ChatThreadModel(x.Id.Value, x.FamilyId.Value, x.Title, x.Kind,
                x.CreatedByMemberId, x.Participants.Select(p => p.MemberId).ToArray(), x.CreatedAtUtc)).ToArray();
    }
}
