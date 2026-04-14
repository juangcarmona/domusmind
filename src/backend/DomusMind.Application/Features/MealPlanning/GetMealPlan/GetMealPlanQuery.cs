using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.MealPlanning;

namespace DomusMind.Application.Features.MealPlanning.GetMealPlan;

public sealed record GetMealPlanQuery(
    Guid FamilyId,
    DateOnly WeekStart,
    Guid RequestedByUserId) : IQuery<GetMealPlanResponse>;
