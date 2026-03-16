using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Family;

namespace DomusMind.Application.Features.Family.UpdateFamilySettings;

public sealed record UpdateFamilySettingsCommand(
    Guid FamilyId,
    Guid RequestedByUserId,
    string Name,
    string? PrimaryLanguageCode,
    string? FirstDayOfWeek,
    string? DateFormatPreference) : ICommand<UpdateFamilySettingsResponse>;
