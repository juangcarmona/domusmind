<!-- pdac-scope: cited -->

## Context

`Recipe` is already a first-class `AggregateRoot<RecipeId>` in the `DomusMind.Domain.MealPlanning` namespace. It has its own identity, raises `RecipeCreated`, and enforces ingredient uniqueness. However, the lifecycle is incomplete: the domain has no `Update`, `Delete`, `RemoveIngredient`, or `UpdateIngredient` methods. At the application layer, only `CreateRecipe` and `GetFamilyRecipes` slices exist. In the web app, recipes are only reachable through the slot inspector inside the weekly meal plan grid — there is no standalone recipe library surface.

<!-- pdac:cite id="BC-MEAL-PLANNING" digest="sha256:bb3de4faf9d7ae3434d0db4180f005236ef81bc3ab84875aaa364e97ac3e66bd" -->

<!-- pdac:cite id="TERM-RECIPE" digest="sha256:3c619b3dae05589a60edac9f0ed0b63a78a963b1070a73cb6c4c5b76e9a570b4" -->

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

<!-- pdac:cite id="BR-RECIPES-DELETE-GUARD" digest="sha256:2e15467fd305be37d291774e1e09606ef41448d1713b4f98d5ff2cbce2ada86e" -->

<!-- pdac:cite id="FR-RECIPES-DELETE" digest="sha256:7ad724d99da2ba8bd340eedddcf5d47e338caef5ccfecbf0a3628252da8e6015" -->

<!-- pdac:cite id="UC-RECIPES-DELETE-RECIPE" digest="sha256:47bb5bbb4aaa4ff7c6a788ca533d974aa3fa9561177752b8ca6dfc7c55ff8b56" -->

**Alternative considered**: soft delete with `IsDeleted` flag. Rejected because it complicates `GetFamilyRecipes`, template application, and future queries. The spec guard is enforced at the application layer, which is the right location.

### 2. Ingredients managed as a nested collection on the Recipe aggregate

Ingredients are value objects within `Recipe`. They are not a separate aggregate and do not have their own REST resource from a domain perspective, but the API exposes them via sub-resource routes for ergonomics:

```
POST   /api/recipes/{id}/ingredients
PUT    /api/recipes/{id}/ingredients/{name}
DELETE /api/recipes/{id}/ingredients/{name}
```

Ingredient identity within a recipe is the ingredient name (case-insensitive). There is no separate `IngredientId`. This matches the existing domain invariant and the spec requirement for deduplication in shopping list derivation.

<!-- pdac:cite id="BR-RECIPES-INGREDIENT-NAME-UNIQUE" digest="sha256:49bf51a252a84807602bd53fcbbe8754840772a71957f7108738501f3cc4e0a0" -->

<!-- pdac:cite id="TERM-INGREDIENT" digest="sha256:e70048c508c2ac43cbd9797dc538c97f9240f658fc0cffcd73c937c64c1586d8" -->

<!-- pdac:cite id="FR-RECIPES-INGREDIENTS" digest="sha256:82e9ab23dc511501d2a8f51222e43dc395c8f99365dd238e91dabebae50b745b" -->

<!-- pdac:cite id="UC-RECIPES-MANAGE-INGREDIENTS" digest="sha256:1aa4b2786d2d9f085881e2c21e1b3622f14f13f9ea52a1813f0a93e319712f00" -->

<!-- pdac:cite id="FR-RECIPES-UPDATE" digest="sha256:beed777fe2618300d42fcb98eb5e6193fc8c62aa1e05a00957e43ed27ed57d83" -->

**Domain methods added to `Recipe`**:
- `Update(name, description, prepTime, cookTime, servings, isFavorite, allowedMealTypes, tags)` — full metadata replace
- `AddIngredient(ingredient)` — already exists, unchanged
- `RemoveIngredient(name)` — removes by name, case-insensitive
- `UpdateIngredient(name, newQuantity, newUnit)` — finds by name, replaces quantity/unit
- `Delete()` — raises `RecipeDeleted`, marks aggregate for removal

### 3. `GetFamilyRecipes` extended with optional `mealType` filter

`GET /api/recipes/family/{familyId}?mealType=Breakfast` — when `mealType` is provided, returns only recipes where `AllowedMealTypes` is empty (no restriction) or contains the given meal type. This matches the spec scenario.

<!-- pdac:cite id="BR-RECIPES-ALLOWED-MEAL-TYPES" digest="sha256:d5576dfc67764576f0e2d0f681e4c56655eb6be09ff5e5ed95b3c1fc87241f3a" -->

<!-- pdac:cite id="FR-RECIPES-LIBRARY" digest="sha256:1340dae095e80b3c9ea2f47bb1780af2e6ae2acbd28baeb5750cd532bd02d34d" -->

<!-- pdac:cite id="UC-RECIPES-BROWSE-LIBRARY" digest="sha256:fbcff790cf18adf9b802721d066aafff3e01dcd239d62fb5ff197f8e629837e9" -->

<!-- pdac:cite id="UC-MEALS-UPDATE-SLOT" digest="sha256:a659fa4c170ec938919501335736ec5ea87ea35c81cc207d793db62e5a6c9d74" -->

The `RecipePickerPanel` component passes the current slot's meal type as a query parameter when fetching or filtering. The existing client-side name search is preserved.

**Slice path**: `Features/MealPlanning/GetFamilyRecipes/` — extend `GetFamilyRecipesQuery` with `MealType?` parameter; update handler.

### 4. `GetRecipeDetail` returns full recipe including ingredient list

`GET /api/recipes/{id}` returns full recipe detail including the ingredient list. `GetFamilyRecipesResponse` continues to return `IngredientCount` (not full ingredients) for the list view — this is appropriate for a summary.

<!-- pdac:cite id="FR-RECIPES-DETAIL" digest="sha256:6d8e670e1225300aac4a0a0490b6da9ac83d30d3d2ffda66dd6204fe08ab59e6" -->

<!-- pdac:cite id="UC-RECIPES-VIEW-RECIPE" digest="sha256:b66dd0aeafaea8725a3204492555ff0a40f50dcd0067c1e2844992e41f5a7c91" -->

**New slice**: `Features/MealPlanning/GetRecipeDetail/`  
**New contract**: `GetRecipeDetailResponse` includes `IReadOnlyList<IngredientDetail>`.

### 5. Web app recipe library as a new feature folder

The recipe library does not belong inside `features/meal-planning` — it is a peer capability, not a sub-view of the weekly grid. A new `features/recipe-library` folder is introduced.

<!-- pdac:cite id="FR-RECIPES-LIBRARY-SURFACE" digest="sha256:c8ab313c8ada872d7cdec7c39b58bc2afef4e736fe51b286f34b398ad93b5858" -->

<!-- pdac:cite id="UC-RECIPES-BROWSE-LIBRARY" digest="sha256:fbcff790cf18adf9b802721d066aafff3e01dcd239d62fb5ff197f8e629837e9" -->

Route: `/recipes` (or `/meal-planning/recipes` — TBD during implementation, but structurally separate from the week view).

State is managed via a new Redux slice `recipeLibrarySlice` (or by extending `mealPlanningSlice` with recipe-library actions — prefer a separate slice to avoid growing `mealPlanningSlice` further).

The `CreateRecipeModal` in the slot inspector is kept but optionally routes to the full recipe form after creation. A new `RecipeFormPage` or `EditRecipeModal` is introduced in `recipe-library`.

## Risks / Trade-offs

- **Ingredient identity by name** → Any rename of an ingredient requires remove + add, which will change the ingredient list order. Mitigation: document this in the spec; no client-side reorder is promised.
- **Active-slot check at delete** → Performed as a projection query in the handler. This is a point-in-time check; a race condition exists if two users act concurrently (one deletes, one assigns). Mitigation: acceptable for a household-scoped product; database constraint or event ordering is out of scope here.
- **`mealType` filter change to `GetFamilyRecipes`** → Existing callers (slot inspector) pass no filter today and get all recipes — this is backward-compatible. New callers can pass the filter. No breaking change.
- **`CreateRecipeModal` scope** → The existing quick-add modal in the slot inspector cannot capture ingredients. After this change, a recipe created inline will have no ingredients until edited in the library surface. Shopping list derivation will produce no items from it. This is an acceptable known limitation; the UI should communicate it (e.g., "No ingredients — add them in the recipe library").

<!-- pdac:cite id="UC-RECIPES-CREATE-RECIPE" digest="sha256:b1982c4e6063006dac46caf79a882dbb237450836e34795ea7dd1751b8ce02a8" -->

<!-- pdac:cite id="BR-RECIPES-INGREDIENT-NAME-UNIQUE" digest="sha256:49bf51a252a84807602bd53fcbbe8754840772a71957f7108738501f3cc4e0a0" -->
