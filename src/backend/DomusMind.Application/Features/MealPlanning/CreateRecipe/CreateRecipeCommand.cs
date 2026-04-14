using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.MealPlanning;

namespace DomusMind.Application.Features.MealPlanning.CreateRecipe;

public sealed record CreateRecipeCommand(
    Guid FamilyId,
    string Name,
    string? Description,
    int? PrepTimeMinutes,
    int? CookTimeMinutes,
    int? Servings,
    string? Instructions,
    string? Notes,
    Guid RequestedByUserId) : ICommand<CreateRecipeResponse>;
