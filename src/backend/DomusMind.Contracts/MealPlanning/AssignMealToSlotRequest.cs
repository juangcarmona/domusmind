using DomusMind.Domain.MealPlanning.ValueObjects;
using DomusMind.Domain.MealPlanning.Enums;

namespace DomusMind.Contracts.MealPlanning;

public sealed record AssignMealToSlotRequest(
    MealSlotId MealSlotId,
    MealType? MealType,
    RecipeId? RecipeId,
    string? Notes);