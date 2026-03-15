using DomusMind.Application.Features.Responsibilities;
using DomusMind.Application.Features.Responsibilities.CreateResponsibilityDomain;
using DomusMind.Domain.Family;
using DomusMind.Domain.Responsibilities;
using DomusMind.Domain.Responsibilities.ValueObjects;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.Features.Responsibilities;

public sealed class CreateResponsibilityDomainCommandHandlerTests
{
    private static DomusMindDbContext CreateDb()
        => new(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static CreateResponsibilityDomainCommandHandler BuildHandler(
        DomusMindDbContext? db = null,
        StubResponsibilitiesAuthorizationService? auth = null)
    {
        var context = db ?? CreateDb();
        return new CreateResponsibilityDomainCommandHandler(
            context,
            new EventLogWriter(context),
            auth ?? new StubResponsibilitiesAuthorizationService());
    }

    [Fact]
    public async Task Handle_WithValidInput_ReturnsResponse()
    {
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var handler = BuildHandler();

        var result = await handler.Handle(
            new CreateResponsibilityDomainCommand("Finances", familyId, userId),
            CancellationToken.None);

        result.ResponsibilityDomainId.Should().NotBeEmpty();
        result.FamilyId.Should().Be(familyId);
        result.Name.Should().Be("Finances");
        result.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_PersistsDomainToDatabase()
    {
        var db = CreateDb();
        var handler = BuildHandler(db: db);
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var result = await handler.Handle(
            new CreateResponsibilityDomainCommand("Health", familyId, userId),
            CancellationToken.None);

        var saved = await db.Set<ResponsibilityDomain>()
            .SingleOrDefaultAsync(d => d.Id == ResponsibilityDomainId.From(result.ResponsibilityDomainId));
        saved.Should().NotBeNull();
        saved!.Name.Value.Should().Be("Health");
        saved.FamilyId.Should().Be(FamilyId.From(familyId));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Handle_EmptyName_ThrowsResponsibilitiesException(string name)
    {
        var handler = BuildHandler();

        var act = () => handler.Handle(
            new CreateResponsibilityDomainCommand(name, Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<ResponsibilitiesException>()
            .Where(e => e.Code == ResponsibilitiesErrorCode.InvalidInput);
    }

    [Fact]
    public async Task Handle_AccessDenied_ThrowsResponsibilitiesException()
    {
        var auth = new StubResponsibilitiesAuthorizationService { CanAccess = false };
        var handler = BuildHandler(auth: auth);

        var act = () => handler.Handle(
            new CreateResponsibilityDomainCommand("Pets", Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<ResponsibilitiesException>()
            .Where(e => e.Code == ResponsibilitiesErrorCode.AccessDenied);
    }
}
