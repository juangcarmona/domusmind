using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using FluentAssertions;

namespace DomusMind.Domain.Tests.HouseholdAssets;

public sealed class AssetTests
{
    [Fact]
    public void ArchiveAndRestore_PreservesIdentityAndDetails()
    {
        var id = AssetId.New();
        var household = FamilyId.New();
        var asset = Asset.Create(id, household, "Boiler", "Other");
        asset.UpdateDetails("Boiler", "Other", "Acme", "One", "123");

        asset.IsArchived.Should().BeFalse();
        asset.Archive();
        asset.IsArchived.Should().BeTrue();
        asset.Restore();

        asset.IsArchived.Should().BeFalse();
        asset.Id.Should().Be(id);
        asset.FamilyId.Should().Be(household);
        asset.Name.Should().Be("Boiler");
        asset.Make.Should().Be("Acme");
        asset.Model.Should().Be("One");
        asset.SerialNumber.Should().Be("123");
    }

    [Fact]
    public void ArchivedAsset_RejectsEditsAndDuplicateTransitions()
    {
        var asset = Asset.Create(AssetId.New(), FamilyId.New(), "Tool", "Other");
        Action restoreActive = asset.Restore;
        restoreActive.Should().Throw<InvalidOperationException>();
        asset.Archive();
        Action edit = () => asset.UpdateDetails("Changed", "Other", null, null, null);
        Action archiveAgain = asset.Archive;
        edit.Should().Throw<InvalidOperationException>();
        archiveAgain.Should().Throw<InvalidOperationException>();
        asset.Name.Should().Be("Tool");
        asset.IsArchived.Should().BeTrue();
    }

    [Fact]
    public void InvalidUpdate_DoesNotPartiallyChangeAsset()
    {
        var asset = Asset.Create(AssetId.New(), FamilyId.New(), "Tool", "Other");
        Action edit = () => asset.UpdateDetails("Changed", " ", null, null, null);
        edit.Should().Throw<ArgumentException>();
        asset.Name.Should().Be("Tool");
        asset.Category.Should().Be("Other");
    }
}
