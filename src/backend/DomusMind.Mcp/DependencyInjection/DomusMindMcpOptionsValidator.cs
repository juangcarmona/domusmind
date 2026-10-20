using Microsoft.Extensions.Options;

namespace DomusMind.Mcp.DependencyInjection;

internal sealed class DomusMindMcpOptionsValidator : IValidateOptions<DomusMindMcpOptions>
{
    public ValidateOptionsResult Validate(string? name, DomusMindMcpOptions options)
    {
        foreach (var origin in options.AllowedOrigins)
        {
            if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) ||
                uri.AbsolutePath != "/" || !string.IsNullOrEmpty(uri.Query) || !string.IsNullOrEmpty(uri.Fragment) ||
                !string.Equals(origin, uri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            {
                return ValidateOptionsResult.Fail("Mcp:AllowedOrigins must contain only HTTP(S) origins without paths, queries, or fragments.");
            }
        }

        if (options.RequireOAuth && string.IsNullOrWhiteSpace(options.ExpectedAudience))
            return ValidateOptionsResult.Fail("Mcp:OAuth:Resource must be configured when MCP OAuth protection is required.");

        if (options.RequestsPerMinute is < 1 or > 600)
            return ValidateOptionsResult.Fail("Mcp:RequestsPerMinute must be between 1 and 600.");

        return ValidateOptionsResult.Success;
    }
}
