using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.MealPlanning;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Application.Features.MealPlanning.AssignMealToSlot;

public sealed record AssignMealToSlotCommand(
    MealSlotId MealSlotId,
    MealType? MealType = null,
    RecipeId? RecipeId = null,
    string? Notes = null) : ICommand<AssignMealToSlotResponse>
{
}