using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.MealPlanning;

public sealed class ShoppingListItemConfiguration : IEntityTypeConfiguration<ShoppingListItem>
{
    public void Configure(EntityTypeBuilder<ShoppingListItem> builder)
    {
        builder.ToTable("shopping_list_items");

        builder.HasKey(sli => sli.Id);

        builder.Property(sli => sli.Id)
            .HasConversion(id => id.Value, value => new ShoppingListItemId(value))
            .HasColumnName("id")
            .IsRequired();

        builder.Property(sli => sli.IngredientId)
            .HasConversion(id => id.Value, value => new IngredientId(value))
            .HasColumnName("ingredient_id")
            .IsRequired();

        builder.Property(sli => sli.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(sli => sli.Unit)
            .HasColumnName("unit")
            .HasMaxLength(50);

        builder.Property(sli => sli.Notes)
            .HasColumnName("notes")
            .HasMaxLength(1000);

        builder.Property(sli => sli.ShoppingListId)
            .HasConversion(id => id.Value, value => new ShoppingListId(value))
            .HasColumnName("shopping_list_id")
            .IsRequired();

        builder.Property(sli => sli.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        // Ignore DomainEvents property (not present on entity)
    }
}