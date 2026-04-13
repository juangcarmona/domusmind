using DomusMind.Domain.Abstractions;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Entities;

public sealed class Ingredient : Entity<IngredientId>
{
    public string Name { get; private set; }
    
    public decimal Quantity { get; private set; }
    
    public string? Unit { get; private set; }
    
    public RecipeId RecipeId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    // Parameterless constructor for EF Core
    private Ingredient() : base(default!)
    {
        Name = string.Empty; // Initialize with default value to satisfy non-null requirement
        RecipeId = default!; // Initialize with default value to satisfy non-null requirement
    }

    public Ingredient(IngredientId id, string name, decimal quantity, string? unit, RecipeId recipeId) : base(id)
    {
        Name = name;
        Quantity = quantity;
        Unit = unit;
        RecipeId = recipeId;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string? name = null, decimal? quantity = null, string? unit = null)
    {
        if (!string.IsNullOrEmpty(name))
            Name = name;
            
        if (quantity.HasValue)
            Quantity = quantity.Value;
            
        if (unit != null)
            Unit = unit;
    }
}