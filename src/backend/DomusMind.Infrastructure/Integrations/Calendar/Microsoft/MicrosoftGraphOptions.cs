namespace DomusMind.Infrastructure.Integrations.Calendar.Microsoft;

public sealed class MicrosoftGraphOptions
{
    public const string SectionName = "MicrosoftGraph";

    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? TenantId { get; set; }
    public string? TokenEncryptionKey { get; set; }
    public string[] AllowedRedirectUris { get; set; } = [];

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(ClientId) ||
        !string.IsNullOrWhiteSpace(ClientSecret) ||
        AllowedRedirectUris.Any(uri => !string.IsNullOrWhiteSpace(uri));

    public string? Validate()
    {
        if (!IsConfigured)
            return null;

        if (string.IsNullOrWhiteSpace(ClientId) || string.IsNullOrWhiteSpace(ClientSecret))
            return "MicrosoftGraph ClientId and ClientSecret must both be set when Outlook integration is configured.";

        var redirectUris = AllowedRedirectUris
            .Where(uri => !string.IsNullOrWhiteSpace(uri))
            .ToArray();

        if (redirectUris.Length == 0 || redirectUris.Any(uri => !IsAllowedRedirectUri(uri)))
            return "MicrosoftGraph must configure at least one HTTPS Outlook callback URI (HTTP is allowed only for loopback development), without query, fragment, or user info.";

        if (!TryGetEncryptionKey(out _))
            return "MicrosoftGraph TokenEncryptionKey must be Base64 for exactly 32 bytes when Outlook integration is configured.";

        return null;
    }

    public bool IsRedirectUriAllowed(string redirectUri) =>
        AllowedRedirectUris.Any(allowed => string.Equals(redirectUri, allowed, StringComparison.Ordinal));

    public bool TryGetEncryptionKey(out byte[] key)
    {
        key = [];
        if (string.IsNullOrWhiteSpace(TokenEncryptionKey))
            return false;

        try
        {
            key = Convert.FromBase64String(TokenEncryptionKey);
            return key.Length == 32;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static bool IsAllowedRedirectUri(string redirectUri)
    {
        if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out var uri) ||
            !string.IsNullOrEmpty(uri.UserInfo) ||
            !string.IsNullOrEmpty(uri.Query) ||
            !string.IsNullOrEmpty(uri.Fragment))
        {
            return false;
        }

        if (!string.Equals(uri.AbsolutePath, "/settings/oauth/outlook/callback", StringComparison.Ordinal))
            return false;

        return uri.Scheme == Uri.UriSchemeHttps ||
            (uri.Scheme == Uri.UriSchemeHttp && uri.IsLoopback);
    }
}
