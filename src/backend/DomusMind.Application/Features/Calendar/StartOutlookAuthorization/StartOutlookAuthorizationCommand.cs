using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Calendar;

namespace DomusMind.Application.Features.Calendar.StartOutlookAuthorization;

public sealed record StartOutlookAuthorizationCommand(
    Guid FamilyId,
    Guid MemberId,
    string RedirectUri,
    Guid RequestedByUserId)
    : ICommand<OutlookAuthorizationUrlResponse>;
