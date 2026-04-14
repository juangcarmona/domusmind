using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.MealPlanning.Events;

public sealed record ShoppingListGenerated(
    Guid Id,
    Guid ShoppingListId,
    Guid GeneratedFromMealPlanId,
    DateTime GeneratedAt
) : IDomainEvent
{
    public DateTime OccurredAtUtc => GeneratedAt;
}