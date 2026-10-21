namespace DomusMind.Mcp.DependencyInjection;

public sealed class DomusMindMcpOptions
{
    public const string SectionName = "Mcp";

    public string[] AllowedOrigins { get; set; } = [];

    public bool RequireOAuth { get; set; }

    public string ProtectedResourceMetadataPath { get; set; } = "/.well-known/oauth-protected-resource/mcp";

    public string? ExpectedAudience { get; set; }

    public int RequestsPerMinute { get; set; } = 60;

    public McpCapabilityOptions Capabilities { get; set; } = new();
}

public sealed class McpCapabilityOptions
{
    public bool HouseholdReadOnly { get; set; }

    public bool HouseholdWrites { get; set; }
}
