using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Features.Calendar;
using DomusMind.Application.Features.Calendar.GetFamilyTimeline;
using DomusMind.Application.Features.Family;
using DomusMind.Application.Features.Family.GetFamilyMembers;
using DomusMind.Contracts.Calendar;
using DomusMind.Mcp.Authorization;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace DomusMind.Mcp.Tools;

[McpServerToolType]
public sealed class DomusMindReadOnlyTools
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [McpServerTool(
        Name = "agenda_get",
        ReadOnly = true,
        UseStructuredContent = true,
        OutputSchemaType = typeof(AgendaOutput))]
    [Description("Read the authorized household agenda for an explicit inclusive yyyy-MM-dd date range of no more than 31 calendar days.")]
    public static async Task<CallToolResult> AgendaGet(
        string from,
        string to,
        IMcpRequestIdentityAccessor identityAccessor,
        IQueryDispatcher dispatcher,
        ILogger<DomusMindReadOnlyTools> logger,
        CancellationToken cancellationToken)
    {
        if (!DateOnly.TryParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fromDate) ||
            !DateOnly.TryParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var toDate) ||
            toDate < fromDate || toDate.DayNumber - fromDate.DayNumber + 1 > 31)
        {
            return Audited(logger, "agenda_get", Error("INVALID_INPUT",
                "Provide valid inclusive yyyy-MM-dd dates no more than 31 calendar days apart."), identityAccessor);
        }

        if (!identityAccessor.TryGet(out var identity))
        {
            return Audited(logger, "agenda_get", Error("ACCESS_DENIED", "Access to this household is denied."), identityAccessor);
        }

        try
        {
            var response = await dispatcher.Dispatch(new GetFamilyTimelineQuery(
                identity.HouseholdId,
                fromDate,
                toDate,
                identity.UserId), cancellationToken);

            var output = new AgendaOutput(
                response.Events.Select(item => new AgendaEventOutput(
                    item.Title,
                    DateOnly.ParseExact(item.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    item.Time,
                    ParseOptionalDate(item.EndDate),
                    item.EndTime,
                    item.Status,
                    item.Participants.Select(participant => new AgendaParticipantOutput(participant.DisplayName)).ToArray())).ToArray(),
                response.ListItems.Select(item => new AgendaListItemOutput(
                    item.ListName,
                    item.ItemName,
                    ParseOptionalDate(item.DueDate),
                    ParseOptionalInstant(item.Reminder),
                    item.Checked,
                    item.Importance)).ToArray(),
                response.Notes.Select(item => new AgendaNoteOutput(
                    item.Text,
                    DateOnly.ParseExact(item.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    item.CreatedAtUtc)).ToArray());

            return Audited(logger, "agenda_get", Success(output), identityAccessor, identity);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            var code = IsAccessDenied(exception) ? "ACCESS_DENIED" : "INTERNAL_ERROR";
            var message = IsAccessDenied(exception)
                ? "Access to this household is denied."
                : "The household agenda could not be read.";
            return Audited(logger, "agenda_get", Error(code, message), identityAccessor, identity);
        }
    }

    [McpServerTool(
        Name = "household_members_list",
        ReadOnly = true,
        UseStructuredContent = true,
        OutputSchemaType = typeof(HouseholdMembersOutput))]
    [Description("List the members of the authorized household with stable IDs, display names, and roles.")]
    public static async Task<CallToolResult> HouseholdMembersList(
        IMcpRequestIdentityAccessor identityAccessor,
        IQueryDispatcher dispatcher,
        ILogger<DomusMindReadOnlyTools> logger,
        CancellationToken cancellationToken)
    {
        if (!identityAccessor.TryGet(out var identity))
        {
            return Audited(logger, "household_members_list",
                Error("ACCESS_DENIED", "Access to this household is denied."), identityAccessor);
        }

        try
        {
            var response = await dispatcher.Dispatch(new GetFamilyMembersQuery(
                identity.HouseholdId,
                identity.UserId), cancellationToken);

            var output = new HouseholdMembersOutput(response.Select(member => new HouseholdMemberOutput(
                member.MemberId,
                string.IsNullOrWhiteSpace(member.PreferredName) ? member.Name : member.PreferredName,
                member.Role)).ToArray());

            return Audited(logger, "household_members_list", Success(output), identityAccessor, identity);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            var code = IsAccessDenied(exception) ? "ACCESS_DENIED" : "INTERNAL_ERROR";
            var message = IsAccessDenied(exception)
                ? "Access to this household is denied."
                : "Household members could not be read.";
            return Audited(logger, "household_members_list", Error(code, message), identityAccessor, identity);
        }
    }

    private static CallToolResult Audited(
        ILogger logger,
        string capability,
        CallToolResult result,
        IMcpRequestIdentityAccessor identityAccessor,
        McpRequestIdentity? identity = null)
    {
        identity ??= identityAccessor.TryGet(out var current) ? current : null;
        logger.LogInformation(
            "MCP capability audit {Outcome} capability={Capability} user={UserId} household={HouseholdId} client={ClientId} trace={TraceId}",
            result.IsError == true ? "tool_error" : "success",
            capability,
            identity?.UserId.ToString() ?? "unknown",
            identity?.HouseholdId.ToString() ?? "unknown",
            identity?.ClientId ?? "unknown",
            identity?.TraceId ?? "unknown");
        return result;
    }

    private static CallToolResult Success<T>(T output)
        => new()
        {
            StructuredContent = JsonSerializer.SerializeToElement(output, JsonOptions),
            Content = [],
        };

    private static CallToolResult Error(string code, string message)
        => new()
        {
            IsError = true,
            StructuredContent = JsonSerializer.SerializeToElement(new McpErrorOutput(code, message), JsonOptions),
            Content = [new TextContentBlock { Text = message }],
        };

    private static bool IsAccessDenied(Exception exception)
        => exception is CalendarException { Code: CalendarErrorCode.AccessDenied }
            || exception is FamilyException { Code: FamilyErrorCode.AccessDenied };

    private static DateOnly? ParseOptionalDate(string? value)
        => value is null ? null : DateOnly.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);

    private static DateTimeOffset? ParseOptionalInstant(string? value)
        => value is null ? null : DateTimeOffset.ParseExact(value, "O", CultureInfo.InvariantCulture);
}

public sealed record AgendaOutput(
    IReadOnlyCollection<AgendaEventOutput> Events,
    IReadOnlyCollection<AgendaListItemOutput> ListItems,
    IReadOnlyCollection<AgendaNoteOutput> Notes);

public sealed record AgendaEventOutput(
    string Title,
    DateOnly Date,
    string? Time,
    DateOnly? EndDate,
    string? EndTime,
    string Status,
    IReadOnlyCollection<AgendaParticipantOutput> Participants);

public sealed record AgendaParticipantOutput(string DisplayName);

public sealed record AgendaListItemOutput(
    string ListName,
    string ItemName,
    DateOnly? DueDate,
    DateTimeOffset? Reminder,
    bool Checked,
    bool Importance);

public sealed record AgendaNoteOutput(string Text, DateOnly Date, DateTime CreatedAtUtc);

public sealed record HouseholdMembersOutput(IReadOnlyCollection<HouseholdMemberOutput> Members);

public sealed record HouseholdMemberOutput(Guid MemberId, string Name, string Role);

public sealed record McpErrorOutput(string Code, string Message);
