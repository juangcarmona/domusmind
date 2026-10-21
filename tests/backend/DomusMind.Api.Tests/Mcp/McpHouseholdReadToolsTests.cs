using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Features.Family.GetFamily;
using DomusMind.Application.Features.Lists.GetFamilyLists;
using DomusMind.Application.Features.Lists.GetListDetail;
using DomusMind.Application.Features.MealPlanning.GetMealPlan;
using DomusMind.Application.Features.Responsibilities.GetHouseholdAreas;
using DomusMind.Application.Features.Tasks.GetRoutinesByFamily;
using DomusMind.Contracts.Family;
using DomusMind.Contracts.Lists;
using DomusMind.Contracts.MealPlanning;
using DomusMind.Contracts.Responsibilities;
using DomusMind.Contracts.Tasks;
using DomusMind.Mcp.Authorization;
using DomusMind.Mcp.Tools;
using Microsoft.Extensions.Logging.Abstractions;

namespace DomusMind.Api.Tests.Mcp;

public sealed class McpHouseholdReadToolsTests
{
    private static readonly Guid HouseholdId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid UserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

    [Fact]
    public async Task Lists_get_uses_the_token_household_for_summaries_and_detail_lookup()
    {
        var dispatcher = new HouseholdReadDispatcher();

        var summaries = await DomusMindHouseholdReadTools.ListsGet(
            null, new StubIdentityAccessor(), dispatcher,
            NullLogger<DomusMindHouseholdReadTools>.Instance, CancellationToken.None);

        Assert.NotEqual(true, summaries.IsError);
        Assert.Equal("Groceries", summaries.StructuredContent!.Value.GetProperty("lists")[0].GetProperty("name").GetString());

        var detail = await DomusMindHouseholdReadTools.ListsGet(
            dispatcher.ListId.ToString(), new StubIdentityAccessor(), dispatcher,
            NullLogger<DomusMindHouseholdReadTools>.Instance, CancellationToken.None);

        Assert.NotEqual(true, detail.IsError);
        Assert.Equal(HouseholdId, dispatcher.ExpectedDetailFamilyId);
        Assert.Equal("Milk", detail.StructuredContent!.Value.GetProperty("list").GetProperty("items")[0].GetProperty("name").GetString());
    }

    [Fact]
    public async Task Meal_plan_get_rejects_wrong_week_boundary_and_orders_slots_from_household_first_day()
    {
        var dispatcher = new HouseholdReadDispatcher
        {
            Family = new FamilyResponse(HouseholdId, "Home", "en", DateTime.UtcNow, 2, "sunday", "yyyy-MM-dd")
        };

        var invalid = await DomusMindHouseholdReadTools.MealPlanGet(
            "2026-10-19", new StubIdentityAccessor(), dispatcher,
            NullLogger<DomusMindHouseholdReadTools>.Instance, CancellationToken.None);
        Assert.True(invalid.IsError);
        Assert.Equal("INVALID_INPUT", invalid.StructuredContent!.Value.GetProperty("code").GetString());

        var valid = await DomusMindHouseholdReadTools.MealPlanGet(
            "2026-10-18", new StubIdentityAccessor(), dispatcher,
            NullLogger<DomusMindHouseholdReadTools>.Instance, CancellationToken.None);
        Assert.NotEqual(true, valid.IsError);
        var slots = valid.StructuredContent!.Value.GetProperty("mealPlan").GetProperty("slots");
        Assert.Equal("Sunday", slots[0].GetProperty("dayOfWeek").GetString());
        Assert.Equal("Monday", slots[1].GetProperty("dayOfWeek").GetString());
    }

    [Fact]
    public async Task Routines_and_areas_get_return_domain_owned_read_models()
    {
        var dispatcher = new HouseholdReadDispatcher();
        var routines = await DomusMindHouseholdReadTools.RoutinesGet(
            new StubIdentityAccessor(), dispatcher,
            NullLogger<DomusMindHouseholdReadTools>.Instance, CancellationToken.None);
        var areas = await DomusMindHouseholdReadTools.AreasGet(
            new StubIdentityAccessor(), dispatcher,
            NullLogger<DomusMindHouseholdReadTools>.Instance, CancellationToken.None);

        Assert.NotEqual(true, routines.IsError);
        Assert.Equal("Morning", routines.StructuredContent!.Value.GetProperty("routines")[0].GetProperty("name").GetString());
        Assert.NotEqual(true, areas.IsError);
        Assert.Equal("Food", areas.StructuredContent!.Value.GetProperty("areas")[0].GetProperty("name").GetString());
    }

    private sealed class StubIdentityAccessor : IMcpRequestIdentityAccessor
    {
        public bool TryGet(out McpRequestIdentity identity)
        {
            identity = new McpRequestIdentity(UserId, HouseholdId, "test-client", "test-trace");
            return true;
        }
    }

    private sealed class HouseholdReadDispatcher : IQueryDispatcher
    {
        public Guid ListId { get; } = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
        public Guid? ExpectedDetailFamilyId { get; private set; }
        public FamilyResponse Family { get; set; } = new(HouseholdId, "Home", "en", DateTime.UtcNow, 2, "monday", "yyyy-MM-dd");

        public Task<TResponse> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            object response = query switch
            {
                GetFamilyListsQuery => new GetFamilyListsResponse([
                    new ListSummary(ListId, "Groceries", "Shopping", null, null, 1)]),
                GetListDetailQuery detail => Detail(detail),
                GetFamilyQuery => Family,
                GetMealPlanQuery => new GetMealPlanResponse(new MealPlanDetail(
                    Guid.NewGuid(), HouseholdId, new DateOnly(2026, 10, 18), new DateOnly(2026, 10, 24),
                    "Draft", null, null, 0, null, [
                        new MealSlotDetail("Monday", "Dinner", "FreeText", null, "Pasta", null, false, false),
                        new MealSlotDetail("Sunday", "Dinner", "FreeText", null, "Soup", null, false, false)])),
                GetRoutinesByFamilyQuery => new RoutineListResponse([
                    new RoutineListItem(Guid.NewGuid(), HouseholdId, "Morning", "Household", "Maintenance", "blue", "Daily", [], [], null, null, null, [], "Active", null, DateTime.UtcNow)]),
                GetHouseholdAreasQuery => new HouseholdAreasResponse([
                    new HouseholdAreaItem(Guid.NewGuid(), "Food", "green", UserId, "Alex", [], false)]),
                _ => throw new InvalidOperationException($"Unsupported query: {query.GetType().Name}")
            };
            return Task.FromResult((TResponse)response);
        }

        private GetListDetailResponse Detail(GetListDetailQuery query)
        {
            ExpectedDetailFamilyId = query.ExpectedFamilyId;
            return new GetListDetailResponse(
                ListId, "Groceries", "Shopping", "green", null, null, null, 1,
                [new ListItemDetail(Guid.NewGuid(), "Milk", false, null, null, 0, false, null, null, null)]);
        }
    }
}
