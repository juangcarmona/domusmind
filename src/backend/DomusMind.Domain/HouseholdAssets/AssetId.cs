using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.HouseholdAssets;

public readonly record struct AssetId(Guid Value) : IAggregateIdentifier
{
    public static AssetId New() => new(Guid.NewGuid());
    public static AssetId From(Guid value) => new(value);
}
