using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.MealPlanning;

namespace DomusMind.Application.Features.MealPlanning.AssignMealToSlot;

public sealed record AssignMealToSlotCommand(
    Guid MealSlotId,
    string? MealType = null,
    Guid? RecipeId = null,
    string? Notes = null,
    Guid? RequestedByUserId = null) : ICommand<AssignMealToSlotResponse>;
