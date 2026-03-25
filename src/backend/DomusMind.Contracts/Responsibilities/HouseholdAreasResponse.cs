namespace DomusMind.Contracts.Responsibilities;

public sealed record HouseholdAreaItem(
    Guid AreaId,
    string Name,
    string Color,
    Guid? PrimaryOwnerId,
    string? PrimaryOwnerName,
    IReadOnlyCollection<Guid> SecondaryOwnerIds);

public sealed record HouseholdAreasResponse(
    IReadOnlyCollection<HouseholdAreaItem> Areas);
