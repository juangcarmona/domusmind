using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.HouseholdAssets;

public sealed class AssetObligationConfiguration : IEntityTypeConfiguration<AssetObligation>
{
    public void Configure(EntityTypeBuilder<AssetObligation> b)
    {
        b.ToTable("asset_obligations"); b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasConversion(x => x.Value, x => AssetObligationId.From(x)).HasColumnName("id");
        b.Property(x => x.FamilyId).HasConversion(x => x.Value, x => FamilyId.From(x)).HasColumnName("family_id");
        b.Property(x => x.AssetId).HasConversion(x => x.Value, x => AssetId.From(x)).HasColumnName("asset_id");
        b.Property(x => x.Name).HasColumnName("name");
        b.Property(x => x.DefinitionOperationId).HasColumnName("definition_operation_id");
        b.Property(x => x.InitialDueDate).HasColumnName("initial_due_date");
        b.Property(x => x.State).HasConversion<string>().HasColumnName("state");
        b.Property(x => x.Version).HasColumnName("version").IsConcurrencyToken();
        b.OwnsOne(x => x.Recurrence, r =>
        {
            r.Property(x => x.Kind).HasConversion<string>().HasColumnName("recurrence");
            r.Property(x => x.Months).HasColumnName("interval_months");
        });
        b.HasIndex(x => new { x.FamilyId, x.DefinitionOperationId }).IsUnique();
        b.HasOne<Asset>().WithMany().HasForeignKey(x => new { x.FamilyId, x.AssetId })
            .HasPrincipalKey(x => new { x.FamilyId, x.Id }).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(x => x.Occurrences).WithOne().HasForeignKey(x => x.ObligationId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(x => x.Operations).WithOne().HasForeignKey(x => x.ObligationId).OnDelete(DeleteBehavior.Restrict);
        b.Navigation(x => x.Occurrences).UsePropertyAccessMode(PropertyAccessMode.Field);
        b.Navigation(x => x.Operations).UsePropertyAccessMode(PropertyAccessMode.Field);
        b.Ignore(x => x.Pending); b.Ignore(x => x.DomainEvents); b.Ignore(x => x.AggregateId);
    }
}

public sealed class AssetOccurrenceConfiguration : IEntityTypeConfiguration<AssetOccurrence>
{
    public void Configure(EntityTypeBuilder<AssetOccurrence> b)
    {
        b.ToTable("asset_occurrences"); b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasConversion(x => x.Value, x => AssetOccurrenceId.From(x)).HasColumnName("id");
        b.Property(x => x.ObligationId).HasConversion(x => x.Value, x => AssetObligationId.From(x)).HasColumnName("obligation_id");
        b.HasAlternateKey(x => new { x.ObligationId, x.Id });
        b.Property(x => x.DueDate).HasColumnName("due_date");
        b.Property(x => x.State).HasConversion<string>().HasColumnName("state");
        b.Property(x => x.CompletedDate).HasColumnName("completed_date");
    }
}

public sealed class ObligationOperationConfiguration : IEntityTypeConfiguration<ObligationOperation>
{
    public void Configure(EntityTypeBuilder<ObligationOperation> b)
    {
        b.ToTable("asset_obligation_operations"); b.HasKey(x => new { x.ObligationId, x.Id });
        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.ObligationId).HasConversion(x => x.Value, x => AssetObligationId.From(x)).HasColumnName("obligation_id");
        b.Property(x => x.CycleId).HasConversion(x => x.Value, x => AssetOccurrenceId.From(x)).HasColumnName("cycle_id");
        b.HasOne<AssetOccurrence>().WithMany().HasForeignKey(x => new { x.ObligationId, x.CycleId })
            .HasPrincipalKey(x => new { x.ObligationId, x.Id }).OnDelete(DeleteBehavior.Restrict);
        b.Property(x => x.Action).HasConversion<string>().HasColumnName("action");
        b.Property(x => x.RequestedDate).HasColumnName("requested_date");
        b.Property(x => x.PreviousDueDate).HasColumnName("previous_due_date");
        b.Property(x => x.ResultObligationState).HasConversion<string>().HasColumnName("result_obligation_state");
        b.Property(x => x.ResultOccurrenceState).HasConversion<string>().HasColumnName("result_occurrence_state");
        b.Property(x => x.ResultDueDate).HasColumnName("result_due_date");
        b.Property(x => x.ResultCompletedDate).HasColumnName("result_completed_date");
        b.Property(x => x.ResultNextCycleId).HasConversion(x => x.HasValue ? x.Value.Value : (Guid?)null,
            x => x.HasValue ? AssetOccurrenceId.From(x.Value) : (AssetOccurrenceId?)null).HasColumnName("result_next_cycle_id");
        b.Property(x => x.ResultNextDueDate).HasColumnName("result_next_due_date");
        b.Property(x => x.RecordedAtUtc).HasColumnName("recorded_at_utc");
    }
}
