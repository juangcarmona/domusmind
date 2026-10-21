using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Features.Family;
using DomusMind.Application.Features.Family.GetFamily;
using DomusMind.Application.Features.Lists;
using DomusMind.Application.Features.Lists.GetFamilyLists;
using DomusMind.Application.Features.Lists.GetListDetail;
using DomusMind.Application.Features.Responsibilities;
using DomusMind.Application.Features.Responsibilities.GetHouseholdAreas;
using DomusMind.Application.Features.Tasks;
using DomusMind.Application.Features.Tasks.GetRoutinesByFamily;
using DomusMind.Application.Features.MealPlanning.GetMealPlan;
using DomusMind.Contracts.MealPlanning;
using DomusMind.Mcp.Authorization;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace DomusMind.Mcp.Tools;

[McpServerToolType]
public sealed class DomusMindHouseholdReadTools
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [McpServerTool(
        Name = "lists_get",
        ReadOnly = true,
        UseStructuredContent = true,
        OutputSchemaType = typeof(HouseholdListsOutput))]
    [Description("Read the authorized household's shared lists, or one shared list and its ordered items when listId is supplied.")]
    public static async Task<CallToolResult> ListsGet(
        IMcpRequestIdentityAccessor identityAccessor,
        IQueryDispatcher dispatcher,
        ILogger<DomusMindHouseholdReadTools> logger,
        CancellationToken cancellationToken,
        string? listId = null)
    {
        if (!identityAccessor.TryGet(out var identity))
            return Audited(logger, "lists_get", Error("ACCESS_DENIED", "Access to this household is denied."), identityAccessor);

        Guid? requestedListId = null;
        if (!string.IsNullOrWhiteSpace(listId) &&
            (!Guid.TryParse(listId, out var parsedListId) || parsedListId == Guid.Empty))
        {
            return Audited(logger, "lists_get", Error("INVALID_INPUT", "listId must be a valid UUID."), identityAccessor, identity);
        }
        if (!string.IsNullOrWhiteSpace(listId))
            requestedListId = Guid.Parse(listId);

        try
        {
            using var culture = new InvariantCultureScope();
            if (requestedListId.HasValue)
            {
                var detail = await dispatcher.Dispatch(new GetListDetailQuery(
                    requestedListId.Value,
                    identity.UserId,
                    identity.HouseholdId), cancellationToken);

                return Audited(logger, "lists_get", Success(new HouseholdListsOutput(
                    [],
                    new HouseholdListDetailOutput(
                        detail.ListId,
                        detail.Name,
                        detail.Kind,
                        detail.Color,
                        detail.AreaId,
                        detail.LinkedPlanId,
                        detail.LinkedPlanDisplayName,
                        detail.UncheckedCount,
                        detail.Items.Select(MapListItem).ToArray()))), identityAccessor, identity);
            }

            var response = await dispatcher.Dispatch(new GetFamilyListsQuery(
                identity.HouseholdId,
                identity.UserId), cancellationToken);
            var output = new HouseholdListsOutput(
                response.Lists.Select(list => new HouseholdListSummaryOutput(
                    list.Id,
                    list.Name,
                    list.Kind,
                    list.AreaId,
                    list.LinkedPlanId,
                    list.UncheckedCount)).ToArray(),
                null);
            return Audited(logger, "lists_get", Success(output), identityAccessor, identity);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            return Audited(logger, "lists_get", Error(
                IsAccessDenied(exception) ? "ACCESS_DENIED" : IsNotFound(exception) ? "NOT_FOUND" : "INTERNAL_ERROR",
                IsAccessDenied(exception) ? "Access to this household is denied." : IsNotFound(exception)
                    ? "The requested household list was not found."
                    : "Household lists could not be read."),
                identityAccessor, identity);
        }
    }

    [McpServerTool(
        Name = "meal_plan_get",
        ReadOnly = true,
        UseStructuredContent = true,
        OutputSchemaType = typeof(MealPlanGetOutput))]
    [Description("Read the authorized household meal plan for a configured meal week identified by an exact yyyy-MM-dd week start.")]
    public static async Task<CallToolResult> MealPlanGet(
        string weekStart,
        IMcpRequestIdentityAccessor identityAccessor,
        IQueryDispatcher dispatcher,
        ILogger<DomusMindHouseholdReadTools> logger,
        CancellationToken cancellationToken)
    {
        if (!DateOnly.TryParseExact(weekStart, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            return Audited(logger, "meal_plan_get", Error("INVALID_INPUT", "weekStart must be an exact yyyy-MM-dd date."), identityAccessor);
        if (!identityAccessor.TryGet(out var identity))
            return Audited(logger, "meal_plan_get", Error("ACCESS_DENIED", "Access to this household is denied."), identityAccessor);

        try
        {
            using var culture = new InvariantCultureScope();
            var family = await dispatcher.Dispatch(new GetFamilyQuery(identity.HouseholdId, identity.UserId), cancellationToken);
            if (!Enum.TryParse<System.DayOfWeek>(family.FirstDayOfWeek, true, out var firstDay))
                return Audited(logger, "meal_plan_get", Error("INVALID_INPUT", "The household has no valid first day of week configured."), identityAccessor, identity);
            if (date.DayOfWeek != firstDay)
                return Audited(logger, "meal_plan_get", Error("INVALID_INPUT", "weekStart must match the household's configured first day of week."), identityAccessor, identity);

            var response = await dispatcher.Dispatch(new GetMealPlanQuery(
                null,
                identity.HouseholdId,
                date,
                identity.UserId), cancellationToken);
            var detail = response.MealPlan;
            var output = detail is null
                ? new MealPlanGetOutput(null)
                : new MealPlanGetOutput(new MealPlanOutput(
                    detail.PlanId,
                    detail.WeekStart,
                    detail.WeekEnd,
                    detail.Status,
                    detail.Slots
                        .OrderBy(slot => DayOffset(ParseDay(slot.DayOfWeek), firstDay))
                        .ThenBy(slot => MealTypeOffset(slot.MealType))
                        .ThenBy(slot => slot.MealType, StringComparer.Ordinal)
                        .Select(MapMealSlot)
                        .ToArray()));
            return Audited(logger, "meal_plan_get", Success(output), identityAccessor, identity);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            return Audited(logger, "meal_plan_get", Error(
                IsAccessDenied(exception) ? "ACCESS_DENIED" : "INTERNAL_ERROR",
                IsAccessDenied(exception) ? "Access to this household is denied." : "The household meal plan could not be read."),
                identityAccessor, identity);
        }
    }

    [McpServerTool(
        Name = "routines_get",
        ReadOnly = true,
        UseStructuredContent = true,
        OutputSchemaType = typeof(RoutinesOutput))]
    [Description("Read the authorized household routines, including schedules, status, target members, and responsibility areas.")]
    public static async Task<CallToolResult> RoutinesGet(
        IMcpRequestIdentityAccessor identityAccessor,
        IQueryDispatcher dispatcher,
        ILogger<DomusMindHouseholdReadTools> logger,
        CancellationToken cancellationToken)
    {
        if (!identityAccessor.TryGet(out var identity))
            return Audited(logger, "routines_get", Error("ACCESS_DENIED", "Access to this household is denied."), identityAccessor);
        try
        {
            using var culture = new InvariantCultureScope();
            var response = await dispatcher.Dispatch(new GetRoutinesByFamilyQuery(identity.HouseholdId, identity.UserId), cancellationToken);
            var output = new RoutinesOutput(response.Routines.Select(r => new RoutineOutput(
                r.RoutineId, r.Name, r.Scope, r.Kind, r.Frequency,
                r.DaysOfWeek.Select(day => day.ToString()).ToArray(),
                r.DaysOfMonth.ToArray(), r.MonthOfYear,
                r.Time?.ToString("HH:mm", CultureInfo.InvariantCulture),
                r.EndTime?.ToString("HH:mm", CultureInfo.InvariantCulture),
                r.TargetMemberIds.ToArray(), r.Status, r.AreaId)).ToArray());
            return Audited(logger, "routines_get", Success(output), identityAccessor, identity);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            return Audited(logger, "routines_get", Error(
                IsAccessDenied(exception) ? "ACCESS_DENIED" : "INTERNAL_ERROR",
                IsAccessDenied(exception) ? "Access to this household is denied." : "Household routines could not be read."),
                identityAccessor, identity);
        }
    }

    [McpServerTool(
        Name = "areas_get",
        ReadOnly = true,
        UseStructuredContent = true,
        OutputSchemaType = typeof(AreasOutput))]
    [Description("Read the active responsibility areas and their household member owners.")]
    public static async Task<CallToolResult> AreasGet(
        IMcpRequestIdentityAccessor identityAccessor,
        IQueryDispatcher dispatcher,
        ILogger<DomusMindHouseholdReadTools> logger,
        CancellationToken cancellationToken)
    {
        if (!identityAccessor.TryGet(out var identity))
            return Audited(logger, "areas_get", Error("ACCESS_DENIED", "Access to this household is denied."), identityAccessor);
        try
        {
            using var culture = new InvariantCultureScope();
            var response = await dispatcher.Dispatch(new GetHouseholdAreasQuery(identity.HouseholdId, identity.UserId), cancellationToken);
            var output = new AreasOutput(response.Areas.Select(area => new AreaOutput(
                area.AreaId,
                area.Name,
                area.Color,
                area.PrimaryOwnerId,
                area.PrimaryOwnerName,
                area.SecondaryOwnerIds.ToArray())).ToArray());
            return Audited(logger, "areas_get", Success(output), identityAccessor, identity);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            return Audited(logger, "areas_get", Error(
                IsAccessDenied(exception) ? "ACCESS_DENIED" : "INTERNAL_ERROR",
                IsAccessDenied(exception) ? "Access to this household is denied." : "Household responsibility areas could not be read."),
                identityAccessor, identity);
        }
    }

    private static ListItemOutput MapListItem(DomusMind.Contracts.Lists.ListItemDetail item)
        => new(item.ItemId, item.Name, item.Checked, item.Quantity, item.Note, item.Order, item.Importance, item.DueDate, item.Reminder, item.TargetMemberId);

    private static MealSlotOutput MapMealSlot(MealSlotDetail slot)
        => new(slot.DayOfWeek, slot.MealType, slot.MealSourceType, slot.Recipe?.Name, slot.FreeText, slot.Notes, slot.IsOptional, slot.IsLocked);

    private static System.DayOfWeek ParseDay(string value)
        => Enum.TryParse<System.DayOfWeek>(value, true, out var day) ? day : throw new InvalidOperationException("Meal plan returned an invalid day.");

    private static int DayOffset(System.DayOfWeek day, System.DayOfWeek firstDay)
        => ((int)day - (int)firstDay + 7) % 7;

    private static int MealTypeOffset(string mealType)
        => mealType switch
        {
            "Breakfast" => 0,
            "MidMorningSnack" => 1,
            "Lunch" => 2,
            "AfternoonSnack" => 3,
            "Dinner" => 4,
            _ => int.MaxValue,
        };

    private static CallToolResult Audited(ILogger logger, string capability, CallToolResult result, IMcpRequestIdentityAccessor identityAccessor, McpRequestIdentity? identity = null)
    {
        identity ??= identityAccessor.TryGet(out var current) ? current : null;
        logger.LogInformation(
            "MCP capability audit {Outcome} capability={Capability} user={UserId} household={HouseholdId} client={ClientId} trace={TraceId}",
            result.IsError == true ? "tool_error" : "success", capability,
            identity?.UserId.ToString() ?? "unknown", identity?.HouseholdId.ToString() ?? "unknown",
            identity?.ClientId ?? "unknown", identity?.TraceId ?? "unknown");
        return result;
    }

    private static CallToolResult Success<T>(T output) => new()
    {
        StructuredContent = JsonSerializer.SerializeToElement(output, JsonOptions), Content = []
    };

    private static CallToolResult Error(string code, string message) => new()
    {
        IsError = true,
        StructuredContent = JsonSerializer.SerializeToElement(new McpErrorOutput(code, message), JsonOptions),
        Content = [new TextContentBlock { Text = message }]
    };

    private static bool IsAccessDenied(Exception exception)
        => exception is UnauthorizedAccessException
            || exception is FamilyException { Code: FamilyErrorCode.AccessDenied }
            || exception is ListException { Code: ListErrorCode.AccessDenied }
            || exception is TasksException { Code: TasksErrorCode.AccessDenied }
            || exception is ResponsibilitiesException { Code: ResponsibilitiesErrorCode.AccessDenied };

    private static bool IsNotFound(Exception exception)
        => exception is ListException { Code: ListErrorCode.ListNotFound };

    private sealed class InvariantCultureScope : IDisposable
    {
        private readonly CultureInfo _culture = CultureInfo.CurrentCulture;
        private readonly CultureInfo _uiCulture = CultureInfo.CurrentUICulture;
        public InvariantCultureScope() { CultureInfo.CurrentCulture = CultureInfo.InvariantCulture; CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture; }
        public void Dispose() { CultureInfo.CurrentCulture = _culture; CultureInfo.CurrentUICulture = _uiCulture; }
    }
}

public sealed record HouseholdListsOutput(
    IReadOnlyCollection<HouseholdListSummaryOutput> Lists,
    HouseholdListDetailOutput? List);

public sealed record HouseholdListSummaryOutput(
    Guid ListId,
    string Name,
    string Kind,
    Guid? AreaId,
    Guid? LinkedPlanId,
    int UncheckedCount);

public sealed record HouseholdListDetailOutput(
    Guid ListId,
    string Name,
    string Kind,
    string? Color,
    Guid? AreaId,
    Guid? LinkedPlanId,
    string? LinkedPlanDisplayName,
    int UncheckedCount,
    IReadOnlyCollection<ListItemOutput> Items);

public sealed record ListItemOutput(
    Guid ItemId,
    string Name,
    bool Checked,
    string? Quantity,
    string? Note,
    int Order,
    bool Importance,
    DateOnly? DueDate,
    DateTimeOffset? Reminder,
    Guid? TargetMemberId);

public sealed record MealPlanGetOutput(MealPlanOutput? MealPlan);

public sealed record MealPlanOutput(
    Guid PlanId,
    DateOnly WeekStart,
    DateOnly WeekEnd,
    string Status,
    IReadOnlyCollection<MealSlotOutput> Slots);

public sealed record MealSlotOutput(
    string DayOfWeek,
    string MealType,
    string MealSourceType,
    string? RecipeName,
    string? FreeText,
    string? Notes,
    bool IsOptional,
    bool IsLocked);

public sealed record RoutinesOutput(IReadOnlyCollection<RoutineOutput> Routines);

public sealed record RoutineOutput(
    Guid RoutineId,
    string Name,
    string Scope,
    string Kind,
    string Frequency,
    IReadOnlyCollection<string> DaysOfWeek,
    IReadOnlyCollection<int> DaysOfMonth,
    int? MonthOfYear,
    string? Time,
    string? EndTime,
    IReadOnlyCollection<Guid> TargetMemberIds,
    string Status,
    Guid? AreaId);

public sealed record AreasOutput(IReadOnlyCollection<AreaOutput> Areas);

public sealed record AreaOutput(
    Guid AreaId,
    string Name,
    string Color,
    Guid? PrimaryOwnerId,
    string? PrimaryOwnerName,
    IReadOnlyCollection<Guid> SecondaryOwnerIds);
