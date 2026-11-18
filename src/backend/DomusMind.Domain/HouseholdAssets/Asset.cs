using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;
using DomusMind.Domain.Responsibilities;

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
    public string? ManagementBasis { get; private set; }
    public AssetId? ParentAssetId { get; private set; }
    public AssetLocationId? LocationId { get; private set; }
    public MemberId? ResponsibleMemberId { get; private set; }
    public ResponsibilityDomainId? ResponsibleAreaId { get; private set; }
    public long Version { get; private set; }
    public bool IsArchived { get; private set; }

    private Asset(AssetId id, FamilyId familyId, string name, string category) : base(id)
    {
        if (id.Value == Guid.Empty || familyId.Value == Guid.Empty)
            throw new ArgumentException("Asset and household identities are required.");
        FamilyId = familyId;
        Name = Required(name, nameof(name));
        Category = AssetCategories.Require(category);
    }

    public static Asset Create(AssetId id, FamilyId familyId, string name, string category)
    {
        var asset = new Asset(id, familyId, name, category);
        asset.RaiseDomainEvent(new AssetRegistered(id.Value, familyId.Value, DateTime.UtcNow));
        return asset;
    }

    public void UpdateDetails(string name, string category, string? make, string? model, string? serialNumber)
        => Update(name, category, make, model, serialNumber, ManagementBasis, ParentAssetId, LocationId,
            ResponsibleMemberId, ResponsibleAreaId);

    public void Update(string name, string category, string? make, string? model, string? serialNumber,
        string? managementBasis, AssetId? parentAssetId, AssetLocationId? locationId,
        MemberId? responsibleMemberId, ResponsibilityDomainId? responsibleAreaId)
    {
        if (IsArchived)
            throw new InvalidOperationException("Archived assets are read-only.");
        var validName = Required(name, nameof(name));
        var validCategory = AssetCategories.Require(category);
        if (parentAssetId == Id)
            throw new ArgumentException("An asset cannot contain itself.", nameof(parentAssetId));
        if (parentAssetId?.Value == Guid.Empty || locationId?.Value == Guid.Empty
            || responsibleMemberId?.Value == Guid.Empty || responsibleAreaId?.Value == Guid.Empty)
            throw new ArgumentException("An optional relationship must have a non-empty identity.");
        Name = validName;
        Category = validCategory;
        Make = make;
        Model = model;
        SerialNumber = serialNumber;
        ManagementBasis = managementBasis;
        ParentAssetId = parentAssetId;
        LocationId = locationId;
        ResponsibleMemberId = responsibleMemberId;
        ResponsibleAreaId = responsibleAreaId;
        Version++;
        RaiseDomainEvent(new AssetUpdated(Id.Value, FamilyId.Value, DateTime.UtcNow));
    }

    public void Archive()
    {
        if (IsArchived)
            throw new InvalidOperationException("Asset is already archived.");
        IsArchived = true;
        Version++;
        RaiseDomainEvent(new AssetArchived(Id.Value, FamilyId.Value, DateTime.UtcNow));
    }

    public void Restore()
    {
        if (!IsArchived)
            throw new InvalidOperationException("Asset is already active.");
        IsArchived = false;
        Version++;
        RaiseDomainEvent(new AssetRestored(Id.Value, FamilyId.Value, DateTime.UtcNow));
    }

    private static string Required(string value, string parameter)
        => string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Value is required.", parameter)
            : value.Trim();
}
