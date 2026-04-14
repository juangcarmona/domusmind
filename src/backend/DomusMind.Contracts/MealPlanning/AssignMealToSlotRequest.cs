namespace DomusMind.Contracts.MealPlanning;

public sealed record AssignMealToSlotRequest(
    string? MealType,
    Guid? RecipeId,
    string? Notes);
