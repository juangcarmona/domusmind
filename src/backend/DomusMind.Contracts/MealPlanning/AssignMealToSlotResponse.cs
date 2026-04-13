using DomusMind.Domain.MealPlanning.ValueObjects;
using DomusMind.Domain.MealPlanning.Enums;

namespace DomusMind.Contracts.MealPlanning;

public sealed record AssignMealToSlotResponse(
    MealSlotId MealSlotId,
    MealPlanId MealPlanId,
    DateOnly Date,
    MealType MealType,
    RecipeId? RecipeId,
    string? Notes);