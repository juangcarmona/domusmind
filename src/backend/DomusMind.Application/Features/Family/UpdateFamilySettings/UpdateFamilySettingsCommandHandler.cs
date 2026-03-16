using DomusMind.Application.Abstractions.Languages;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Family;
using DomusMind.Contracts.Family;
using DomusMind.Domain.Family;
using DomusMind.Domain.Family.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Family.UpdateFamilySettings;

public sealed class UpdateFamilySettingsCommandHandler
    : ICommandHandler<UpdateFamilySettingsCommand, UpdateFamilySettingsResponse>
{
    private static readonly HashSet<string> ValidDaysOfWeek = new(StringComparer.OrdinalIgnoreCase)
    {
        "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday",
    };

    private static readonly HashSet<string> ValidDateFormats = new(StringComparer.Ordinal)
    {
        "dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "d/M/yyyy", "M/d/yyyy",
    };

    private readonly IDomusMindDbContext _dbContext;
    private readonly IEventLogWriter _eventLogWriter;
    private readonly IFamilyAuthorizationService _authorizationService;
    private readonly ISupportedLanguageReader _languageReader;

    public UpdateFamilySettingsCommandHandler(
        IDomusMindDbContext dbContext,
        IEventLogWriter eventLogWriter,
        IFamilyAuthorizationService authorizationService,
        ISupportedLanguageReader languageReader)
    {
        _dbContext = dbContext;
        _eventLogWriter = eventLogWriter;
        _authorizationService = authorizationService;
        _languageReader = languageReader;
    }

    public async Task<UpdateFamilySettingsResponse> Handle(
        UpdateFamilySettingsCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new FamilyException(FamilyErrorCode.InvalidInput, "Family name is required.");

        if (command.Name.Trim().Length > 100)
            throw new FamilyException(FamilyErrorCode.InvalidInput, "Family name cannot exceed 100 characters.");

        string? languageCode = null;
        if (!string.IsNullOrWhiteSpace(command.PrimaryLanguageCode))
        {
            var isValid = await _languageReader.IsActiveAsync(command.PrimaryLanguageCode, cancellationToken);
            if (!isValid)
                throw new FamilyException(
                    FamilyErrorCode.InvalidInput,
                    $"Language code '{command.PrimaryLanguageCode}' is not a supported language.");
            languageCode = command.PrimaryLanguageCode;
        }

        if (command.FirstDayOfWeek is not null && !ValidDaysOfWeek.Contains(command.FirstDayOfWeek))
            throw new FamilyException(
                FamilyErrorCode.InvalidInput,
                $"'{command.FirstDayOfWeek}' is not a valid day of week.");

        if (command.DateFormatPreference is not null && !ValidDateFormats.Contains(command.DateFormatPreference))
            throw new FamilyException(
                FamilyErrorCode.InvalidInput,
                $"'{command.DateFormatPreference}' is not a supported date format.");

        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            command.RequestedByUserId, command.FamilyId, cancellationToken);

        if (!canAccess)
            throw new FamilyException(FamilyErrorCode.AccessDenied, "Access to this family is denied.");

        var family = await _dbContext.Set<Domain.Family.Family>()
            .SingleOrDefaultAsync(f => f.Id == FamilyId.From(command.FamilyId), cancellationToken);

        if (family is null)
            throw new FamilyException(FamilyErrorCode.FamilyNotFound, "Family was not found.");

        var name = FamilyName.Create(command.Name);
        var now = DateTime.UtcNow;

        family.UpdateSettings(name, languageCode, command.FirstDayOfWeek, command.DateFormatPreference, now);

        await _eventLogWriter.WriteAsync(family.DomainEvents, cancellationToken);
        family.ClearDomainEvents();

        return new UpdateFamilySettingsResponse(
            family.Id.Value,
            family.Name.Value,
            family.PrimaryLanguageCode,
            family.FirstDayOfWeek,
            family.DateFormatPreference);
    }
}
