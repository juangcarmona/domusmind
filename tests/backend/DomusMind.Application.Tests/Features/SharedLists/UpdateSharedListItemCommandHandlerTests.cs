using DomusMind.Application.Features.SharedLists;
using DomusMind.Application.Features.SharedLists.UpdateSharedListItem;
using DomusMind.Domain.Family;
using DomusMind.Domain.SharedLists;
using DomusMind.Domain.SharedLists.ValueObjects;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.Features.SharedLists;

public sealed class UpdateSharedListItemCommandHandlerTests
{
    private static DomusMindDbContext CreateDb()
        => new(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static UpdateSharedListItemCommandHandler BuildHandler(
        DomusMindDbContext? db = null,
        StubSharedListAuthorizationService? auth = null)
    {
        var context = db ?? CreateDb();
        return new UpdateSharedListItemCommandHandler(
            context, new EventLogWriter(context),
            auth ?? new StubSharedListAuthorizationService());
    }

    private static async Task<(DomusMindDbContext db, SharedList list, SharedListItemId itemId)> SeedListWithItemAsync()
    {
        var db = CreateDb();
        var familyId = FamilyId.New();
        var list = SharedListTestHelpers.MakeList(familyId);
        var itemId = SharedListItemId.New();
        list.AddItem(itemId, SharedListItemName.Create("Original Name"), null, null, DateTime.UtcNow);
        list.ClearDomainEvents();
        db.Set<SharedList>().Add(list);
        await db.SaveChangesAsync();
        return (db, list, itemId);
    }

    [Fact]
    public async Task Handle_UpdatesItemName()
    {
        var (db, list, itemId) = await SeedListWithItemAsync();
        var handler = BuildHandler(db: db);

        var result = await handler.Handle(
            new UpdateSharedListItemCommand(list.Id.Value, itemId.Value, "Updated Name", null, null, Guid.NewGuid()),
            CancellationToken.None);

        result.Name.Should().Be("Updated Name");
    }

    [Fact]
    public async Task Handle_UpdatesQuantityAndNote()
    {
        var (db, list, itemId) = await SeedListWithItemAsync();
        var handler = BuildHandler(db: db);

        var result = await handler.Handle(
            new UpdateSharedListItemCommand(list.Id.Value, itemId.Value, "Milk", "2L", "full-fat", Guid.NewGuid()),
            CancellationToken.None);

        result.Quantity.Should().Be("2L");
        result.Note.Should().Be("full-fat");
    }

    [Fact]
    public async Task Handle_WithEmptyName_ThrowsSharedListException()
    {
        var (db, list, itemId) = await SeedListWithItemAsync();
        var handler = BuildHandler(db: db);

        var act = async () => await handler.Handle(
            new UpdateSharedListItemCommand(list.Id.Value, itemId.Value, "   ", null, null, Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<SharedListException>()
            .Where(ex => ex.Code == SharedListErrorCode.InvalidInput);
    }

    [Fact]
    public async Task Handle_WithNameExceedingMaxLength_ThrowsSharedListException()
    {
        var (db, list, itemId) = await SeedListWithItemAsync();
        var handler = BuildHandler(db: db);
        var longName = new string('x', SharedListItemName.MaxLength + 1);

        var act = async () => await handler.Handle(
            new UpdateSharedListItemCommand(list.Id.Value, itemId.Value, longName, null, null, Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<SharedListException>()
            .Where(ex => ex.Code == SharedListErrorCode.InvalidInput);
    }

    [Fact]
    public async Task Handle_WithUnknownItemId_ThrowsItemNotFound()
    {
        var (db, list, _) = await SeedListWithItemAsync();
        var handler = BuildHandler(db: db);

        var act = async () => await handler.Handle(
            new UpdateSharedListItemCommand(list.Id.Value, Guid.NewGuid(), "Name", null, null, Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<SharedListException>()
            .Where(ex => ex.Code == SharedListErrorCode.ItemNotFound);
    }

    [Fact]
    public async Task Handle_WithUnknownListId_ThrowsListNotFound()
    {
        var handler = BuildHandler();

        var act = async () => await handler.Handle(
            new UpdateSharedListItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Name", null, null, Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<SharedListException>()
            .Where(ex => ex.Code == SharedListErrorCode.ListNotFound);
    }

    [Fact]
    public async Task Handle_PersistsChangeToDB()
    {
        var (db, list, itemId) = await SeedListWithItemAsync();
        var handler = BuildHandler(db: db);

        await handler.Handle(
            new UpdateSharedListItemCommand(list.Id.Value, itemId.Value, "Persisted Name", null, null, Guid.NewGuid()),
            CancellationToken.None);

        var reloaded = await db.Set<SharedList>()
            .Include(l => l.Items)
            .SingleAsync(l => l.Id == list.Id);
        reloaded.Items.Should().Contain(i => i.Name.Value == "Persisted Name");
    }
}
