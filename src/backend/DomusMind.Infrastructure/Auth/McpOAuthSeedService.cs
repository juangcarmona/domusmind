using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OpenIddict.Abstractions;
using DomusMind.Infrastructure.Persistence;

namespace DomusMind.Infrastructure.Auth;

public static class McpOAuthSeedService
{
    public static async Task SeedClientAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        var options = services.GetRequiredService<IOptions<McpOAuthOptions>>().Value;
        var writesEnabled = services.GetRequiredService<IConfiguration>()
            .GetValue<bool>("Mcp:Capabilities:HouseholdWrites");
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

        if (writesEnabled && await scopeManager.FindByNameAsync(McpOAuthConstants.WriteScope, cancellationToken) is null)
        {
            var scopeDescriptor = new OpenIddictScopeDescriptor
            {
                Name = McpOAuthConstants.WriteScope,
                DisplayName = "Write DomusMind household data after explicit confirmation",
            };
            scopeDescriptor.Resources.Add(options.Resource);
            await scopeManager.CreateAsync(scopeDescriptor, cancellationToken);
        }

        var existingApplication = await manager.FindByClientIdAsync(options.ClientId, cancellationToken);
        if (existingApplication is not null)
        {
            if (writesEnabled &&
                !await manager.HasPermissionAsync(
                    existingApplication,
                    OpenIddictConstants.Permissions.Prefixes.Scope + McpOAuthConstants.WriteScope,
                    cancellationToken))
            {
                var existingDescriptor = new OpenIddictApplicationDescriptor();
                await manager.PopulateAsync(existingApplication, existingDescriptor, cancellationToken);
                existingDescriptor.Permissions.Add(
                    OpenIddictConstants.Permissions.Prefixes.Scope + McpOAuthConstants.WriteScope);
                await manager.UpdateAsync(existingApplication, existingDescriptor, cancellationToken);
            }

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
        if (writesEnabled)
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + McpOAuthConstants.WriteScope);
        descriptor.AddResourcePermissions(options.Resource);
        descriptor.Requirements.Add(OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange);

        await manager.CreateAsync(descriptor, cancellationToken);
        await services.GetRequiredService<DomusMindDbContext>().SaveChangesAsync(cancellationToken);
    }
}
