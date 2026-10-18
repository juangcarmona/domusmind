using System.Net.Http.Json;
using System.Text.Json;
using DomusMind.Application.Features.Inbox;
using Microsoft.Extensions.Configuration;

namespace DomusMind.Infrastructure.Inbox;

public sealed class OllamaInboxInterpreter(IHttpClientFactory clients, IConfiguration configuration) : IInboxInterpreter
{
    public async Task<IReadOnlyList<InterpretedProposal>> InterpretAsync(string text, CancellationToken cancellationToken)
    {
        var endpoint = configuration["OLLAMA_LLAMA3_2_URI"] ?? throw new HttpRequestException("The local Ollama model is not configured.");
        var model = configuration["OLLAMA_LLAMA3_2_MODEL"] ?? "llama3.2:3b";
        var request = new
        {
            model,
            stream = false,
            format = InboxInterpretationProtocol.Schema,
            system = InboxInterpretationProtocol.SystemPrompt,
            prompt = $"Current UTC date: {DateTime.UtcNow:yyyy-MM-dd}. Note:\n{text}",
            options = new { temperature = 0 }
        };
        using var response = await clients.CreateClient(nameof(OllamaInboxInterpreter))
            .PostAsJsonAsync(new Uri(new Uri(endpoint), "/api/generate"), request, cancellationToken);
        response.EnsureSuccessStatusCode();
        using var body = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var document = await JsonDocument.ParseAsync(body, cancellationToken: cancellationToken);
        var content = document.RootElement.GetProperty("response").GetString()
            ?? throw new InvalidOperationException("The local interpreter returned no result.");
        return InboxInterpretationProtocol.Parse(content);
    }
}
