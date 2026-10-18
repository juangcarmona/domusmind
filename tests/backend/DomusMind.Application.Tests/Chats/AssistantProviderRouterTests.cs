using DomusMind.Application.Features.Chats.Assistant;
using FluentAssertions;

namespace DomusMind.Application.Tests.Chats;

public sealed class AssistantProviderRouterTests
{
    [Fact]
    public async Task CompleteAsync_WhenPrimaryIsRetryable_UsesNextTargetAndReportsFallback()
    {
        var familyId = Guid.NewGuid();
        var route = new AssistantProviderRoute(familyId,
        [
            Target("primary", "Ollama"),
            Target("backup", "OpenAICompatible")
        ], false);
        var router = new AssistantProviderRouter(new RouteStore(route),
        [
            new Provider("Ollama", (_, _, _) => throw new ChatAssistantTargetException("offline", true)),
            new Provider("OpenAICompatible", (_, _, _) => Task.FromResult(new AssistantCompletion("answer", null)))
        ]);

        var result = await router.CompleteAsync(familyId, Request(), CancellationToken.None);

        result.ProviderName.Should().Be("backup");
        result.FallbackOccurred.Should().BeTrue();
        result.Completion.Answer.Should().Be("answer");
    }

    [Fact]
    public async Task CompleteAsync_WhenPrimaryRejectsRequest_DoesNotFallback()
    {
        var familyId = Guid.NewGuid();
        var secondaryCalls = 0;
        var router = new AssistantProviderRouter(new RouteStore(new AssistantProviderRoute(familyId,
        [Target("primary", "Ollama"), Target("backup", "OpenAICompatible")], false)),
        [
            new Provider("Ollama", (_, _, _) => throw new ChatAssistantTargetException("rejected", false)),
            new Provider("OpenAICompatible", (_, _, _) =>
            {
                secondaryCalls++;
                return Task.FromResult(new AssistantCompletion("answer", null));
            })
        ]);

        await FluentActions.Invoking(() => router.CompleteAsync(familyId, Request(), CancellationToken.None))
            .Should().ThrowAsync<ChatAssistantUnavailableException>();

        secondaryCalls.Should().Be(0);
    }

    private static AssistantProviderTarget Target(string name, string protocol) =>
        new(Guid.NewGuid(), name, "Local", protocol, "http://localhost", "model", null);

    private static AssistantProviderRequest Request() => new("question", [], "{}", true);

    private sealed class RouteStore(AssistantProviderRoute route) : IAssistantProviderRouteStore
    {
        public Task<AssistantProviderRoute> GetAsync(Guid familyId, CancellationToken cancellationToken) =>
            Task.FromResult(route);

        public Task<AssistantProviderRoute> ReplaceAsync(Guid familyId,
            IReadOnlyList<AssistantProviderTarget> targets, CancellationToken cancellationToken) =>
            throw new NotSupportedException();
    }

    private sealed class Provider(string protocol,
        Func<AssistantProviderTarget, AssistantProviderRequest, CancellationToken, Task<AssistantCompletion>> complete)
        : IChatAssistantProvider
    {
        public string Protocol => protocol;

        public Task<AssistantCompletion> CompleteAsync(AssistantProviderTarget target,
            AssistantProviderRequest request, CancellationToken cancellationToken) =>
            complete(target, request, cancellationToken);
    }
}
