using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DomusMind.Application.Features.Chats.Assistant;
using DomusMind.Infrastructure.Chats;
using DomusMind.Application.Features.Inbox;

namespace DomusMind.Infrastructure.Inbox;

public sealed class OllamaInboxInterpreter(IHttpClientFactory clients) : IInboxInterpreterProvider
{
    public string Protocol => "Ollama";

    public async Task<IReadOnlyList<InterpretedProposal>> InterpretAsync(AssistantProviderTarget target,
        string text, CancellationToken cancellationToken)
    {
        var request = new
        {
            model = target.Model,
            stream = false,
            format = InboxInterpretationProtocol.Schema,
            system = InboxInterpretationProtocol.SystemPrompt,
            prompt = $"Current UTC date: {DateTime.UtcNow:yyyy-MM-dd}. Note:\n{text}",
            options = new { temperature = 0 }
        };
        try
        {
            using var response = await clients.CreateClient(nameof(OllamaInboxInterpreter))
                .PostAsJsonAsync(new Uri(new Uri(target.Endpoint), "/api/generate"), request, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode) throw CreateStatusException(response.StatusCode, content);
            using var document = JsonDocument.Parse(content);
            var result = document.RootElement.GetProperty("response").GetString()
                ?? throw new ChatAssistantTargetException("Ollama returned no interpretation.", true);
            return InboxInterpretationProtocol.Parse(result);
        }
        catch (ChatAssistantTargetException) { throw; }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { throw; }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException)
        {
            throw new ChatAssistantTargetException("The Ollama target is unavailable or returned an invalid interpretation.", true, ex);
        }
    }

    private static ChatAssistantTargetException CreateStatusException(HttpStatusCode status, string body)
    {
        var capabilityFailure = AssistantResponseProtocol.IsCapabilityRejection(status, body);
        return new ChatAssistantTargetException($"Ollama returned HTTP {(int)status}.",
            capabilityFailure || AssistantResponseProtocol.IsRetryableStatus(status));
    }
}
