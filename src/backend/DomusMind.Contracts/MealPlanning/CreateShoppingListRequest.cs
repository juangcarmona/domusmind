using DomusMind.Domain.MealPlanning.ValueObjects;
using DomusMind.Domain.Family;

namespace DomusMind.Contracts.MealPlanning;

public sealed record CreateShoppingListRequest(
    ShoppingListId Id,
    FamilyId FamilyId,
    string Name,
    MealPlanId? GeneratedFromMealPlanId,
    DateTime CreatedAt,
    DateTime UpdatedAt);