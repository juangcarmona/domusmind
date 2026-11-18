using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.HouseholdAssets;
using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using DomusMind.Domain.Responsibilities;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.HouseholdAssets;

internal static class AssetAccess
{
    internal static async Task Authorize(IFamilyAuthorizationService authorization, Guid userId, Guid familyId, CancellationToken ct)
    {
        if (!await authorization.CanAccessFamilyAsync(userId, familyId, ct))
            throw new AssetsException(AssetsErrorCode.AccessDenied, "Access to this household is denied.");
    }

    internal static async Task<Asset> Find(IDomusMindDbContext db, Guid familyId, Guid id, CancellationToken ct)
        => await db.Set<Asset>().SingleOrDefaultAsync(x => x.FamilyId == FamilyId.From(familyId) && x.Id == AssetId.From(id), ct)
            ?? throw new AssetsException(AssetsErrorCode.NotFound, "Asset was not found in this household.");

    internal static async Task ValidateReferences(IDomusMindDbContext db, Guid familyId, AssetId assetId,
        AssetDetailsRequest details, CancellationToken ct)
    {
        var household = FamilyId.From(familyId);
        if (details.LocationId is { } location && !await db.Set<AssetLocation>().AsNoTracking()
            .AnyAsync(x => x.FamilyId == household && x.Id == AssetLocationId.From(location), ct))
            throw InvalidReference();
        if (details.ResponsibleAreaId is { } area && !await db.Set<ResponsibilityDomain>().AsNoTracking()
            .AnyAsync(x => x.FamilyId == household && x.Id == ResponsibilityDomainId.From(area), ct))
            throw InvalidReference();
        if (details.ResponsibleMemberId is { } member && !await db.Set<Domain.Family.Family>().AsNoTracking()
            .AnyAsync(x => x.Id == household && x.Members.Any(m => m.Id == MemberId.From(member)), ct))
            throw InvalidReference();

        // This runs only after acquiring the household transaction lock. Read fresh ancestry,
        // including archived ancestors, and reject both proposed cycles and already corrupt paths.
        var visited = new HashSet<AssetId> { assetId };
        var parent = details.ParentAssetId is { } id ? AssetId.From(id) : (AssetId?)null;
        while (parent is { } current)
        {
            if (!visited.Add(current))
                throw new AssetsException(AssetsErrorCode.InvalidInput, "Containment must not create a cycle.");
            var ancestor = await db.Set<Asset>().AsNoTracking()
                .SingleOrDefaultAsync(x => x.FamilyId == household && x.Id == current, ct);
            if (ancestor is null) throw InvalidReference();
            parent = ancestor.ParentAssetId;
        }
    }

    internal static void Apply(Asset asset, AssetDetailsRequest details)
    {
        try
        {
            asset.Update(details.Name, details.Category, details.Make, details.Model, details.SerialNumber,
                details.ManagementBasis, details.ParentAssetId is { } parent ? AssetId.From(parent) : null,
                details.LocationId is { } location ? AssetLocationId.From(location) : null,
                details.ResponsibleMemberId is { } member ? MemberId.From(member) : null,
                details.ResponsibleAreaId is { } area ? ResponsibilityDomainId.From(area) : null);
        }
        catch (ArgumentException ex)
        {
            throw new AssetsException(AssetsErrorCode.InvalidInput, ex.Message);
        }
    }

    internal static AssetResponse Map(Asset asset) => new(asset.Id.Value, asset.FamilyId.Value, asset.Name,
        asset.Category, asset.Make, asset.Model, asset.SerialNumber, asset.ManagementBasis,
        asset.ParentAssetId?.Value, asset.LocationId?.Value, asset.ResponsibleMemberId?.Value,
        asset.ResponsibleAreaId?.Value, asset.IsArchived ? "Archived" : "Active");

    private static AssetsException InvalidReference() => new(AssetsErrorCode.InvalidInput,
        "Asset relationships must refer to existing records in this household.");
}
