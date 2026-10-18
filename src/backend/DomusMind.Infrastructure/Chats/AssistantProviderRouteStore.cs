using System.Text.Json;
using DomusMind.Application.Features.Chats.Assistant;
using DomusMind.Infrastructure.Persistence;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DomusMind.Infrastructure.Chats;

public sealed class AssistantProviderRouteStore(
    DomusMindDbContext db,
    IDataProtectionProvider protection,
    IConfiguration configuration) : IAssistantProviderRouteStore
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IDataProtector _protector = protection.CreateProtector("DomusMind.Assistant.ProviderCredential.v1");

    public async Task<AssistantProviderRoute> GetAsync(Guid familyId, CancellationToken cancellationToken)
    {
        var saved = await db.AssistantProviderRoutes.AsNoTracking()
            .SingleOrDefaultAsync(x => x.FamilyId == familyId, cancellationToken);
        if (saved is null) return GetDeploymentDefault(familyId);

        var stored = JsonSerializer.Deserialize<List<StoredTarget>>(saved.TargetsJson, JsonOptions) ?? [];
        return new AssistantProviderRoute(familyId, stored.Select(ToTarget).ToArray(), false);
    }

    public async Task<AssistantProviderRoute> ReplaceAsync(Guid familyId,
        IReadOnlyList<AssistantProviderTarget> targets, CancellationToken cancellationToken)
    {
        var current = await GetAsync(familyId, cancellationToken);
        var previous = current.Targets.ToDictionary(x => x.Id);
        var stored = targets.Select(target =>
        {
            var apiKey = string.IsNullOrEmpty(target.ApiKey) && previous.TryGetValue(target.Id, out var existing)
                ? existing.ApiKey
                : target.ApiKey;
            return new StoredTarget(target.Id, target.Name, target.Hosting, target.Protocol, target.Endpoint,
                target.Model, string.IsNullOrEmpty(apiKey) ? null : _protector.Protect(apiKey));
        }).ToList();

        var record = await db.AssistantProviderRoutes.SingleOrDefaultAsync(
            x => x.FamilyId == familyId, cancellationToken);
        if (record is null)
        {
            record = new AssistantProviderRouteRecord { FamilyId = familyId };
            db.AssistantProviderRoutes.Add(record);
        }
        record.TargetsJson = JsonSerializer.Serialize(stored, JsonOptions);
        record.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken);
        return new AssistantProviderRoute(familyId, stored.Select(ToTarget).ToArray(), false);
    }

    private AssistantProviderRoute GetDeploymentDefault(Guid familyId)
    {
        var provider = configuration["Inbox:Interpreter:Provider"] ?? "Ollama";
        var endpoint = provider == "Ollama"
            ? configuration["OLLAMA_LLAMA3_2_URI"]
            : configuration["Inbox:Interpreter:Endpoint"];
        var model = provider == "Ollama"
            ? configuration["OLLAMA_LLAMA3_2_MODEL"] ?? "llama3.2:3b"
            : configuration["Inbox:Interpreter:Model"];
        if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(model))
            return new AssistantProviderRoute(familyId, [], true);

        var apiKey = configuration["Inbox:Interpreter:ApiKey"];
        var hosting = configuration["Inbox:Interpreter:Hosting"] ?? (provider == "Ollama" ? "Local" : "Hosted");
        var protocol = provider == "Ollama" ? "Ollama" : "OpenAICompatible";
        return new AssistantProviderRoute(familyId,
            [new AssistantProviderTarget(Guid.Parse("00000000-0000-0000-0000-000000000001"),
                "Deployment default", hosting, protocol, endpoint, model, apiKey)], true);
    }

    private AssistantProviderTarget ToTarget(StoredTarget target) => new(target.Id, target.Name,
        target.Hosting, target.Protocol, target.Endpoint, target.Model,
        string.IsNullOrEmpty(target.ProtectedApiKey) ? null : _protector.Unprotect(target.ProtectedApiKey));

    private sealed record StoredTarget(Guid Id, string Name, string Hosting, string Protocol,
        string Endpoint, string Model, string? ProtectedApiKey);
}
