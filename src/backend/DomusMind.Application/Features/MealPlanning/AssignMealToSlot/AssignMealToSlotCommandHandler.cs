using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Features.MealPlanning.AssignMealToSlot;
using DomusMind.Contracts.MealPlanning;
using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.MealPlanning.AssignMealToSlot;

public sealed class AssignMealToSlotCommandHandler : ICommandHandler<AssignMealToSlotCommand, AssignMealToSlotResponse>
{
    private readonly IDomusMindDbContext _dbContext;

    public AssignMealToSlotCommandHandler(IDomusMindDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AssignMealToSlotResponse> Handle(
        AssignMealToSlotCommand command,
        CancellationToken cancellationToken)
    {
        var mealSlotId = MealSlotId.From(command.MealSlotId);

        var mealSlot = await _dbContext.Set<MealSlot>()
            .FirstOrDefaultAsync(ms => ms.Id == mealSlotId, cancellationToken);

        if (mealSlot is null)
            throw new InvalidOperationException($"Meal slot '{command.MealSlotId}' not found.");

        var mealType = command.MealType is not null
            ? Enum.Parse<Domain.MealPlanning.Enums.MealType>(command.MealType, ignoreCase: true)
            : (Domain.MealPlanning.Enums.MealType?)null;

        var recipeId = command.RecipeId.HasValue
            ? RecipeId.From(command.RecipeId.Value)
            : (RecipeId?)null;

        mealSlot.Update(mealType, recipeId, command.Notes);

        await _dbContext.SaveChangesAsync(cancellationToken);

        string? recipeName = null;
        if (mealSlot.RecipeId.HasValue)
        {
            recipeName = await _dbContext.Set<Recipe>()
                .AsNoTracking()
                .Where(r => r.Id == mealSlot.RecipeId.Value)
                .Select(r => r.Name)
                .FirstOrDefaultAsync(cancellationToken);
        }

        return new AssignMealToSlotResponse(
            mealSlot.Id.Value,
            mealSlot.MealPlanId.Value,
            mealSlot.DayOfWeek.ToString(),
            mealSlot.MealType.ToString(),
            mealSlot.RecipeId?.Value,
            recipeName,
            mealSlot.Notes);
    }
}