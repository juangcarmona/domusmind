using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using FluentAssertions;

namespace DomusMind.Domain.Tests.HouseholdAssets;

public sealed class AssetLocationTests
{
    [Fact]
    public void NamedLocation_CanBeCreatedWithoutAddressAndUpdatedLater()
    {
        var id = AssetLocationId.New();
        var familyId = FamilyId.New();
        var location = AssetLocation.Create(id, familyId, "Home");
        location.Address.Should().BeNull();

        location.Update("Home", "Example address");

        location.Id.Should().Be(id);
        location.FamilyId.Should().Be(familyId);
        location.Address.Should().Be("Example address");
    }

    [Fact]
    public void MissingName_IsRejectedWithoutChangingLocation()
    {
        var location = AssetLocation.Create(AssetLocationId.New(), FamilyId.New(), "Home");
        Action update = () => location.Update(" ", "Example address");
        update.Should().Throw<ArgumentException>();
        location.Name.Should().Be("Home");
        location.Address.Should().BeNull();
    }
}
