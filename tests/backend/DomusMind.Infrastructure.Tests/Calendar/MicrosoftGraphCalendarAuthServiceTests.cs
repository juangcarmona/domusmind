using DomusMind.Application.Features.Calendar;
using DomusMind.Infrastructure.Integrations.Calendar.Microsoft;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace DomusMind.Infrastructure.Tests.Calendar;

public sealed class MicrosoftGraphCalendarAuthServiceTests
{
    private static readonly string CallbackUri = "https://domusmind.example/settings/oauth/outlook/callback";

    [Fact]
    public async Task CreateAuthorizationUrl_usesRandomBoundStateAndS256Challenge()
    {
        await using var dbContext = CreateDbContext();
        var service = CreateService(dbContext, CreateConfiguredOptions());
        var familyId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var url = await service.CreateAuthorizationUrlAsync(familyId, memberId, userId, CallbackUri);

        url.Should().NotBeNull();
        var query = QueryHelpers.ParseQuery(new Uri(url!).Query);
        var state = query["state"].ToString();
        state.Should().HaveLength(43);
        state.Should().NotContain(familyId.ToString());
        state.Should().NotContain(memberId.ToString());
        query["scope"].ToString().Should().Be("https://graph.microsoft.com/Calendars.Read offline_access");
        query["code_challenge_method"].ToString().Should().Be("S256");
        query["code_challenge"].ToString().Should().HaveLength(43);
        query["redirect_uri"].ToString().Should().Be(CallbackUri);

        var transaction = await dbContext.Set<ExternalCalendarOAuthTransaction>().SingleAsync();
        transaction.StateHash.Should().NotBe(state);
        transaction.ProtectedPkceVerifier.Should().StartWith("v1.");
    }

    [Fact]
    public async Task AuthorizationState_isBoundToUserFamilyMemberAndRedirectAndConsumedOnce()
    {
        await using var dbContext = CreateDbContext();
        var service = CreateService(dbContext, CreateConfiguredOptions(CallbackUri, "http://localhost:5173/settings/oauth/outlook/callback"));
        var familyId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var url = await service.CreateAuthorizationUrlAsync(familyId, memberId, userId, CallbackUri);
        var state = QueryHelpers.ParseQuery(new Uri(url!).Query)["state"].ToString();

        (await service.ConsumeAuthorizationStateAsync(state, familyId, memberId, Guid.NewGuid(), CallbackUri)).Should().BeNull();
        (await service.ConsumeAuthorizationStateAsync(state, familyId, memberId, userId, "http://localhost:5173/settings/oauth/outlook/callback")).Should().BeNull();

        var verifier = await service.ConsumeAuthorizationStateAsync(state, familyId, memberId, userId, CallbackUri);
        verifier.Should().NotBeNullOrWhiteSpace();

        (await service.ConsumeAuthorizationStateAsync(state, familyId, memberId, userId, CallbackUri)).Should().BeNull();
    }

    [Fact]
    public async Task ExchangeAuthorizationCode_rejectsUnknownStateBeforeCallingMicrosoft()
    {
        await using var dbContext = CreateDbContext();
        var service = CreateService(dbContext, CreateConfiguredOptions());

        var act = () => service.ExchangeAuthorizationCodeAsync(
            "unused-code",
            "unknown-state",
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            CallbackUri);

        await act.Should().ThrowAsync<CalendarException>()
            .Where(exception => exception.Code == CalendarErrorCode.InvalidInput);
    }

    [Fact]
    public void MicrosoftGraphOptions_requireCompleteSecureConfigurationWhenEnabled()
    {
        var missingKey = CreateConfiguredOptions();
        missingKey.TokenEncryptionKey = "";
        missingKey.Validate().Should().Contain("TokenEncryptionKey");

        var invalidKey = CreateConfiguredOptions();
        invalidKey.TokenEncryptionKey = Convert.ToBase64String(new byte[16]);
        invalidKey.Validate().Should().Contain("32 bytes");

        var invalidRedirect = CreateConfiguredOptions("http://public.example/settings/oauth/outlook/callback");
        invalidRedirect.Validate().Should().Contain("callback URI");

        new MicrosoftGraphOptions().Validate().Should().BeNull();
    }

    [Fact]
    public void MicrosoftGraphOptions_onlyAllowExactConfiguredCallbackUris()
    {
        var options = CreateConfiguredOptions();

        options.IsRedirectUriAllowed(CallbackUri).Should().BeTrue();
        options.IsRedirectUriAllowed("https://attacker.example/settings/oauth/outlook/callback").Should().BeFalse();
        options.IsRedirectUriAllowed(CallbackUri + "?next=https://attacker.example").Should().BeFalse();
    }

    [Fact]
    public void TokenCacheProtector_authenticatesDataAndRejectsLegacyUnversionedValues()
    {
        var protector = new MicrosoftTokenCacheProtector(CreateConfiguredOptions());
        var protectedValue = protector.Protect("serialized-msal-cache");

        protectedValue.Should().StartWith("v1.");
        protector.TryUnprotect(protectedValue, out var plainText).Should().BeTrue();
        plainText.Should().Be("serialized-msal-cache");
        protector.TryUnprotect(Convert.ToBase64String("legacy-plaintext"u8), out _).Should().BeFalse();

        var envelope = Convert.FromBase64String(protectedValue[3..]);
        envelope[^1] ^= 0x80;
        protector.TryUnprotect("v1." + Convert.ToBase64String(envelope), out _).Should().BeFalse();
    }

    [Fact]
    public void GrantedScopes_mustBeExactlyReadOnlyCalendarPermission()
    {
        MicrosoftGraphScopePolicy.HasOnlyCalendarRead(["Calendars.Read"]).Should().BeTrue();
        MicrosoftGraphScopePolicy.HasOnlyCalendarRead(["https://graph.microsoft.com/Calendars.Read", "offline_access"]).Should().BeTrue();
        MicrosoftGraphScopePolicy.HasOnlyCalendarRead([]).Should().BeFalse();
        MicrosoftGraphScopePolicy.HasOnlyCalendarRead(["Calendars.Read", "Calendars.ReadWrite"]).Should().BeFalse();
        MicrosoftGraphScopePolicy.HasOnlyCalendarRead(["Calendars.Read", "User.Read"]).Should().BeFalse();
    }

    private static DomusMindDbContext CreateDbContext() => new(
        new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static MicrosoftGraphCalendarAuthService CreateService(
        DomusMindDbContext dbContext,
        MicrosoftGraphOptions options) => new(
            dbContext,
            Options.Create(options),
            NullLogger<MicrosoftGraphCalendarAuthService>.Instance);

    private static MicrosoftGraphOptions CreateConfiguredOptions(params string[] redirectUris) => new()
    {
        ClientId = "test-client-id",
        ClientSecret = "test-client-secret",
        TenantId = "common",
        TokenEncryptionKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
        AllowedRedirectUris = redirectUris.Length == 0 ? [CallbackUri] : redirectUris,
    };
}
