using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Entities;

public sealed class DietaryConstraint : Entity<DietaryConstraintId>
{
    public FamilyId FamilyId { get; private set; }
    
    public string Name { get; private set; }
    
    public string? Description { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    // Parameterless constructor for EF Core
    private DietaryConstraint() : base(default!)
    {
        Name = string.Empty; // Initialize with default value to satisfy non-null requirement
    }

    public DietaryConstraint(DietaryConstraintId id, FamilyId familyId, string name, string? description, DateTime createdAt) : base(id)
    {
        FamilyId = familyId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        CreatedAt = createdAt;
    }

    public void Update(string? name = null, string? description = null)
    {
        if (!string.IsNullOrEmpty(name))
            Name = name;
            
        if (description != null)
            Description = description;
    }
}