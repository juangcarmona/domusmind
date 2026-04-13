using DomusMind.Domain.MealPlanning.ValueObjects;
using DomusMind.Domain.Family;

namespace DomusMind.Contracts.MealPlanning;

public sealed record CreateMealPlanRequest(
    MealPlanId Id,
    FamilyId FamilyId,
    DateOnly WeekStart,
    DateTime CreatedAt,
    DateTime UpdatedAt);