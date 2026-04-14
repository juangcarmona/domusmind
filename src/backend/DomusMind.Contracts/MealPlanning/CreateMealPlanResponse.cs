namespace DomusMind.Contracts.MealPlanning;

public sealed record CreateMealPlanResponse(
    Guid Id,
    Guid FamilyId,
    DateOnly WeekStart,
    DateTime CreatedAt,
    DateTime UpdatedAt);