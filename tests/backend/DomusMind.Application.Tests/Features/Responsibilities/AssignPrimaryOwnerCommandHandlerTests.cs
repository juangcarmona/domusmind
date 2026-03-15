using DomusMind.Application.Features.Responsibilities;
using DomusMind.Application.Features.Responsibilities.AssignPrimaryOwner;
using DomusMind.Domain.Family;
using DomusMind.Domain.Responsibilities;
using DomusMind.Domain.Responsibilities.ValueObjects;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.Features.Responsibilities;

public sealed class AssignPrimaryOwnerCommandHandlerTests
{
    private static DomusMindDbContext CreateDb()
        => new(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static AssignPrimaryOwnerCommandHandler BuildHandler(
        DomusMindDbContext db,
        StubResponsibilitiesAuthorizationService? auth = null)
        => new(db, new EventLogWriter(db), auth ?? new StubResponsibilitiesAuthorizationService());

    private static async Task<(DomusMindDbContext Db, ResponsibilityDomain Domain)> BuildWithDomainAsync()
    {
        var db = CreateDb();
        var domain = ResponsibilityDomain.Create(
            ResponsibilityDomainId.New(),
            FamilyId.New(),
            ResponsibilityAreaName.Create("Finances"),
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
        var memberId = Guid.NewGuid();
        var handler = BuildHandler(db);

        var result = await handler.Handle(
            new AssignPrimaryOwnerCommand(domain.Id.Value, memberId, Guid.NewGuid()),
            CancellationToken.None);

        result.ResponsibilityDomainId.Should().Be(domain.Id.Value);
        result.MemberId.Should().Be(memberId);
    }

    [Fact]
    public async Task Handle_PersistsPrimaryOwnerToDatabase()
    {
        var (db, domain) = await BuildWithDomainAsync();
        var memberId = Guid.NewGuid();
        var handler = BuildHandler(db);

        await handler.Handle(
            new AssignPrimaryOwnerCommand(domain.Id.Value, memberId, Guid.NewGuid()),
            CancellationToken.None);

        var saved = await db.Set<ResponsibilityDomain>()
            .SingleOrDefaultAsync(d => d.Id == domain.Id);
        saved.Should().NotBeNull();
        saved!.PrimaryOwnerId.Should().Be(MemberId.From(memberId));
    }

    [Fact]
    public async Task Handle_DomainNotFound_ThrowsResponsibilitiesException()
    {
        var db = CreateDb();
        var handler = BuildHandler(db);

        var act = () => handler.Handle(
            new AssignPrimaryOwnerCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
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
            new AssignPrimaryOwnerCommand(domain.Id.Value, Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<ResponsibilitiesException>()
            .Where(e => e.Code == ResponsibilitiesErrorCode.AccessDenied);
    }
}
