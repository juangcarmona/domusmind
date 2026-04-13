using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.MealPlanning;
using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Application.Features.MealPlanning.CreateMealPlan;

public sealed record CreateMealPlanCommand(
    MealPlanId Id,
    FamilyId FamilyId,
    DateOnly WeekStart,
    DateTime CreatedAt,
    DateTime UpdatedAt) : ICommand<CreateMealPlanResponse>
{
}