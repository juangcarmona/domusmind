using System.Text.Json;
using DomusMind.Domain.Family;
using DomusMind.Domain.Responsibilities;
using DomusMind.Domain.Responsibilities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DomusMind.Infrastructure.Persistence.Configurations.Responsibilities;

public sealed class ResponsibilityDomainConfiguration : IEntityTypeConfiguration<ResponsibilityDomain>
{
    public void Configure(EntityTypeBuilder<ResponsibilityDomain> builder)
    {
        builder.ToTable("responsibility_domains");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(
                id => id.Value,
                value => ResponsibilityDomainId.From(value))
            .HasColumnName("id")
            .IsRequired();

        builder.Property(d => d.FamilyId)
            .HasConversion(
                id => id.Value,
                value => FamilyId.From(value))
            .HasColumnName("family_id")
            .IsRequired();

        builder.Property(d => d.Name)
            .HasConversion(
                name => name.Value,
                value => ResponsibilityAreaName.Create(value))
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.PrimaryOwnerId)
            .HasConversion(
                id => id.HasValue ? id.Value.Value : (Guid?)null,
                value => value.HasValue ? MemberId.From(value.Value) : (MemberId?)null)
            .HasColumnName("primary_owner_id");

        builder.Property(d => d.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        // Secondary owner IDs stored as a JSON array string.
        // Stored in a text column; works with both PostgreSQL and EF InMemory.
        var secondaryOwnerConverter = new ValueConverter<List<MemberId>, string>(
            ids => JsonSerializer.Serialize(ids.Select(id => id.Value).ToList(), (JsonSerializerOptions?)null),
            json => JsonSerializer.Deserialize<List<Guid>>(json, (JsonSerializerOptions?)null)!
                .Select(MemberId.From).ToList());

        var secondaryOwnerComparer = new ValueComparer<List<MemberId>>(
            (x, y) => x != null && y != null && x.SequenceEqual(y),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());

        builder.Property<List<MemberId>>("_secondaryOwnerIds")
            .HasField("_secondaryOwnerIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasConversion(secondaryOwnerConverter, secondaryOwnerComparer)
            .HasColumnName("secondary_owner_ids")
            .HasColumnType("text")
            .IsRequired();

        builder.Ignore(d => d.SecondaryOwnerIds);

        builder.Ignore(d => d.DomainEvents);
    }
}
