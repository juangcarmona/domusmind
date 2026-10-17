using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.MealPlanning.ValueObjects;

public readonly record struct RecipeId(Guid Value) : IAggregateIdentifier
{
    public static RecipeId New() => new(Guid.NewGuid());
    public static RecipeId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}