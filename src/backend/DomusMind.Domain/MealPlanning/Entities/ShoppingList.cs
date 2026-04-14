using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.Events;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Entities;

public sealed class ShoppingList : AggregateRoot<ShoppingListId>
{
    public FamilyId FamilyId { get; private set; }
    
    public string Name { get; private set; }
    
    public MealPlanId? GeneratedFromMealPlanId { get; private set; }
    
    public DateTime CreatedAtUtc { get; private set; }
    
    public DateTime UpdatedAtUtc { get; private set; }
    
    private readonly List<ShoppingListItem> _items = new();
    public IReadOnlyList<ShoppingListItem> Items => _items.AsReadOnly();

    // Parameterless constructor for EF Core
    private ShoppingList() : base(default!)
    {
        Name = string.Empty; // Initialize with default value to satisfy non-null requirement
    }

    private ShoppingList(ShoppingListId id, FamilyId familyId, string name, DateTime createdAtUtc, DateTime updatedAtUtc) : base(id)
    {
        FamilyId = familyId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
    }

    public static ShoppingList Create(ShoppingListId id, FamilyId familyId, string name, DateTime createdAtUtc, DateTime updatedAtUtc)
    {
        var shoppingList = new ShoppingList(id, familyId, name, createdAtUtc, updatedAtUtc);
        shoppingList.RaiseDomainEvent(new ShoppingListGenerated(Guid.NewGuid(), id.Value, Guid.Empty, createdAtUtc));
        return shoppingList;
    }

    public void AddItem(ShoppingListItem item)
    {
        _items.Add(item);
    }

    public void RemoveItem(ShoppingListItem item)
    {
        _items.Remove(item);
    }

    public void SetGeneratedFromMealPlan(MealPlanId mealPlanId)
    {
        GeneratedFromMealPlanId = mealPlanId;
    }
}