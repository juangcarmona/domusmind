using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using DomusMind.Infrastructure.Persistence;

namespace DomusMind.Infrastructure.Auth;

public static class McpOAuthSeedService
{
    public static async Task SeedClientAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        var options = services.GetRequiredService<IOptions<McpOAuthOptions>>().Value;
        if (!options.Enabled)
            return;

        var manager = services.GetRequiredService<IOpenIddictApplicationManager>();
        var scopeManager = services.GetRequiredService<IOpenIddictScopeManager>();

        if (await scopeManager.FindByNameAsync(McpOAuthConstants.ReadScope, cancellationToken) is null)
        {
            var scopeDescriptor = new OpenIddictScopeDescriptor
            {
                Name = McpOAuthConstants.ReadScope,
                DisplayName = "Read DomusMind household data",
            };
            scopeDescriptor.Resources.Add(options.Resource);
            await scopeManager.CreateAsync(scopeDescriptor, cancellationToken);
        }

        if (await manager.FindByClientIdAsync(options.ClientId, cancellationToken) is not null)
        {
            await services.GetRequiredService<DomusMindDbContext>().SaveChangesAsync(cancellationToken);
            return;
        }

        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = options.ClientId,
            DisplayName = "DomusMind MCP client",
            ClientType = OpenIddictConstants.ClientTypes.Public,
            ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
        };

        foreach (var redirectUri in options.RedirectUris)
        {
            if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out var uri))
                throw new InvalidOperationException($"Mcp:OAuth:RedirectUris contains an invalid URI: '{redirectUri}'.");

            descriptor.RedirectUris.Add(uri);
        }

        descriptor.Permissions.UnionWith(
        [
            OpenIddictConstants.Permissions.Endpoints.Authorization,
            OpenIddictConstants.Permissions.Endpoints.Token,
            OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
            OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
            OpenIddictConstants.Permissions.ResponseTypes.Code,
            OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.OpenId,
            OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.Profile,
            OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.OfflineAccess,
            OpenIddictConstants.Permissions.Prefixes.Scope + McpOAuthConstants.ReadScope,
        ]);
        descriptor.AddResourcePermissions(options.Resource);
        descriptor.Requirements.Add(OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange);

        await manager.CreateAsync(descriptor, cancellationToken);
        await services.GetRequiredService<DomusMindDbContext>().SaveChangesAsync(cancellationToken);
    }
}
