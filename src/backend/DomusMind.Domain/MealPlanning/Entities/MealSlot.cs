using DomusMind.Domain.Abstractions;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Entities;

public sealed class MealSlot : Entity<MealSlotId>
{
    public Enums.DayOfWeek DayOfWeek { get; private set; }
    
    public MealType MealType { get; private set; }
    
    public MealPlanId MealPlanId { get; private set; }
    
    public RecipeId? RecipeId { get; private set; }
    
    public string? Notes { get; private set; }
    
    public DateTime CreatedAtUtc { get; private set; }
    
    public DateTime UpdatedAtUtc { get; private set; }

    // Parameterless constructor for EF Core
    private MealSlot() : base(default!)
    {
        MealPlanId = default!; // Initialize with default value to satisfy non-null requirement
    }

    private MealSlot(MealSlotId id, Enums.DayOfWeek dayOfWeek, MealType mealType, MealPlanId mealPlanId, RecipeId? recipeId, string? notes, DateTime createdAtUtc, DateTime updatedAtUtc) : base(id)
    {
        DayOfWeek = dayOfWeek;
        MealType = mealType;
        MealPlanId = mealPlanId;
        RecipeId = recipeId;
        Notes = notes;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
    }

    public static MealSlot Create(MealSlotId id, Enums.DayOfWeek dayOfWeek, MealType mealType, MealPlanId mealPlanId, RecipeId? recipeId, string? notes, DateTime createdAtUtc, DateTime updatedAtUtc)
    {
        return new MealSlot(id, dayOfWeek, mealType, mealPlanId, recipeId, notes, createdAtUtc, updatedAtUtc);
    }

    public void Update(MealType? mealType = null, RecipeId? recipeId = null, string? notes = null)
    {
        if (mealType.HasValue)
            MealType = mealType.Value;
            
        if (recipeId != null)
            RecipeId = recipeId;
            
        if (notes != null)
            Notes = notes;
            
        UpdatedAtUtc = DateTime.UtcNow;
    }
}