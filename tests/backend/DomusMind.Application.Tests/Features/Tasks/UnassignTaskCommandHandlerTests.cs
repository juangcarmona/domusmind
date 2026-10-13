using DomusMind.Application.Features.Tasks;
using DomusMind.Application.Features.Tasks.UnassignTask;
using DomusMind.Domain.Family;
using DomusMind.Domain.Tasks;
using DomusMind.Domain.Tasks.Events;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.Features.Tasks;

public sealed class UnassignTaskCommandHandlerTests
{
    private static DomusMindDbContext CreateDb()
        => new(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    [Fact]
    public async Task Handle_UnassignsTaskAndWritesEvent()
    {
        var db = CreateDb();
        var familyId = FamilyId.New();
        var assigneeId = DomusMind.Domain.Family.MemberId.New();
        var task = TaskTestHelpers.MakeTask(familyId, "Clean Kitchen", new DateOnly(2026, 4, 1));
        task.Assign(assigneeId);
        task.ClearDomainEvents();
        db.Set<HouseholdTask>().Add(task);
        await db.SaveChangesAsync();
        var handler = new UnassignTaskCommandHandler(
            db, new EventLogWriter(db), new StubTasksAuthorizationService());

        var result = await handler.Handle(
            new UnassignTaskCommand(task.Id.Value, Guid.NewGuid()), CancellationToken.None);

        result.TaskId.Should().Be(task.Id.Value);
        result.PreviousAssigneeId.Should().Be(assigneeId.Value);
        (await db.Set<HouseholdTask>().SingleAsync(t => t.Id == task.Id)).AssigneeId.Should().BeNull();
        db.EventLog.Should().Contain(e => e.EventType == nameof(TaskUnassigned));
    }

    [Fact]
    public async Task Handle_UnassignedTask_ReturnsInvalidInput()
    {
        var db = CreateDb();
        var task = TaskTestHelpers.MakeTask(FamilyId.New(), "Clean Kitchen", new DateOnly(2026, 4, 1));
        db.Set<HouseholdTask>().Add(task);
        await db.SaveChangesAsync();
        var handler = new UnassignTaskCommandHandler(
            db, new EventLogWriter(db), new StubTasksAuthorizationService());

        var act = () => handler.Handle(
            new UnassignTaskCommand(task.Id.Value, Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<TasksException>()
            .Where(e => e.Code == TasksErrorCode.InvalidInput);
        (await db.Set<HouseholdTask>().SingleAsync(t => t.Id == task.Id)).AssigneeId.Should().BeNull();
    }
}
