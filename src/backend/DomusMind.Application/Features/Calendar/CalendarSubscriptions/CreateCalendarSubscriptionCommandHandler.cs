using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Domain.Calendar;
using DomusMind.Domain.Family;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Calendar.CalendarSubscriptions;

public sealed class CreateCalendarSubscriptionCommandHandler(
    IDomusMindDbContext db,
    IEventLogWriter eventLog,
    IFamilyAuthorizationService authorization,
    ICalendarSubscriptionTokenGenerator tokens,
    TimeProvider clock)
    : ICommandHandler<CreateCalendarSubscriptionCommand, CreatedCalendarSubscription>
{
    public async Task<CreatedCalendarSubscription> Handle(CreateCalendarSubscriptionCommand command,
        CancellationToken cancellationToken)
    {
        if (!await authorization.CanAccessFamilyAsync(command.RequestedByUserId, command.FamilyId, cancellationToken))
            throw new UnauthorizedAccessException("Access to this family is denied.");
        if (string.IsNullOrWhiteSpace(command.Label) || command.Label.Trim().Length > 100)
            throw new ArgumentException("Label must contain between 1 and 100 characters.");
        if (command.MemberId.HasValue && !await db.Set<Domain.Family.Family>().AsNoTracking()
                .Where(f => f.Id == FamilyId.From(command.FamilyId))
                .SelectMany(f => f.Members).AnyAsync(m => m.Id == MemberId.From(command.MemberId.Value), cancellationToken))
            throw new ArgumentException("The selected member does not belong to this household.");

        var (token, hash) = tokens.Create();
        var subscription = CalendarSubscription.Create(command.FamilyId, command.RequestedByUserId,
            command.Label, command.MemberId, hash, clock.GetUtcNow().UtcDateTime);
        db.Set<CalendarSubscription>().Add(subscription);
        await eventLog.WriteAsync([subscription], cancellationToken);
        return new(subscription.Id.Value, subscription.Label, subscription.MemberId, subscription.ExpiresAtUtc, token);
    }
}
