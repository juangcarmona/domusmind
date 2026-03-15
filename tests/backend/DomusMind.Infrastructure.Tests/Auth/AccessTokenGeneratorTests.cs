using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DomusMind.Infrastructure.Auth;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace DomusMind.Infrastructure.Tests.Auth;

public sealed class AccessTokenGeneratorTests
{
    private static AccessTokenGenerator BuildSut(string key = "test-signing-key-must-be-32-chars!!")
    {
        var options = Options.Create(new JwtOptions
        {
            SigningKey = key,
            Issuer = "domusmind-test",
            Audience = "domusmind-test",
            ExpiryMinutes = 60,
        });

        return new AccessTokenGenerator(options);
    }

    [Fact]
    public void Generate_ReturnsNonEmptyToken()
    {
        var sut = BuildSut();

        var token = sut.Generate(Guid.NewGuid(), "user@test.com");

        token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Generate_ProducesValidJwtWithExpectedClaims()
    {
        var userId = Guid.NewGuid();
        var email = "user@test.com";
        var sut = BuildSut();

        var tokenString = sut.Generate(userId, email);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(tokenString);

        jwt.Subject.Should().Be(userId.ToString());
        // JwtSecurityTokenHandler maps ClaimTypes.Email → short name "email"
        jwt.Claims.Should().Contain(c => c.Type == "email" && c.Value == email);
        // ClaimTypes.NameIdentifier → "nameid"
        jwt.Claims.Should().Contain(c => c.Type == "nameid" && c.Value == userId.ToString());
        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Jti);
        jwt.Issuer.Should().Be("domusmind-test");
        jwt.Audiences.Should().Contain("domusmind-test");
    }

    [Fact]
    public void Generate_WithRoles_IncludesRoleClaims()
    {
        var sut = BuildSut();
        var roles = new[] { "admin", "moderator" };

        var tokenString = sut.Generate(Guid.NewGuid(), "admin@test.com", roles);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);

        // ClaimTypes.Role → short name "role" in JWT
        jwt.Claims.Should().Contain(c => c.Type == "role" && c.Value == "admin");
        jwt.Claims.Should().Contain(c => c.Type == "role" && c.Value == "moderator");
        jwt.Claims.Should().Contain(c => c.Type == "roles" && c.Value == "admin");
    }

    [Fact]
    public void Generate_HasFutureExpiry()
    {
        var sut = BuildSut();

        var tokenString = sut.Generate(Guid.NewGuid(), "user@test.com");

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);

        jwt.ValidTo.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public void Generate_ProducesUniqueTokesForSameUser()
    {
        var sut = BuildSut();
        var userId = Guid.NewGuid();

        var token1 = sut.Generate(userId, "same@test.com");
        var token2 = sut.Generate(userId, "same@test.com");

        token1.Should().NotBe(token2, "each token should have a unique jti");
    }
}
