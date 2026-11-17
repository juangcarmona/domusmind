using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.HouseholdAssets;

public readonly record struct AssetLocationId(Guid Value) : IAggregateIdentifier
{
    public static AssetLocationId New() => new(Guid.NewGuid());
    public static AssetLocationId From(Guid value) => new(value);
}
