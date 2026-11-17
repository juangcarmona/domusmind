using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;

namespace DomusMind.Domain.HouseholdAssets;

/// <summary>A durable household-managed thing. Authorization is enforced by its caller.</summary>
public sealed class Asset : AggregateRoot<AssetId>
{
    public FamilyId FamilyId { get; private set; }
    public string Name { get; private set; }
    public string Category { get; private set; }
    public string? Make { get; private set; }
    public string? Model { get; private set; }
    public string? SerialNumber { get; private set; }
    public bool IsArchived { get; private set; }

    private Asset(AssetId id, FamilyId familyId, string name, string category) : base(id)
    {
        if (id.Value == Guid.Empty || familyId.Value == Guid.Empty)
            throw new ArgumentException("Asset and household identities are required.");
        FamilyId = familyId;
        Name = Required(name, nameof(name));
        Category = Required(category, nameof(category));
    }

    public static Asset Create(AssetId id, FamilyId familyId, string name, string category)
        => new(id, familyId, name, category);

    public void UpdateDetails(string name, string category, string? make, string? model, string? serialNumber)
    {
        if (IsArchived)
            throw new InvalidOperationException("Archived assets are read-only.");
        var validName = Required(name, nameof(name));
        var validCategory = Required(category, nameof(category));
        Name = validName;
        Category = validCategory;
        Make = make;
        Model = model;
        SerialNumber = serialNumber;
    }

    public void Archive()
    {
        if (IsArchived)
            throw new InvalidOperationException("Asset is already archived.");
        IsArchived = true;
    }

    public void Restore()
    {
        if (!IsArchived)
            throw new InvalidOperationException("Asset is already active.");
        IsArchived = false;
    }

    private static string Required(string value, string parameter)
        => string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Value is required.", parameter)
            : value.Trim();
}
