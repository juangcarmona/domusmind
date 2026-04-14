using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Features.MealPlanning.CreateRecipe;
using DomusMind.Contracts.MealPlanning;
using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.ValueObjects;
using DomusMind.Domain.Family;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.MealPlanning.CreateRecipe;

public sealed class CreateRecipeCommandHandler : ICommandHandler<CreateRecipeCommand, CreateRecipeResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IEventLogWriter _eventLogWriter;

    public CreateRecipeCommandHandler(
        IDomusMindDbContext dbContext,
        IEventLogWriter eventLogWriter)
    {
        _dbContext = dbContext;
        _eventLogWriter = eventLogWriter;
    }

    public async Task<CreateRecipeResponse> Handle(
        CreateRecipeCommand command,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var recipe = Recipe.Create(
            RecipeId.New(),
            FamilyId.From(command.FamilyId),
            command.Name,
            command.Description,
            command.PrepTimeMinutes,
            command.CookTimeMinutes,
            command.Servings,
            command.Instructions,
            command.Notes,
            now,
            now);

        _dbContext.Set<Recipe>().Add(recipe);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _eventLogWriter.WriteAsync(recipe.DomainEvents, cancellationToken);
        recipe.ClearDomainEvents();

        return new CreateRecipeResponse(
            recipe.Id.Value,
            recipe.FamilyId.Value,
            recipe.Name,
            recipe.Description,
            recipe.PrepTimeMinutes,
            recipe.CookTimeMinutes,
            recipe.Servings,
            recipe.Instructions,
            recipe.Notes,
            recipe.CreatedAtUtc,
            recipe.UpdatedAtUtc);
    }
}