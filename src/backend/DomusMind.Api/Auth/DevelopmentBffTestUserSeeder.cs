using System.Security.Cryptography;
using DomusMind.Application.Abstractions.Security;

namespace DomusMind.Api.Auth;

public static class DevelopmentBffTestUserSeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration, CancellationToken cancellationToken)
    {
        var emails = configuration.GetSection("Bff:SeededTestUsers").Get<string[]>() ?? [];
        if (emails.Length == 0)
            return;

        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;
        var users = provider.GetRequiredService<IAuthUserRepository>();
        var passwordHasher = provider.GetRequiredService<IPasswordHasher>();

        foreach (var configuredEmail in emails)
        {
            var email = configuredEmail.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(email) || await users.FindByEmailAsync(email, cancellationToken) is not null)
                continue;

            var opaquePassword = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            await users.AddAsync(new AuthUserRecord(
                Guid.NewGuid(),
                email,
                passwordHasher.Hash(opaquePassword),
                DisplayName: "Development test account"), cancellationToken);
        }

        await users.SaveChangesAsync(cancellationToken);
    }
}
