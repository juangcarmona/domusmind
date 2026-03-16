namespace DomusMind.Contracts.Family;

public sealed record UpdateFamilySettingsRequest(
    string Name,
    string? PrimaryLanguageCode,
    string? FirstDayOfWeek,
    string? DateFormatPreference);
