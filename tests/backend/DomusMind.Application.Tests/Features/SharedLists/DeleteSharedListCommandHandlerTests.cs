using DomusMind.Application.Features.SharedLists;
using DomusMind.Application.Features.SharedLists.DeleteSharedList;
using DomusMind.Domain.Family;
using DomusMind.Domain.SharedLists;
using DomusMind.Domain.SharedLists.ValueObjects;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.Features.SharedLists;

public sealed class DeleteSharedListCommandHandlerTests
{
    private static DomusMindDbContext CreateDb()
        => new(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static DeleteSharedListCommandHandler BuildHandler(
        DomusMindDbContext? db = null,
        StubSharedListAuthorizationService? auth = null)
    {
        var context = db ?? CreateDb();
        return new DeleteSharedListCommandHandler(
            context, new EventLogWriter(context),
            auth ?? new StubSharedListAuthorizationService());
    }

    private static async Task<(DomusMindDbContext db, SharedList list)> SeedListAsync(
        Action<SharedList>? configure = null)
    {
        var db = CreateDb();
        var list = SharedListTestHelpers.MakeList(FamilyId.New());
        configure?.Invoke(list);
        list.ClearDomainEvents();
        db.Set<SharedList>().Add(list);
        await db.SaveChangesAsync();
        return (db, list);
    }

    [Fact]
    public async Task Handle_DeletesList_ReturnsTrue()
    {
        var (db, list) = await SeedListAsync();
        var handler = BuildHandler(db: db);

        var result = await handler.Handle(
            new DeleteSharedListCommand(list.Id.Value, Guid.NewGuid()),
            CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_RemovesListFromDatabase()
    {
        var (db, list) = await SeedListAsync();
        var handler = BuildHandler(db: db);

        await handler.Handle(
            new DeleteSharedListCommand(list.Id.Value, Guid.NewGuid()),
            CancellationToken.None);

        var remaining = await db.Set<SharedList>().CountAsync();
        remaining.Should().Be(0);
    }

    [Fact]
    public async Task Handle_DeletesLinkedList_Succeeds()
    {
        var (db, list) = await SeedListAsync(l =>
        {
            l.LinkToEntity("CalendarEvent", Guid.NewGuid(), DateTime.UtcNow);
            l.ClearDomainEvents();
        });
        var handler = BuildHandler(db: db);

        var result = await handler.Handle(
            new DeleteSharedListCommand(list.Id.Value, Guid.NewGuid()),
            CancellationToken.None);

        result.Should().BeTrue();
        var remaining = await db.Set<SharedList>().CountAsync();
        remaining.Should().Be(0);
    }

    [Fact]
    public async Task Handle_DeletesListWithItems_RemovesAll()
    {
        var (db, list) = await SeedListAsync(l =>
        {
            l.AddItem(SharedListItemId.New(), SharedListItemName.Create("Item A"), null, null, DateTime.UtcNow);
            l.AddItem(SharedListItemId.New(), SharedListItemName.Create("Item B"), null, null, DateTime.UtcNow);
            l.ClearDomainEvents();
        });
        var handler = BuildHandler(db: db);

        await handler.Handle(
            new DeleteSharedListCommand(list.Id.Value, Guid.NewGuid()),
            CancellationToken.None);

        var remaining = await db.Set<SharedList>()
            .Include(l => l.Items)
            .CountAsync();
        remaining.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ListNotFound_ThrowsSharedListException()
    {
        var handler = BuildHandler();

        var act = async () => await handler.Handle(
            new DeleteSharedListCommand(Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<SharedListException>()
            .Where(e => e.Code == SharedListErrorCode.ListNotFound);
    }

    [Fact]
    public async Task Handle_AccessDenied_ThrowsSharedListException()
    {
        var (db, list) = await SeedListAsync();
        var auth = new StubSharedListAuthorizationService { CanAccess = false };
        var handler = BuildHandler(db: db, auth: auth);

        var act = async () => await handler.Handle(
            new DeleteSharedListCommand(list.Id.Value, Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<SharedListException>()
            .Where(e => e.Code == SharedListErrorCode.AccessDenied);
    }
}
