namespace DomusMind.Contracts.MealPlanning;

public sealed record FamilyRecipeItem(
    Guid Id,
    Guid FamilyId,
    string Name,
    string? Description,
    int? PrepTimeMinutes,
    int? CookTimeMinutes,
    int? TotalTimeMinutes,
    int? Servings,
    bool IsFavorite,
    IReadOnlyList<string> Tags,
    int IngredientCount,
    DateTime CreatedAtUtc);

public sealed record GetFamilyRecipesResponse(IReadOnlyList<FamilyRecipeItem> Recipes);
