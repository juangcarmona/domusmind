using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.Responsibilities.ValueObjects;

public sealed class ResponsibilityAreaName : ValueObject
{
    public string Value { get; }

    private ResponsibilityAreaName(string value) => Value = value;

    public static ResponsibilityAreaName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Responsibility area name cannot be empty.", nameof(value));

        if (value.Trim().Length > 100)
            throw new ArgumentException("Responsibility area name cannot exceed 100 characters.", nameof(value));

        return new ResponsibilityAreaName(value.Trim());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
