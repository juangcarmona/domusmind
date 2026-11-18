using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.HouseholdAssets;
using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.HouseholdAssets;

public sealed record GetAssetLocationsQuery(Guid FamilyId, Guid RequestedByUserId) : IQuery<IReadOnlyList<AssetLocationResponse>>;

public sealed class GetAssetLocationsQueryHandler(IDomusMindDbContext db, IFamilyAuthorizationService authorization)
    : IQueryHandler<GetAssetLocationsQuery, IReadOnlyList<AssetLocationResponse>>
{
    public async Task<IReadOnlyList<AssetLocationResponse>> Handle(GetAssetLocationsQuery query, CancellationToken cancellationToken)
    {
        await AssetAccess.Authorize(authorization, query.RequestedByUserId, query.FamilyId, cancellationToken);
        var locations = await db.Set<AssetLocation>().AsNoTracking().Where(x => x.FamilyId == FamilyId.From(query.FamilyId))
            .OrderBy(x => x.Name).ThenBy(x => x.Id).ToListAsync(cancellationToken);
        return locations.Select(x => new AssetLocationResponse(x.Id.Value, x.FamilyId.Value, x.Name, x.Address)).ToArray();
    }
}
