using DomusMind.Application.Features.Responsibilities;
using DomusMind.Application.Features.Responsibilities.TransferResponsibility;
using DomusMind.Domain.Family;
using DomusMind.Domain.Responsibilities;
using DomusMind.Domain.Responsibilities.ValueObjects;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.Features.Responsibilities;

public sealed class TransferResponsibilityCommandHandlerTests
{
    private static DomusMindDbContext CreateDb()
        => new(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static TransferResponsibilityCommandHandler BuildHandler(
        DomusMindDbContext db,
        StubResponsibilitiesAuthorizationService? auth = null)
        => new(db, new EventLogWriter(db), auth ?? new StubResponsibilitiesAuthorizationService());

    private static async Task<(DomusMindDbContext Db, ResponsibilityDomain Domain)> BuildWithDomainAsync()
    {
        var db = CreateDb();
        var domain = ResponsibilityDomain.Create(
            ResponsibilityDomainId.New(),
            FamilyId.New(),
            ResponsibilityAreaName.Create("Education"),
            DateTime.UtcNow);
        db.Set<ResponsibilityDomain>().Add(domain);
        await db.SaveChangesAsync();
        domain.ClearDomainEvents();
        return (db, domain);
    }

    [Fact]
    public async Task Handle_WithValidInput_ReturnsResponse()
    {
        var (db, domain) = await BuildWithDomainAsync();
        var newOwnerId = Guid.NewGuid();
        var handler = BuildHandler(db);

        var result = await handler.Handle(
            new TransferResponsibilityCommand(domain.Id.Value, newOwnerId, Guid.NewGuid()),
            CancellationToken.None);

        result.ResponsibilityDomainId.Should().Be(domain.Id.Value);
        result.NewPrimaryOwnerId.Should().Be(newOwnerId);
    }

    [Fact]
    public async Task Handle_PersistsNewPrimaryOwnerToDatabase()
    {
        var (db, domain) = await BuildWithDomainAsync();
        var newOwnerId = Guid.NewGuid();
        var handler = BuildHandler(db);

        await handler.Handle(
            new TransferResponsibilityCommand(domain.Id.Value, newOwnerId, Guid.NewGuid()),
            CancellationToken.None);

        var saved = await db.Set<ResponsibilityDomain>()
            .SingleOrDefaultAsync(d => d.Id == domain.Id);
        saved.Should().NotBeNull();
        saved!.PrimaryOwnerId.Should().Be(MemberId.From(newOwnerId));
    }

    [Fact]
    public async Task Handle_DomainNotFound_ThrowsResponsibilitiesException()
    {
        var db = CreateDb();
        var handler = BuildHandler(db);

        var act = () => handler.Handle(
            new TransferResponsibilityCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<ResponsibilitiesException>()
            .Where(e => e.Code == ResponsibilitiesErrorCode.ResponsibilityDomainNotFound);
    }

    [Fact]
    public async Task Handle_AccessDenied_ThrowsResponsibilitiesException()
    {
        var (db, domain) = await BuildWithDomainAsync();
        var auth = new StubResponsibilitiesAuthorizationService { CanAccess = false };
        var handler = BuildHandler(db, auth);

        var act = () => handler.Handle(
            new TransferResponsibilityCommand(domain.Id.Value, Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<ResponsibilitiesException>()
            .Where(e => e.Code == ResponsibilitiesErrorCode.AccessDenied);
    }
}
