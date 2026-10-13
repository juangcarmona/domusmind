using DomusMind.Application.Features.Responsibilities;
using DomusMind.Application.Features.Responsibilities.ArchiveResponsibilityDomain;
using DomusMind.Domain.Family;
using DomusMind.Domain.Responsibilities;
using DomusMind.Domain.Responsibilities.ValueObjects;
using DomusMind.Domain.Shared;
using DomusMind.Domain.Tasks;
using DomusMind.Domain.Tasks.Enums;
using DomusMind.Domain.Tasks.Events;
using DomusMind.Domain.Tasks.ValueObjects;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.Features.Responsibilities;

public sealed class ArchiveResponsibilityDomainCommandHandlerTests
{
    private static DomusMindDbContext CreateDb()
        => new(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    [Fact]
    public async Task Handle_ArchivesAreaAndClearsTaskAndRoutineReferences()
    {
        var db = CreateDb();
        var familyId = FamilyId.New();
        var area = ResponsibilityDomain.Create(
            ResponsibilityDomainId.New(), familyId, ResponsibilityAreaName.Create("Home"),
            HexColor.From("#6A4C93"), DateTime.UtcNow);
        var task = HouseholdTask.Create(
            TaskId.New(), familyId, TaskTitle.Create("Clean kitchen"), null,
            TaskSchedule.NoSchedule(), HexColor.From("#3B82F6"), area.Id);
        var routine = Routine.Create(
            RoutineId.New(), familyId, RoutineName.Create("Weekly cleanup"),
            RoutineScope.Household, RoutineKind.Scheduled, HexColor.From("#3B82F6"),
            RoutineSchedule.Weekly([DayOfWeek.Monday]), area.Id, Array.Empty<MemberId>());
        db.Set<ResponsibilityDomain>().Add(area);
        db.Set<HouseholdTask>().Add(task);
        db.Set<Routine>().Add(routine);
        await db.SaveChangesAsync();
        area.ClearDomainEvents();
        task.ClearDomainEvents();
        routine.ClearDomainEvents();
        var handler = new ArchiveResponsibilityDomainCommandHandler(
            db, new EventLogWriter(db), new StubResponsibilitiesAuthorizationService());

        var result = await handler.Handle(
            new ArchiveResponsibilityDomainCommand(area.Id.Value, Guid.NewGuid()), CancellationToken.None);

        result.ResponsibilityDomainId.Should().Be(area.Id.Value);
        (await db.Set<ResponsibilityDomain>().SingleAsync(d => d.Id == area.Id)).IsArchived.Should().BeTrue();
        (await db.Set<HouseholdTask>().SingleAsync(t => t.Id == task.Id)).AreaId.Should().BeNull();
        (await db.Set<Routine>().SingleAsync(r => r.Id == routine.Id)).AreaId.Should().BeNull();
        db.EventLog.Select(e => e.EventType).Should().Contain([
            "ResponsibilityDomainArchived", nameof(TaskAreaCleared), nameof(RoutineAreaCleared)]);
    }

    [Fact]
    public async Task Handle_AccessDenied_DoesNotChangeAreaOrLinkedWork()
    {
        var db = CreateDb();
        var familyId = FamilyId.New();
        var area = ResponsibilityDomain.Create(
            ResponsibilityDomainId.New(), familyId, ResponsibilityAreaName.Create("Home"),
            HexColor.From("#6A4C93"), DateTime.UtcNow);
        db.Set<ResponsibilityDomain>().Add(area);
        await db.SaveChangesAsync();
        area.ClearDomainEvents();
        var handler = new ArchiveResponsibilityDomainCommandHandler(
            db, new EventLogWriter(db), new StubResponsibilitiesAuthorizationService { CanAccess = false });

        var act = () => handler.Handle(
            new ArchiveResponsibilityDomainCommand(area.Id.Value, Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<ResponsibilitiesException>()
            .Where(e => e.Code == ResponsibilitiesErrorCode.AccessDenied);
        (await db.Set<ResponsibilityDomain>().SingleAsync(d => d.Id == area.Id)).IsArchived.Should().BeFalse();
        db.EventLog.Should().BeEmpty();
    }
}
