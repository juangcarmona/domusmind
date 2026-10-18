namespace DomusMind.Application.Features.Chats.Assistant;

public sealed class AssistantProviderRouter(
    IAssistantProviderRouteStore routes,
    IEnumerable<IChatAssistantProvider> providers) : IChatAssistant
{
    public async Task<AssistantProviderResult> CompleteAsync(Guid familyId,
        AssistantProviderRequest request, CancellationToken cancellationToken)
    {
        var route = await routes.GetAsync(familyId, cancellationToken);
        if (route.Targets.Count == 0)
            throw new ChatAssistantUnavailableException("No assistant provider is configured for this household.");

        var adapters = providers.ToDictionary(x => x.Protocol, StringComparer.OrdinalIgnoreCase);
        var fallbackOccurred = false;
        ChatAssistantTargetException? lastRetryableFailure = null;
        foreach (var target in route.Targets)
        {
            if (!adapters.TryGetValue(target.Protocol, out var adapter))
            {
                fallbackOccurred = true;
                lastRetryableFailure = new ChatAssistantTargetException(
                    $"The configured protocol '{target.Protocol}' is not supported.", retryable: true);
                continue;
            }

            try
            {
                var completion = await adapter.CompleteAsync(target, request, cancellationToken);
                return new AssistantProviderResult(completion, target.Name, target.Hosting, fallbackOccurred);
            }
            catch (ChatAssistantTargetException ex) when (ex.Retryable)
            {
                lastRetryableFailure = ex;
                fallbackOccurred = true;
            }
            catch (ChatAssistantTargetException ex)
            {
                throw new ChatAssistantUnavailableException(
                    "The configured assistant provider rejected this request.", ex);
            }
        }

        throw new ChatAssistantUnavailableException(
            "No configured assistant provider could handle this request.", lastRetryableFailure);
    }
}
