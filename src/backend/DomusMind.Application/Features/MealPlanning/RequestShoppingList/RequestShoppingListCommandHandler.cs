using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.MealPlanning;
using DomusMind.Domain.Family;
using DomusMind.Domain.Lists;
using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.MealPlanning.RequestShoppingList;

public sealed class RequestShoppingListCommandHandler
    : ICommandHandler<RequestShoppingListCommand, RequestShoppingListResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IEventLogWriter _eventLogWriter;
    private readonly IFamilyAuthorizationService _authorizationService;

    public RequestShoppingListCommandHandler(
        IDomusMindDbContext dbContext,
        IEventLogWriter eventLogWriter,
        IFamilyAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _eventLogWriter = eventLogWriter;
        _authorizationService = authorizationService;
    }

    public async Task<RequestShoppingListResponse> Handle(
        RequestShoppingListCommand command,
        CancellationToken cancellationToken)
    {
        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            command.RequestedByUserId, command.FamilyId, cancellationToken);
        if (!canAccess)
            throw new UnauthorizedAccessException("Access to this family is denied.");

        var planId = MealPlanId.From(command.MealPlanId);
        var plan = await _dbContext.Set<MealPlan>()
            .Include(mp => mp.MealSlots)
            .FirstOrDefaultAsync(mp => mp.Id == planId, cancellationToken);

        if (plan is null)
            throw new InvalidOperationException($"Meal plan '{command.MealPlanId}' not found.");

        if (plan.FamilyId != FamilyId.From(command.FamilyId))
            throw new InvalidOperationException("Meal plan does not belong to this family.");

        if (!plan.MealSlots.Any(s => s.MealSourceType == MealSourceType.Recipe && s.RecipeId.HasValue))
            throw new InvalidOperationException("Cannot derive a shopping list: no recipe slots are assigned.");

        var now = DateTime.UtcNow;
        var listName = !string.IsNullOrWhiteSpace(command.ShoppingListName)
            ? command.ShoppingListName
            : $"Shopping list - week of {plan.WeekStart:yyyy-MM-dd}";

        var listId = ListId.New();
        plan.RequestShoppingList(listId.Value, listName, now);

        await _eventLogWriter.WriteAsync([plan], cancellationToken);
        plan.ClearDomainEvents();

        var itemCount = await _dbContext.Set<SharedList>()
            .AsNoTracking()
            .Where(list => list.Id == listId)
            .Select(list => list.Items.Count)
            .SingleAsync(cancellationToken);

        return new RequestShoppingListResponse(
            plan.Id.Value,
            listId.Value,
            listName,
            itemCount);
    }
}
