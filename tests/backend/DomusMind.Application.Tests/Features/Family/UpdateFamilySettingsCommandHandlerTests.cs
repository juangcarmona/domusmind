using DomusMind.Application.Features.Family;
using DomusMind.Application.Features.Family.UpdateFamilySettings;
using DomusMind.Domain.Family;
using DomusMind.Domain.Family.ValueObjects;
using DomusMind.Infrastructure.Events;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.Features.Family;

public sealed class UpdateFamilySettingsCommandHandlerTests
{
    private static DomusMindDbContext CreateDb()
        => new(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static async Task<(DomusMindDbContext Db, Domain.Family.Family Family)> BuildWithFamilyAsync(
        string name = "Test Family",
        string? languageCode = null)
    {
        var db = CreateDb();
        var family = Domain.Family.Family.Create(
            FamilyId.New(),
            FamilyName.Create(name),
            languageCode,
            DateTime.UtcNow);
        db.Set<Domain.Family.Family>().Add(family);
        await db.SaveChangesAsync();
        return (db, family);
    }

    private static UpdateFamilySettingsCommandHandler BuildHandler(
        DomusMindDbContext db,
        StubFamilyAuthorizationService? auth = null,
        StubSupportedLanguageReader? languages = null)
        => new(db, new EventLogWriter(db), auth ?? new StubFamilyAuthorizationService(), languages ?? new StubSupportedLanguageReader());

    [Fact]
    public async Task Handle_UpdatesNameAndPersists()
    {
        var (db, family) = await BuildWithFamilyAsync("Old Name");
        var handler = BuildHandler(db);

        var result = await handler.Handle(
            new UpdateFamilySettingsCommand(family.Id.Value, Guid.NewGuid(), "New Name", null, null, null),
            CancellationToken.None);

        result.Name.Should().Be("New Name");
        var persisted = await db.Set<Domain.Family.Family>().SingleAsync(f => f.Id == family.Id);
        persisted.Name.Value.Should().Be("New Name");
    }

    [Fact]
    public async Task Handle_UpdatesLanguageCode()
    {
        var (db, family) = await BuildWithFamilyAsync();
        var handler = BuildHandler(db);

        var result = await handler.Handle(
            new UpdateFamilySettingsCommand(family.Id.Value, Guid.NewGuid(), "Test Family", "es", null, null),
            CancellationToken.None);

        result.PrimaryLanguageCode.Should().Be("es");
    }

    [Fact]
    public async Task Handle_UpdatesFirstDayOfWeek()
    {
        var (db, family) = await BuildWithFamilyAsync();
        var handler = BuildHandler(db);

        var result = await handler.Handle(
            new UpdateFamilySettingsCommand(family.Id.Value, Guid.NewGuid(), "Test Family", null, "monday", null),
            CancellationToken.None);

        result.FirstDayOfWeek.Should().Be("monday");
    }

    [Fact]
    public async Task Handle_UpdatesDateFormatPreference()
    {
        var (db, family) = await BuildWithFamilyAsync();
        var handler = BuildHandler(db);

        var result = await handler.Handle(
            new UpdateFamilySettingsCommand(family.Id.Value, Guid.NewGuid(), "Test Family", null, null, "dd/MM/yyyy"),
            CancellationToken.None);

        result.DateFormatPreference.Should().Be("dd/MM/yyyy");
    }

    [Fact]
    public async Task Handle_ClearsOptionalFields_WhenNullProvided()
    {
        var (db, family) = await BuildWithFamilyAsync("Family", "en");
        var handler = BuildHandler(db);

        // First set some values
        await handler.Handle(
            new UpdateFamilySettingsCommand(family.Id.Value, Guid.NewGuid(), "Family", "es", "monday", "dd/MM/yyyy"),
            CancellationToken.None);

        // Then clear them
        var result = await handler.Handle(
            new UpdateFamilySettingsCommand(family.Id.Value, Guid.NewGuid(), "Family", null, null, null),
            CancellationToken.None);

        result.PrimaryLanguageCode.Should().BeNull();
        result.FirstDayOfWeek.Should().BeNull();
        result.DateFormatPreference.Should().BeNull();
    }

    [Fact]
    public async Task Handle_EmptyName_ThrowsFamilyException()
    {
        var (db, family) = await BuildWithFamilyAsync();
        var handler = BuildHandler(db);

        var act = () => handler.Handle(
            new UpdateFamilySettingsCommand(family.Id.Value, Guid.NewGuid(), "   ", null, null, null),
            CancellationToken.None);

        await act.Should().ThrowAsync<FamilyException>()
            .Where(e => e.Code == FamilyErrorCode.InvalidInput);
    }

    [Fact]
    public async Task Handle_InvalidLanguageCode_ThrowsFamilyException()
    {
        var (db, family) = await BuildWithFamilyAsync();
        var handler = BuildHandler(db, languages: new StubSupportedLanguageReader(["en"]));

        var act = () => handler.Handle(
            new UpdateFamilySettingsCommand(family.Id.Value, Guid.NewGuid(), "Test", "xx", null, null),
            CancellationToken.None);

        await act.Should().ThrowAsync<FamilyException>()
            .Where(e => e.Code == FamilyErrorCode.InvalidInput);
    }

    [Fact]
    public async Task Handle_InvalidFirstDayOfWeek_ThrowsFamilyException()
    {
        var (db, family) = await BuildWithFamilyAsync();
        var handler = BuildHandler(db);

        var act = () => handler.Handle(
            new UpdateFamilySettingsCommand(family.Id.Value, Guid.NewGuid(), "Test", null, "notaday", null),
            CancellationToken.None);

        await act.Should().ThrowAsync<FamilyException>()
            .Where(e => e.Code == FamilyErrorCode.InvalidInput);
    }

    [Fact]
    public async Task Handle_InvalidDateFormat_ThrowsFamilyException()
    {
        var (db, family) = await BuildWithFamilyAsync();
        var handler = BuildHandler(db);

        var act = () => handler.Handle(
            new UpdateFamilySettingsCommand(family.Id.Value, Guid.NewGuid(), "Test", null, null, "invalid"),
            CancellationToken.None);

        await act.Should().ThrowAsync<FamilyException>()
            .Where(e => e.Code == FamilyErrorCode.InvalidInput);
    }

    [Fact]
    public async Task Handle_AccessDenied_ThrowsFamilyException()
    {
        var (db, family) = await BuildWithFamilyAsync();
        var auth = new StubFamilyAuthorizationService { CanAccess = false };
        var handler = BuildHandler(db, auth);

        var act = () => handler.Handle(
            new UpdateFamilySettingsCommand(family.Id.Value, Guid.NewGuid(), "Test", null, null, null),
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
            new UpdateFamilySettingsCommand(Guid.NewGuid(), Guid.NewGuid(), "Test", null, null, null),
            CancellationToken.None);

        await act.Should().ThrowAsync<FamilyException>()
            .Where(e => e.Code == FamilyErrorCode.FamilyNotFound);
    }

    [Fact]
    public async Task Handle_EmitsFamilySettingsUpdatedEvent()
    {
        var (db, family) = await BuildWithFamilyAsync();
        var stubEvents = new StubEventLogWriter();
        var handler = new UpdateFamilySettingsCommandHandler(
            db, stubEvents, new StubFamilyAuthorizationService(), new StubSupportedLanguageReader());

        await handler.Handle(
            new UpdateFamilySettingsCommand(family.Id.Value, Guid.NewGuid(), "New Name", null, null, null),
            CancellationToken.None);

        stubEvents.WrittenEvents.OfType<DomusMind.Domain.Family.Events.FamilySettingsUpdated>()
            .Should().HaveCount(1);
    }
}
