using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Contracts.MealPlanning;

public sealed record AddIngredientToRecipeRequest(
    IngredientId Id,
    string Name,
    RecipeId RecipeId,
    decimal Quantity,
    string Unit,
    string? Notes);