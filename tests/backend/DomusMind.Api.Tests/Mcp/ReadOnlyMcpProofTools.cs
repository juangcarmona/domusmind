using System.Globalization;
using System.ComponentModel;
using System.Text.Json;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Features.Calendar.GetFamilyTimeline;
using DomusMind.Application.Features.Family.GetFamilyMembers;
using DomusMind.Contracts.Calendar;
using DomusMind.Contracts.Family;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace DomusMind.Api.Tests.Mcp;

[McpServerToolType]
public static class ReadOnlyMcpProofTools
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [McpServerTool(
        Name = "agenda_get",
        ReadOnly = true,
        UseStructuredContent = true,
        OutputSchemaType = typeof(AgendaOutput))]
    [Description("Read the synthetic household timeline for an explicit, bounded calendar-date range.")]
    public static async Task<CallToolResult> AgendaGet(
        string from,
        string to,
        ReadOnlyMcpTestIdentity identity,
        IQueryDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        if (!DateOnly.TryParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fromDate) ||
            !DateOnly.TryParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var toDate) ||
            toDate < fromDate || toDate.DayNumber - fromDate.DayNumber + 1 > 31)
        {
            return Error("INVALID_INPUT", "Provide valid inclusive yyyy-MM-dd dates no more than 31 calendar days apart.");
        }

        try
        {
            using var invariantCulture = new InvariantCultureScope();
            var response = await dispatcher.Dispatch(new GetFamilyTimelineQuery(
                identity.FamilyId,
                fromDate,
                toDate,
                identity.RequesterUserId), cancellationToken);

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
                response.Notes.Select(item => new AgendaNoteOutput(item.Text,
                    DateOnly.ParseExact(item.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture), item.CreatedAtUtc)).ToArray());

            return Success(output);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            return Error(IsAccessDenied(exception) ? "ACCESS_DENIED" : "INTERNAL_ERROR",
                IsAccessDenied(exception) ? "Access to this household is denied." : "The household timeline could not be read.");
        }
    }

    [McpServerTool(
        Name = "household_members_list",
        ReadOnly = true,
        UseStructuredContent = true,
        OutputSchemaType = typeof(HouseholdMembersOutput))]
    [Description("List the members of the synthetic household associated with the test identity.")]
    public static async Task<CallToolResult> HouseholdMembersList(
        ReadOnlyMcpTestIdentity identity,
        IQueryDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await dispatcher.Dispatch(new GetFamilyMembersQuery(
                identity.FamilyId,
                identity.RequesterUserId), cancellationToken);

            var output = new HouseholdMembersOutput(response.Select(member => new HouseholdMemberOutput(
                member.MemberId,
                string.IsNullOrWhiteSpace(member.PreferredName) ? member.Name : member.PreferredName,
                member.Role)).ToArray());

            return Success(output);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            return Error(IsAccessDenied(exception) ? "ACCESS_DENIED" : "INTERNAL_ERROR",
                IsAccessDenied(exception) ? "Access to this household is denied." : "Household members could not be read.");
        }
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
        => exception is DomusMind.Application.Features.Calendar.CalendarException calendarException &&
            calendarException.Code.ToString() == "AccessDenied"
            || exception is DomusMind.Application.Features.Family.FamilyException familyException &&
            familyException.Code.ToString() == "AccessDenied";

    private static DateOnly? ParseOptionalDate(string? value)
        => value is null ? null : DateOnly.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);

    private static DateTimeOffset? ParseOptionalInstant(string? value)
        => value is null ? null : DateTimeOffset.ParseExact(value, "O", CultureInfo.InvariantCulture);
}

internal sealed class InvariantCultureScope : IDisposable
{
    private readonly CultureInfo _originalCulture = CultureInfo.CurrentCulture;
    private readonly CultureInfo _originalUiCulture = CultureInfo.CurrentUICulture;

    public InvariantCultureScope()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
    }

    public void Dispose()
    {
        CultureInfo.CurrentCulture = _originalCulture;
        CultureInfo.CurrentUICulture = _originalUiCulture;
    }
}

public sealed record ReadOnlyMcpTestIdentity(Guid FamilyId, Guid RequesterUserId);
public sealed record AgendaOutput(IReadOnlyCollection<AgendaEventOutput> Events,
    IReadOnlyCollection<AgendaListItemOutput> ListItems, IReadOnlyCollection<AgendaNoteOutput> Notes);
public sealed record AgendaEventOutput(string Title, DateOnly Date, string? Time, DateOnly? EndDate, string? EndTime,
    string Status, IReadOnlyCollection<AgendaParticipantOutput> Participants);
public sealed record AgendaParticipantOutput(string DisplayName);
public sealed record AgendaListItemOutput(string ListName, string ItemName, DateOnly? DueDate, DateTimeOffset? Reminder,
    bool Checked, bool Importance);
public sealed record AgendaNoteOutput(string Text, DateOnly Date, DateTime CreatedAtUtc);
public sealed record HouseholdMembersOutput(IReadOnlyCollection<HouseholdMemberOutput> Members);
public sealed record HouseholdMemberOutput(Guid MemberId, string Name, string Role);
public sealed record McpErrorOutput(string Code, string Message);
