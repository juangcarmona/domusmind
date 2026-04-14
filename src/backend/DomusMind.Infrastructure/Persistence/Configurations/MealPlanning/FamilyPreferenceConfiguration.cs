using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.MealPlanning;

public sealed class FamilyPreferenceConfiguration : IEntityTypeConfiguration<FamilyPreference>
{
    public void Configure(EntityTypeBuilder<FamilyPreference> builder)
    {
        builder.ToTable("family_preferences");

        builder.HasKey(fp => fp.Id);

        builder.Property(fp => fp.Id)
            .HasConversion(id => id.Value, value => new FamilyPreferenceId(value))
            .HasColumnName("id")
            .IsRequired();

        builder.Property(fp => fp.FamilyId)
            .HasConversion(id => id.Value, value => new FamilyId(value))
            .HasColumnName("family_id")
            .IsRequired();

        builder.Property(fp => fp.PreferredMealTypes)
            .HasConversion(
                types => string.Join(",", types.Select(t => t.ToString())),
                value => value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => Enum.Parse<MealType>(s.Trim()))
                    .ToList())
            .HasColumnName("preferred_meal_types");

        builder.Property(fp => fp.WeekendFlexibility)
            .HasColumnName("weekend_flexibility")
            .IsRequired();

        builder.Property(fp => fp.DefaultDietaryConstraints)
            .HasConversion(
                constraints => string.Join(",", constraints.Select(c => c.Value)),
                value => value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => new DietaryConstraintId(Guid.Parse(s.Trim())))
                    .ToList())
            .HasColumnName("default_dietary_constraints");

        builder.Property(fp => fp.CreatedAtUtc)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(fp => fp.UpdatedAtUtc)
            .HasColumnName("updated_at")
            .IsRequired();

        // Remove DomainEvents ignore - this entity doesn't have DomainEvents
    }
}