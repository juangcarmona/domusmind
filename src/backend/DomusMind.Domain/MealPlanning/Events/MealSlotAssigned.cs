using DomusMind.Domain.Abstractions;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Events;

public sealed record MealSlotAssigned(
    MealSlotId MealSlotId,
    Guid MealPlanId,
    Guid? RecipeId,
    string? Notes
) : IDomainEvent
{
    public DateTime OccurredAtUtc => DateTime.UtcNow;
}