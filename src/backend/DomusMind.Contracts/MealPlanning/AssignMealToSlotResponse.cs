namespace DomusMind.Contracts.MealPlanning;

public sealed record AssignMealToSlotResponse(
    Guid SlotId,
    Guid MealPlanId,
    string DayOfWeek,
    string MealType,
    Guid? RecipeId,
    string? RecipeName,
    string? Notes);