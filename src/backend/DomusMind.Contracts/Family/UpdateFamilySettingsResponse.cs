namespace DomusMind.Contracts.Family;

public sealed record UpdateFamilySettingsResponse(
    Guid FamilyId,
    string Name,
    string? PrimaryLanguageCode,
    string? FirstDayOfWeek,
    string? DateFormatPreference);
