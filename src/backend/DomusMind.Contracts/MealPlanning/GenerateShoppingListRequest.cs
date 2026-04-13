using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Contracts.MealPlanning;

public sealed record GenerateShoppingListRequest(
    MealPlanId MealPlanId);