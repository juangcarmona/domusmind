namespace DomusMind.Contracts.MealPlanning;

public sealed record CreateRecipeRequest(
    Guid FamilyId,
    string Name,
    string? Description,
    int? PrepTimeMinutes,
    int? CookTimeMinutes,
    int? Servings,
    string? Instructions,
    string? Notes);
