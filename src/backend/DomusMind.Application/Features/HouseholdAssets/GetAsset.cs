using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.HouseholdAssets;
using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.HouseholdAssets;

public sealed record GetAssetQuery(Guid FamilyId, Guid AssetId, Guid RequestedByUserId) : IQuery<AssetResponse>;

public sealed class GetAssetQueryHandler(IDomusMindDbContext db, IFamilyAuthorizationService authorization)
    : IQueryHandler<GetAssetQuery, AssetResponse>
{
    public async Task<AssetResponse> Handle(GetAssetQuery query, CancellationToken cancellationToken)
    {
        await AssetAccess.Authorize(authorization, query.RequestedByUserId, query.FamilyId, cancellationToken);
        var asset = await db.Set<Asset>().AsNoTracking().SingleOrDefaultAsync(x => x.FamilyId == FamilyId.From(query.FamilyId)
            && x.Id == AssetId.From(query.AssetId), cancellationToken)
            ?? throw new AssetsException(AssetsErrorCode.NotFound, "Asset was not found in this household.");
        return AssetAccess.Map(asset);
    }
}
