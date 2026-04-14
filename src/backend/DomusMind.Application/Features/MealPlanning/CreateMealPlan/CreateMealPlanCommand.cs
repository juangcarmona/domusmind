using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.MealPlanning;

namespace DomusMind.Application.Features.MealPlanning.CreateMealPlan;

public sealed record CreateMealPlanCommand(
    Guid FamilyId,
    DateOnly WeekStart,
    Guid RequestedByUserId) : ICommand<CreateMealPlanResponse>;
