using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.HouseholdAssets;
using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.HouseholdAssets;

public sealed record GetAssetCategoriesQuery(Guid FamilyId, Guid RequestedByUserId) : IQuery<IReadOnlyList<string>>;

public sealed class GetAssetCategoriesQueryHandler(IFamilyAuthorizationService authorization)
    : IQueryHandler<GetAssetCategoriesQuery, IReadOnlyList<string>>
{
    public async Task<IReadOnlyList<string>> Handle(GetAssetCategoriesQuery query, CancellationToken cancellationToken)
    {
        await AssetAccess.Authorize(authorization, query.RequestedByUserId, query.FamilyId, cancellationToken);
        return AssetCategories.All;
    }
}
