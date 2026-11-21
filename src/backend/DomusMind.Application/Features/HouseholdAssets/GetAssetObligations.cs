using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.HouseholdAssets;
using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.HouseholdAssets;

public sealed record GetAssetObligationsQuery(Guid FamilyId, Guid AssetId, Guid RequestedByUserId) : IQuery<IReadOnlyList<AssetObligationResponse>>;
public sealed class GetAssetObligationsQueryHandler(IDomusMindDbContext db, IFamilyAuthorizationService authorization,
    TimeProvider? clock = null) : IQueryHandler<GetAssetObligationsQuery, IReadOnlyList<AssetObligationResponse>>
{
    public async Task<IReadOnlyList<AssetObligationResponse>> Handle(GetAssetObligationsQuery query, CancellationToken ct)
    {
        await AssetAccess.Authorize(authorization, query.RequestedByUserId, query.FamilyId, ct);
        var asset = await db.Set<Asset>().AsNoTracking().SingleOrDefaultAsync(x => x.Id == AssetId.From(query.AssetId)
            && x.FamilyId == FamilyId.From(query.FamilyId), ct)
            ?? throw new AssetsException(AssetsErrorCode.NotFound, "Asset was not found in this household.");
        var obligations = await ObligationAccess.Full(db).AsNoTracking().Where(x => x.FamilyId == asset.FamilyId && x.AssetId == asset.Id)
            .OrderBy(x => x.Name).ThenBy(x => x.Id).ToListAsync(ct);
        if (obligations.Count == 0) return [];
        var zone = await ObligationAccess.TimeZone(db, query.FamilyId, ct);
        return obligations.Select(x => ObligationAccess.Map(x, zone, (clock ?? TimeProvider.System).GetUtcNow(), asset.IsArchived)).ToArray();
    }
}
