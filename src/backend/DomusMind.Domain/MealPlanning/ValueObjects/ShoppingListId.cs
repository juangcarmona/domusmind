using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.MealPlanning.ValueObjects;

public readonly record struct ShoppingListId(Guid Value)
{
    public static ShoppingListId New() => new(Guid.NewGuid());
    public static ShoppingListId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}