using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.MealPlanning.ValueObjects;

public readonly record struct FamilyPreferenceId(Guid Value)
{
    public static FamilyPreferenceId New() => new(Guid.NewGuid());
    public static FamilyPreferenceId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}