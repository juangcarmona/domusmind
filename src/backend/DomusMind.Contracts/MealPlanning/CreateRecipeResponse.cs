using DomusMind.Domain.MealPlanning.ValueObjects;
using DomusMind.Domain.Family;

namespace DomusMind.Contracts.MealPlanning;

public sealed record CreateRecipeResponse(
    RecipeId Id,
    FamilyId FamilyId,
    string Name,
    string? Description,
    int? PrepTimeMinutes,
    int? CookTimeMinutes,
    int? Servings,
    string? Instructions,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt);