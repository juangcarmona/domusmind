namespace DomusMind.Contracts.HouseholdAssets;

public sealed record AssetDetailsRequest(
    string Name, string Category, string? Make = null, string? Model = null, string? SerialNumber = null,
    string? ManagementBasis = null, Guid? ParentAssetId = null, Guid? LocationId = null,
    Guid? ResponsibleMemberId = null, Guid? ResponsibleAreaId = null);

public sealed record AssetResponse(
    Guid Id, Guid FamilyId, string Name, string Category, string? Make, string? Model, string? SerialNumber,
    string? ManagementBasis, Guid? ParentAssetId, Guid? LocationId, Guid? ResponsibleMemberId,
    Guid? ResponsibleAreaId, string Status);

public sealed record CreateAssetLocationRequest(string Name, string? Address = null);
public sealed record AssetLocationResponse(Guid Id, Guid FamilyId, string Name, string? Address);
