using DomusMind.Application.Features.SharedLists;
using DomusMind.Application.Features.SharedLists.RenameSharedList;
using DomusMind.Domain.Family;
using DomusMind.Domain.SharedLists;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.Features.SharedLists;

public sealed class RenameSharedListCommandHandlerTests
{
    private static DomusMindDbContext CreateDb()
        => new(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static RenameSharedListCommandHandler BuildHandler(
        DomusMindDbContext? db = null,
        StubSharedListAuthorizationService? auth = null)
    {
        var context = db ?? CreateDb();
        return new RenameSharedListCommandHandler(
            context, new EventLogWriter(context),
            auth ?? new StubSharedListAuthorizationService());
    }

    private static async Task<(DomusMindDbContext db, SharedList list)> SeedListAsync()
    {
        var db = CreateDb();
        var list = SharedListTestHelpers.MakeList(FamilyId.New());
        list.ClearDomainEvents();
        db.Set<SharedList>().Add(list);
        await db.SaveChangesAsync();
        return (db, list);
    }

    [Fact]
    public async Task Handle_RenamesList_ReturnsNewName()
    {
        var (db, list) = await SeedListAsync();
        var handler = BuildHandler(db: db);

        var result = await handler.Handle(
            new RenameSharedListCommand(list.Id.Value, "Renamed List", Guid.NewGuid()),
            CancellationToken.None);

        result.ListId.Should().Be(list.Id.Value);
        result.Name.Should().Be("Renamed List");
    }

    [Fact]
    public async Task Handle_PersistsNewName()
    {
        var (db, list) = await SeedListAsync();
        var handler = BuildHandler(db: db);

        await handler.Handle(
            new RenameSharedListCommand(list.Id.Value, "Persisted Name", Guid.NewGuid()),
            CancellationToken.None);

        var saved = await db.Set<SharedList>().SingleAsync(l => l.Id == list.Id);
        saved.Name.Value.Should().Be("Persisted Name");
    }

    [Fact]
    public async Task Handle_EmptyName_ThrowsSharedListException()
    {
        var (db, list) = await SeedListAsync();
        var handler = BuildHandler(db: db);

        var act = async () => await handler.Handle(
            new RenameSharedListCommand(list.Id.Value, "   ", Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<SharedListException>()
            .Where(e => e.Code == SharedListErrorCode.InvalidInput);
    }

    [Fact]
    public async Task Handle_ListNotFound_ThrowsSharedListException()
    {
        var handler = BuildHandler();

        var act = async () => await handler.Handle(
            new RenameSharedListCommand(Guid.NewGuid(), "Name", Guid.NewGuid()),
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
            new RenameSharedListCommand(list.Id.Value, "New Name", Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<SharedListException>()
            .Where(e => e.Code == SharedListErrorCode.AccessDenied);
    }
}
