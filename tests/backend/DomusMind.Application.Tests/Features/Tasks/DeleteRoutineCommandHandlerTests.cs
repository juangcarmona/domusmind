using DomusMind.Application.Features.Tasks;
using DomusMind.Application.Features.Tasks.DeleteRoutine;
using DomusMind.Domain.Family;
using DomusMind.Domain.Shared;
using DomusMind.Domain.Tasks;
using DomusMind.Domain.Tasks.Enums;
using DomusMind.Domain.Tasks.Events;
using DomusMind.Domain.Tasks.ValueObjects;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.Features.Tasks;

public sealed class DeleteRoutineCommandHandlerTests
{
    private static DomusMindDbContext CreateDb()
        => new(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static Routine CreateRoutine(FamilyId familyId)
        => Routine.Create(
            RoutineId.New(), familyId, RoutineName.Create("Weekly cleanup"),
            RoutineScope.Household, RoutineKind.Scheduled, HexColor.From("#3B82F6"),
            RoutineSchedule.Weekly([DayOfWeek.Monday]), Array.Empty<MemberId>());

    [Fact]
    public async Task Handle_RemovesRoutineAndLogsDeletionEvent()
    {
        var db = CreateDb();
        var routine = CreateRoutine(FamilyId.New());
        db.Set<Routine>().Add(routine);
        await db.SaveChangesAsync();
        routine.ClearDomainEvents();
        var handler = new DeleteRoutineCommandHandler(
            db, new EventLogWriter(db), new StubTasksAuthorizationService());

        var result = await handler.Handle(
            new DeleteRoutineCommand(routine.Id.Value, Guid.NewGuid()), CancellationToken.None);

        result.RoutineId.Should().Be(routine.Id.Value);
        (await db.Set<Routine>().AnyAsync(r => r.Id == routine.Id)).Should().BeFalse();
        db.EventLog.Should().Contain(e => e.EventType == nameof(RoutineDeleted));
    }

    [Fact]
    public async Task Handle_AccessDenied_LeavesRoutinePresent()
    {
        var db = CreateDb();
        var routine = CreateRoutine(FamilyId.New());
        db.Set<Routine>().Add(routine);
        await db.SaveChangesAsync();
        var handler = new DeleteRoutineCommandHandler(
            db, new EventLogWriter(db), new StubTasksAuthorizationService { CanAccess = false });

        var act = () => handler.Handle(
            new DeleteRoutineCommand(routine.Id.Value, Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<TasksException>()
            .Where(e => e.Code == TasksErrorCode.AccessDenied);
        (await db.Set<Routine>().AnyAsync(r => r.Id == routine.Id)).Should().BeTrue();
        db.EventLog.Should().BeEmpty();
    }
}
