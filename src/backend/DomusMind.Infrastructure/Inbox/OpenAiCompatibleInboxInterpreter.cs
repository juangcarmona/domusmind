using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace DomusMind.Infrastructure.Inbox;

public sealed class OpenAiCompatibleInboxInterpreter(IHttpClientFactory clients, IConfiguration configuration)
    : DomusMind.Application.Features.Inbox.IInboxInterpreter
{
    public async Task<IReadOnlyList<DomusMind.Application.Features.Inbox.InterpretedProposal>> InterpretAsync(
        string text, CancellationToken cancellationToken)
    {
        var endpoint = configuration["Inbox:Interpreter:Endpoint"]
            ?? throw new InvalidOperationException("Inbox:Interpreter:Endpoint is required for the OpenAI-compatible provider.");
        var model = configuration["Inbox:Interpreter:Model"]
            ?? throw new InvalidOperationException("Inbox:Interpreter:Model is required for the OpenAI-compatible provider.");
        var apiKey = configuration["Inbox:Interpreter:ApiKey"];
        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = JsonContent.Create(new
            {
                model,
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
        if (!string.IsNullOrWhiteSpace(apiKey))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        using var response = await clients.CreateClient(nameof(OpenAiCompatibleInboxInterpreter))
            .SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        using var body = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var document = await JsonDocument.ParseAsync(body, cancellationToken: cancellationToken);
        var content = document.RootElement.GetProperty("choices")[0].GetProperty("message")
            .GetProperty("content").GetString()
            ?? throw new InvalidOperationException("The configured interpreter returned no result.");
        return InboxInterpretationProtocol.Parse(content);
    }
}
