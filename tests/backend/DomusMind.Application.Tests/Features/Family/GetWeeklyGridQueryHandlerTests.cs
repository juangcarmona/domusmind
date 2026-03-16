using DomusMind.Application.Features.Family;
using DomusMind.Application.Features.Family.GetWeeklyGrid;
using DomusMind.Domain.Calendar;
using DomusMind.Domain.Calendar.ValueObjects;
using DomusMind.Domain.Family;
using DomusMind.Domain.Family.ValueObjects;
using DomusMind.Domain.Tasks;
using DomusMind.Domain.Tasks.ValueObjects;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.Features.Family;

public sealed class GetWeeklyGridQueryHandlerTests
{
    private static DomusMindDbContext CreateDb()
        => new(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static GetWeeklyGridQueryHandler BuildHandler(
        DomusMindDbContext db,
        StubFamilyTimelineAuthorizationService? auth = null)
        => new(db, auth ?? new StubFamilyTimelineAuthorizationService());

    private static Domain.Family.Family MakeFamily(FamilyId familyId, params (MemberId Id, string Name)[] members)
    {
        var family = Domain.Family.Family.Create(
            familyId, FamilyName.Create("Test Family"), null, DateTime.UtcNow);
        foreach (var (id, name) in members)
            family.AddMember(id, MemberName.Create(name), MemberRole.Create("Adult"), DateTime.UtcNow);
        family.ClearDomainEvents();
        return family;
    }

    private static Domain.Calendar.CalendarEvent MakeEvent(
        FamilyId familyId,
        string title,
        DateTime startTime,
        MemberId? participant = null)
    {
        var evt = Domain.Calendar.CalendarEvent.Create(
            CalendarEventId.New(), familyId,
            EventTitle.Create(title), null,
            startTime, null, DateTime.UtcNow);
        if (participant is not null)
            evt.AddParticipant(participant.Value);
        return evt;
    }

    private static HouseholdTask MakeTask(
        FamilyId familyId,
        string title,
        DateTime dueDate,
        MemberId? assignee = null)
    {
        var task = HouseholdTask.Create(
            TaskId.New(), familyId,
            TaskTitle.Create(title), null,
            dueDate, DateTime.UtcNow);
        if (assignee is not null)
            task.Assign(assignee.Value);
        return task;
    }

    private static Routine MakeRoutine(FamilyId familyId, string name)
        => Routine.Create(RoutineId.New(), familyId, RoutineName.Create(name), "Daily", DateTime.UtcNow);

    // ---- Authorization / guarding ----

    [Fact]
    public async Task Handle_AccessDenied_ThrowsFamilyException()
    {
        var db = CreateDb();
        var auth = new StubFamilyTimelineAuthorizationService { CanAccess = false };
        var handler = BuildHandler(db, auth);

        var act = () => handler.Handle(
            new GetWeeklyGridQuery(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<FamilyException>()
            .Where(e => e.Code == FamilyErrorCode.AccessDenied);
    }

    [Fact]
    public async Task Handle_FamilyNotFound_ThrowsFamilyException()
    {
        var db = CreateDb();
        var handler = BuildHandler(db);

        var act = () => handler.Handle(
            new GetWeeklyGridQuery(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<FamilyException>()
            .Where(e => e.Code == FamilyErrorCode.FamilyNotFound);
    }

    // ---- UTC normalization ----

    [Fact]
    public async Task Handle_WeekStartWithUnspecifiedKind_NormalizesToUtcAndExecutesQuery()
    {
        // Arrange — DateTimeKind.Unspecified is what ASP.NET Core / DateTime.Date can produce
        var unspecified = DateTime.SpecifyKind(new DateTime(2026, 3, 16), DateTimeKind.Unspecified);
        unspecified.Kind.Should().Be(DateTimeKind.Unspecified);

        var db = CreateDb();
        var familyId = FamilyId.New();
        db.Set<Domain.Family.Family>().Add(MakeFamily(familyId));
        await db.SaveChangesAsync();
        var handler = BuildHandler(db);

        // Act — must not throw even though the input Kind is Unspecified
        // (the handler normalises it to UTC before querying)
        var result = await handler.Handle(
            new GetWeeklyGridQuery(familyId.Value, unspecified, Guid.NewGuid()),
            CancellationToken.None);

        // Assert — response carries UTC values
        result.WeekStart.Kind.Should().Be(DateTimeKind.Utc);
        result.WeekEnd.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public async Task Handle_WeekStartAlreadyUtc_RemainsUtc()
    {
        var utcDate = new DateTime(2026, 3, 16, 0, 0, 0, DateTimeKind.Utc);
        var db = CreateDb();
        var familyId = FamilyId.New();
        db.Set<Domain.Family.Family>().Add(MakeFamily(familyId));
        await db.SaveChangesAsync();
        var handler = BuildHandler(db);

        var result = await handler.Handle(
            new GetWeeklyGridQuery(familyId.Value, utcDate, Guid.NewGuid()),
            CancellationToken.None);

        result.WeekStart.Should().Be(utcDate);
        result.WeekStart.Kind.Should().Be(DateTimeKind.Utc);
    }

    // ---- Omitted / defaulted weekStart ----

    [Fact]
    public async Task Handle_WhenWeekStartIsCurrentWeek_ReturnsSevenDayCells()
    {
        var db = CreateDb();
        var familyId = FamilyId.New();
        var memberId = MemberId.New();
        db.Set<Domain.Family.Family>().Add(MakeFamily(familyId, (memberId, "Alice")));
        await db.SaveChangesAsync();
        var handler = BuildHandler(db);

        var weekStart = DateTime.SpecifyKind(DateTime.UtcNow.Date, DateTimeKind.Utc);
        var result = await handler.Handle(
            new GetWeeklyGridQuery(familyId.Value, weekStart, Guid.NewGuid()),
            CancellationToken.None);

        result.Members.Should().ContainSingle();
        result.Members.First().Cells.Should().HaveCount(7);
    }

    // ---- Event inclusion in range ----

    [Fact]
    public async Task Handle_EventInsideWeekWindow_AppearsInMemberCell()
    {
        var db = CreateDb();
        var familyId = FamilyId.New();
        var memberId = MemberId.New();
        db.Set<Domain.Family.Family>().Add(MakeFamily(familyId, (memberId, "Bob")));

        var weekStart = new DateTime(2026, 3, 16, 0, 0, 0, DateTimeKind.Utc);
        var eventTime = weekStart.AddDays(2).AddHours(9); // Wednesday 09:00 UTC
        db.Set<Domain.Calendar.CalendarEvent>().Add(MakeEvent(familyId, "School Run", eventTime, memberId));
        await db.SaveChangesAsync();
        var handler = BuildHandler(db);

        var result = await handler.Handle(
            new GetWeeklyGridQuery(familyId.Value, weekStart, Guid.NewGuid()),
            CancellationToken.None);

        var wednesdayCell = result.Members.First().Cells.Single(c => c.Date.Date == eventTime.Date);
        wednesdayCell.Events.Should().ContainSingle(e => e.Title == "School Run");
    }

    [Fact]
    public async Task Handle_EventOutsideWeekWindow_IsExcluded()
    {
        var db = CreateDb();
        var familyId = FamilyId.New();
        var memberId = MemberId.New();
        db.Set<Domain.Family.Family>().Add(MakeFamily(familyId, (memberId, "Carol")));

        var weekStart = new DateTime(2026, 3, 16, 0, 0, 0, DateTimeKind.Utc);
        var outsideEvent = weekStart.AddDays(8); // beyond the window
        db.Set<Domain.Calendar.CalendarEvent>().Add(MakeEvent(familyId, "Future Event", outsideEvent, memberId));
        await db.SaveChangesAsync();
        var handler = BuildHandler(db);

        var result = await handler.Handle(
            new GetWeeklyGridQuery(familyId.Value, weekStart, Guid.NewGuid()),
            CancellationToken.None);

        result.Members.First().Cells.SelectMany(c => c.Events).Should().BeEmpty();
    }

    // ---- Task inclusion in range ----

    [Fact]
    public async Task Handle_TaskDueInsideWeekWindow_AppearsInMemberCell()
    {
        var db = CreateDb();
        var familyId = FamilyId.New();
        var memberId = MemberId.New();
        db.Set<Domain.Family.Family>().Add(MakeFamily(familyId, (memberId, "Dan")));

        var weekStart = new DateTime(2026, 3, 16, 0, 0, 0, DateTimeKind.Utc);
        var dueDate = weekStart.AddDays(1); // Tuesday
        db.Set<HouseholdTask>().Add(MakeTask(familyId, "Take out bins", dueDate, memberId));
        await db.SaveChangesAsync();
        var handler = BuildHandler(db);

        var result = await handler.Handle(
            new GetWeeklyGridQuery(familyId.Value, weekStart, Guid.NewGuid()),
            CancellationToken.None);

        var tuesdayCell = result.Members.First().Cells.Single(c => c.Date.Date == dueDate.Date);
        tuesdayCell.Tasks.Should().ContainSingle(t => t.Title == "Take out bins");
    }

    [Fact]
    public async Task Handle_TaskDueOutsideWeekWindow_IsExcluded()
    {
        var db = CreateDb();
        var familyId = FamilyId.New();
        var memberId = MemberId.New();
        db.Set<Domain.Family.Family>().Add(MakeFamily(familyId, (memberId, "Eve")));

        var weekStart = new DateTime(2026, 3, 16, 0, 0, 0, DateTimeKind.Utc);
        var pastTask = weekStart.AddDays(-1); // before the window
        db.Set<HouseholdTask>().Add(MakeTask(familyId, "Old task", pastTask, memberId));
        await db.SaveChangesAsync();
        var handler = BuildHandler(db);

        var result = await handler.Handle(
            new GetWeeklyGridQuery(familyId.Value, weekStart, Guid.NewGuid()),
            CancellationToken.None);

        result.Members.First().Cells.SelectMany(c => c.Tasks).Should().BeEmpty();
    }

    // ---- Routines ----

    [Fact]
    public async Task Handle_ActiveRoutine_AppearsInRoutinesCollection()
    {
        var db = CreateDb();
        var familyId = FamilyId.New();
        db.Set<Domain.Family.Family>().Add(MakeFamily(familyId));
        db.Set<Routine>().Add(MakeRoutine(familyId, "Morning Walk"));
        await db.SaveChangesAsync();
        var handler = BuildHandler(db);

        var result = await handler.Handle(
            new GetWeeklyGridQuery(familyId.Value, DateTime.UtcNow, Guid.NewGuid()),
            CancellationToken.None);

        result.Routines.Should().ContainSingle(r => r.Name == "Morning Walk");
    }

    // ---- Response shape ----

    [Fact]
    public async Task Handle_WeekBoundaries_AreCorrect()
    {
        var db = CreateDb();
        var familyId = FamilyId.New();
        db.Set<Domain.Family.Family>().Add(MakeFamily(familyId));
        await db.SaveChangesAsync();
        var handler = BuildHandler(db);

        var weekStart = new DateTime(2026, 3, 16, 0, 0, 0, DateTimeKind.Utc);
        var result = await handler.Handle(
            new GetWeeklyGridQuery(familyId.Value, weekStart, Guid.NewGuid()),
            CancellationToken.None);

        result.WeekStart.Should().Be(weekStart);
        result.WeekEnd.Should().Be(weekStart.AddDays(6));
    }

    [Fact]
    public async Task Handle_EmptyFamily_ReturnsMemberRowsWithEmptyCells()
    {
        var db = CreateDb();
        var familyId = FamilyId.New();
        var memberId = MemberId.New();
        db.Set<Domain.Family.Family>().Add(MakeFamily(familyId, (memberId, "Fred")));
        await db.SaveChangesAsync();
        var handler = BuildHandler(db);

        var result = await handler.Handle(
            new GetWeeklyGridQuery(familyId.Value, DateTime.UtcNow, Guid.NewGuid()),
            CancellationToken.None);

        result.Members.Should().ContainSingle(m => m.Name == "Fred");
        result.Members.First().Cells.Should().AllSatisfy(c =>
        {
            c.Events.Should().BeEmpty();
            c.Tasks.Should().BeEmpty();
        });
    }
}
