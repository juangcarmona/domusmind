namespace DomusMind.Infrastructure.Auth;

public sealed class McpOAuthOptions
{
    public const string SectionName = "Mcp:OAuth";

    public bool Enabled { get; set; }

    public string Issuer { get; set; } = "https://localhost:5001";

    public string Resource { get; set; } = "https://localhost:5001/mcp";

    public string ClientId { get; set; } = "domusmind-mcp-client";

    public string[] RedirectUris { get; set; } = [];

    public int AccessTokenLifetimeSeconds { get; set; } = 300;

    public int RefreshTokenLifetimeDays { get; set; } = 30;

    public int AuthorizationCodeLifetimeSeconds { get; set; } = 60;
}
