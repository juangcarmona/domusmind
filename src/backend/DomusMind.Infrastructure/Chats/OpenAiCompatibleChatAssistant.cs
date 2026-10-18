using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DomusMind.Application.Features.Chats.Assistant;

namespace DomusMind.Infrastructure.Chats;

public sealed class OpenAiCompatibleChatAssistant(IHttpClientFactory clients) : IChatAssistantProvider
{
    public string Protocol => "OpenAICompatible";

    public async Task<AssistantCompletion> CompleteAsync(AssistantProviderTarget target,
        AssistantProviderRequest request, CancellationToken ct)
    {
        var messages = new List<object> { new { role = "system", content = AssistantResponseProtocol.SystemPrompt } };
        messages.AddRange(request.Conversation.Select(x => (object)new { role = x.Role, content = x.Text }));
        messages.Add(new { role = "user", content = OllamaChatAssistant.BuildLatestRequest(request) });
        using var message = new HttpRequestMessage(HttpMethod.Post, target.Endpoint)
        {
            Content = JsonContent.Create(new
            {
                model = target.Model,
                temperature = 0.1,
                messages,
                response_format = new
                {
                    type = "json_schema",
                    json_schema = new
                    {
                        name = "domusmind_assistant_response",
                        strict = true,
                        schema = AssistantResponseProtocol.Schema
                    }
                }
            })
        };
        if (!string.IsNullOrWhiteSpace(target.ApiKey))
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", target.ApiKey);

        try
        {
            using var response = await clients.CreateClient(nameof(OpenAiCompatibleChatAssistant)).SendAsync(message, ct);
            var content = await response.Content.ReadAsStringAsync(ct);
            if (!response.IsSuccessStatusCode) throw CreateStatusException(response.StatusCode, content);
            using var document = JsonDocument.Parse(content);
            var answer = document.RootElement.GetProperty("choices")[0]
                .GetProperty("message").GetProperty("content").GetString();
            return AssistantResponseProtocol.Parse(answer ?? string.Empty);
        }
        catch (ChatAssistantTargetException) { throw; }
        catch (OperationCanceledException) when (ct.IsCancellationRequested) { throw; }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException or IndexOutOfRangeException)
        {
            throw new ChatAssistantTargetException("The OpenAI-compatible target is unavailable or returned an invalid structured response.", true, ex);
        }
    }

    private static ChatAssistantTargetException CreateStatusException(HttpStatusCode status, string body)
    {
        var capabilityFailure = AssistantResponseProtocol.IsCapabilityRejection(status, body);
        return new ChatAssistantTargetException(
            capabilityFailure ? "The target does not support the required structured output."
                : $"The target returned HTTP {(int)status}.",
            capabilityFailure || AssistantResponseProtocol.IsRetryableStatus(status));
    }
}
