using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Domain.Family;
using DomusMind.Domain.Lists;
using DomusMind.Domain.Lists.Events;
using DomusMind.Domain.Lists.ValueObjects;
using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.Events;
using DomusMind.Domain.MealPlanning.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Lists.CreateShoppingListFromMealPlan;

public sealed class CreateShoppingListFromMealPlanEventHandler
    : IDomainEventHandler<ShoppingListRequested>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IEventLogWriter _eventLogWriter;

    public CreateShoppingListFromMealPlanEventHandler(
        IDomusMindDbContext dbContext,
        IEventLogWriter eventLogWriter)
    {
        _dbContext = dbContext;
        _eventLogWriter = eventLogWriter;
    }

    public async Task Handle(ShoppingListRequested domainEvent, CancellationToken cancellationToken)
    {
        var listId = ListId.From(domainEvent.ShoppingListId);
        var existingList = await _dbContext.Set<SharedList>()
            .AsNoTracking()
            .FirstOrDefaultAsync(list => list.Id == listId, cancellationToken);

        if (existingList is not null)
        {
            if (existingList.FamilyId.Value != domainEvent.FamilyId
                || existingList.LinkedEntityType != "MealPlan"
                || existingList.LinkedEntityId != domainEvent.MealPlanId)
            {
                throw new InvalidOperationException(
                    $"Shopping list '{domainEvent.ShoppingListId}' already exists for a different request.");
            }

            return;
        }

        var mealPlanId = MealPlanId.From(domainEvent.MealPlanId);
        var mealPlan = await _dbContext.Set<MealPlan>()
            .Include(plan => plan.MealSlots)
            .FirstOrDefaultAsync(plan => plan.Id == mealPlanId, cancellationToken)
            ?? throw new InvalidOperationException($"Meal plan '{domainEvent.MealPlanId}' not found.");

        if (mealPlan.FamilyId.Value != domainEvent.FamilyId)
            throw new InvalidOperationException("Meal plan does not belong to the event's family.");

        var recipeIds = mealPlan.MealSlots
            .Where(slot => slot.MealSourceType == MealSourceType.Recipe && slot.RecipeId.HasValue)
            .Select(slot => slot.RecipeId!.Value)
            .Distinct()
            .ToList();

        if (recipeIds.Count == 0)
            throw new InvalidOperationException("Cannot derive a shopping list: no recipe slots are assigned.");

        var recipes = await _dbContext.Set<Recipe>()
            .Include(recipe => recipe.Ingredients)
            .AsNoTracking()
            .Where(recipe => recipeIds.Contains(recipe.Id))
            .ToListAsync(cancellationToken);

        var ingredients = recipes
            .SelectMany(recipe => recipe.Ingredients)
            .GroupBy(ingredient => (
                Name: ingredient.Name.ToLowerInvariant(),
                Unit: (ingredient.Unit ?? string.Empty).ToLowerInvariant()))
            .Select(group => new
            {
                Name = group.First().Name,
                Unit = group.First().Unit,
                Quantity = group.Any(ingredient => ingredient.Quantity.HasValue)
                    ? group.Where(ingredient => ingredient.Quantity.HasValue)
                        .Sum(ingredient => ingredient.Quantity!.Value)
                    : (decimal?)null
            })
            .ToList();

        var sharedList = SharedList.Create(
            listId,
            FamilyId.From(domainEvent.FamilyId),
            ListName.Create(domainEvent.ShoppingListName),
            ListKind.Create("shopping"),
            areaId: null,
            linkedEntityType: "MealPlan",
            linkedEntityId: domainEvent.MealPlanId,
            createdAtUtc: domainEvent.OccurredAtUtc);

        foreach (var ingredient in ingredients)
        {
            var quantity = ingredient.Quantity.HasValue
                ? $"{ingredient.Quantity.Value}{(string.IsNullOrEmpty(ingredient.Unit) ? "" : " " + ingredient.Unit)}"
                : null;

            sharedList.AddItem(
                ListItemId.New(),
                ListItemName.Create(ingredient.Name),
                quantity,
                note: null,
                domainEvent.OccurredAtUtc);
        }

        _dbContext.Set<SharedList>().Add(sharedList);
        await _eventLogWriter.WriteAsync([sharedList], cancellationToken);
        sharedList.ClearDomainEvents();
    }
}
