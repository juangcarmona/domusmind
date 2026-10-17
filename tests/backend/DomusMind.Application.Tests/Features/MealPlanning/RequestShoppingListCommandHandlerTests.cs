using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Features.Lists.CreateShoppingListFromMealPlan;
using DomusMind.Application.Features.MealPlanning.RequestShoppingList;
using DomusMind.Domain.Family;
using DomusMind.Domain.Lists;
using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.Events;
using DomusMind.Domain.MealPlanning.ValueObjects;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Messaging;
using DomusMind.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DomusMind.Application.Tests.Features.MealPlanning;

public sealed class RequestShoppingListCommandHandlerTests
{
    [Fact]
    public async Task Handle_PersistsMealPlanAndDispatchesShoppingListCreationToLists()
    {
        await using var db = CreateDb();
        var provider = CreateEventServices(db, out var writer);
        using (provider)
        {
            var (plan, _, familyId) = await SeedPlanWithRecipe(db);
            var handler = new RequestShoppingListCommandHandler(db, writer, new StubMealPlanningAuthService());

            var response = await handler.Handle(
                new RequestShoppingListCommand(plan.Id.Value, familyId.Value, "Groceries", Guid.NewGuid()),
                CancellationToken.None);

            Assert.Equal(plan.Id.Value, response.MealPlanId);
            Assert.Equal("Groceries", response.ShoppingListName);
            Assert.Equal(2, response.ItemCount);

            var updatedPlan = await db.Set<MealPlan>().SingleAsync(item => item.Id == plan.Id);
            Assert.Equal(response.ShoppingListId, updatedPlan.ShoppingListId);
            Assert.Equal(1, updatedPlan.ShoppingListVersion);

            var list = await db.Set<SharedList>()
                .Include(item => item.Items)
                .SingleAsync(item => item.Id.Value == response.ShoppingListId);
            Assert.Equal(familyId, list.FamilyId);
            Assert.Equal("shopping", list.Kind.Value);
            Assert.Equal("MealPlan", list.LinkedEntityType);
            Assert.Equal(plan.Id.Value, list.LinkedEntityId);
            Assert.Equal(2, list.Items.Count);
            Assert.Contains(list.Items, item => item.Name.Value == "Flour" && item.Quantity == "501 g");
            Assert.Contains(list.Items, item => item.Name.Value == "Sugar" && item.Quantity == "2 tbsp");

            var eventRows = await db.EventLog.ToListAsync();
            Assert.Contains(eventRows, row => row.EventType == nameof(ShoppingListRequested)
                && row.Module == "MealPlanning"
                && row.AggregateId == plan.Id.Value.ToString("D"));
            Assert.Contains(eventRows, row => row.AggregateType == nameof(SharedList)
                && row.Module == "Lists"
                && row.AggregateId == response.ShoppingListId.ToString("D"));

            var requested = eventRows.Single(row => row.EventType == nameof(ShoppingListRequested));
            Assert.Contains(response.ShoppingListId.ToString("D"), requested.PayloadJson);

            var beforeRedelivery = eventRows.Count;
            var requestEvent = new ShoppingListRequested(
                Guid.NewGuid(), plan.Id.Value, familyId.Value, response.ShoppingListId, response.ShoppingListName,
                DateTime.UtcNow);
            await provider.GetRequiredService<IDomainEventHandler<ShoppingListRequested>>()
                .Handle(requestEvent, CancellationToken.None);

            Assert.Equal(1, await db.Set<SharedList>().CountAsync(item => item.Id.Value == response.ShoppingListId));
            Assert.Equal(beforeRedelivery, await db.EventLog.CountAsync());
            Assert.Equal(2, await db.Set<Recipe>().CountAsync());
        }
    }

    [Fact]
    public async Task Handle_WhenAccessIsDenied_DoesNotMutateTheMealPlanOrCreateEvents()
    {
        await using var db = CreateDb();
        var (plan, _, familyId) = await SeedPlanWithRecipe(db);
        var handler = new RequestShoppingListCommandHandler(
            db,
            new EventLogWriter(db),
            new StubMealPlanningAuthService { CanAccess = false });

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(
            new RequestShoppingListCommand(plan.Id.Value, familyId.Value, null, Guid.NewGuid()),
            CancellationToken.None));

        Assert.Empty(db.EventLog);
        Assert.Empty(db.Set<SharedList>());
        Assert.Equal(0, (await db.Set<MealPlan>().SingleAsync(item => item.Id == plan.Id)).ShoppingListVersion);
    }

    [Fact]
    public async Task Handle_WhenRequestedAgain_CreatesANewListAndKeepsThePreviousList()
    {
        await using var db = CreateDb();
        var provider = CreateEventServices(db, out var writer);
        using (provider)
        {
            var (plan, _, familyId) = await SeedPlanWithRecipe(db);
            var handler = new RequestShoppingListCommandHandler(db, writer, new StubMealPlanningAuthService());
            var command = new RequestShoppingListCommand(plan.Id.Value, familyId.Value, null, Guid.NewGuid());

            var first = await handler.Handle(command, CancellationToken.None);
            var second = await handler.Handle(command, CancellationToken.None);

            Assert.NotEqual(first.ShoppingListId, second.ShoppingListId);
            Assert.Equal(2, await db.Set<SharedList>().CountAsync());
            Assert.Equal(second.ShoppingListId, (await db.Set<MealPlan>().SingleAsync(item => item.Id == plan.Id)).ShoppingListId);
            Assert.Equal(2, (await db.Set<MealPlan>().SingleAsync(item => item.Id == plan.Id)).ShoppingListVersion);
            Assert.Contains(await db.Set<SharedList>().ToListAsync(), list => list.Id.Value == first.ShoppingListId);
            Assert.Contains(await db.Set<SharedList>().ToListAsync(), list => list.Id.Value == second.ShoppingListId);
        }
    }

    private static ServiceProvider CreateEventServices(DomusMindDbContext db, out IEventLogWriter writer)
    {
        IEventLogWriter? scopedWriter = null;
        var services = new ServiceCollection();
        services.AddSingleton<IDomusMindDbContext>(db);
        services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddSingleton<IDomainEventHandler<ShoppingListRequested>, CreateShoppingListFromMealPlanEventHandler>();
        services.AddSingleton<IEventLogWriter>(_ => scopedWriter!);

        var provider = services.BuildServiceProvider();
        scopedWriter = new EventLogWriter(db, provider.GetRequiredService<IDomainEventDispatcher>());
        writer = scopedWriter;
        return provider;
    }

    private static async Task<(MealPlan Plan, IReadOnlyList<Recipe> Recipes, FamilyId FamilyId)> SeedPlanWithRecipe(
        DomusMindDbContext db)
    {
        var now = DateTime.UtcNow;
        var familyId = FamilyId.New();
        var firstRecipeId = RecipeId.New();
        var firstRecipe = Recipe.Create(firstRecipeId, familyId, "Pasta", null, null, null, null, false, null, null, now);
        firstRecipe.AddIngredient(Ingredient.Create(IngredientId.New(), "Flour", firstRecipeId, 500, "g", now));
        firstRecipe.AddIngredient(Ingredient.Create(IngredientId.New(), "Sugar", firstRecipeId, 2, "tbsp", now));
        var secondRecipeId = RecipeId.New();
        var secondRecipe = Recipe.Create(secondRecipeId, familyId, "Bread", null, null, null, null, false, null, null, now);
        secondRecipe.AddIngredient(Ingredient.Create(IngredientId.New(), "flour", secondRecipeId, 1, "G", now));

        var plan = MealPlan.Create(MealPlanId.New(), familyId, new DateOnly(2026, 10, 12), true, now);
        plan.UpdateSlot(
            DomusMind.Domain.MealPlanning.Enums.DayOfWeek.Monday,
            MealType.Dinner,
            MealSourceType.Recipe,
            firstRecipeId,
            freeText: null,
            notes: null,
            isOptional: false,
            isLocked: false,
            now);
        plan.UpdateSlot(
            DomusMind.Domain.MealPlanning.Enums.DayOfWeek.Tuesday,
            MealType.Dinner,
            MealSourceType.Recipe,
            secondRecipeId,
            freeText: null,
            notes: null,
            isOptional: false,
            isLocked: false,
            now);
        firstRecipe.ClearDomainEvents();
        secondRecipe.ClearDomainEvents();
        plan.ClearDomainEvents();
        db.Set<Recipe>().AddRange(firstRecipe, secondRecipe);
        db.Set<MealPlan>().Add(plan);
        await db.SaveChangesAsync();
        return (plan, [firstRecipe, secondRecipe], familyId);
    }

    private static DomusMindDbContext CreateDb()
        => new(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
}
