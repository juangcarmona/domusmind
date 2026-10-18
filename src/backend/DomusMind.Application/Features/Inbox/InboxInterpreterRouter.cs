using DomusMind.Application.Features.Chats.Assistant;

namespace DomusMind.Application.Features.Inbox;

public sealed class InboxInterpreterRouter(IAssistantProviderRouteStore routes,
    IEnumerable<IInboxInterpreterProvider> providers) : IInboxInterpreter
{
    public async Task<InboxInterpretationResult> InterpretAsync(Guid familyId, string text,
        CancellationToken cancellationToken)
    {
        var route = await routes.GetAsync(familyId, cancellationToken);
        if (route.Targets.Count == 0)
            throw new ChatAssistantUnavailableException("No assistant provider is configured for this household.");
        var adapters = providers.ToDictionary(x => x.Protocol, StringComparer.OrdinalIgnoreCase);
        var fallbackOccurred = false;
        ChatAssistantTargetException? lastFailure = null;
        foreach (var target in route.Targets)
        {
            if (!adapters.TryGetValue(target.Protocol, out var adapter))
            {
                fallbackOccurred = true;
                lastFailure = new ChatAssistantTargetException("The configured provider protocol is unsupported.", true);
                continue;
            }
            try
            {
                var proposals = await adapter.InterpretAsync(target, text, cancellationToken);
                return new InboxInterpretationResult(proposals, target.Name, target.Hosting, fallbackOccurred);
            }
            catch (ChatAssistantTargetException ex) when (ex.Retryable)
            {
                lastFailure = ex;
                fallbackOccurred = true;
            }
            catch (ChatAssistantTargetException ex)
            {
                throw new ChatAssistantUnavailableException("The configured assistant provider rejected this request.", ex);
            }
        }
        throw new ChatAssistantUnavailableException("No configured provider could interpret this Inbox note.", lastFailure);
    }
}
