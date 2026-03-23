using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Auth;
using DomusMind.Application.Features.Family;
using DomusMind.Application.Features.Family.DisableMemberAccess;
using DomusMind.Application.Tests.Features.Auth;
using DomusMind.Domain.Family;
using DomusMind.Domain.Family.ValueObjects;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.Features.Family;

public sealed class DisableMemberAccessCommandHandlerTests
{
    private static DomusMindDbContext CreateDb()
        => new(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static DisableMemberAccessCommandHandler BuildHandler(
        DomusMindDbContext db,
        InMemoryAuthUserRepository? users = null,
        StubFamilyAuthorizationService? auth = null,
        IRefreshTokenService? refreshTokens = null)
        => new(
            db,
            auth ?? new StubFamilyAuthorizationService(),
            users ?? new InMemoryAuthUserRepository(),
            refreshTokens ?? new StubRefreshTokenService());

    private static async Task<(DomusMindDbContext Db, Domain.Family.Family Family, Guid ManagerUserId, Guid LinkedUserId, Guid TargetMemberId)>
        BuildFamilyWithLinkedMemberAsync()
    {
        var db = CreateDb();
        var managerUserId = Guid.NewGuid();
        var linkedUserId = Guid.NewGuid();
        var targetMemberId = MemberId.New();

        var family = Domain.Family.Family.Create(
            FamilyId.New(),
            FamilyName.Create("Test Family"),
            null,
            DateTime.UtcNow);

        family.AddMember(
            MemberId.New(),
            MemberName.Create("Manager"),
            MemberRole.Adult,
            isManager: true,
            birthDate: null,
            DateTime.UtcNow,
            authUserId: managerUserId);

        family.AddMember(
            targetMemberId,
            MemberName.Create("Dave"),
            MemberRole.Adult,
            isManager: false,
            birthDate: null,
            DateTime.UtcNow,
            authUserId: linkedUserId);

        db.Set<Domain.Family.Family>().Add(family);
        await db.SaveChangesAsync();
        family.ClearDomainEvents();
        return (db, family, managerUserId, linkedUserId, targetMemberId.Value);
    }

    [Fact]
    public async Task Handle_DisablesAuthUser()
    {
        var (db, family, managerUserId, linkedUserId, targetMemberId) = await BuildFamilyWithLinkedMemberAsync();
        var userRepo = new InMemoryAuthUserRepository(
        [
            new AuthUserRecord(linkedUserId, "dave@home.local", "HASHED:pass", false),
        ]);
        var handler = BuildHandler(db, users: userRepo);

        await handler.Handle(
            new DisableMemberAccessCommand(family.Id.Value, targetMemberId, managerUserId),
            CancellationToken.None);

        var user = userRepo.Users.Single(u => u.UserId == linkedUserId);
        user.IsDisabled.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ReturnsMemberId()
    {
        var (db, family, managerUserId, linkedUserId, targetMemberId) = await BuildFamilyWithLinkedMemberAsync();
        var userRepo = new InMemoryAuthUserRepository(
        [
            new AuthUserRecord(linkedUserId, "dave@home.local", "HASHED:pass", false),
        ]);
        var handler = BuildHandler(db, users: userRepo);

        var result = await handler.Handle(
            new DisableMemberAccessCommand(family.Id.Value, targetMemberId, managerUserId),
            CancellationToken.None);

        result.MemberId.Should().Be(targetMemberId);
    }

    [Fact]
    public async Task Handle_RevokesAllRefreshTokens()
    {
        var (db, family, managerUserId, linkedUserId, targetMemberId) = await BuildFamilyWithLinkedMemberAsync();
        var userRepo = new InMemoryAuthUserRepository(
        [
            new AuthUserRecord(linkedUserId, "dave@home.local", "HASHED:pass", false),
        ]);
        var revokeTrack = new TrackingRefreshTokenService();
        var handler = BuildHandler(db, users: userRepo, refreshTokens: revokeTrack);

        await handler.Handle(
            new DisableMemberAccessCommand(family.Id.Value, targetMemberId, managerUserId),
            CancellationToken.None);

        revokeTrack.RevokedUserId.Should().Be(linkedUserId);
    }

    [Fact]
    public async Task Handle_NonManager_ThrowsAccessDenied()
    {
        var (db, family, _, _, targetMemberId) = await BuildFamilyWithLinkedMemberAsync();
        var nonManagerUserId = Guid.NewGuid();

        var nmMemberId = MemberId.New();
        family.AddMember(nmMemberId, MemberName.Create("Nonmanager"), MemberRole.Adult,
            isManager: false, birthDate: null, DateTime.UtcNow, authUserId: nonManagerUserId);
        await db.SaveChangesAsync();
        family.ClearDomainEvents();

        var handler = BuildHandler(db);

        var act = () => handler.Handle(
            new DisableMemberAccessCommand(family.Id.Value, targetMemberId, nonManagerUserId),
            CancellationToken.None);

        await act.Should().ThrowAsync<FamilyException>()
            .Where(e => e.Code == FamilyErrorCode.AccessDenied);
    }

    [Fact]
    public async Task Handle_MemberWithoutAccount_ThrowsInvalidInput()
    {
        var (db, family, managerUserId, _, _) = await BuildFamilyWithLinkedMemberAsync();

        var noAccountMemberId = MemberId.New();
        family.AddMember(noAccountMemberId, MemberName.Create("Noone"), MemberRole.Adult,
            isManager: false, birthDate: null, DateTime.UtcNow, authUserId: null);
        await db.SaveChangesAsync();
        family.ClearDomainEvents();

        var handler = BuildHandler(db);

        var act = () => handler.Handle(
            new DisableMemberAccessCommand(family.Id.Value, noAccountMemberId.Value, managerUserId),
            CancellationToken.None);

        await act.Should().ThrowAsync<FamilyException>()
            .Where(e => e.Code == FamilyErrorCode.InvalidInput);
    }

    private sealed class TrackingRefreshTokenService : IRefreshTokenService
    {
        public Guid? RevokedUserId { get; private set; }

        public Task<string> CreateAsync(Guid userId, CancellationToken ct)
            => Task.FromResult($"token-{userId}");

        public Task<RotateRefreshTokenResult> ValidateAndRotateAsync(string token, CancellationToken ct)
            => Task.FromResult(new RotateRefreshTokenResult(false, null, null, null));

        public Task RevokeAsync(string token, CancellationToken ct) => Task.CompletedTask;

        public Task RevokeAllForUserAsync(Guid userId, CancellationToken ct)
        {
            RevokedUserId = userId;
            return Task.CompletedTask;
        }
    }
}
