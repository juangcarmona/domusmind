using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using DomusMind.Domain.Responsibilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.HouseholdAssets;

public sealed class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("household_assets");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => AssetId.From(x)).HasColumnName("id");
        builder.Property(x => x.FamilyId).HasConversion(x => x.Value, x => FamilyId.From(x)).HasColumnName("family_id");
        builder.Property(x => x.Name).HasColumnName("name").IsRequired();
        builder.Property(x => x.Category).HasColumnName("category").IsRequired();
        builder.Property(x => x.Make).HasColumnName("make");
        builder.Property(x => x.Model).HasColumnName("model");
        builder.Property(x => x.SerialNumber).HasColumnName("serial_number");
        builder.Property(x => x.ManagementBasis).HasColumnName("management_basis");
        builder.Property(x => x.ParentAssetId).HasConversion(x => x.HasValue ? x.Value.Value : (Guid?)null,
            x => x.HasValue ? AssetId.From(x.Value) : (AssetId?)null).HasColumnName("parent_asset_id");
        builder.Property(x => x.LocationId).HasConversion(x => x.HasValue ? x.Value.Value : (Guid?)null,
            x => x.HasValue ? AssetLocationId.From(x.Value) : (AssetLocationId?)null).HasColumnName("location_id");
        builder.Property(x => x.ResponsibleMemberId).HasConversion(x => x.HasValue ? x.Value.Value : (Guid?)null,
            x => x.HasValue ? MemberId.From(x.Value) : (MemberId?)null).HasColumnName("responsible_member_id");
        builder.Property(x => x.ResponsibleAreaId).HasConversion(x => x.HasValue ? x.Value.Value : (Guid?)null,
            x => x.HasValue ? ResponsibilityDomainId.From(x.Value) : (ResponsibilityDomainId?)null).HasColumnName("responsible_area_id");
        builder.Property(x => x.IsArchived).HasColumnName("is_archived");
        builder.Property(x => x.Version).HasColumnName("version").IsConcurrencyToken();
        builder.HasAlternateKey(x => new { x.FamilyId, x.Id });
        builder.HasIndex(x => new { x.FamilyId, x.IsArchived });
        builder.HasOne<Asset>().WithMany().HasForeignKey(x => new { x.FamilyId, x.ParentAssetId })
            .HasPrincipalKey(x => new { x.FamilyId, x.Id }).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<AssetLocation>().WithMany().HasForeignKey(x => new { x.FamilyId, x.LocationId })
            .HasPrincipalKey(x => new { x.FamilyId, x.Id }).OnDelete(DeleteBehavior.Restrict);
        builder.Ignore(x => x.DomainEvents);
        builder.Ignore(x => x.AggregateId);
    }
}
