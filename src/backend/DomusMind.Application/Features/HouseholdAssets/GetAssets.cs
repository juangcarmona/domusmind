using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.HouseholdAssets;
using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.HouseholdAssets;

public sealed record GetAssetsQuery(Guid FamilyId, Guid RequestedByUserId, bool? Archived = null) : IQuery<IReadOnlyList<AssetResponse>>;

public sealed class GetAssetsQueryHandler(IDomusMindDbContext db, IFamilyAuthorizationService authorization)
    : IQueryHandler<GetAssetsQuery, IReadOnlyList<AssetResponse>>
{
    public async Task<IReadOnlyList<AssetResponse>> Handle(GetAssetsQuery query, CancellationToken cancellationToken)
    {
        await AssetAccess.Authorize(authorization, query.RequestedByUserId, query.FamilyId, cancellationToken);
        var assets = db.Set<Asset>().AsNoTracking().Where(x => x.FamilyId == FamilyId.From(query.FamilyId));
        if (query.Archived is { } archived) assets = assets.Where(x => x.IsArchived == archived);
        return (await assets.OrderBy(x => x.Name).ThenBy(x => x.Id).ToListAsync(cancellationToken)).Select(AssetAccess.Map).ToArray();
    }
}
