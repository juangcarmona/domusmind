using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.MealPlanning;

public sealed class ShoppingListConfiguration : IEntityTypeConfiguration<ShoppingList>
{
    public void Configure(EntityTypeBuilder<ShoppingList> builder)
    {
        builder.ToTable("shopping_lists");

        builder.HasKey(sl => sl.Id);

        builder.Property(sl => sl.Id)
            .HasConversion(id => id.Value, value => new ShoppingListId(value))
            .HasColumnName("id")
            .IsRequired();

        builder.Property(sl => sl.FamilyId)
            .HasConversion(id => id.Value, value => new FamilyId(value))
            .HasColumnName("family_id")
            .IsRequired();

        builder.Property(sl => sl.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(sl => sl.GeneratedFromMealPlanId)
            .HasConversion(
                id => id.HasValue ? id.Value.Value : (Guid?)null,
                value => value.HasValue ? new MealPlanId(value.Value) : null)
            .HasColumnName("generated_from_meal_plan_id");

        builder.Property(sl => sl.CreatedAtUtc)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(sl => sl.UpdatedAtUtc)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasMany(sl => sl.Items)
            .WithOne()
            .HasForeignKey("ShoppingListId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(sl => sl.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Remove DomainEvents ignore - this entity doesn't have DomainEvents
    }
}