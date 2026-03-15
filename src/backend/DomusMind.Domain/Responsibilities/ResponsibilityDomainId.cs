namespace DomusMind.Domain.Responsibilities;

public readonly record struct ResponsibilityDomainId(Guid Value)
{
    public static ResponsibilityDomainId New() => new(Guid.NewGuid());
    public static ResponsibilityDomainId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
