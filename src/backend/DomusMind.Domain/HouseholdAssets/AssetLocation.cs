using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;

namespace DomusMind.Domain.HouseholdAssets;

/// <summary>A separately identified household location that assets can share.</summary>
public sealed class AssetLocation : AggregateRoot<AssetLocationId>
{
    public FamilyId FamilyId { get; private set; }
    public string Name { get; private set; }
    public string? Address { get; private set; }

    private AssetLocation(AssetLocationId id, FamilyId familyId, string name, string? address) : base(id)
    {
        if (id.Value == Guid.Empty || familyId.Value == Guid.Empty)
            throw new ArgumentException("Location and household identities are required.");
        FamilyId = familyId;
        Name = RequiredName(name);
        Address = address;
    }

    public static AssetLocation Create(AssetLocationId id, FamilyId familyId, string name, string? address = null)
        => new(id, familyId, name, address);

    public void Update(string name, string? address)
    {
        Name = RequiredName(name);
        Address = address;
    }

    private static string RequiredName(string name)
        => string.IsNullOrWhiteSpace(name)
            ? throw new ArgumentException("Location name is required.", nameof(name))
            : name.Trim();
}
