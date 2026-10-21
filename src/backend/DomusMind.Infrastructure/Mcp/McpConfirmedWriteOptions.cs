namespace DomusMind.Infrastructure.Mcp;

public sealed class McpConfirmedWriteOptions
{
    public const string SectionName = "Mcp:Write";

    public int ApprovalLifetimeSeconds { get; set; } = 600;
    public int ResultRetentionDays { get; set; } = 30;
    public int TombstoneRetentionDays { get; set; } = 180;
}
