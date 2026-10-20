namespace DomusMind.Mcp.DependencyInjection;

public sealed class DomusMindMcpOptions
{
    public const string SectionName = "Mcp";

    public string[] AllowedOrigins { get; set; } = [];

    public bool RequireOAuth { get; set; }

    public string ProtectedResourceMetadataPath { get; set; } = "/.well-known/oauth-protected-resource/mcp";
}
