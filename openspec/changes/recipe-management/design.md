<!-- pdac-scope: cited -->

## Context

`Recipe` is already a first-class `AggregateRoot<RecipeId>` in the `DomusMind.Domain.MealPlanning` namespace. It has its own identity, raises `RecipeCreated`, and enforces ingredient uniqueness. However, the lifecycle is incomplete: the domain has no `Update`, `Delete`, `RemoveIngredient`, or `UpdateIngredient` methods. At the application layer, only `CreateRecipe` and `GetFamilyRecipes` slices exist. In the web app, recipes are only reachable through the slot inspector inside the weekly meal plan grid — there is no standalone recipe library surface.

<!-- pdac:cite id="BC-MEAL-PLANNING" digest="sha256:3e2d211a42325e9e719480512210f6ec9c5255f4fa647518cae3598e2bd6e835" -->

<!-- pdac:cite id="TERM-RECIPE" digest="sha256:a0a6852e2fe6cc29247e1d0cf8e2159cea630e420d1f40adc5458c02a4e7fcd1" -->

The proposal scope is:
1. Complete the `Recipe` aggregate mutation surface (update metadata, manage ingredients, delete with guard)
2. Add `GetRecipeDetail` for full single-recipe retrieval
3. Add `allowedMealTypes` filtering to `GetFamilyRecipes`
4. Add a standalone recipe library surface in the web app

## Goals / Non-Goals

**Goals:**
- `Recipe` aggregate has a complete mutation surface (create, update, delete, ingredient management)
- API exposes full CRUD for recipes plus ingredient sub-resource endpoints
- Web app has a dedicated recipe library page (list, view, create, edit, delete)
- `allowedMealTypes` filtering is applied in the recipe picker inside the slot inspector
- Spec-stated delete guard (active-slot reference check) is enforced

**Non-Goals:**
- Recipe search beyond name-based client-side filtering
- Recipe import/export
- Per-member ownership (recipes remain household-scoped)
- Nutrition data, photos, or step-by-step instructions
- Redefining meal plan or slot assignment behavior

## Decisions

### 1. Hard delete with active-slot guard (not soft delete)

The spec states a recipe "may not be deleted if currently referenced by any active meal plan slot." This is a guard condition, not a lifecycle state. A soft-delete column would bleed infrastructure concerns into domain semantics and require all queries to filter it. Instead:

- `Recipe` raises a `RecipeDeleted` domain event.
- The `DeleteRecipe` handler queries for active-plan slots referencing the recipe *before* calling `Delete()` on the aggregate. If any exist, it returns a domain error — the aggregate delete is never called.
- The record is physically removed from the database.

<!-- pdac:cite id="BR-RECIPES-DELETE-GUARD" digest="sha256:974103e77810c33d6bd1ecae8266284a454470810f64eb426758fbcda52b8e89" -->

<!-- pdac:cite id="FR-RECIPES-DELETE" digest="sha256:cb195db1bf181dfd62617a60604b4438d2ea9d476d78dbb62fe8c68fc96089a6" -->

<!-- pdac:cite id="UC-RECIPES-DELETE-RECIPE" digest="sha256:b9dabcfacb713eefa11031b17c4992a5eb43a220c2eae3662a9b5bb96de11a85" -->

**Alternative considered**: soft delete with `IsDeleted` flag. Rejected because it complicates `GetFamilyRecipes`, template application, and future queries. The spec guard is enforced at the application layer, which is the right location.

### 2. Ingredients managed as a nested collection on the Recipe aggregate

Ingredients are value objects within `Recipe`. They are not a separate aggregate and do not have their own REST resource from a domain perspective, but the API exposes them via sub-resource routes for ergonomics:

```
POST   /api/recipes/{id}/ingredients
PUT    /api/recipes/{id}/ingredients/{name}
DELETE /api/recipes/{id}/ingredients/{name}
```

Ingredient identity within a recipe is the ingredient name (case-insensitive). There is no separate `IngredientId`. This matches the existing domain invariant and the spec requirement for deduplication in shopping list derivation.

<!-- pdac:cite id="BR-RECIPES-INGREDIENT-NAME-UNIQUE" digest="sha256:e2c4c7915e08a7cd51d1c20b7af3bb4776ead0f8073329d3856d0724f9e7e7f1" -->

<!-- pdac:cite id="TERM-INGREDIENT" digest="sha256:856986e747f9963a266837a7aced64b419cb5e0e31b02af7503dcd6785aecbe5" -->

<!-- pdac:cite id="FR-RECIPES-INGREDIENTS" digest="sha256:7059e97cd2eeaaf68396610bb1f2b4c6a382615c1491df62a94506dce8e70a71" -->

<!-- pdac:cite id="UC-RECIPES-MANAGE-INGREDIENTS" digest="sha256:e46fd68c79fa8d3e690b955ba261bc0853f23fa28a9ae42853f1017faf7fb9cd" -->

<!-- pdac:cite id="FR-RECIPES-UPDATE" digest="sha256:b9a2910eede27b053d5a6ef69a9353774eca4f2410fadc76647c818b50d12fbd" -->

**Domain methods added to `Recipe`**:
- `Update(name, description, prepTime, cookTime, servings, isFavorite, allowedMealTypes, tags)` — full metadata replace
- `AddIngredient(ingredient)` — already exists, unchanged
- `RemoveIngredient(name)` — removes by name, case-insensitive
- `UpdateIngredient(name, newQuantity, newUnit)` — finds by name, replaces quantity/unit
- `Delete()` — raises `RecipeDeleted`, marks aggregate for removal

### 3. `GetFamilyRecipes` extended with optional `mealType` filter

`GET /api/recipes/family/{familyId}?mealType=Breakfast` — when `mealType` is provided, returns only recipes where `AllowedMealTypes` is empty (no restriction) or contains the given meal type. This matches the spec scenario.

<!-- pdac:cite id="BR-RECIPES-ALLOWED-MEAL-TYPES" digest="sha256:4b2522790207b70224c50687e2a59dc8261512ae7154781dfcf7185f2986f20f" -->

<!-- pdac:cite id="FR-RECIPES-LIBRARY" digest="sha256:702c8c7996f89cc2fd0f678d4a163db664aae0ca384e742133ad8cdd9001bcfc" -->

<!-- pdac:cite id="UC-RECIPES-BROWSE-LIBRARY" digest="sha256:457336010457babd21f0b02fee26cef2f00e543726236d9e802629ed8f98833f" -->

<!-- pdac:cite id="UC-MEALS-UPDATE-SLOT" digest="sha256:717756771462a30e271a209fda42ebbdbad3244f7544420f05079851a42bd301" -->

The `RecipePickerPanel` component passes the current slot's meal type as a query parameter when fetching or filtering. The existing client-side name search is preserved.

**Slice path**: `Features/MealPlanning/GetFamilyRecipes/` — extend `GetFamilyRecipesQuery` with `MealType?` parameter; update handler.

### 4. `GetRecipeDetail` returns full recipe including ingredient list

`GET /api/recipes/{id}` returns full recipe detail including the ingredient list. `GetFamilyRecipesResponse` continues to return `IngredientCount` (not full ingredients) for the list view — this is appropriate for a summary.

<!-- pdac:cite id="FR-RECIPES-DETAIL" digest="sha256:0e0d422c5299e2fd97d969a06cb652814c18c14c9c7ae459bf3c964d29caca3b" -->

<!-- pdac:cite id="UC-RECIPES-VIEW-RECIPE" digest="sha256:06c252bac16722b4a109c9bcc28fce527edc4e44d5f22e09c222efa889df90ca" -->

**New slice**: `Features/MealPlanning/GetRecipeDetail/`  
**New contract**: `GetRecipeDetailResponse` includes `IReadOnlyList<IngredientDetail>`.

### 5. Web app recipe library as a new feature folder

The recipe library does not belong inside `features/meal-planning` — it is a peer capability, not a sub-view of the weekly grid. A new `features/recipe-library` folder is introduced.

<!-- pdac:cite id="FR-RECIPES-LIBRARY-SURFACE" digest="sha256:39fb69cb69311e5b14ddf29498d7f94a462cc0540eac56770c15680feccf6a5f" -->

<!-- pdac:cite id="UC-RECIPES-BROWSE-LIBRARY" digest="sha256:457336010457babd21f0b02fee26cef2f00e543726236d9e802629ed8f98833f" -->

Route: `/recipes` (or `/meal-planning/recipes` — TBD during implementation, but structurally separate from the week view).

State is managed via a new Redux slice `recipeLibrarySlice` (or by extending `mealPlanningSlice` with recipe-library actions — prefer a separate slice to avoid growing `mealPlanningSlice` further).

The `CreateRecipeModal` in the slot inspector is kept but optionally routes to the full recipe form after creation. A new `RecipeFormPage` or `EditRecipeModal` is introduced in `recipe-library`.

## Risks / Trade-offs

- **Ingredient identity by name** → Any rename of an ingredient requires remove + add, which will change the ingredient list order. Mitigation: document this in the spec; no client-side reorder is promised.
- **Active-slot check at delete** → Performed as a projection query in the handler. This is a point-in-time check; a race condition exists if two users act concurrently (one deletes, one assigns). Mitigation: acceptable for a household-scoped product; database constraint or event ordering is out of scope here.
- **`mealType` filter change to `GetFamilyRecipes`** → Existing callers (slot inspector) pass no filter today and get all recipes — this is backward-compatible. New callers can pass the filter. No breaking change.
- **`CreateRecipeModal` scope** → The existing quick-add modal in the slot inspector cannot capture ingredients. After this change, a recipe created inline will have no ingredients until edited in the library surface. Shopping list derivation will produce no items from it. This is an acceptable known limitation; the UI should communicate it (e.g., "No ingredients — add them in the recipe library").

<!-- pdac:cite id="UC-RECIPES-CREATE-RECIPE" digest="sha256:c9aba5e8213215812c9767138d04130ab2fa2875d493c7a72f060f8cbd2b6e35" -->

<!-- pdac:cite id="BR-RECIPES-INGREDIENT-NAME-UNIQUE" digest="sha256:e2c4c7915e08a7cd51d1c20b7af3bb4776ead0f8073329d3856d0724f9e7e7f1" -->
