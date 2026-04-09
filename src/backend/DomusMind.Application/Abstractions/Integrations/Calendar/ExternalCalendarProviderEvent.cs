namespace DomusMind.Application.Abstractions.Integrations.Calendar;

/// <summary>
/// Represents an event occurrence returned from a provider calendar.
/// </summary>
public sealed record ExternalCalendarProviderEvent(
    string ExternalEventId,
    string? ICalUId,
    string? SeriesMasterId,
    string Title,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    bool IsAllDay,
    string? Location,
    string? ParticipantSummaryJson,
    string Status,
    string? OpenInProviderUrl,
    DateTime? ProviderModifiedAtUtc,
    bool IsDeleted);
