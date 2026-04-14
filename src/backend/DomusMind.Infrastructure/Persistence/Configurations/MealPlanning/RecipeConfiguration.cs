using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.MealPlanning;

public sealed class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("recipes");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasConversion(id => id.Value, value => new RecipeId(value))
            .HasColumnName("id")
            .IsRequired();

        builder.Property(r => r.FamilyId)
            .HasConversion(id => id.Value, value => new FamilyId(value))
            .HasColumnName("family_id")
            .IsRequired();

        builder.Property(r => r.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(r => r.Description)
            .HasColumnName("description")
            .HasMaxLength(2000);

        builder.Property(r => r.PrepTimeMinutes)
            .HasColumnName("prep_time_minutes");

        builder.Property(r => r.CookTimeMinutes)
            .HasColumnName("cook_time_minutes");

        builder.Property(r => r.Servings)
            .HasColumnName("servings");

        builder.Property(r => r.Instructions)
            .HasColumnName("instructions");

        builder.Property(r => r.Notes)
            .HasColumnName("notes");

        builder.Property(r => r.CreatedAtUtc)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(r => r.UpdatedAtUtc)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasMany(r => r.Ingredients)
            .WithOne()
            .HasForeignKey("RecipeId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(r => r.Ingredients)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(r => r.DomainEvents);
    }
}