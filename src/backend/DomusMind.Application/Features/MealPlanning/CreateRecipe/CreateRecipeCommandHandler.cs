using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Features.MealPlanning.CreateRecipe;
using DomusMind.Contracts.MealPlanning;
using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.ValueObjects;
using DomusMind.Domain.Family;

namespace DomusMind.Application.Features.MealPlanning.CreateRecipe;

public sealed class CreateRecipeCommandHandler : ICommandHandler<CreateRecipeCommand, CreateRecipeResponse>
{
    private readonly IDomusMindDbContext _dbContext;

    public CreateRecipeCommandHandler(IDomusMindDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateRecipeResponse> Handle(
        CreateRecipeCommand command,
        CancellationToken cancellationToken)
    {
        var recipe = new Recipe(
            command.Id,
            command.FamilyId,
            command.Name,
            command.Description,
            command.PrepTimeMinutes,
            command.CookTimeMinutes,
            command.Servings,
            command.Instructions,
            command.Notes,
            command.CreatedAt,
            command.UpdatedAt);

        _dbContext.Set<Recipe>().Add(recipe);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateRecipeResponse(
            recipe.Id,
            recipe.FamilyId,
            recipe.Name,
            recipe.Description,
            recipe.PrepTimeMinutes,
            recipe.CookTimeMinutes,
            recipe.Servings,
            recipe.Instructions,
            recipe.Notes,
            recipe.CreatedAt,
            recipe.UpdatedAt);
    }
}