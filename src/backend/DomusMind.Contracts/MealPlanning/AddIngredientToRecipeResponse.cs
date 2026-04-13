using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Contracts.MealPlanning;

public sealed record AddIngredientToRecipeResponse(
    IngredientId Id,
    RecipeId RecipeId,
    string Name,
    decimal Quantity,
    string Unit,
    string? Notes,
    DateTime CreatedAt);