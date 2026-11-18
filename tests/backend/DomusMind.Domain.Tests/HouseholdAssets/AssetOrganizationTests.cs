using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using DomusMind.Domain.Responsibilities;

namespace DomusMind.Domain.Tests.HouseholdAssets;

public sealed class AssetOrganizationTests
{
    [Fact]
    public void LifecyclePreservesIndependentOrganization()
    {
        var asset = Asset.Create(AssetId.New(), FamilyId.New(), "Borrowed drill", "Equipment");
        var parent = AssetId.New();
        var location = AssetLocationId.New();
        var member = MemberId.New();
        var area = ResponsibilityDomainId.New();
        asset.Update("Borrowed drill", "Equipment", "Acme", "One", "123", "Borrowed", parent, location, member, area);
        asset.Archive();
        Assert.Throws<InvalidOperationException>(() => asset.Update("Changed", "Other", null, null, null, null, null, null, null, null));
        asset.Restore();
        Assert.Equal(parent, asset.ParentAssetId);
        Assert.Equal(location, asset.LocationId);
        Assert.Equal(member, asset.ResponsibleMemberId);
        Assert.Equal(area, asset.ResponsibleAreaId);
        Assert.Equal("Borrowed", asset.ManagementBasis);
        Assert.Equal("Borrowed drill", asset.Name);
    }

    [Fact]
    public void InvalidCategoryAndSelfContainmentAreAtomic()
    {
        var asset = Asset.Create(AssetId.New(), FamilyId.New(), "Tool", "Other");
        Assert.Throws<ArgumentException>(() => asset.Update("Changed", "Unrecognized", null, null, null, null, null, null, null, null));
        Assert.Throws<ArgumentException>(() => asset.Update("Changed", "Other", null, null, null, null, asset.Id, null, null, null));
        Assert.Equal("Tool", asset.Name);
        Assert.Null(asset.ParentAssetId);
    }
}
