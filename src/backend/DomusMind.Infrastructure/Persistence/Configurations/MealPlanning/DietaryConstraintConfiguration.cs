using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.MealPlanning;

public sealed class DietaryConstraintConfiguration : IEntityTypeConfiguration<DietaryConstraint>
{
    public void Configure(EntityTypeBuilder<DietaryConstraint> builder)
    {
        builder.ToTable("dietary_constraints");

        builder.HasKey(dc => dc.Id);

        builder.Property(dc => dc.Id)
            .HasConversion(id => id.Value, value => new DietaryConstraintId(value))
            .HasColumnName("id")
            .IsRequired();

        builder.Property(dc => dc.FamilyId)
            .HasConversion(id => id.Value, value => new FamilyId(value))
            .HasColumnName("family_id")
            .IsRequired();

        builder.Property(dc => dc.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(dc => dc.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.Property(dc => dc.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        // Remove DomainEvents ignore - this entity doesn't have DomainEvents
    }
}