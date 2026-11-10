using System.Security.Claims;
using DomusMind.Api.Auth;
using DomusMind.Application.Abstractions.Security;
using Microsoft.Extensions.Configuration;

namespace DomusMind.Api.Tests.Auth;

public sealed class DevelopmentBffIdentityMapperTests
{
    private static readonly Guid UserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private const string Subject = "development-subject";
    private const string Email = "member@domusmind.local";

    [Fact]
    public async Task MapAsync_ShouldMapVerifiedConfiguredSubjectToActiveLocalUser()
    {
        var user = new AuthUserRecord(UserId, Email, "hash");
        var mapped = await DevelopmentBffIdentityMapper.MapAsync(
            Identity(Subject, Email, "true"),
            Configuration((Subject, Email)),
            new TestAuthUserRepository(user),
            CancellationToken.None);

        Assert.NotNull(mapped);
        Assert.Equal(UserId.ToString(), mapped.FindFirstValue(ClaimTypes.NameIdentifier));
        Assert.Equal(Email, mapped.FindFirstValue(ClaimTypes.Email));
    }

    [Theory]
    [InlineData("true", "other@domusmind.local", true)]
    [InlineData("false", Email, true)]
    [InlineData("true", Email, false)]
    public async Task MapAsync_ShouldRejectUnverifiedMismatchedOrUnconfiguredIdentity(
        string verified,
        string assertedEmail,
        bool includeSubjectMapping)
    {
        var mapped = await DevelopmentBffIdentityMapper.MapAsync(
            Identity(Subject, assertedEmail, verified),
            includeSubjectMapping ? Configuration((Subject, Email)) : Configuration(),
            new TestAuthUserRepository(new AuthUserRecord(UserId, Email, "hash")),
            CancellationToken.None);

        Assert.Null(mapped);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task MapAsync_ShouldRejectDisabledOrMustChangeLocalUser(bool disabled, bool mustChangePassword)
    {
        var mapped = await DevelopmentBffIdentityMapper.MapAsync(
            Identity(Subject, Email, "true"),
            Configuration((Subject, Email)),
            new TestAuthUserRepository(new AuthUserRecord(
                UserId, Email, "hash", MustChangePassword: mustChangePassword, IsDisabled: disabled)),
            CancellationToken.None);

        Assert.Null(mapped);
    }

    [Fact]
    public async Task MapAsync_ShouldRejectIdentityWithoutLocalAccount()
    {
        var mapped = await DevelopmentBffIdentityMapper.MapAsync(
            Identity(Subject, Email, "true"),
            Configuration((Subject, Email)),
            new TestAuthUserRepository(null),
            CancellationToken.None);

        Assert.Null(mapped);
    }

    private static ClaimsPrincipal Identity(string subject, string email, string verified) => new(
        new ClaimsIdentity(
        [
            new Claim("sub", subject),
            new Claim("email", email),
            new Claim("email_verified", verified),
        ], "oidc"));

    private static IConfiguration Configuration(params (string Subject, string Email)[] mappings) =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(mappings.ToDictionary(
                mapping => $"Bff:TestUsers:{mapping.Subject}",
                mapping => (string?)mapping.Email))
            .Build();

    private sealed class TestAuthUserRepository(AuthUserRecord? user) : IAuthUserRepository
    {
        public Task<AuthUserRecord?> FindByEmailAsync(string email, CancellationToken cancellationToken) =>
            Task.FromResult(user is not null && string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase) ? user : null);

        public Task<AuthUserRecord?> FindByIdAsync(Guid userId, CancellationToken cancellationToken) =>
            Task.FromResult(user?.UserId == userId ? user : null);

        public Task<IReadOnlyDictionary<Guid, AuthUserStatusProjection>> GetStatusByIdsAsync(
            IReadOnlyCollection<Guid> userIds,
            CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyDictionary<Guid, AuthUserStatusProjection>>(new Dictionary<Guid, AuthUserStatusProjection>());

        public Task AddAsync(AuthUserRecord user, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task UpdatePasswordHashAsync(Guid userId, string newPasswordHash, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task UpdateMustChangePasswordAsync(Guid userId, bool mustChangePassword, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task UpdatePasswordChangedAtAsync(Guid userId, DateTime changedAtUtc, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task DisableUserAsync(Guid userId, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task EnableUserAsync(Guid userId, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task UpdateLastLoginAtAsync(Guid userId, DateTime lastLoginAtUtc, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task<bool> AnyUsersAsync(CancellationToken cancellationToken) => Task.FromResult(user is not null);
        public Task SaveChangesAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
