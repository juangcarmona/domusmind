using DomusMind.Application.Abstractions.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DomusMind.Infrastructure.Auth;

/// <summary>
/// Seeds the initial admin auth user on first startup when enabled and no users exist.
/// </summary>
public sealed class AuthSeedService
{
    public static async Task SeedAdminAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        using var scope = services.CreateScope();
        var sp = scope.ServiceProvider;

        var options = sp.GetRequiredService<IOptions<BootstrapAdminOptions>>().Value;
        var logger = sp.GetRequiredService<ILogger<AuthSeedService>>();

        if (!options.Enabled)
        {
            logger.LogDebug("Auth bootstrap is disabled. Skipping.");
            return;
        }

        var repository = sp.GetRequiredService<IAuthUserRepository>();
        var hasher = sp.GetRequiredService<IPasswordHasher>();

        if (await repository.AnyUsersAsync(cancellationToken))
        {
            logger.LogInformation("Auth bootstrap skipped: users already exist.");
            return;
        }

        var email = options.Email.Trim().ToLowerInvariant();
        var user = new AuthUserRecord(Guid.NewGuid(), email, hasher.Hash(options.Password));

        await repository.AddAsync(user, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Bootstrap admin user created for {Email}.", email);
    }
}
