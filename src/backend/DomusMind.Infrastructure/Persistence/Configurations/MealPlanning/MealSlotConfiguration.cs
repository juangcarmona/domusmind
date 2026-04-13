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
            .HasConversion(id => id.Value, value => new MealSlotId(value))
            .HasColumnName("id")
            .IsRequired();

        builder.Property(ms => ms.DayOfWeek)
            .HasConversion(d => d.ToString(), value => Enum.Parse<DomusMind.Domain.MealPlanning.Enums.DayOfWeek>(value))
            .HasColumnName("day_of_week")
            .IsRequired();

        builder.Property(ms => ms.MealType)
            .HasConversion(m => m.ToString(), value => Enum.Parse<MealType>(value))
            .HasColumnName("meal_type")
            .IsRequired();

        builder.Property(ms => ms.MealPlanId)
            .HasConversion(id => id.Value, value => new MealPlanId(value))
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

        builder.Property(ms => ms.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(ms => ms.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        // Remove DomainEvents ignore - this entity doesn't have DomainEvents
    }
}