namespace DomusMind.Contracts.MealPlanning;

public sealed record CreateRecipeResponse(
    Guid Id,
    Guid FamilyId,
    string Name,
    string? Description,
    int? PrepTimeMinutes,
    int? CookTimeMinutes,
    int? Servings,
    string? Instructions,
    string? Notes,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);