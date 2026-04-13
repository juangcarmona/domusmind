using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.MealPlanning;
using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Application.Features.MealPlanning.CreateRecipe;

public sealed record CreateRecipeCommand(
    RecipeId Id,
    FamilyId FamilyId,
    string Name,
    string? Description,
    int? PrepTimeMinutes,
    int? CookTimeMinutes,
    int? Servings,
    string? Instructions,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt) : ICommand<CreateRecipeResponse>
{
}