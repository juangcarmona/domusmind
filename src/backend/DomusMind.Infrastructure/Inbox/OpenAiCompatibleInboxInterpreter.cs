using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DomusMind.Application.Features.Chats.Assistant;
using DomusMind.Infrastructure.Chats;
using DomusMind.Application.Features.Inbox;

namespace DomusMind.Infrastructure.Inbox;

public sealed class OpenAiCompatibleInboxInterpreter(IHttpClientFactory clients) : IInboxInterpreterProvider
{
    public string Protocol => "OpenAICompatible";

    public async Task<IReadOnlyList<InterpretedProposal>> InterpretAsync(AssistantProviderTarget target,
        string text, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, target.Endpoint)
        {
            Content = JsonContent.Create(new
            {
                model = target.Model,
                temperature = 0,
                messages = new[]
                {
                    new { role = "system", content = InboxInterpretationProtocol.SystemPrompt },
                    new { role = "user", content = $"Current UTC date: {DateTime.UtcNow:yyyy-MM-dd}. Note:\n{text}" }
                },
                response_format = new
                {
                    type = "json_schema",
                    json_schema = new { name = "inbox_proposals", strict = true, schema = InboxInterpretationProtocol.Schema }
                }
            })
        };
        if (!string.IsNullOrWhiteSpace(target.ApiKey))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", target.ApiKey);

        try
        {
            using var response = await clients.CreateClient(nameof(OpenAiCompatibleInboxInterpreter))
                .SendAsync(request, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode) throw CreateStatusException(response.StatusCode, content);
            using var document = JsonDocument.Parse(content);
            var result = document.RootElement.GetProperty("choices")[0]
                .GetProperty("message").GetProperty("content").GetString()
                ?? throw new ChatAssistantTargetException("The provider returned no Inbox interpretation.", true);
            return InboxInterpretationProtocol.Parse(result);
        }
        catch (ChatAssistantTargetException) { throw; }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { throw; }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException or IndexOutOfRangeException)
        {
            throw new ChatAssistantTargetException("The OpenAI-compatible target is unavailable or returned an invalid interpretation.", true, ex);
        }
    }

    private static ChatAssistantTargetException CreateStatusException(HttpStatusCode status, string body)
    {
        var capabilityFailure = AssistantResponseProtocol.IsCapabilityRejection(status, body);
        return new ChatAssistantTargetException($"The provider returned HTTP {(int)status}.",
            capabilityFailure || AssistantResponseProtocol.IsRetryableStatus(status));
    }
}
