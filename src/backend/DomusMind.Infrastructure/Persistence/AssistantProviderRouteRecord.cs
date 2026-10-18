namespace DomusMind.Infrastructure.Persistence;

public sealed class AssistantProviderRouteRecord
{
    public Guid FamilyId { get; set; }
    public string TargetsJson { get; set; } = "[]";
    public DateTime UpdatedAtUtc { get; set; }
}
