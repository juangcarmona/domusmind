using DomusMind.Application.Features.Lists;
using DomusMind.Application.Features.Lists.SetItemTemporal;
using DomusMind.Domain.Family;
using DomusMind.Domain.Lists;
using DomusMind.Domain.Lists.ValueObjects;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.Features.Lists;

public sealed class SetItemTemporalCommandHandlerTests
{
    [Fact]
    public async Task Handle_ClearingOneField_PreservesOtherFields()
    {
        var db = new DomusMindDbContext(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
        var list = ListTestHelpers.MakeList(FamilyId.New());
        var itemId = ListItemId.New();
        var dueDate = new DateOnly(2026, 4, 11);
        var reminder = DateTimeOffset.UtcNow.AddDays(1);
        list.AddItem(itemId, ListItemName.Create("Task"), null, null, DateTime.UtcNow);
        list.SetItemTemporal(itemId, dueDate, reminder, "weekly", DateTime.UtcNow);
        list.ClearDomainEvents();
        db.Set<SharedList>().Add(list);
        await db.SaveChangesAsync();

        var handler = new SetItemTemporalCommandHandler(
            db, new EventLogWriter(db), new StubListAuthorizationService());
        var result = await handler.Handle(
            new SetItemTemporalCommand(
                list.Id.Value, itemId.Value, null, null, null, Guid.NewGuid(), ClearDueDate: true),
            CancellationToken.None);

        result.DueDate.Should().BeNull();
        result.Reminder.Should().Be(reminder);
        result.Repeat.Should().Be("weekly");
        list.Items.Single().HasTemporalData.Should().BeTrue();
    }
}
