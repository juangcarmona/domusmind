namespace DomusMind.Contracts.MealPlanning;

public sealed record FamilyRecipeItem(
    Guid Id,
    Guid FamilyId,
    string Name,
    string? Description,
    int? PrepTimeMinutes,
    int? CookTimeMinutes,
    int? Servings,
    DateTime CreatedAtUtc);

public sealed record GetFamilyRecipesResponse(IReadOnlyList<FamilyRecipeItem> Recipes);
