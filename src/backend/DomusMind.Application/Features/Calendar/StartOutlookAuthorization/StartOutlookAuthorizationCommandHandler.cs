using DomusMind.Application.Abstractions.Integrations.Calendar;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Calendar;
using DomusMind.Contracts.Calendar;
using DomusMind.Domain.Family;
using Microsoft.EntityFrameworkCore;
using FamilyEntity = DomusMind.Domain.Family.Family;

namespace DomusMind.Application.Features.Calendar.StartOutlookAuthorization;

public sealed class StartOutlookAuthorizationCommandHandler
    : ICommandHandler<StartOutlookAuthorizationCommand, OutlookAuthorizationUrlResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IFamilyAuthorizationService _authorizationService;
    private readonly IExternalCalendarAuthService _authService;

    public StartOutlookAuthorizationCommandHandler(
        IDomusMindDbContext dbContext,
        IFamilyAuthorizationService authorizationService,
        IExternalCalendarAuthService authService)
    {
        _dbContext = dbContext;
        _authorizationService = authorizationService;
        _authService = authService;
    }

    public async Task<OutlookAuthorizationUrlResponse> Handle(
        StartOutlookAuthorizationCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.RedirectUri))
            throw new CalendarException(CalendarErrorCode.InvalidInput, "OAuth redirect URI is required.");

        if (!await _authorizationService.CanAccessFamilyAsync(
                command.RequestedByUserId, command.FamilyId, cancellationToken))
        {
            throw new CalendarException(CalendarErrorCode.AccessDenied, "Access to this family is denied.");
        }

        var familyId = FamilyId.From(command.FamilyId);
        var memberId = MemberId.From(command.MemberId);
        var memberBelongsToFamily = await _dbContext.Set<FamilyEntity>()
            .AsNoTracking()
            .AnyAsync(family => family.Id == familyId && family.Members.Any(member => member.Id == memberId), cancellationToken);

        if (!memberBelongsToFamily)
            throw new CalendarException(CalendarErrorCode.AccessDenied, "The requested member does not belong to this family.");

        var authUrl = await _authService.CreateAuthorizationUrlAsync(
            command.FamilyId,
            command.MemberId,
            command.RequestedByUserId,
            command.RedirectUri,
            cancellationToken);

        if (authUrl is null)
            throw new CalendarException(CalendarErrorCode.ProviderNotConfigured, "Outlook integration is not configured.");

        return new OutlookAuthorizationUrlResponse(authUrl);
    }
}
