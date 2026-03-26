namespace DomusMind.Contracts.SharedLists;

public sealed record UpdateSharedListItemRequest(
    string Name,
    string? Quantity,
    string? Note);
