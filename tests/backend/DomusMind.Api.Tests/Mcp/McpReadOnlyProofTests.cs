using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using DomusMind.Api.Controllers;
using DomusMind.Api.Mcp;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Calendar.GetFamilyTimeline;
using DomusMind.Application.Features.Family.GetFamily;
using DomusMind.Application.Features.Family.GetFamilyMembers;
using DomusMind.Application.Features.Lists.GetFamilyLists;
using DomusMind.Application.Features.Lists.GetListDetail;
using DomusMind.Application.Features.MealPlanning.GetMealPlan;
using DomusMind.Application.Features.Responsibilities.GetHouseholdAreas;
using DomusMind.Application.Features.Tasks.GetRoutinesByFamily;
using DomusMind.Contracts.Calendar;
using DomusMind.Contracts.Family;
using DomusMind.Contracts.Lists;
using DomusMind.Contracts.MealPlanning;
using DomusMind.Domain.Calendar;
using DomusMind.Domain.Calendar.ValueObjects;
using DomusMind.Domain.Family;
using DomusMind.Domain.Family.ValueObjects;
using DomusMind.Domain.Lists;
using DomusMind.Domain.Lists.ValueObjects;
using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.ValueObjects;
using DomusMind.Domain.Responsibilities;
using DomusMind.Domain.Responsibilities.ValueObjects;
using DomusMind.Domain.Shared;
using DomusMind.Domain.Tasks;
using DomusMind.Domain.Tasks.Enums;
using DomusMind.Domain.Tasks.ValueObjects;
using DomusMind.Infrastructure.Persistence;
using DomusMind.Mcp.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace DomusMind.Api.Tests.Mcp;

public sealed class McpReadOnlyProofTests
{
    private static readonly DateOnly From = new(2026, 10, 20);
    private static readonly DateOnly To = new(2026, 10, 31);

    [Fact]
    public async Task Production_tools_dispatch_authorized_reads_through_application_queries()
    {
        await using var host = await ReadOnlyProofHost.StartAsync(registerProductionTools: true);
        await using var client = await host.CreateClientAsync();

        var tools = await client.ListToolsAsync();
        Assert.Equal(["agenda_get", "areas_get", "household_members_list", "lists_get", "meal_plan_get", "routines_get"],
            tools.Select(tool => tool.Name).OrderBy(name => name));
        var agendaTool = Assert.Single(tools, tool => tool.Name == "agenda_get");
        AssertSchemaProperties(agendaTool.ProtocolTool.InputSchema, "from", "to");
        Assert.DoesNotContain("familyId", agendaTool.ProtocolTool.InputSchema.GetRawText(), StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("userId", agendaTool.ProtocolTool.InputSchema.GetRawText(), StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("memberId", agendaTool.ProtocolTool.InputSchema.GetRawText(), StringComparison.OrdinalIgnoreCase);

        var agendaResult = await client.CallToolAsync("agenda_get", new Dictionary<string, object?>
        {
            ["from"] = From.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ["to"] = To.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        });
        Assert.NotEqual(true, agendaResult.IsError);
        Assert.Contains(agendaResult.StructuredContent!.Value.GetProperty("events").EnumerateArray(),
            item => item.GetProperty("title").GetString() == "Family A appointment");
        Assert.Equal("2026-10-20", agendaResult.StructuredContent.Value.GetProperty("events")[0].GetProperty("date").GetString());
        Assert.DoesNotContain("Family B secret", agendaResult.StructuredContent.Value.GetRawText(), StringComparison.Ordinal);

        var membersResult = await client.CallToolAsync("household_members_list", new Dictionary<string, object?>());
        Assert.NotEqual(true, membersResult.IsError);
        Assert.Contains(membersResult.StructuredContent!.Value.GetProperty("members").EnumerateArray(),
            member => member.GetProperty("name").GetString() == "Preferred Alex");

        var invalidResult = await client.CallToolAsync("agenda_get", new Dictionary<string, object?>
        {
            ["from"] = "20/10/2026",
            ["to"] = "2026-10-31",
        });
        Assert.True(invalidResult.IsError);
        Assert.Equal("INVALID_INPUT", invalidResult.StructuredContent!.Value.GetProperty("code").GetString());

        host.Authorization.ThrowUnexpectedFailure = true;
        var failureResult = await client.CallToolAsync("agenda_get", new Dictionary<string, object?>
        {
            ["from"] = From.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ["to"] = To.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        });
        Assert.True(failureResult.IsError);
        Assert.Equal("INTERNAL_ERROR", failureResult.StructuredContent!.Value.GetProperty("code").GetString());
        Assert.DoesNotContain("test-only-secret", failureResult.StructuredContent.Value.GetRawText(), StringComparison.Ordinal);
        Assert.Contains(host.AuditMessages, message =>
            message.Contains("MCP capability audit", StringComparison.Ordinal) &&
            message.Contains("capability=agenda_get", StringComparison.Ordinal) &&
            message.Contains("tool_error", StringComparison.Ordinal));
        Assert.DoesNotContain(host.AuditMessages, message =>
            message.Contains("2026-10-20", StringComparison.Ordinal) ||
            message.Contains("Family A appointment", StringComparison.Ordinal));

        host.Authorization.ThrowUnexpectedFailure = false;
        host.Authorization.BlockForCancellation = true;
        using var cancellation = new CancellationTokenSource();
        var pending = client.CallToolAsync("agenda_get", new Dictionary<string, object?>
        {
            ["from"] = From.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ["to"] = To.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        }, cancellationToken: cancellation.Token);
        await host.Authorization.WaitUntilStartedAsync();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await pending);
        await host.Authorization.WaitUntilCancellationObservedAsync();

        Assert.Equal(0, host.Database.ChangeTracker.Entries().Count(entry =>
            entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted));
    }

    [Fact]
    public async Task Expanded_tools_use_the_official_client_and_real_application_handlers()
    {
        await using var host = await ReadOnlyProofHost.StartAsync(registerProductionTools: true);
        await using var client = await host.CreateClientAsync();

        var tools = await client.ListToolsAsync();
        foreach (var name in new[] { "lists_get", "meal_plan_get", "routines_get", "areas_get" })
        {
            var tool = Assert.Single(tools, candidate => candidate.Name == name);
            Assert.Equal(true, tool.ProtocolTool.Annotations?.ReadOnlyHint);
            Assert.NotNull(tool.ProtocolTool.OutputSchema);
        }

        var lists = await client.CallToolAsync("lists_get", new Dictionary<string, object?>());
        Assert.NotEqual(true, lists.IsError);
        var listId = lists.StructuredContent!.Value.GetProperty("lists")[0].GetProperty("listId").GetGuid();

        var detail = await client.CallToolAsync("lists_get", new Dictionary<string, object?>
        {
            ["listId"] = listId.ToString(),
        });
        Assert.NotEqual(true, detail.IsError);
        Assert.Equal("Synthetic item", detail.StructuredContent!.Value.GetProperty("list").GetProperty("items")[0].GetProperty("name").GetString());

        var malformedList = await client.CallToolAsync("lists_get", new Dictionary<string, object?>
        {
            ["listId"] = "not-a-uuid",
        });
        Assert.True(malformedList.IsError);
        Assert.Equal("INVALID_INPUT", malformedList.StructuredContent!.Value.GetProperty("code").GetString());

        var familyBList = await client.CallToolAsync("lists_get", new Dictionary<string, object?>
        {
            ["listId"] = host.FamilyBListId.ToString(),
        });
        Assert.True(familyBList.IsError);
        Assert.Equal("ACCESS_DENIED", familyBList.StructuredContent!.Value.GetProperty("code").GetString());
        Assert.DoesNotContain("Family B", familyBList.StructuredContent.Value.GetRawText(), StringComparison.Ordinal);

        host.Authorization.AdditionalAllowedFamilyIds.Add(host.FamilyBId);
        var userCanReadFamilyB = await host.Dispatcher.Dispatch<GetListDetailResponse>(
            new GetListDetailQuery(host.FamilyBListId, host.RequesterUserId, host.FamilyBId), CancellationToken.None);
        Assert.Equal("Family B private list", userCanReadFamilyB.Name);
        var tokenStillCannotReadFamilyB = await client.CallToolAsync("lists_get", new Dictionary<string, object?>
        {
            ["listId"] = host.FamilyBListId.ToString(),
        });
        Assert.True(tokenStillCannotReadFamilyB.IsError);
        Assert.Equal("ACCESS_DENIED", tokenStillCannotReadFamilyB.StructuredContent!.Value.GetProperty("code").GetString());

        var meal = await client.CallToolAsync("meal_plan_get", new Dictionary<string, object?>
        {
            ["weekStart"] = "2026-10-18",
        });
        Assert.NotEqual(true, meal.IsError);
        Assert.Equal(35, meal.StructuredContent!.Value.GetProperty("mealPlan").GetProperty("slots").GetArrayLength());
        Assert.Equal("Sunday", meal.StructuredContent.Value.GetProperty("mealPlan").GetProperty("slots")[0].GetProperty("dayOfWeek").GetString());

        var emptyMeal = await client.CallToolAsync("meal_plan_get", new Dictionary<string, object?>
        {
            ["weekStart"] = "2026-10-25",
        });
        Assert.NotEqual(true, emptyMeal.IsError);
        Assert.Equal(JsonValueKind.Null, emptyMeal.StructuredContent!.Value.GetProperty("mealPlan").ValueKind);

        var routines = await client.CallToolAsync("routines_get", new Dictionary<string, object?>());
        Assert.NotEqual(true, routines.IsError);
        Assert.Contains(routines.StructuredContent!.Value.GetProperty("routines").EnumerateArray(),
            item => item.GetProperty("name").GetString() == "Synthetic morning routine");

        var areas = await client.CallToolAsync("areas_get", new Dictionary<string, object?>());
        Assert.NotEqual(true, areas.IsError);
        Assert.Contains(areas.StructuredContent!.Value.GetProperty("areas").EnumerateArray(),
            item => item.GetProperty("name").GetString() == "Synthetic food area");

        Assert.Contains(host.AuditMessages, message => message.Contains("capability=lists_get", StringComparison.Ordinal));
        Assert.Contains(host.AuditMessages, message => message.Contains("capability=meal_plan_get", StringComparison.Ordinal));
        Assert.Contains(host.AuditMessages, message => message.Contains("capability=routines_get", StringComparison.Ordinal));
        Assert.Contains(host.AuditMessages, message => message.Contains("capability=areas_get", StringComparison.Ordinal));
        Assert.Contains(host.AuditMessages, message => message.Contains($"user={host.RequesterUserId}", StringComparison.Ordinal)
            && message.Contains($"household={host.FamilyAId}", StringComparison.Ordinal)
            && message.Contains("client=synthetic-proof-client", StringComparison.Ordinal));

        host.Authorization.ThrowUnexpectedFailure = true;
        var safeFailure = await client.CallToolAsync("areas_get", new Dictionary<string, object?>());
        Assert.True(safeFailure.IsError);
        Assert.Equal("INTERNAL_ERROR", safeFailure.StructuredContent!.Value.GetProperty("code").GetString());
        Assert.Contains(host.AuditMessages, message => message.Contains("capability=areas_get", StringComparison.Ordinal)
            && message.Contains("tool_error", StringComparison.Ordinal));
        host.Authorization.ThrowUnexpectedFailure = false;

        host.Authorization.BlockForCancellation = true;
        using var cancellation = new CancellationTokenSource();
        var pending = client.CallToolAsync("routines_get", new Dictionary<string, object?>(), cancellationToken: cancellation.Token);
        await host.Authorization.WaitUntilStartedAsync();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await pending);
        await host.Authorization.WaitUntilCancellationObservedAsync();
        Assert.Equal(0, host.Database.ChangeTracker.Entries().Count(entry =>
            entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted));
    }

    [Fact]
    public async Task Official_client_reads_bounded_production_timeline_and_minimal_member_directory()
    {
        await using var host = await ReadOnlyProofHost.StartAsync(registerProductionTools: true);
        await using var client = await host.CreateClientAsync();

        var tools = await client.ListToolsAsync();
        var agendaTool = Assert.Single(tools, tool => tool.Name == "agenda_get");
        var membersTool = Assert.Single(tools, tool => tool.Name == "household_members_list");
        AssertSchemaProperties(agendaTool.ProtocolTool.InputSchema, "from", "to");
        AssertSchemaProperties(membersTool.ProtocolTool.InputSchema);
        var agendaSchema = agendaTool.ProtocolTool.OutputSchema!.Value;
        var membersSchema = membersTool.ProtocolTool.OutputSchema!.Value;
        AssertSchemaObjectProperties(agendaSchema, agendaSchema, "events", "listItems", "notes");
        AssertSchemaRequired(agendaSchema, agendaSchema, "events", "listItems", "notes");
        var eventSchema = SchemaProperty(agendaSchema, agendaSchema, "events").GetProperty("items");
        AssertSchemaObjectProperties(agendaSchema, eventSchema, "title", "date", "time", "endDate", "endTime", "status", "participants");
        AssertSchemaRequired(agendaSchema, eventSchema, "title", "date", "time", "endDate", "endTime", "status", "participants");
        AssertSchemaFormat(agendaSchema, SchemaProperty(agendaSchema, eventSchema, "date"), "date");
        AssertSchemaFormat(agendaSchema, SchemaProperty(agendaSchema, eventSchema, "endDate"), "date");
        AssertSchemaAllowsNull(agendaSchema, SchemaProperty(agendaSchema, eventSchema, "time"));
        AssertSchemaAllowsNull(agendaSchema, SchemaProperty(agendaSchema, eventSchema, "endDate"));
        var participantSchema = SchemaProperty(agendaSchema, eventSchema, "participants").GetProperty("items");
        AssertSchemaObjectProperties(agendaSchema, participantSchema, "displayName");
        AssertSchemaRequired(agendaSchema, participantSchema, "displayName");
        var listItemSchema = SchemaProperty(agendaSchema, agendaSchema, "listItems").GetProperty("items");
        AssertSchemaObjectProperties(agendaSchema, listItemSchema, "listName", "itemName", "dueDate", "reminder", "checked", "importance");
        AssertSchemaRequired(agendaSchema, listItemSchema, "listName", "itemName", "dueDate", "reminder", "checked", "importance");
        AssertSchemaFormat(agendaSchema, SchemaProperty(agendaSchema, listItemSchema, "dueDate"), "date");
        AssertSchemaFormat(agendaSchema, SchemaProperty(agendaSchema, listItemSchema, "reminder"), "date-time");
        AssertSchemaAllowsNull(agendaSchema, SchemaProperty(agendaSchema, listItemSchema, "dueDate"));
        AssertSchemaAllowsNull(agendaSchema, SchemaProperty(agendaSchema, listItemSchema, "reminder"));
        var noteSchema = SchemaProperty(agendaSchema, agendaSchema, "notes").GetProperty("items");
        AssertSchemaObjectProperties(agendaSchema, noteSchema, "text", "date", "createdAtUtc");
        AssertSchemaRequired(agendaSchema, noteSchema, "text", "date", "createdAtUtc");
        AssertSchemaFormat(agendaSchema, SchemaProperty(agendaSchema, noteSchema, "date"), "date");
        AssertSchemaFormat(agendaSchema, SchemaProperty(agendaSchema, noteSchema, "createdAtUtc"), "date-time");
        AssertSchemaObjectProperties(membersSchema, membersSchema, "members");
        AssertSchemaRequired(membersSchema, membersSchema, "members");
        var memberSchema = SchemaProperty(membersSchema, membersSchema, "members").GetProperty("items");
        AssertSchemaObjectProperties(membersSchema, memberSchema, "memberId", "name", "role");
        AssertSchemaRequired(membersSchema, memberSchema, "memberId", "name", "role");
        AssertSchemaFormat(membersSchema, SchemaProperty(membersSchema, memberSchema, "memberId"), "uuid");
        Assert.Equal("string", agendaTool.ProtocolTool.InputSchema.GetProperty("properties").GetProperty("from").GetProperty("type").GetString());
        Assert.Equal("string", agendaTool.ProtocolTool.InputSchema.GetProperty("properties").GetProperty("to").GetProperty("type").GetString());
        Assert.Equal(new[] { "from", "to" },
            agendaTool.ProtocolTool.InputSchema.GetProperty("required").EnumerateArray().Select(x => x.GetString()).ToArray());
        Assert.DoesNotContain("familyId", agendaTool.ProtocolTool.InputSchema.GetRawText(), StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("userId", agendaTool.ProtocolTool.InputSchema.GetRawText(), StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("memberId", agendaTool.ProtocolTool.InputSchema.GetRawText(), StringComparison.OrdinalIgnoreCase);

        var agendaResult = await client.CallToolAsync("agenda_get", new Dictionary<string, object?>
        {
            ["from"] = From.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ["to"] = To.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        });

        Assert.NotEqual(true, agendaResult.IsError);
        Assert.NotNull(agendaResult.StructuredContent);
        var agenda = agendaResult.StructuredContent!.Value;
        AssertJsonProperties(agenda, "events", "listItems", "notes");
        var eventTitles = agenda.GetProperty("events").EnumerateArray()
            .Select(item => item.GetProperty("title").GetString()).ToArray();
        Assert.Contains("Family A appointment", eventTitles);
        Assert.Contains("Family A end boundary", eventTitles);
        Assert.Equal(2, eventTitles.Length);
        Assert.Equal("2026-10-20", agenda.GetProperty("events")[0].GetProperty("date").GetString());
        Assert.Equal("2026-10-22", agenda.GetProperty("notes")[0].GetProperty("date").GetString());
        AssertJsonProperties(agenda.GetProperty("events")[0], "title", "date", "time", "endDate", "endTime", "status", "participants");
        AssertJsonProperties(agenda.GetProperty("events")[0].GetProperty("participants")[0], "displayName");
        Assert.Equal("Family A note", agenda.GetProperty("notes")[0].GetProperty("text").GetString());
        AssertJsonProperties(agenda.GetProperty("notes")[0], "text", "date", "createdAtUtc");
        var listItemNames = agenda.GetProperty("listItems").EnumerateArray()
            .Select(item => item.GetProperty("itemName").GetString()).ToArray();
        var scheduledItem = Assert.Single(agenda.GetProperty("listItems").EnumerateArray(),
            item => item.GetProperty("itemName").GetString() == "Synthetic item");
        Assert.Equal("Household list", scheduledItem.GetProperty("listName").GetString());
        Assert.Equal("2026-10-25", scheduledItem.GetProperty("dueDate").GetString());
        AssertJsonProperties(scheduledItem, "listName", "itemName", "dueDate", "reminder", "checked", "importance");
        Assert.Contains("Reminder at window start", listItemNames);
        Assert.Contains("Reminder at window end", listItemNames);
        Assert.DoesNotContain("Reminder before window", listItemNames);
        Assert.DoesNotContain("Reminder after window", listItemNames);
        Assert.DoesNotContain("Family B secret", agenda.GetRawText(), StringComparison.Ordinal);
        Assert.DoesNotContain("calendarEventId", agenda.GetRawText(), StringComparison.Ordinal);
        Assert.DoesNotContain("areaId", agenda.GetRawText(), StringComparison.Ordinal);

        var maxRangeResult = await client.CallToolAsync("agenda_get", new Dictionary<string, object?>
        {
            ["from"] = From.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ["to"] = "2026-11-19",
        });
        Assert.NotEqual(true, maxRangeResult.IsError);
        Assert.Contains(maxRangeResult.StructuredContent!.Value.GetProperty("events").EnumerateArray(),
            item => item.GetProperty("title").GetString() == "Family A maximum boundary");

        var membersResult = await client.CallToolAsync("household_members_list", new Dictionary<string, object?>());
        Assert.NotEqual(true, membersResult.IsError);
        Assert.NotNull(membersResult.StructuredContent);
        AssertJsonProperties(membersResult.StructuredContent!.Value, "members");
        var members = membersResult.StructuredContent!.Value.GetProperty("members").EnumerateArray().ToArray();
        Assert.Equal(3, members.Length);
        AssertJsonProperties(members[0], "memberId", "name", "role");
        Assert.Equal(2, members.Count(member => member.GetProperty("name").GetString() == "Alex"));
        Assert.Equal(2, members.Where(member => member.GetProperty("name").GetString() == "Alex")
            .Select(member => member.GetProperty("memberId").GetGuid()).Distinct().Count());
        Assert.Contains(members, member => member.GetProperty("name").GetString() == "Preferred Alex");
        Assert.DoesNotContain("familyId", membersResult.StructuredContent.Value.GetRawText(), StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("linkedEmail", membersResult.StructuredContent.Value.GetRawText(), StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("birthDate", membersResult.StructuredContent.Value.GetRawText(), StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Family B member", membersResult.StructuredContent.Value.GetRawText(), StringComparison.Ordinal);

        Assert.Equal(0, host.Database.ChangeTracker.Entries().Count(entry =>
            entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted));
        Assert.Equal(3, host.Authorization.CheckedFamilyIds.Count(id => id == host.FamilyAId));
        Assert.All(host.Authorization.CheckedUserIds, id => Assert.Equal(host.RequesterUserId, id));
    }

    [Theory]
    [InlineData("20/10/2026", "2026-10-31")]
    [InlineData("2026-02-30", "2026-03-01")]
    [InlineData("2026-10-31", "2026-10-20")]
    [InlineData("2026-10-01", "2026-11-01")]
    public async Task Agenda_rejects_ambiguous_invalid_reversed_and_overlong_ranges_safely(string from, string to)
    {
        await using var host = await ReadOnlyProofHost.StartAsync(registerProductionTools: true);
        await using var client = await host.CreateClientAsync();

        var result = await client.CallToolAsync("agenda_get", new Dictionary<string, object?>
        {
            ["from"] = from,
            ["to"] = to,
        });

        Assert.True(result.IsError);
        AssertJsonProperties(result.StructuredContent!.Value, "code", "message");
        Assert.Equal("INVALID_INPUT", result.StructuredContent!.Value.GetProperty("code").GetString());
        Assert.DoesNotContain("Exception", result.StructuredContent.Value.GetRawText(), StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("connection", result.StructuredContent.Value.GetRawText(), StringComparison.OrdinalIgnoreCase);
        Assert.Empty(host.Authorization.CheckedFamilyIds);
    }

    [Fact]
    public async Task MCP_argument_binding_errors_for_missing_partial_null_and_wrong_type_inputs_are_safe()
    {
        await using var host = await ReadOnlyProofHost.StartAsync(registerProductionTools: true);
        await using var client = await host.CreateClientAsync();
        Dictionary<string, object?>[] invalidArguments =
        [
            new(),
            new() { ["from"] = "2026-10-20" },
            new() { ["to"] = "2026-10-31" },
            new() { ["from"] = null, ["to"] = "2026-10-31" },
            new() { ["from"] = 20261020, ["to"] = "2026-10-31" },
            new() { ["from"] = "2026-10-20", ["to"] = new { invalid = true } },
        ];

        foreach (var arguments in invalidArguments)
        {
            var result = await client.CallToolAsync("agenda_get", arguments);
            Assert.True(result.IsError);
            var errorText = string.Join(" ", result.Content.OfType<TextContentBlock>().Select(block => block.Text));
            Assert.DoesNotContain("test-only-secret", errorText, StringComparison.Ordinal);
            Assert.DoesNotContain("System.InvalidOperationException", errorText, StringComparison.Ordinal);
            if (result.StructuredContent is { } structured)
            {
                AssertJsonProperties(structured, "code", "message");
                Assert.Equal("INVALID_INPUT", structured.GetProperty("code").GetString());
            }
        }

        Assert.Empty(host.Authorization.CheckedFamilyIds);
    }

    [Fact]
    public async Task Agenda_propagates_cancellation_from_the_MCP_client_to_the_application_query()
    {
        await using var host = await ReadOnlyProofHost.StartAsync(registerProductionTools: true);
        await using var client = await host.CreateClientAsync();
        using var cancellation = new CancellationTokenSource();
        host.Authorization.BlockForCancellation = true;
        var pending = client.CallToolAsync("agenda_get",
            new Dictionary<string, object?>
            {
                ["from"] = From.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                ["to"] = To.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            }, cancellationToken: cancellation.Token);
        await host.Authorization.WaitUntilStartedAsync();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await pending);
        await host.Authorization.WaitUntilCancellationObservedAsync();
        Assert.Contains(host.FamilyAId, host.Authorization.CheckedFamilyIds);
    }

    [Fact]
    public async Task Unexpected_handler_failure_returns_a_safe_structured_error()
    {
        await using var host = await ReadOnlyProofHost.StartAsync(registerProductionTools: true);
        await using var client = await host.CreateClientAsync();
        host.Authorization.ThrowUnexpectedFailure = true;

        var result = await client.CallToolAsync("agenda_get", new Dictionary<string, object?>
        {
            ["from"] = From.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ["to"] = To.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        });

        Assert.True(result.IsError);
        AssertJsonProperties(result.StructuredContent!.Value, "code", "message");
        Assert.Equal("INTERNAL_ERROR", result.StructuredContent.Value.GetProperty("code").GetString());
        Assert.DoesNotContain("test-only-secret", result.StructuredContent.Value.GetRawText(), StringComparison.Ordinal);
        Assert.DoesNotContain("test-only-secret", string.Join(" ", result.Content.OfType<TextContentBlock>().Select(block => block.Text)), StringComparison.Ordinal);
    }

    [Fact]
    public async Task Direct_query_for_other_synthetic_household_is_denied_and_identity_never_comes_from_tool_input()
    {
        await using var host = await ReadOnlyProofHost.StartAsync(registerProductionTools: true);
        await using var client = await host.CreateClientAsync();

        var tools = await client.ListToolsAsync();
        var agenda = Assert.Single(tools, tool => tool.Name == "agenda_get");
        Assert.DoesNotContain("familyId", agenda.ProtocolTool.InputSchema.GetRawText(), StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("userId", agenda.ProtocolTool.InputSchema.GetRawText(), StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("memberId", agenda.ProtocolTool.InputSchema.GetRawText(), StringComparison.OrdinalIgnoreCase);

        var otherFamilyQuery = new GetFamilyTimelineQuery(host.FamilyBId, From, To, host.RequesterUserId);
        var denied = await Assert.ThrowsAsync<DomusMind.Application.Features.Calendar.CalendarException>(
            () => host.Dispatcher.Dispatch<FamilyTimelineResponse>(otherFamilyQuery, CancellationToken.None));
        Assert.Equal("AccessDenied", denied.Code.ToString());
        Assert.Contains(host.FamilyBId, host.Authorization.CheckedFamilyIds);
        Assert.All(host.Authorization.CheckedUserIds, id => Assert.Equal(host.RequesterUserId, id));

        var ownResult = await client.CallToolAsync("agenda_get", new Dictionary<string, object?>
        {
            ["from"] = From.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ["to"] = To.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ["familyId"] = host.FamilyBId,
            ["userId"] = Guid.NewGuid(),
        });
        Assert.DoesNotContain("Family B secret", ownResult.StructuredContent!.Value.GetRawText(), StringComparison.Ordinal);
    }

    private static void AssertSchemaProperties(System.Text.Json.JsonElement schema, params string[] properties)
        => AssertSchemaObjectProperties(schema, schema, properties);

    private static void AssertSchemaObjectProperties(System.Text.Json.JsonElement root, System.Text.Json.JsonElement schema,
        params string[] properties)
    {
        schema = ResolveSchema(root, schema);
        var actual = schema.GetProperty("properties").EnumerateObject().Select(property => property.Name)
            .Order(StringComparer.Ordinal).ToArray();
        Assert.Equal(properties.Order(StringComparer.Ordinal).ToArray(), actual);
    }

    private static void AssertSchemaRequired(System.Text.Json.JsonElement root, System.Text.Json.JsonElement schema,
        params string[] required)
    {
        schema = ResolveSchema(root, schema);
        var actual = schema.GetProperty("required").EnumerateArray().Select(item => item.GetString())
            .Order(StringComparer.Ordinal).ToArray();
        Assert.Equal(required.Order(StringComparer.Ordinal).ToArray(), actual);
    }

    private static System.Text.Json.JsonElement SchemaProperty(System.Text.Json.JsonElement root,
        System.Text.Json.JsonElement schema, string property)
    {
        schema = ResolveSchema(root, schema);
        return schema.GetProperty("properties").GetProperty(property);
    }

    private static void AssertSchemaFormat(System.Text.Json.JsonElement root,
        System.Text.Json.JsonElement schema, string format)
    {
        schema = ResolveSchema(root, schema);
        Assert.Equal(format, schema.GetProperty("format").GetString());
    }

    private static void AssertSchemaAllowsNull(System.Text.Json.JsonElement root,
        System.Text.Json.JsonElement schema)
    {
        schema = ResolveSchema(root, schema);
        if (schema.TryGetProperty("type", out var type))
        {
            if (type.ValueKind == System.Text.Json.JsonValueKind.Array &&
                type.EnumerateArray().Any(value => value.GetString() == "null"))
                return;
            if (type.ValueKind == System.Text.Json.JsonValueKind.String && type.GetString() == "null")
                return;
        }
        if (schema.TryGetProperty("anyOf", out var anyOf) && anyOf.EnumerateArray()
                .Any(branch => ResolveSchema(root, branch).TryGetProperty("type", out var branchType) && branchType.GetString() == "null"))
            return;
        if (schema.TryGetProperty("oneOf", out var oneOf) && oneOf.EnumerateArray()
                .Any(branch => ResolveSchema(root, branch).TryGetProperty("type", out var branchType) && branchType.GetString() == "null"))
            return;
        Assert.Fail($"Expected schema node to accept null: {schema.GetRawText()}");
    }

    private static System.Text.Json.JsonElement ResolveSchema(System.Text.Json.JsonElement root,
        System.Text.Json.JsonElement schema)
    {
        if (!schema.TryGetProperty("$ref", out var reference))
            return schema;

        var path = reference.GetString() ?? throw new InvalidOperationException("Schema reference cannot be empty.");
        var current = root;
        foreach (var segment in path.Split('/').Skip(2))
            current = current.GetProperty(segment.Replace("~1", "/", StringComparison.Ordinal).Replace("~0", "~", StringComparison.Ordinal));
        return ResolveSchema(root, current);
    }

    private static void AssertJsonProperties(System.Text.Json.JsonElement value, params string[] properties)
    {
        var actual = value.EnumerateObject().Select(property => property.Name).Order(StringComparer.Ordinal).ToArray();
        Assert.Equal(properties.Order(StringComparer.Ordinal).ToArray(), actual);
    }

    private sealed class ReadOnlyProofHost : IAsyncDisposable
    {
        private readonly WebApplication _app;
        private readonly string _webRoot;

        private ReadOnlyProofHost(WebApplication app, string webRoot, DomusMindDbContext database,
            TestQueryDispatcher dispatcher, TestFamilyAuthorizationService authorization,
            Guid familyAId, Guid familyBId, Guid familyBListId, Guid requesterUserId, List<string> auditMessages)
        {
            _app = app;
            _webRoot = webRoot;
            Database = database;
            Dispatcher = dispatcher;
            Authorization = authorization;
            FamilyAId = familyAId;
            FamilyBId = familyBId;
            FamilyBListId = familyBListId;
            RequesterUserId = requesterUserId;
            AuditMessages = auditMessages;
        }

        public DomusMindDbContext Database { get; }
        public TestQueryDispatcher Dispatcher { get; }
        public TestFamilyAuthorizationService Authorization { get; }
        public Guid FamilyAId { get; }
        public Guid FamilyBId { get; }
        public Guid FamilyBListId { get; }
        public Guid RequesterUserId { get; }
        public IReadOnlyCollection<string> AuditMessages { get; }

        public static async Task<ReadOnlyProofHost> StartAsync(bool registerProductionTools = false)
        {
            var webRoot = Path.Combine(Path.GetTempPath(), $"domusmind-mcp-read-test-{Guid.NewGuid():N}");
            Directory.CreateDirectory(webRoot);
            await File.WriteAllTextAsync(Path.Combine(webRoot, "index.html"), "synthetic-only");

            var writeGuard = new RejectPersistenceWritesInterceptor();
            var database = new DomusMindDbContext(new DbContextOptionsBuilder<DomusMindDbContext>()
                .UseInMemoryDatabase($"mcp-read-proof-{Guid.NewGuid():N}")
                .AddInterceptors(writeGuard)
                .Options);
            var seed = await SeedAsync(database);
            writeGuard.RejectWrites = true;
            var authorization = new TestFamilyAuthorizationService(seed.RequesterUserId, seed.FamilyAId);
            var users = new EmptyAuthUserRepository();
            var dispatcher = new TestQueryDispatcher(database, authorization, users);

            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                EnvironmentName = "Testing",
                ContentRootPath = webRoot,
                WebRootPath = webRoot,
            });
            builder.WebHost.UseTestServer();
            builder.Logging.ClearProviders();
            var auditMessages = new List<string>();
            builder.Logging.AddProvider(new CapturingLoggerProvider(auditMessages));
            builder.Services.AddControllers().AddApplicationPart(typeof(SystemController).Assembly);
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Mcp:AllowedOrigins:0"] = "https://trusted.example",
                ["Mcp:OAuth:Enabled"] = registerProductionTools.ToString(),
                ["Mcp:OAuth:Resource"] = "https://domusmind.test/mcp",
                ["Mcp:RequireOAuth"] = registerProductionTools.ToString(),
                ["Mcp:Capabilities:HouseholdReadOnly"] = registerProductionTools.ToString(),
            }).Build();
            builder.Services.AddSingleton<IDomusMindDbContext>(database);
            builder.Services.AddSingleton(authorization);
            builder.Services.AddSingleton<IFamilyAuthorizationService>(authorization);
            builder.Services.AddSingleton<IAuthUserRepository>(users);
            builder.Services.AddSingleton<IQueryDispatcher>(dispatcher);
            builder.Services.AddSingleton(new ReadOnlyMcpTestIdentity(seed.FamilyAId, seed.RequesterUserId));
            var mcpBuilder = builder.Services.AddDomusMindMcp(config);
            if (registerProductionTools)
                mcpBuilder.WithTools([typeof(DomusMind.Mcp.Tools.DomusMindReadOnlyTools)]);
            else
                mcpBuilder.WithTools([typeof(ReadOnlyMcpProofTools)]);

            var app = builder.Build();
            app.Use(async (context, next) =>
            {
                var originalCulture = CultureInfo.CurrentCulture;
                var originalUiCulture = CultureInfo.CurrentUICulture;
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("th-TH");
                CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("th-TH");
                try
                {
                    await next();
                }
                finally
                {
                    CultureInfo.CurrentCulture = originalCulture;
                    CultureInfo.CurrentUICulture = originalUiCulture;
                }
            });
            app.UseStaticFiles();
            app.UseDomusMindMcpOriginValidation();
            if (registerProductionTools)
            {
                app.Use(async (context, next) =>
                {
                    var seedIdentity = $"{seed.RequesterUserId}";
                    var seedHousehold = $"{seed.FamilyAId}";
                    var identity = new ClaimsIdentity(
                    [
                        new Claim("sub", seedIdentity),
                        new Claim("dm_household_id", seedHousehold),
                        new Claim("client_id", "synthetic-proof-client"),
                    ], "SyntheticProof");
                    context.User = new ClaimsPrincipal(identity);
                    await next();
                });
            }
            app.MapDomusMindEndpoints(webRoot);
            await app.StartAsync();
            return new ReadOnlyProofHost(app, webRoot, database, dispatcher, authorization,
                seed.FamilyAId, seed.FamilyBId, seed.FamilyBListId, seed.RequesterUserId, auditMessages);
        }

        public async Task<McpClient> CreateClientAsync()
        {
            var transport = new HttpClientTransport(new HttpClientTransportOptions
            {
                Endpoint = new Uri("http://localhost/mcp"),
                TransportMode = HttpTransportMode.StreamableHttp,
                EnableStandaloneGetStream = false,
            }, _app.GetTestClient(), ownsHttpClient: true);
            return await McpClient.CreateAsync(transport, new McpClientOptions
            {
                ClientInfo = new Implementation { Name = "DomusMind.Api.Tests", Version = "1.0" },
                ProtocolVersion = "2026-07-28",
            });
        }

        public async ValueTask DisposeAsync()
        {
            await _app.DisposeAsync();
            await Database.DisposeAsync();
            Directory.Delete(_webRoot, recursive: true);
        }

        private static async Task<Seed> SeedAsync(DomusMindDbContext database)
        {
            var now = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc);
            var familyA = Domain.Family.Family.Create(FamilyId.New(), FamilyName.Create("Synthetic A"), null, now);
            var familyB = Domain.Family.Family.Create(FamilyId.New(), FamilyName.Create("Synthetic B"), null, now);
            familyA.UpdateSettings(FamilyName.Create("Synthetic A"), "en", "sunday", "yyyy-MM-dd", now);
            var requesterUserId = Guid.NewGuid();
            var alexOne = MemberId.New();
            var alexTwo = MemberId.New();
            familyA.AddMember(alexOne, MemberName.Create("Alex"), MemberRole.Adult,
                isManager: true, birthDate: null, now, authUserId: requesterUserId);
            familyA.AddMember(alexTwo, MemberName.Create("Alex"), MemberRole.Child, now);
            var preferred = familyA.AddMember(MemberId.New(), MemberName.Create("Alexander"), MemberRole.Adult,
                addedAtUtc: now);
            familyA.UpdateMemberProfile(preferred.Id, "Preferred Alex", null, null, null, null, null, now);
            var familyBMember = familyB.AddMember(MemberId.New(), MemberName.Create("Family B member"), MemberRole.Adult,
                addedAtUtc: now);

            var eventA = CreateEvent(familyA.Id, "Family A appointment", new DateOnly(2026, 10, 20), alexOne);
            var eventAEnd = CreateEvent(familyA.Id, "Family A end boundary", new DateOnly(2026, 10, 31), alexTwo);
            var eventAOutside = CreateEvent(familyA.Id, "Out of range event", new DateOnly(2026, 11, 1), alexTwo);
            var eventAAtMaxRange = CreateEvent(familyA.Id, "Family A maximum boundary", new DateOnly(2026, 11, 19), alexOne);
            var eventB = CreateEvent(familyB.Id, "Family B secret", new DateOnly(2026, 10, 20), familyBMember.Id);
            var list = SharedList.Create(ListId.New(), familyA.Id, ListName.Create("Household list"),
                ListKind.Create("Shopping"), null, null, null, now);
            var familyBList = SharedList.Create(ListId.New(), familyB.Id, ListName.Create("Family B private list"),
                ListKind.Create("Shopping"), null, null, null, now);
            var item = list.AddItem(ListItemId.New(), ListItemName.Create("Synthetic item"), null, null, now);
            list.SetItemTemporal(item.Id, new DateOnly(2026, 10, 25), null, null, now);
            AddReminderOnlyItem(list, "Reminder before window",
                new DateTimeOffset(2026, 10, 20, 0, 0, 0, TimeSpan.Zero).AddTicks(-1), now);
            AddReminderOnlyItem(list, "Reminder at window start",
                new DateTimeOffset(2026, 10, 20, 0, 0, 0, TimeSpan.Zero), now);
            AddReminderOnlyItem(list, "Reminder at window end",
                new DateTimeOffset(2026, 10, 31, 23, 59, 59, TimeSpan.Zero).AddTicks(9_999_999), now);
            AddReminderOnlyItem(list, "Reminder after window",
                new DateTimeOffset(2026, 10, 31, 23, 59, 59, TimeSpan.Zero).AddTicks(10_000_000), now);

            var mealPlan = MealPlan.Create(MealPlanId.New(), familyA.Id,
                new DateOnly(2026, 10, 18), true, now);
            mealPlan.UpdateSlot(
                DomusMind.Domain.MealPlanning.Enums.DayOfWeek.Sunday,
                MealType.Dinner,
                MealSourceType.FreeText,
                null,
                "Sunday soup",
                null,
                false,
                null,
                now);

            var area = ResponsibilityDomain.Create(
                ResponsibilityDomainId.New(), familyA.Id,
                ResponsibilityAreaName.Create("Synthetic food area"),
                HexColor.From("#22C55E"), now);
            area.AssignPrimaryOwner(alexOne);
            var routine = Routine.Create(
                RoutineId.New(), familyA.Id,
                RoutineName.Create("Synthetic morning routine"),
                RoutineScope.Household,
                RoutineKind.Scheduled,
                HexColor.From("#3B82F6"),
                RoutineSchedule.Daily(new TimeOnly(7, 0)),
                area.Id,
                [],
                now);

            database.Set<Domain.Family.Family>().AddRange(familyA, familyB);
            database.Set<Domain.Calendar.CalendarEvent>().AddRange(eventA, eventAEnd, eventAOutside, eventAAtMaxRange, eventB);
            database.Set<SharedList>().AddRange(list, familyBList);
            database.Set<MealPlan>().Add(mealPlan);
            database.Set<ResponsibilityDomain>().Add(area);
            database.Set<Routine>().Add(routine);
            database.Set<TimelineNote>().Add(TimelineNote.Create(TimelineNoteId.New(), familyA.Id,
                new DateOnly(2026, 10, 22), "Family A note", now));
            database.Set<TimelineNote>().Add(TimelineNote.Create(TimelineNoteId.New(), familyB.Id,
                new DateOnly(2026, 10, 22), "Family B secret note", now));
            await database.SaveChangesAsync();
            database.ChangeTracker.Clear();
            return new Seed(familyA.Id.Value, familyB.Id.Value, familyBList.Id.Value, requesterUserId);
        }

        private static Domain.Calendar.CalendarEvent CreateEvent(FamilyId familyId, string title, DateOnly date,
            MemberId participant)
        {
            var calendarEvent = Domain.Calendar.CalendarEvent.Create(CalendarEventId.New(), familyId,
                EventTitle.Create(title), null, EventTime.Moment(date, new TimeOnly(10, 0)),
                HexColor.From("#3B82F6"));
            calendarEvent.AddParticipant(participant);
            return calendarEvent;
        }

        private static void AddReminderOnlyItem(SharedList list, string name, DateTimeOffset reminder, DateTime now)
        {
            var item = list.AddItem(ListItemId.New(), ListItemName.Create(name), null, null, now);
            list.SetItemTemporal(item.Id, null, reminder, null, now);
        }

        private sealed record Seed(Guid FamilyAId, Guid FamilyBId, Guid FamilyBListId, Guid RequesterUserId);
    }

    private sealed class RejectPersistenceWritesInterceptor : SaveChangesInterceptor
    {
        public bool RejectWrites { get; set; }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (RejectWrites)
                throw new InvalidOperationException("The read-only MCP proof rejects all persistence writes.");
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (RejectWrites)
                throw new InvalidOperationException("The read-only MCP proof rejects all persistence writes.");
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }

    private sealed class CapturingLoggerProvider(ICollection<string> messages) : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new CapturingLogger(messages);
        public void Dispose() { }
    }

    private sealed class CapturingLogger(ICollection<string> messages) : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
            => messages.Add(formatter(state, exception));

        private sealed class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new();
            public void Dispose() { }
        }
    }

    private sealed class TestFamilyAuthorizationService(Guid allowedUserId, Guid allowedFamilyId)
        : IFamilyAuthorizationService
    {
        private readonly TaskCompletionSource _started = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly TaskCompletionSource _cancellationObserved = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public List<Guid> CheckedFamilyIds { get; } = [];
        public List<Guid> CheckedUserIds { get; } = [];
        public bool BlockForCancellation { get; set; }
        public bool ThrowUnexpectedFailure { get; set; }
        public HashSet<Guid> AdditionalAllowedFamilyIds { get; } = [];
        public Task WaitUntilStartedAsync() => _started.Task.WaitAsync(TimeSpan.FromSeconds(5));
        public Task WaitUntilCancellationObservedAsync() => _cancellationObserved.Task.WaitAsync(TimeSpan.FromSeconds(5));

        public async Task<bool> CanAccessFamilyAsync(Guid userId, Guid familyId, CancellationToken cancellationToken = default)
        {
            CheckedFamilyIds.Add(familyId);
            CheckedUserIds.Add(userId);
            if (ThrowUnexpectedFailure)
                throw new InvalidOperationException("test-only-secret");
            if (BlockForCancellation)
            {
                _started.TrySetResult();
                try
                {
                    await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    _cancellationObserved.TrySetResult();
                    throw;
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
            return userId == allowedUserId && (familyId == allowedFamilyId || AdditionalAllowedFamilyIds.Contains(familyId));
        }
    }

    private sealed class EmptyAuthUserRepository : IAuthUserRepository
    {
        public Task<AuthUserRecord?> FindByEmailAsync(string email, CancellationToken cancellationToken) => Task.FromResult<AuthUserRecord?>(null);
        public Task<AuthUserRecord?> FindByIdAsync(Guid userId, CancellationToken cancellationToken) => Task.FromResult<AuthUserRecord?>(null);
        public Task<IReadOnlyDictionary<Guid, AuthUserStatusProjection>> GetStatusByIdsAsync(IReadOnlyCollection<Guid> userIds, CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyDictionary<Guid, AuthUserStatusProjection>>(new Dictionary<Guid, AuthUserStatusProjection>());
        public Task AddAsync(AuthUserRecord user, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task UpdatePasswordHashAsync(Guid userId, string newPasswordHash, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task UpdateMustChangePasswordAsync(Guid userId, bool mustChangePassword, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task UpdatePasswordChangedAtAsync(Guid userId, DateTime changedAtUtc, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task DisableUserAsync(Guid userId, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task EnableUserAsync(Guid userId, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task UpdateLastLoginAtAsync(Guid userId, DateTime lastLoginAtUtc, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task<bool> AnyUsersAsync(CancellationToken cancellationToken) => Task.FromResult(false);
        public Task SaveChangesAsync(CancellationToken cancellationToken) => throw new NotSupportedException();
    }

    public sealed class TestQueryDispatcher(DomusMindDbContext database, IFamilyAuthorizationService authorization,
        IAuthUserRepository users) : IQueryDispatcher
    {
        public async Task<TResponse> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            object response = query switch
            {
                GetFamilyTimelineQuery timeline => await new GetFamilyTimelineQueryHandler(database, authorization)
                    .Handle(timeline, cancellationToken),
                GetFamilyMembersQuery members => await new GetFamilyMembersQueryHandler(database, authorization, users)
                    .Handle(members, cancellationToken),
                GetFamilyListsQuery lists => await new GetFamilyListsQueryHandler(database, authorization)
                    .Handle(lists, cancellationToken),
                GetListDetailQuery list => await new GetListDetailQueryHandler(database, authorization)
                    .Handle(list, cancellationToken),
                GetFamilyQuery family => await new GetFamilyQueryHandler(database, authorization)
                    .Handle(family, cancellationToken),
                GetMealPlanQuery mealPlan => await new GetMealPlanQueryHandler(database, authorization)
                    .Handle(mealPlan, cancellationToken),
                GetRoutinesByFamilyQuery routines => await new GetRoutinesByFamilyQueryHandler(database, authorization)
                    .Handle(routines, cancellationToken),
                GetHouseholdAreasQuery areas => await new GetHouseholdAreasQueryHandler(database, authorization)
                    .Handle(areas, cancellationToken),
                _ => throw new InvalidOperationException("Unsupported query in test-only MCP dispatcher."),
            };
            return (TResponse)response;
        }
    }
}
