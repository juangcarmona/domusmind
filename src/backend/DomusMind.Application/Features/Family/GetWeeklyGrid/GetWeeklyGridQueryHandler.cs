using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Family;
using DomusMind.Domain.Calendar;
using DomusMind.Domain.Family;
using DomusMind.Domain.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Family.GetWeeklyGrid;

public sealed class GetWeeklyGridQueryHandler
    : IQueryHandler<GetWeeklyGridQuery, WeeklyGridResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IFamilyAuthorizationService _authorizationService;

    public GetWeeklyGridQueryHandler(
        IDomusMindDbContext dbContext,
        IFamilyAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _authorizationService = authorizationService;
    }

    public async Task<WeeklyGridResponse> Handle(
        GetWeeklyGridQuery query,
        CancellationToken cancellationToken)
    {
        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            query.RequestedByUserId, query.FamilyId, cancellationToken);
        if (!canAccess)
            throw new FamilyException(FamilyErrorCode.AccessDenied, "Access to this family is denied.");

        var familyId = FamilyId.From(query.FamilyId);

        // .Date strips Kind → always re-apply Utc before using in EF Core queries
        // against PostgreSQL timestamptz columns which reject DateTimeKind.Unspecified.
        var weekStart = DateTime.SpecifyKind(query.WeekStart.Date, DateTimeKind.Utc);
        var weekEnd = weekStart.AddDays(7); // AddDays preserves Kind

        // Load members
        var family = await _dbContext.Set<Domain.Family.Family>()
            .AsNoTracking()
            .Include(f => f.Members)
            .SingleOrDefaultAsync(f => f.Id == familyId, cancellationToken);

        if (family is null)
            throw new FamilyException(FamilyErrorCode.FamilyNotFound, "Family not found.");

        // Load calendar events in the week window (non-cancelled)
        var events = await _dbContext.Set<CalendarEvent>()
            .AsNoTracking()
            .Where(e => e.FamilyId == familyId
                     && e.StartTime >= weekStart
                     && e.StartTime < weekEnd
                     && e.Status != EventStatus.Cancelled)
            .ToListAsync(cancellationToken);

        // Load tasks with due dates in the week window (active only)
        var tasks = await _dbContext.Set<HouseholdTask>()
            .AsNoTracking()
            .Where(t => t.FamilyId == familyId
                     && t.DueDate.HasValue
                     && t.DueDate.Value >= weekStart
                     && t.DueDate.Value < weekEnd
                     && t.Status == Domain.Tasks.TaskStatus.Pending)
            .ToListAsync(cancellationToken);

        // Load active routines
        var routines = await _dbContext.Set<Routine>()
            .AsNoTracking()
            .Where(r => r.FamilyId == familyId && r.Status == RoutineStatus.Active)
            .ToListAsync(cancellationToken);

        // Build 7 day dates
        var days = Enumerable.Range(0, 7)
            .Select(i => weekStart.AddDays(i))
            .ToList();

        // Build member rows
        var memberRows = family.Members
            .Select(member =>
            {
                var cells = days
                    .Select(day =>
                    {
                        var memberEvents = events
                            .Where(e => e.StartTime.Date == day
                                     && e.ParticipantIds.Any(p => p.Value == member.Id.Value))
                            .Select(e => new WeeklyGridEventItem(
                                e.Id.Value,
                                e.Title.Value,
                                e.StartTime,
                                e.EndTime,
                                e.Status.ToString()))
                            .ToList();

                        var memberTasks = tasks
                            .Where(t => t.AssigneeId?.Value == member.Id.Value
                                     && t.DueDate.HasValue
                                     && t.DueDate.Value.Date == day)
                            .Select(t => new WeeklyGridTaskItem(
                                t.Id.Value,
                                t.Title.Value,
                                t.DueDate,
                                t.Status.ToString()))
                            .ToList();

                        return new WeeklyGridCell(day, memberEvents, memberTasks);
                    })
                    .ToList();

                return new WeeklyGridMember(
                    member.Id.Value,
                    member.Name.Value,
                    member.Role.Value,
                    cells);
            })
            .ToList();

        // Build routine items (family-wide, not member-specific)
        var routineItems = routines
            .Select(r => new WeeklyGridRoutineItem(
                r.Id.Value,
                r.Name.Value,
                r.Cadence,
                r.Status.ToString()))
            .ToList();

        return new WeeklyGridResponse(
            weekStart,
            weekEnd.AddDays(-1),
            memberRows,
            routineItems);
    }
}
