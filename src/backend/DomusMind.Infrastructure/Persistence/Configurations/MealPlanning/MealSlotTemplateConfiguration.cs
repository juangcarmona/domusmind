using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.MealPlanning;

public sealed class MealSlotTemplateConfiguration : IEntityTypeConfiguration<MealSlotTemplate>
{
    public void Configure(EntityTypeBuilder<MealSlotTemplate> builder)
    {
        builder.ToTable("meal_slot_templates");

        builder.HasKey(mst => mst.Id);

        builder.Property(mst => mst.Id)
            .HasConversion(id => id.Value, value => new MealSlotTemplateId(value))
            .HasColumnName("id")
            .IsRequired();

        builder.Property(mst => mst.DayOfWeek)
            .HasConversion(d => d.ToString(), value => Enum.Parse<DomusMind.Domain.MealPlanning.Enums.DayOfWeek>(value))
            .HasColumnName("day_of_week")
            .IsRequired();

        builder.Property(mst => mst.MealType)
            .HasConversion(m => m.ToString(), value => Enum.Parse<MealType>(value))
            .HasColumnName("meal_type")
            .IsRequired();

        builder.Property(mst => mst.WeeklyTemplateId)
            .HasConversion(id => id.Value, value => new WeeklyTemplateId(value))
            .HasColumnName("weekly_template_id")
            .IsRequired();

        builder.Property(mst => mst.RecipeId)
            .HasConversion(                
                id => id.HasValue ? id.Value.Value : (Guid?)null,
                value => value.HasValue ? new RecipeId(value.Value) : (RecipeId?)null)
            .HasColumnName("recipe_id");

        builder.Property(mst => mst.Notes)
            .HasColumnName("notes")
            .HasMaxLength(1000);

        builder.Property(mst => mst.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(mst => mst.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        // Remove DomainEvents ignore - this entity doesn't have DomainEvents
    }
}