namespace DomusMind.Contracts.MealPlanning;

public sealed record MealSlotDetail(
    Guid Id,
    string DayOfWeek,
    string MealType,
    Guid? RecipeId,
    string? RecipeName,
    string? Notes);

public sealed record MealPlanDetail(
    Guid Id,
    Guid FamilyId,
    string WeekStart,
    DateTime CreatedAtUtc,
    IReadOnlyList<MealSlotDetail> Slots);

/// <summary>
/// Returns null MealPlan when no plan exists for the requested week.
/// </summary>
public sealed record GetMealPlanResponse(MealPlanDetail? MealPlan);
