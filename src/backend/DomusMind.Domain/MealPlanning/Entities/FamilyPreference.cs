using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Entities;

public sealed class FamilyPreference : Entity<FamilyPreferenceId>
{
    public FamilyId FamilyId { get; private set; }
    
    public List<MealType> PreferredMealTypes { get; private set; }
    
    public bool WeekendFlexibility { get; private set; }
    
    public List<DietaryConstraintId> DefaultDietaryConstraints { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }

    // Parameterless constructor for EF Core
    private FamilyPreference() : base(default!)
    {
        PreferredMealTypes = new List<MealType>(); // Initialize with empty list to satisfy non-null requirement
        DefaultDietaryConstraints = new List<DietaryConstraintId>(); // Initialize with empty list to satisfy non-null requirement
    }

    public FamilyPreference(FamilyPreferenceId id, FamilyId familyId, List<MealType> preferredMealTypes, bool weekendFlexibility, 
        List<DietaryConstraintId> defaultDietaryConstraints, DateTime createdAt, DateTime updatedAt) : base(id)
    {
        FamilyId = familyId;
        PreferredMealTypes = preferredMealTypes ?? throw new ArgumentNullException(nameof(preferredMealTypes));
        WeekendFlexibility = weekendFlexibility;
        DefaultDietaryConstraints = defaultDietaryConstraints ?? throw new ArgumentNullException(nameof(defaultDietaryConstraints));
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public void Update(List<MealType>? preferredMealTypes = null, bool? weekendFlexibility = null, 
        List<DietaryConstraintId>? defaultDietaryConstraints = null)
    {
        if (preferredMealTypes != null)
            PreferredMealTypes = preferredMealTypes;
            
        if (weekendFlexibility.HasValue)
            WeekendFlexibility = weekendFlexibility.Value;
            
        if (defaultDietaryConstraints != null)
            DefaultDietaryConstraints = defaultDietaryConstraints;
            
        UpdatedAt = DateTime.UtcNow;
    }
}