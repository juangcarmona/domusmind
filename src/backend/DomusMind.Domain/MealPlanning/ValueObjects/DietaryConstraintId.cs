using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.MealPlanning.ValueObjects;

public readonly record struct DietaryConstraintId(Guid Value)
{
    public static DietaryConstraintId New() => new(Guid.NewGuid());
    public static DietaryConstraintId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}