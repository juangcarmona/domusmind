using DomusMind.Domain.Abstractions;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Entities;

public sealed class ShoppingListItem : Entity<ShoppingListItemId>
{
    public IngredientId IngredientId { get; private set; }
    
    public decimal Quantity { get; private set; }
    
    public string? Unit { get; private set; }
    
    public string? Notes { get; private set; }
    
    public ShoppingListId ShoppingListId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }


    public ShoppingListItem(ShoppingListItemId id, IngredientId ingredientId, decimal quantity, 
        string? unit, string? notes, ShoppingListId shoppingListId) : base(id)
    {
        IngredientId = ingredientId;
        Quantity = quantity;
        Unit = unit;
        Notes = notes;
        ShoppingListId = shoppingListId;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(decimal? quantity = null, string? unit = null, string? notes = null)
    {
        if (quantity.HasValue)
            Quantity = quantity.Value;
            
        if (unit != null)
            Unit = unit;
            
        if (notes != null)
            Notes = notes;
    }
}