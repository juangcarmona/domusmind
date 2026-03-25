namespace DomusMind.Contracts.SharedLists;

public sealed record UpdateSharedListItemResponse(
    Guid ItemId,
    string Name,
    string? Quantity,
    string? Note,
    DateTime UpdatedAtUtc);
