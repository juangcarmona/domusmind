using DomusMind.Domain.Abstractions;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Events;

public sealed record ShoppingListGenerated(
    ShoppingListId ShoppingListId,
    Guid GeneratedFromMealPlanId,
    DateTime GeneratedAt
) : IDomainEvent
{
    public DateTime OccurredAtUtc => GeneratedAt;
}