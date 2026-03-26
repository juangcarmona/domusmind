using DomusMind.Application.Features.SharedLists;
using DomusMind.Application.Features.SharedLists.RemoveSharedListItem;
using DomusMind.Domain.Family;
using DomusMind.Domain.SharedLists;
using DomusMind.Domain.SharedLists.ValueObjects;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.Features.SharedLists;

public sealed class RemoveSharedListItemCommandHandlerTests
{
    private static DomusMindDbContext CreateDb()
        => new(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static RemoveSharedListItemCommandHandler BuildHandler(
        DomusMindDbContext? db = null,
        StubSharedListAuthorizationService? auth = null)
    {
        var context = db ?? CreateDb();
        return new RemoveSharedListItemCommandHandler(
            context, new EventLogWriter(context),
            auth ?? new StubSharedListAuthorizationService());
    }

    private static async Task<(DomusMindDbContext db, SharedList list, SharedListItemId itemId)> SeedListWithItemAsync()
    {
        var db = CreateDb();
        var familyId = FamilyId.New();
        var list = SharedListTestHelpers.MakeList(familyId);
        var itemId = SharedListItemId.New();
        list.AddItem(itemId, SharedListItemName.Create("Remove Me"), null, null, DateTime.UtcNow);
        list.ClearDomainEvents();
        db.Set<SharedList>().Add(list);
        await db.SaveChangesAsync();
        return (db, list, itemId);
    }

    [Fact]
    public async Task Handle_RemovesItemFromList()
    {
        var (db, list, itemId) = await SeedListWithItemAsync();
        var handler = BuildHandler(db: db);

        var result = await handler.Handle(
            new RemoveSharedListItemCommand(list.Id.Value, itemId.Value, Guid.NewGuid()),
            CancellationToken.None);

        result.Should().BeTrue();
        var reloaded = await db.Set<SharedList>()
            .Include(l => l.Items)
            .SingleAsync(l => l.Id == list.Id);
        reloaded.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithUnknownItemId_ThrowsItemNotFound()
    {
        var (db, list, _) = await SeedListWithItemAsync();
        var handler = BuildHandler(db: db);

        var act = async () => await handler.Handle(
            new RemoveSharedListItemCommand(list.Id.Value, Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<SharedListException>()
            .Where(ex => ex.Code == SharedListErrorCode.ItemNotFound);
    }

    [Fact]
    public async Task Handle_WithUnknownListId_ThrowsListNotFound()
    {
        var handler = BuildHandler();

        var act = async () => await handler.Handle(
            new RemoveSharedListItemCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<SharedListException>()
            .Where(ex => ex.Code == SharedListErrorCode.ListNotFound);
    }

    [Fact]
    public async Task Handle_RemainingItemsNotAffected()
    {
        var db = CreateDb();
        var familyId = FamilyId.New();
        var list = SharedListTestHelpers.MakeList(familyId);
        var id1 = SharedListItemId.New();
        var id2 = SharedListItemId.New();
        list.AddItem(id1, SharedListItemName.Create("Keep"), null, null, DateTime.UtcNow);
        list.AddItem(id2, SharedListItemName.Create("Remove"), null, null, DateTime.UtcNow);
        list.ClearDomainEvents();
        db.Set<SharedList>().Add(list);
        await db.SaveChangesAsync();
        var handler = BuildHandler(db: db);

        await handler.Handle(
            new RemoveSharedListItemCommand(list.Id.Value, id2.Value, Guid.NewGuid()),
            CancellationToken.None);

        var reloaded = await db.Set<SharedList>()
            .Include(l => l.Items)
            .SingleAsync(l => l.Id == list.Id);
        reloaded.Items.Should().HaveCount(1);
        reloaded.Items.Should().Contain(i => i.Name.Value == "Keep");
    }
}
