using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Contracts.MealPlanning;

public sealed record ApplyWeeklyTemplateResponse(
    MealPlanId MealPlanId,
    WeeklyTemplateId WeeklyTemplateId);