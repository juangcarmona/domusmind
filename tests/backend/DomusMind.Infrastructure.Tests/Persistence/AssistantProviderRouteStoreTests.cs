using DomusMind.Application.Features.Chats.Assistant;
using DomusMind.Infrastructure.Chats;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DomusMind.Infrastructure.Tests.Persistence;

public sealed class AssistantProviderRouteStoreTests
{
    [Fact]
    public async Task ReplaceAsync_ProtectsCredentialAndPreservesItWhenOmitted()
    {
        var familyId = Guid.NewGuid();
        var options = new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var db = new DomusMindDbContext(options);
        var store = new AssistantProviderRouteStore(db, new EphemeralDataProtectionProvider(), new ConfigurationManager());
        var target = new AssistantProviderTarget(Guid.NewGuid(), "Primary", "Hosted", "OpenAICompatible",
            "https://example.test/v1/chat/completions", "model", "secret-provider-key");

        await store.ReplaceAsync(familyId, [target], CancellationToken.None);
        var persisted = await db.AssistantProviderRoutes.SingleAsync();
        persisted.TargetsJson.Should().NotContain("secret-provider-key");

        var updated = await store.ReplaceAsync(familyId, [target with { ApiKey = null }], CancellationToken.None);

        updated.Targets.Should().ContainSingle().Which.ApiKey.Should().Be("secret-provider-key");
    }
}
