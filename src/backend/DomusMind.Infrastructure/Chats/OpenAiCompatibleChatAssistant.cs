using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DomusMind.Application.Features.Chats.Assistant;
using Microsoft.Extensions.Configuration;

namespace DomusMind.Infrastructure.Chats;

public sealed class OpenAiCompatibleChatAssistant(IHttpClientFactory clients, IConfiguration configuration)
    : IChatAssistant
{
    public async Task<string> AnswerAsync(string question, string authorizedContext, CancellationToken ct)
    {
        var endpoint = configuration["Inbox:Interpreter:Endpoint"]
            ?? throw new ChatAssistantUnavailableException("Inbox:Interpreter:Endpoint is not configured.");
        var model = configuration["Inbox:Interpreter:Model"]
            ?? throw new ChatAssistantUnavailableException("Inbox:Interpreter:Model is not configured.");
        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = JsonContent.Create(new
            {
                model,
                temperature = 0.1,
                messages = new[]
                {
                    new { role = "system", content = ChatAssistantPrompt.System },
                    new { role = "user", content = $"Question:\n{question}\n\nAuthorized household data (JSON):\n{authorizedContext}" }
                }
            })
        };
        var apiKey = configuration["Inbox:Interpreter:ApiKey"];
        if (!string.IsNullOrWhiteSpace(apiKey))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        try
        {
            using var response = await clients.CreateClient(nameof(OpenAiCompatibleChatAssistant)).SendAsync(request, ct);
            response.EnsureSuccessStatusCode();
            using var body = await response.Content.ReadAsStreamAsync(ct);
            using var document = await JsonDocument.ParseAsync(body, cancellationToken: ct);
            return document.RootElement.GetProperty("choices")[0].GetProperty("message")
                .GetProperty("content").GetString()?.Trim() is { Length: > 0 } answer ? answer
                : throw new ChatAssistantUnavailableException("The configured assistant returned no answer.");
        }
        catch (ChatAssistantUnavailableException) { throw; }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException)
        {
            throw new ChatAssistantUnavailableException("The configured assistant is unavailable.", ex);
        }
    }
}
