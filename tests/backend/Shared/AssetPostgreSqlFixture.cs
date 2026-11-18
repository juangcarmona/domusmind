using DomusMind.Domain.Family;
using DomusMind.Domain.Family.ValueObjects;
using DomusMind.Domain.Responsibilities;
using DomusMind.Domain.Responsibilities.ValueObjects;
using DomusMind.Domain.Shared;
using DomusMind.Infrastructure.Auth;
using DomusMind.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Xunit;

namespace DomusMind.Tests.HouseholdAssets;

public sealed class AssetPostgreSqlFixture : IAsyncLifetime
{
    private readonly string schema = $"asset_foundation_{Guid.NewGuid():N}";
    private string? adminConnection;
    public DbContextOptions<DomusMindDbContext> Options { get; private set; } = null!;
    public Guid UserId { get; } = Guid.NewGuid();
    public FamilyId FamilyId { get; } = FamilyId.New();
    public FamilyId ForeignFamilyId { get; } = FamilyId.New();
    public MemberId MemberId { get; } = MemberId.New();
    public MemberId ForeignMemberId { get; } = MemberId.New();
    public ResponsibilityDomainId AreaId { get; } = ResponsibilityDomainId.New();
    public ResponsibilityDomainId ForeignAreaId { get; } = ResponsibilityDomainId.New();

    public DomusMindDbContext CreateContext() => new(Options);

    public async Task InitializeAsync()
    {
        adminConnection = Environment.GetEnvironmentVariable("DOMUSMIND_TEST_POSTGRES");
        if (string.IsNullOrWhiteSpace(adminConnection)) return;
        await using var admin = new NpgsqlConnection(adminConnection);
        await admin.OpenAsync();
        await using var create = admin.CreateCommand();
        create.CommandText = $"CREATE SCHEMA \"{schema}\"";
        await create.ExecuteNonQueryAsync();
        var connection = new NpgsqlConnectionStringBuilder(adminConnection) { SearchPath = schema }.ConnectionString;
        Options = new DbContextOptionsBuilder<DomusMindDbContext>().UseNpgsql(connection).Options;
        await using var db = CreateContext();
        await db.Database.MigrateAsync();
        var family = Family.Create(FamilyId, FamilyName.Create("Assets household"), "en", DateTime.UtcNow);
        family.AddMember(MemberId, MemberName.Create("Responsible member"), MemberRole.Adult, DateTime.UtcNow);
        var other = Family.Create(ForeignFamilyId, FamilyName.Create("Other household"), "en", DateTime.UtcNow);
        other.AddMember(ForeignMemberId, MemberName.Create("Other member"), MemberRole.Adult, DateTime.UtcNow);
        db.Families.AddRange(family, other);
        db.AuthUsers.Add(new AuthUser { UserId = UserId, Email = $"{UserId:N}@test.invalid", PasswordHash = "Test-only", CreatedAtUtc = DateTime.UtcNow });
        db.UserFamilyAccesses.Add(new UserFamilyAccess { UserId = UserId, FamilyId = FamilyId.Value, GrantedAtUtc = DateTime.UtcNow });
        db.ResponsibilityDomains.AddRange(
            ResponsibilityDomain.Create(AreaId, FamilyId, ResponsibilityAreaName.Create("Maintenance"), HexColor.From("#6A4C93"), DateTime.UtcNow),
            ResponsibilityDomain.Create(ForeignAreaId, ForeignFamilyId, ResponsibilityAreaName.Create("Other area"), HexColor.From("#6A4C93"), DateTime.UtcNow));
        await db.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        if (string.IsNullOrWhiteSpace(adminConnection)) return;
        await using var admin = new NpgsqlConnection(adminConnection);
        await admin.OpenAsync();
        await using var drop = admin.CreateCommand();
        drop.CommandText = $"DROP SCHEMA IF EXISTS \"{schema}\" CASCADE";
        await drop.ExecuteNonQueryAsync();
    }
}

public sealed class AssetPostgreSqlFactAttribute : FactAttribute
{
    public AssetPostgreSqlFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DOMUSMIND_TEST_POSTGRES")))
            Skip = "Set DOMUSMIND_TEST_POSTGRES to run the PostgreSQL asset tests.";
    }
}
