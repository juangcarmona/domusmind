using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Responsibilities;
using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Responsibilities;
using DomusMind.Domain.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Responsibilities.ArchiveResponsibilityDomain;

public sealed class ArchiveResponsibilityDomainCommandHandler
    : ICommandHandler<ArchiveResponsibilityDomainCommand, ArchiveResponsibilityDomainResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IEventLogWriter _eventLogWriter;
    private readonly IFamilyAuthorizationService _authorizationService;

    public ArchiveResponsibilityDomainCommandHandler(
        IDomusMindDbContext dbContext,
        IEventLogWriter eventLogWriter,
        IFamilyAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _eventLogWriter = eventLogWriter;
        _authorizationService = authorizationService;
    }

    public async Task<ArchiveResponsibilityDomainResponse> Handle(
        ArchiveResponsibilityDomainCommand command,
        CancellationToken cancellationToken)
    {
        var area = await _dbContext.Set<ResponsibilityDomain>()
            .SingleOrDefaultAsync(d => d.Id == ResponsibilityDomainId.From(command.ResponsibilityDomainId), cancellationToken);
        if (area is null)
            throw new ResponsibilitiesException(ResponsibilitiesErrorCode.ResponsibilityDomainNotFound, "Area was not found.");

        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            command.RequestedByUserId, area.FamilyId.Value, cancellationToken);
        if (!canAccess)
            throw new ResponsibilitiesException(ResponsibilitiesErrorCode.AccessDenied, "Access to this family is denied.");

        if (area.IsArchived)
            throw new ResponsibilitiesException(ResponsibilitiesErrorCode.AlreadyArchived, "Area is already archived.");

        var areaId = area.Id;
        var tasks = await _dbContext.Set<HouseholdTask>()
            .Where(t => t.FamilyId == area.FamilyId && t.AreaId == areaId)
            .ToListAsync(cancellationToken);
        var routines = await _dbContext.Set<Routine>()
            .Where(r => r.FamilyId == area.FamilyId && r.AreaId == areaId)
            .ToListAsync(cancellationToken);

        area.Archive();
        foreach (var task in tasks) task.ClearArea();
        foreach (var routine in routines) routine.ClearArea();

        var events = new List<IDomainEvent>();
        events.AddRange(area.DomainEvents);
        events.AddRange(tasks.SelectMany(task => task.DomainEvents));
        events.AddRange(routines.SelectMany(routine => routine.DomainEvents));
        await _eventLogWriter.WriteAsync(events, cancellationToken);

        area.ClearDomainEvents();
        foreach (var task in tasks) task.ClearDomainEvents();
        foreach (var routine in routines) routine.ClearDomainEvents();

        return new ArchiveResponsibilityDomainResponse(area.Id.Value);
    }
}
