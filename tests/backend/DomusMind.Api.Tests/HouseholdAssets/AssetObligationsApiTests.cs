using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using DomusMind.Api.Controllers;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.DependencyInjection;
using DomusMind.Contracts.HouseholdAssets;
using DomusMind.Domain.HouseholdAssets;
using DomusMind.Infrastructure.Auth;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Messaging;
using DomusMind.Infrastructure.Persistence;
using DomusMind.Tests.HouseholdAssets;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;

namespace DomusMind.Api.Tests.HouseholdAssets;

public sealed class AssetObligationsApiTests(AssetPostgreSqlFixture fixture) : IClassFixture<AssetPostgreSqlFixture>
{
    private string Assets => $"/api/families/{fixture.FamilyId.Value}/assets";
    private string Root(Guid asset) => $"{Assets}/{asset}/obligations";
    private HttpClient AuthorizedClient(IHost host)
    {
        var client = host.GetTestClient(); client.DefaultRequestHeaders.Add("X-Test-User", fixture.UserId.ToString()); return client;
    }
    private async Task<AssetResponse> Asset(HttpClient client)
    {
        await using (var db = fixture.CreateContext())
        {
            var family = await db.Families.SingleAsync(x => x.Id == fixture.FamilyId);
            family.ConfigureInitialTimeZone("Europe/Madrid"); await db.SaveChangesAsync();
        }
        var response = await client.PostAsJsonAsync(Assets, new AssetDetailsRequest("Boiler", "Appliance"));
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<AssetResponse>())!;
    }
    private async Task<ObligationDefinedResponse> Define(HttpClient client, Guid asset, string rule = "MonthlyLastDay")
    {
        var response = await client.PostAsJsonAsync(Root(asset), new DefineObligationRequest(Guid.NewGuid(), "Service", new(2025, 1, 31), rule));
        Assert.Equal(HttpStatusCode.Created, response.StatusCode); return (await response.Content.ReadFromJsonAsync<ObligationDefinedResponse>())!;
    }
    private Task<HttpResponseMessage> Act(HttpClient client, ObligationDefinedResponse o, string action,
        DateOnly? date = null, Guid? operation = null, Guid? cycle = null)
        => client.PostAsJsonAsync($"{Root(o.AssetId)}/{o.Id}/operations", new ActOnObligationRequest(operation ?? Guid.NewGuid(), cycle ?? o.InitialCycleId, action, date));
    private async Task<AssetObligationResponse> Read(HttpClient client, ObligationDefinedResponse o)
        => Assert.Single((await client.GetFromJsonAsync<AssetObligationResponse[]>(Root(o.AssetId)))!);

    [AssetPostgreSqlFact]
    public async Task LifecycleApiPreservesHistoryRejectsSuspendedActionsAndReturnsOriginalRetries()
    {
        using var host = await CreateHost(); using var client = AuthorizedClient(host);
        var asset = await Asset(client); var o = await Define(client, asset.Id); var request = Guid.NewGuid();
        var initial = await Read(client, o); Assert.Equal("Active", initial.State); Assert.Equal("Overdue", initial.DueState);
        var suspend = await Act(client, o, "Suspend"); Assert.Equal(HttpStatusCode.OK, suspend.StatusCode);
        foreach (var action in new[] { "Complete", "Skip", "Reschedule", "Suspend" })
            Assert.Equal(HttpStatusCode.Conflict, (await Act(client, o, action, action is "Complete" or "Reschedule" ? new DateOnly(2025, 2, 1) : null)).StatusCode);
        var suspended = await Read(client, o); Assert.Null(suspended.DueState); Assert.Single(suspended.History);
        Assert.Equal(HttpStatusCode.OK, (await Act(client, o, "Reactivate")).StatusCode);
        var resumed = await Read(client, o); Assert.Equal(initial.Occurrences[0], resumed.Occurrences[0]); Assert.Equal("Overdue", resumed.DueState);
        Assert.Equal(HttpStatusCode.OK, (await Act(client, o, "Reschedule", new(2025, 2, 5))).StatusCode);
        var complete = await Act(client, o, "Complete", new(2025, 2, 7), request); Assert.Equal(HttpStatusCode.OK, complete.StatusCode);
        var accepted = (await complete.Content.ReadFromJsonAsync<ObligationOperationResponse>())!;
        Assert.Equal(new DateOnly(2025, 3, 31), accepted.NextDueDate);
        Assert.Equal(HttpStatusCode.OK, (await Act(client, o, "Skip", cycle: accepted.NextCycleId)).StatusCode);
        var retry = await Act(client, o, "Complete", new(2025, 2, 7), request);
        Assert.Equal(HttpStatusCode.OK, retry.StatusCode); Assert.Equal(accepted, await retry.Content.ReadFromJsonAsync<ObligationOperationResponse>());
        Assert.Equal(HttpStatusCode.Conflict, (await Act(client, o, "Skip", operation: request)).StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, (await Act(client, o, "Skip")).StatusCode);
        var saved = await Read(client, o); Assert.Equal(5, saved.History.Count); Assert.Equal(3, saved.Occurrences.Count);
        Assert.Single(saved.Occurrences, x => x.State == "Completed"); Assert.Single(saved.Occurrences, x => x.State == "Skipped");
    }

    [AssetPostgreSqlFact]
    public async Task ArchivedAssetRefusesDefinitionsAndMutationsButAllowsConsultationAndAcceptedRetries()
    {
        using var host = await CreateHost(); using var client = AuthorizedClient(host);
        var asset = await Asset(client); var o = await Define(client, asset.Id); var op = Guid.NewGuid();
        var response = await Act(client, o, "Suspend", operation: op); var accepted = await response.Content.ReadFromJsonAsync<ObligationOperationResponse>();
        Assert.Equal(HttpStatusCode.OK, (await client.DeleteAsync($"{Assets}/{asset.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, (await client.PostAsJsonAsync(Root(asset.Id), new DefineObligationRequest(Guid.NewGuid(), "Other", new(2025, 1, 31), "OneTime"))).StatusCode);
        foreach (var action in new[] { "Complete", "Skip", "Reschedule", "Reactivate", "Cancel" })
            Assert.Equal(HttpStatusCode.Conflict, (await Act(client, o, action, action is "Complete" or "Reschedule" ? new DateOnly(2025, 2, 1) : null)).StatusCode);
        var retry = await Act(client, o, "Suspend", operation: op); Assert.Equal(HttpStatusCode.OK, retry.StatusCode);
        Assert.Equal(accepted, await retry.Content.ReadFromJsonAsync<ObligationOperationResponse>());
        Assert.Single((await Read(client, o)).History);
        Assert.Equal(HttpStatusCode.OK, (await client.PostAsync($"{Assets}/{asset.Id}/restore", null)).StatusCode);
        var restored = await Read(client, o); Assert.Equal("Suspended", restored.State); Assert.Null(restored.DueState);
        Assert.Equal(o.InitialDueDate, Assert.Single(restored.Occurrences).DueDate);
    }

    [AssetPostgreSqlFact]
    public async Task OneTimeCompletionSkipAndCancellationAreDistinctTerminalOutcomes()
    {
        using var host = await CreateHost(); using var client = AuthorizedClient(host);
        foreach (var action in new[] { "Complete", "Skip", "Cancel" })
        {
            var asset = await Asset(client); var o = await Define(client, asset.Id, "OneTime");
            Assert.Equal(HttpStatusCode.OK, (await Act(client, o, action, action == "Complete" ? new DateOnly(2025, 2, 1) : null)).StatusCode);
            var saved = await Read(client, o); Assert.Equal(action == "Cancel" ? "Cancelled" : "Finished", saved.State);
            Assert.Equal(action switch { "Complete" => "Completed", "Skip" => "Skipped", _ => "Cancelled" }, Assert.Single(saved.Occurrences).State);
            if (action != "Complete") Assert.Null(saved.Occurrences[0].CompletedDate);
            foreach (var forbidden in new[] { "Complete", "Skip", "Reschedule", "Suspend", "Reactivate", "Cancel" })
                Assert.Equal(HttpStatusCode.Conflict, (await Act(client, o, forbidden, forbidden is "Complete" or "Reschedule" ? new DateOnly(2025, 2, 1) : null)).StatusCode);
            Assert.Single((await Read(client, o)).History);
        }
    }

    [AssetPostgreSqlFact]
    public async Task AuthorizationAndCrossHouseholdLookupsNeverDiscloseOrMutateObligations()
    {
        using var host = await CreateHost(); using var client = AuthorizedClient(host);
        var asset = await Asset(client); var o = await Define(client, asset.Id);
        client.DefaultRequestHeaders.Remove("X-Test-User");
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync(Root(asset.Id))).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await Act(client, o, "Skip")).StatusCode);
        client.DefaultRequestHeaders.Add("X-Test-User", Guid.NewGuid().ToString());
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync(Root(asset.Id))).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await Act(client, o, "Skip")).StatusCode);
        client.DefaultRequestHeaders.Remove("X-Test-User"); client.DefaultRequestHeaders.Add("X-Test-User", fixture.UserId.ToString());
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync($"/api/families/{fixture.ForeignFamilyId.Value}/assets/{asset.Id}/obligations")).StatusCode);
        var other = await Asset(client);
        Assert.Equal(HttpStatusCode.NotFound, (await client.PostAsJsonAsync($"{Root(other.Id)}/{o.Id}/operations", new ActOnObligationRequest(Guid.NewGuid(), o.InitialCycleId, "Skip"))).StatusCode);
        Assert.Empty((await Read(client, o)).History);
    }

    [AssetPostgreSqlFact]
    public async Task InvalidInputAndConflictingDefinitionRetriesAreRejectedWithoutMutation()
    {
        using var host = await CreateHost(); using var client = AuthorizedClient(host); var asset = await Asset(client);
        foreach (var request in new[] {
            new DefineObligationRequest(Guid.Empty, "Service", new(2025, 1, 31), "OneTime"),
            new DefineObligationRequest(Guid.NewGuid(), " ", new(2025, 1, 31), "OneTime"),
            new DefineObligationRequest(Guid.NewGuid(), "Service", default, "OneTime"),
            new DefineObligationRequest(Guid.NewGuid(), "Service", new(2025, 1, 31), "Weekly"),
            new DefineObligationRequest(Guid.NewGuid(), "Service", new(2025, 1, 31), "CompletionRelative", 0) })
            Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync(Root(asset.Id), request)).StatusCode);
        Assert.Empty((await client.GetFromJsonAsync<AssetObligationResponse[]>(Root(asset.Id)))!);
        var o = await Define(client, asset.Id);
        Assert.Equal(HttpStatusCode.Conflict, (await client.PostAsJsonAsync(Root(asset.Id), new DefineObligationRequest(o.OperationId, "Different", o.InitialDueDate, "MonthlyLastDay"))).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await Act(client, o, "Complete")).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await Act(client, o, "Skip", new(2025, 1, 31))).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await Act(client, o, "999")).StatusCode);
        Assert.Empty((await Read(client, o)).History);
    }

    [AssetPostgreSqlFact]
    public async Task HouseholdTimezoneRequiresManagerConfigurationAndHasNoImplicitServerDefault()
    {
        using var host = await CreateHost(); using var client = AuthorizedClient(host);
        var familyId = DomusMind.Domain.Family.FamilyId.New(); var otherUser = Guid.NewGuid();
        await using (var db = fixture.CreateContext())
        {
            var family = DomusMind.Domain.Family.Family.Create(familyId,
                DomusMind.Domain.Family.ValueObjects.FamilyName.Create("Timezone household"), "en", DateTime.UtcNow);
            family.AddMember(DomusMind.Domain.Family.MemberId.New(), DomusMind.Domain.Family.ValueObjects.MemberName.Create("Manager"),
                DomusMind.Domain.Family.ValueObjects.MemberRole.Adult, true, null, DateTime.UtcNow, fixture.UserId);
            db.Families.Add(family);
            db.AuthUsers.Add(new AuthUser { UserId = otherUser, Email = $"{otherUser:N}@test.invalid", PasswordHash = "Test-only", CreatedAtUtc = DateTime.UtcNow });
            db.UserFamilyAccesses.AddRange(new UserFamilyAccess { UserId = fixture.UserId, FamilyId = familyId.Value, GrantedAtUtc = DateTime.UtcNow },
                new UserFamilyAccess { UserId = otherUser, FamilyId = familyId.Value, GrantedAtUtc = DateTime.UtcNow });
            await db.SaveChangesAsync();
        }
        var assets = $"/api/families/{familyId.Value}/assets";
        var asset = (await (await client.PostAsJsonAsync(assets, new AssetDetailsRequest("Tool", "Other"))).Content.ReadFromJsonAsync<AssetResponse>())!;
        var definition = new DefineObligationRequest(Guid.NewGuid(), "Service", new(2025, 1, 31), "OneTime");
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync($"{assets}/{asset.Id}/obligations", definition)).StatusCode);
        var settings = $"/api/families/{familyId.Value}/settings";
        var request = new DomusMind.Contracts.Family.UpdateFamilySettingsRequest("Timezone household", null, null, null, "Pacific/Auckland");
        client.DefaultRequestHeaders.Remove("X-Test-User"); client.DefaultRequestHeaders.Add("X-Test-User", otherUser.ToString());
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PutAsJsonAsync(settings, request)).StatusCode);
        client.DefaultRequestHeaders.Remove("X-Test-User"); client.DefaultRequestHeaders.Add("X-Test-User", fixture.UserId.ToString());
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PutAsJsonAsync(settings, request with { TimeZoneId = "Invalid/Zone" })).StatusCode);
        var configured = await client.PutAsJsonAsync(settings, request); Assert.Equal(HttpStatusCode.OK, configured.StatusCode);
        Assert.Equal("Pacific/Auckland", (await configured.Content.ReadFromJsonAsync<DomusMind.Contracts.Family.UpdateFamilySettingsResponse>())!.TimeZoneId);
        Assert.Equal(HttpStatusCode.OK, (await client.PutAsJsonAsync(settings, request)).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PutAsJsonAsync(settings, request with { TimeZoneId = "UTC" })).StatusCode);
        Assert.Equal(HttpStatusCode.Created, (await client.PostAsJsonAsync($"{assets}/{asset.Id}/obligations", definition)).StatusCode);
    }

    private Task<IHost> CreateHost() => new HostBuilder().ConfigureWebHost(builder => builder.UseTestServer().ConfigureServices(services =>
    {
        services.AddLogging();
        services.AddHttpContextAccessor();
        services.AddSingleton(fixture.Options);
        services.AddScoped<DomusMindDbContext>();
        services.AddScoped<IDomusMindDbContext>(sp => sp.GetRequiredService<DomusMindDbContext>());
        services.AddApplication();
        services.AddScoped<DomusMind.Application.Abstractions.Languages.ISupportedLanguageReader, DomusMind.Infrastructure.Languages.SupportedLanguageReader>();
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        services.AddScoped<IEventLogWriter, EventLogWriter>();
        services.AddScoped<IAssetMutationLock, AssetMutationLock>();
        services.AddScoped<IFamilyAuthorizationService, FamilyAuthorizationService>();
        services.AddScoped<ICurrentUser, CurrentUserAccessor>();
        services.AddAuthentication("AssetTest").AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("AssetTest", _ => { });
        services.AddAuthorization();
        services.AddControllers().AddApplicationPart(typeof(HouseholdAssetsController).Assembly);
    }).Configure(app =>
    {
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    })).StartAsync();

    private sealed class TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
        : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Guid.TryParse(Request.Headers["X-Test-User"], out var user))
                return Task.FromResult(AuthenticateResult.NoResult());
            var identity = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, user.ToString())], Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name)));
        }
    }
}
