using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Chats.Assistant;
using DomusMind.Contracts.Chats;
using DomusMind.Domain.Family;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Chats.ConfigureAssistantProviders;

public sealed class GetAssistantProviderRouteHandler(
    IDomusMindDbContext db,
    IFamilyAuthorizationService authorization,
    IAssistantProviderRouteStore routes)
    : IQueryHandler<GetAssistantProviderRouteQuery, AssistantProviderRouteResponse>
{
    public async Task<AssistantProviderRouteResponse> Handle(GetAssistantProviderRouteQuery query,
        CancellationToken cancellationToken)
    {
        var family = await AssistantProviderRouteAccess.GetFamilyAsync(db, authorization,
            query.FamilyId, query.UserId, cancellationToken);
        var member = family.Members.SingleOrDefault(x => x.AuthUserId == query.UserId);
        var route = await routes.GetAsync(query.FamilyId, cancellationToken);
        return AssistantProviderRouteMapping.Map(route, member?.IsManager == true);
    }
}

public sealed class ReplaceAssistantProviderRouteHandler(
    IDomusMindDbContext db,
    IFamilyAuthorizationService authorization,
    IAssistantProviderRouteStore routes)
    : ICommandHandler<ReplaceAssistantProviderRouteCommand, AssistantProviderRouteResponse>
{
    public async Task<AssistantProviderRouteResponse> Handle(ReplaceAssistantProviderRouteCommand command,
        CancellationToken cancellationToken)
    {
        var family = await AssistantProviderRouteAccess.GetFamilyAsync(db, authorization,
            command.FamilyId, command.UserId, cancellationToken);
        var member = family.Members.SingleOrDefault(x => x.AuthUserId == command.UserId);
        if (member?.IsManager != true)
            throw new UnauthorizedAccessException("Only a household manager can configure assistant providers.");

        Validate(command.Targets);
        var current = await routes.GetAsync(command.FamilyId, cancellationToken);
        var existing = current.Targets.ToDictionary(x => x.Id);
        var targets = command.Targets.Select(x => new AssistantProviderTarget(
            x.Id == Guid.Empty ? Guid.NewGuid() : x.Id,
            x.Name.Trim(),
            x.Hosting,
            x.Protocol,
            x.Endpoint.Trim(),
            x.Model.Trim(),
            string.IsNullOrWhiteSpace(x.ApiKey) && existing.TryGetValue(x.Id, out var saved)
                ? saved.ApiKey
                : string.IsNullOrWhiteSpace(x.ApiKey) ? null : x.ApiKey.Trim())).ToArray();

        var route = await routes.ReplaceAsync(command.FamilyId, targets, cancellationToken);
        return AssistantProviderRouteMapping.Map(route, canManage: true);
    }

    private static void Validate(IReadOnlyList<AssistantProviderTargetRequest> targets)
    {
        if (targets.Count is < 1 or > 3)
            throw new ArgumentException("A provider route must contain between one and three targets.");
        if (targets.Select(x => x.Id).Where(x => x != Guid.Empty).Distinct().Count()
            != targets.Count(x => x.Id != Guid.Empty))
            throw new ArgumentException("Provider target identifiers must be unique.");

        foreach (var target in targets)
        {
            if (string.IsNullOrWhiteSpace(target.Name) || target.Name.Trim().Length > 60)
                throw new ArgumentException("Each provider target needs a name of at most 60 characters.");
            if (target.Hosting is not ("Hosted" or "Local"))
                throw new ArgumentException("Provider hosting must be Hosted or Local.");
            if (target.Protocol is not ("Ollama" or "OpenAICompatible"))
                throw new ArgumentException("Provider protocol must be Ollama or OpenAICompatible.");
            if (string.IsNullOrWhiteSpace(target.Model) || target.Model.Trim().Length > 120)
                throw new ArgumentException("Each provider target needs a model identifier of at most 120 characters.");
            if (string.IsNullOrWhiteSpace(target.Endpoint) || target.Endpoint.Trim().Length > 1000
                || !Uri.TryCreate(target.Endpoint.Trim(), UriKind.Absolute, out var endpoint)
                || endpoint.Scheme is not ("http" or "https") || !string.IsNullOrEmpty(endpoint.UserInfo))
                throw new ArgumentException("Each provider target needs an absolute HTTP or HTTPS endpoint without embedded credentials.");
            if (target.Hosting == "Hosted" && endpoint.Scheme != Uri.UriSchemeHttps)
                throw new ArgumentException("Hosted provider endpoints must use HTTPS.");
            if (target.ApiKey?.Length > 4000)
                throw new ArgumentException("Provider credentials cannot exceed 4000 characters.");
        }
    }
}

internal static class AssistantProviderRouteAccess
{
    public static async Task<Domain.Family.Family> GetFamilyAsync(IDomusMindDbContext db,
        IFamilyAuthorizationService authorization, Guid familyId, Guid userId, CancellationToken ct)
    {
        if (!await authorization.CanAccessFamilyAsync(userId, familyId, ct))
            throw new UnauthorizedAccessException();
        return await db.Set<Domain.Family.Family>().AsNoTracking().Include(x => x.Members)
            .SingleOrDefaultAsync(x => x.Id == FamilyId.From(familyId), ct)
            ?? throw new KeyNotFoundException("Household was not found.");
    }
}

internal static class AssistantProviderRouteMapping
{
    public static AssistantProviderRouteResponse Map(AssistantProviderRoute route, bool canManage) =>
        new(route.FamilyId, canManage, route.IsDeploymentDefault, route.Targets.Select(x =>
            new AssistantProviderTargetResponse(x.Id, x.Name, x.Hosting, x.Protocol,
                x.Endpoint, x.Model, !string.IsNullOrEmpty(x.ApiKey))).ToArray());
}
