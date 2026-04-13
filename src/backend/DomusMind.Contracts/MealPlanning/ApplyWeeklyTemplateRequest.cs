using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Contracts.MealPlanning;

public sealed record ApplyWeeklyTemplateRequest(
    MealPlanId MealPlanId,
    WeeklyTemplateId WeeklyTemplateId);