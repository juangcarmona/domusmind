namespace DomusMind.Application.Abstractions.Integrations.Calendar;

/// <summary>
/// Represents the result of a provider calendar list discovery.
/// </summary>
public sealed record ExternalCalendarProviderCalendar(
    string CalendarId,
    string CalendarName,
    bool IsDefault);
