using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.MealPlanning;

public sealed class MealSlotConfiguration : IEntityTypeConfiguration<MealSlot>
{
    public void Configure(EntityTypeBuilder<MealSlot> builder)
    {
        builder.ToTable("meal_slots");

        builder.HasKey(ms => ms.Id);

        builder.Property(ms => ms.Id)
            .HasConversion(id => id.Value, value => MealSlotId.From(value))
            .HasColumnName("id")
            .IsRequired();

        builder.Property(ms => ms.DayOfWeek)
            .HasConversion<string>()
            .HasColumnName("day_of_week")
            .IsRequired();

        builder.Property(ms => ms.MealType)
            .HasConversion<int>()
            .HasColumnName("meal_type")
            .IsRequired();

        builder.Property(ms => ms.MealPlanId)
            .HasConversion(id => id.Value, value => MealPlanId.From(value))
            .HasColumnName("meal_plan_id")
            .IsRequired();

        builder.Property(ms => ms.RecipeId)
            .HasConversion(
                id => id.HasValue ? id.Value.Value : (Guid?)null,
                value => value.HasValue ? RecipeId.From(value.Value) : (RecipeId?)null)
            .HasColumnName("recipe_id");

        builder.Property(ms => ms.Notes)
            .HasColumnName("notes")
            .HasMaxLength(1000);

        builder.Property(ms => ms.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(ms => ms.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .IsRequired();

        // Remove DomainEvents ignore - this entity doesn't have DomainEvents
    }
}