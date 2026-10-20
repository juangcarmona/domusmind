namespace DomusMind.Mcp.DependencyInjection;

public sealed class DomusMindMcpOptions
{
    public const string SectionName = "Mcp";

    public string[] AllowedOrigins { get; set; } = [];
}
