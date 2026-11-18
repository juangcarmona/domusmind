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

public sealed class HouseholdAssetsApiTests(AssetPostgreSqlFixture fixture) : IClassFixture<AssetPostgreSqlFixture>
{
    private string Assets => $"/api/families/{fixture.FamilyId.Value}/assets";
    private string Locations => $"/api/families/{fixture.FamilyId.Value}/asset-locations";

    [AssetPostgreSqlFact]
    public async Task LifecycleUsesRealAuthorizationAndPreservesIdentityDetailsReferencesAndHistory()
    {
        using var server = await CreateHost();
        using var client = AuthorizedClient(server);
        var categories = await client.GetFromJsonAsync<string[]>($"{Assets}/categories");
        Assert.Contains("Other", categories!);
        var location = await CreateLocation(client, "Home");
        Assert.Null(location.Address);
        var parent = await Register(client, new("Property", "Property", LocationId: location.Id));
        var details = new AssetDetailsRequest("Boiler", "Appliance", "Acme", "One", "123", "Managed",
            parent.Id, location.Id, fixture.MemberId.Value, fixture.AreaId.Value);
        var asset = await Register(client, details);
        Assert.Equal("Active", asset.Status);
        Assert.NotEqual(fixture.UserId, asset.ResponsibleMemberId);
        Assert.NotEqual(location.Id, asset.Id);
        Dictionary<Guid, string> history;
        await using (var db = fixture.CreateContext())
            history = await db.EventLog.Where(x => x.AggregateId == asset.Id.ToString("D"))
                .ToDictionaryAsync(x => x.EventId, x => x.PayloadJson);
        Assert.NotEmpty(history);

        var removed = await client.DeleteAsync($"{Assets}/{asset.Id}");
        Assert.Equal(HttpStatusCode.OK, removed.StatusCode);
        var archived = await removed.Content.ReadFromJsonAsync<AssetResponse>();
        Assert.Equal(asset with { Status = "Archived" }, archived);
        Assert.Equal(archived, await client.GetFromJsonAsync<AssetResponse>($"{Assets}/{asset.Id}"));
        var archivedList = await client.GetFromJsonAsync<AssetResponse[]>($"{Assets}?archived=true");
        Assert.Contains(archivedList!, x => x.Id == asset.Id);
        Assert.Equal(HttpStatusCode.Conflict, (await client.PutAsJsonAsync($"{Assets}/{asset.Id}", details with { Name = "Changed" })).StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, (await client.PostAsync($"{Assets}/{asset.Id}/archive", null)).StatusCode);
        Assert.Equal("Active", (await client.GetFromJsonAsync<AssetResponse>($"{Assets}/{parent.Id}"))!.Status);

        client.DefaultRequestHeaders.Remove("X-Test-User");
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync($"{Assets}/{asset.Id}")).StatusCode);
        client.DefaultRequestHeaders.Add("X-Test-User", Guid.NewGuid().ToString());
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PostAsync($"{Assets}/{asset.Id}/restore", null)).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.DeleteAsync($"{Assets}/{asset.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync($"{Assets}/{asset.Id}")).StatusCode);
        client.DefaultRequestHeaders.Remove("X-Test-User");
        client.DefaultRequestHeaders.Add("X-Test-User", fixture.UserId.ToString());
        var restored = await client.PostAsync($"{Assets}/{asset.Id}/restore", null);
        Assert.Equal(HttpStatusCode.OK, restored.StatusCode);
        Assert.Equal(asset, await restored.Content.ReadFromJsonAsync<AssetResponse>());
        Assert.Equal(HttpStatusCode.Conflict, (await client.PostAsync($"{Assets}/{asset.Id}/restore", null)).StatusCode);
        await using (var db = fixture.CreateContext())
            foreach (var entry in history)
                Assert.Equal(entry.Value, (await db.EventLog.SingleAsync(x => x.EventId == entry.Key)).PayloadJson);
    }

    [AssetPostgreSqlFact]
    public async Task CrossHouseholdReferencesAndResourceLookupsAreRejectedWithoutMutation()
    {
        using var server = await CreateHost();
        using var client = AuthorizedClient(server);
        var foreignAsset = AssetId.New();
        var foreignLocation = AssetLocationId.New();
        await using (var db = fixture.CreateContext())
        {
            db.Set<Asset>().Add(Asset.Create(foreignAsset, fixture.ForeignFamilyId, "Private", "Other"));
            db.Set<AssetLocation>().Add(AssetLocation.Create(foreignLocation, fixture.ForeignFamilyId, "Private location"));
            await db.SaveChangesAsync();
        }
        var original = await Register(client, new("Tool", "Other"));
        var invalid = new[]
        {
            new AssetDetailsRequest("Changed", "Other", ParentAssetId: foreignAsset.Value),
            new AssetDetailsRequest("Changed", "Other", LocationId: foreignLocation.Value),
            new AssetDetailsRequest("Changed", "Other", ResponsibleMemberId: fixture.ForeignMemberId.Value),
            new AssetDetailsRequest("Changed", "Other", ResponsibleAreaId: fixture.ForeignAreaId.Value),
            new AssetDetailsRequest("Changed", "Other", ResponsibleMemberId: fixture.UserId),
            new AssetDetailsRequest("Changed", "Other", LocationId: Guid.NewGuid())
        };
        foreach (var details in invalid)
        {
            Assert.Equal(HttpStatusCode.BadRequest, (await client.PutAsJsonAsync($"{Assets}/{original.Id}", details)).StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync(Assets, details)).StatusCode);
            Assert.Equal(original, await client.GetFromJsonAsync<AssetResponse>($"{Assets}/{original.Id}"));
        }
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync($"{Assets}/{foreignAsset.Value}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.DeleteAsync($"{Assets}/{foreignAsset.Value}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.PostAsync($"{Assets}/{foreignAsset.Value}/restore", null)).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync($"/api/families/{fixture.ForeignFamilyId.Value}/assets/{foreignAsset.Value}")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PostAsJsonAsync($"/api/families/{fixture.ForeignFamilyId.Value}/assets", new AssetDetailsRequest("Other", "Other"))).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PostAsJsonAsync($"/api/families/{fixture.ForeignFamilyId.Value}/asset-locations", new CreateAssetLocationRequest("Other"))).StatusCode);
    }

    [AssetPostgreSqlFact]
    public async Task SharedLocationDoesNotImplyContainmentAndCyclesAreRejected()
    {
        using var server = await CreateHost();
        using var client = AuthorizedClient(server);
        var location = await CreateLocation(client, "Workshop", "Example street");
        var a = await Register(client, new("A", "Equipment", LocationId: location.Id));
        var b = await Register(client, new("B", "Equipment", LocationId: location.Id));
        Assert.Null(a.ParentAssetId);
        Assert.Null(b.ParentAssetId);
        Assert.Null(a.ResponsibleAreaId);
        Assert.Null(b.ResponsibleMemberId);
        Assert.Equal(HttpStatusCode.OK, (await client.PutAsJsonAsync($"{Assets}/{b.Id}", new AssetDetailsRequest("B", "Equipment", ParentAssetId: a.Id, LocationId: location.Id))).StatusCode);
        var c = await Register(client, new("C", "Other", ParentAssetId: b.Id));
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PutAsJsonAsync($"{Assets}/{a.Id}", new AssetDetailsRequest("Changed", "Other", ParentAssetId: c.Id))).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PutAsJsonAsync($"{Assets}/{a.Id}", new AssetDetailsRequest("Changed", "Other", ParentAssetId: a.Id))).StatusCode);
        Assert.Equal(a, await client.GetFromJsonAsync<AssetResponse>($"{Assets}/{a.Id}"));
        Assert.Equal(HttpStatusCode.OK, (await client.DeleteAsync($"{Assets}/{a.Id}")).StatusCode);
        Assert.Equal("Active", (await client.GetFromJsonAsync<AssetResponse>($"{Assets}/{b.Id}"))!.Status);
        // Removing optional relationships is an explicit ordinary edit of an active asset.
        var detached = await client.PutAsJsonAsync($"{Assets}/{b.Id}", new AssetDetailsRequest("B", "Equipment", ManagementBasis: "Borrowed"));
        Assert.Equal(HttpStatusCode.OK, detached.StatusCode);
        var body = await detached.Content.ReadFromJsonAsync<AssetResponse>();
        Assert.Null(body!.ParentAssetId);
        Assert.Null(body.LocationId);
        Assert.Equal("Borrowed", body.ManagementBasis);
        var locations = await client.GetFromJsonAsync<AssetLocationResponse[]>(Locations);
        Assert.Contains(location, locations!);
    }

    [AssetPostgreSqlFact]
    public async Task RequiredDetailsAndCategoryShortlistAreValidated()
    {
        using var server = await CreateHost();
        using var client = AuthorizedClient(server);
        foreach (var invalid in new[] { new AssetDetailsRequest(" ", "Other"), new AssetDetailsRequest("Tool", "Unknown") })
            Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync(Assets, invalid)).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync(Locations, new CreateAssetLocationRequest(" "))).StatusCode);
        var asset = await Register(client, new("Simple asset without records", "Other"));
        Assert.Equal(HttpStatusCode.OK, (await client.DeleteAsync($"{Assets}/{asset.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await client.PostAsync($"{Assets}/{asset.Id}/restore", null)).StatusCode);
    }

    private HttpClient AuthorizedClient(IHost server)
    {
        var client = server.GetTestClient();
        client.DefaultRequestHeaders.Add("X-Test-User", fixture.UserId.ToString());
        return client;
    }

    private async Task<AssetResponse> Register(HttpClient client, AssetDetailsRequest details)
    {
        var response = await client.PostAsJsonAsync(Assets, details);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        return (await response.Content.ReadFromJsonAsync<AssetResponse>())!;
    }

    private async Task<AssetLocationResponse> CreateLocation(HttpClient client, string name, string? address = null)
    {
        var response = await client.PostAsJsonAsync(Locations, new CreateAssetLocationRequest(name, address));
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<AssetLocationResponse>())!;
    }

    private Task<IHost> CreateHost() => new HostBuilder().ConfigureWebHost(builder => builder.UseTestServer().ConfigureServices(services =>
    {
        services.AddLogging();
        services.AddHttpContextAccessor();
        services.AddSingleton(fixture.Options);
        services.AddScoped<DomusMindDbContext>();
        services.AddScoped<IDomusMindDbContext>(sp => sp.GetRequiredService<DomusMindDbContext>());
        services.AddApplication();
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
