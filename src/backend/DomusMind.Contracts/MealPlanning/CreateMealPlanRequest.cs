namespace DomusMind.Contracts.MealPlanning;

public sealed record CreateMealPlanRequest(
    Guid FamilyId,
    DateOnly WeekStart);
