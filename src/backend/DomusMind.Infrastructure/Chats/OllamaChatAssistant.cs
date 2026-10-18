using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DomusMind.Application.Features.Chats.Assistant;

namespace DomusMind.Infrastructure.Chats;

public sealed class OllamaChatAssistant(IHttpClientFactory clients) : IChatAssistantProvider
{
    public string Protocol => "Ollama";

    public async Task<AssistantCompletion> CompleteAsync(AssistantProviderTarget target,
        AssistantProviderRequest request, CancellationToken ct)
    {
        var messages = new List<object> { new { role = "system", content = AssistantResponseProtocol.SystemPrompt } };
        messages.AddRange(request.Conversation.Select(x => (object)new { role = x.Role, content = x.Text }));
        messages.Add(new { role = "user", content = BuildLatestRequest(request) });
        var body = new
        {
            model = target.Model,
            messages,
            stream = false,
            format = AssistantResponseProtocol.Schema,
            options = new { temperature = 0.1 }
        };

        try
        {
            using var response = await clients.CreateClient(nameof(OllamaChatAssistant)).PostAsJsonAsync(
                new Uri(new Uri(target.Endpoint), "/api/chat"), body, ct);
            var content = await response.Content.ReadAsStringAsync(ct);
            if (!response.IsSuccessStatusCode) throw CreateStatusException(response.StatusCode, content);
            using var document = JsonDocument.Parse(content);
            var modelResponse = document.RootElement.GetProperty("message").GetProperty("content").GetString();
            return AssistantResponseProtocol.Parse(modelResponse ?? string.Empty);
        }
        catch (ChatAssistantTargetException) { throw; }
        catch (OperationCanceledException) when (ct.IsCancellationRequested) { throw; }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException)
        {
            throw new ChatAssistantTargetException("The Ollama target is unavailable or returned an invalid structured response.", true, ex);
        }
    }

    private static ChatAssistantTargetException CreateStatusException(HttpStatusCode status, string body)
    {
        var capabilityFailure = AssistantResponseProtocol.IsCapabilityRejection(status, body);
        return new ChatAssistantTargetException(
            capabilityFailure ? "The Ollama target does not support the required structured output."
                : $"The Ollama target returned HTTP {(int)status}.",
            capabilityFailure || AssistantResponseProtocol.IsRetryableStatus(status));
    }

    internal static string BuildLatestRequest(AssistantProviderRequest request) =>
        $"Current UTC date: {DateTime.UtcNow:yyyy-MM-dd}.\nMember request:\n{request.Question}\n\nAuthorized household data (JSON):\n{request.AuthorizedContext}\n\nCan stage a proposal: {request.CanStageProposal}.";
}
