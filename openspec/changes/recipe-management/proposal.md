<!-- pdac-scope: cited -->

## Why

Recipe is a first-class aggregate in the Meal Planning context, but its lifecycle is incomplete. Creation exists; update, delete, and ingredient editing do not. The only surface entry point is the slot inspector inside the weekly meal plan — there is no way to browse, view, or manage the recipe library independently. Several spec-stated requirements (recipe update, delete guard, `allowedMealTypes` filtering) are unimplemented. Completing the recipe lifecycle unblocks shopping list accuracy, reuse through templates, and a coherent recipe library experience.

<!-- pdac:cite id="BC-MEAL-PLANNING" digest="sha256:bb3de4faf9d7ae3434d0db4180f005236ef81bc3ab84875aaa364e97ac3e66bd" -->

<!-- pdac:cite id="TERM-RECIPE" digest="sha256:3c619b3dae05589a60edac9f0ed0b63a78a963b1070a73cb6c4c5b76e9a570b4" -->

## What Changes

- Add `UpdateRecipe` capability: name, description, times, servings, tags, `allowedMealTypes`, `isFavorite`
- Add `DeleteRecipe` capability: guarded by active-plan-slot check
- Add `GetRecipeDetail` capability: returns full recipe including ingredients
- Add `AddIngredient` / `RemoveIngredient` / `UpdateIngredient` capabilities on a recipe (post-create mutation)
- Enforce `allowedMealTypes` filtering in `GetFamilyRecipes` when a slot context is provided
- Complete the `CreateRecipe` UI form: expose ingredients, tags, `allowedMealTypes`, `isFavorite`
- Add a standalone recipe library surface in the web app (browse, view, create, edit, delete)

<!-- pdac:cite id="FR-RECIPES-UPDATE" digest="sha256:733871393da840075fb57813e3e5453c6a84949ea91f7a51568168e3a7d0894b" -->

<!-- pdac:cite id="UC-RECIPES-UPDATE-RECIPE" digest="sha256:0fb0ec3cf0b3ef1ad1159d45b722e6be18e00d694f7290a93b9a29f5a26c27f5" -->

<!-- pdac:cite id="BR-RECIPES-NAME-UNIQUE" digest="sha256:54fe88bd63ffb8100bf3dbb97bf05c0275a1dfcc89645b9659c013783a93e7a8" -->

<!-- pdac:cite id="FR-RECIPES-DELETE" digest="sha256:8c2e5784a8e8bd51c1e1709dba553bb976376fe7e3011801831423d4c2e1d63b" -->

<!-- pdac:cite id="UC-RECIPES-DELETE-RECIPE" digest="sha256:47bb5bbb4aaa4ff7c6a788ca533d974aa3fa9561177752b8ca6dfc7c55ff8b56" -->

<!-- pdac:cite id="BR-RECIPES-DELETE-GUARD" digest="sha256:2e15467fd305be37d291774e1e09606ef41448d1713b4f98d5ff2cbce2ada86e" -->

<!-- pdac:cite id="FR-RECIPES-DETAIL" digest="sha256:6d8e670e1225300aac4a0a0490b6da9ac83d30d3d2ffda66dd6204fe08ab59e6" -->

<!-- pdac:cite id="UC-RECIPES-VIEW-RECIPE" digest="sha256:b66dd0aeafaea8725a3204492555ff0a40f50dcd0067c1e2844992e41f5a7c91" -->

<!-- pdac:cite id="FR-RECIPES-INGREDIENTS" digest="sha256:82e9ab23dc511501d2a8f51222e43dc395c8f99365dd238e91dabebae50b745b" -->

<!-- pdac:cite id="UC-RECIPES-MANAGE-INGREDIENTS" digest="sha256:1aa4b2786d2d9f085881e2c21e1b3622f14f13f9ea52a1813f0a93e319712f00" -->

<!-- pdac:cite id="BR-RECIPES-INGREDIENT-NAME-UNIQUE" digest="sha256:49bf51a252a84807602bd53fcbbe8754840772a71957f7108738501f3cc4e0a0" -->

<!-- pdac:cite id="BR-RECIPES-ALLOWED-MEAL-TYPES" digest="sha256:d5576dfc67764576f0e2d0f681e4c56655eb6be09ff5e5ed95b3c1fc87241f3a" -->

<!-- pdac:cite id="UC-RECIPES-CREATE-RECIPE" digest="sha256:b1982c4e6063006dac46caf79a882dbb237450836e34795ea7dd1751b8ce02a8" -->

<!-- pdac:cite id="FR-RECIPES-LIBRARY" digest="sha256:1340dae095e80b3c9ea2f47bb1780af2e6ae2acbd28baeb5750cd532bd02d34d" -->

<!-- pdac:cite id="FR-RECIPES-LIBRARY-SURFACE" digest="sha256:c8ab313c8ada872d7cdec7c39b58bc2afef4e736fe51b286f34b398ad93b5858" -->

<!-- pdac:cite id="UC-RECIPES-BROWSE-LIBRARY" digest="sha256:fbcff790cf18adf9b802721d066aafff3e01dcd239d62fb5ff197f8e629837e9" -->

Out of scope for this change:
- Redefining the Meal Plan or slot assignment model
- Recipe search beyond name-based filtering
- Recipe import/export
- Per-member recipe ownership (recipes are always household-scoped)

<!-- pdac:cite id="BR-MEALS-HOUSEHOLD-SCOPED-MEALS" digest="sha256:ef16950ff7825bfa0a1af1d5492432640c6f8ab77987aa3bf0fd4b20ca6aaa26" -->

## Capabilities

### New Capabilities

- `recipe-management`: Full lifecycle management of a household recipe — update, delete (with guard), detail retrieval, and ingredient post-create mutation. Includes the web app recipe library surface.

### Modified Capabilities

- `meal-planning`: Add `allowedMealTypes` filtering to `GetFamilyRecipes` when a slot context is provided. No other meal plan behavior changes.

<!-- pdac:cite id="BR-RECIPES-ALLOWED-MEAL-TYPES" digest="sha256:d5576dfc67764576f0e2d0f681e4c56655eb6be09ff5e5ed95b3c1fc87241f3a" -->

<!-- pdac:cite id="UC-MEALS-UPDATE-SLOT" digest="sha256:a659fa4c170ec938919501335736ec5ea87ea35c81cc207d793db62e5a6c9d74" -->

## Impact

**Backend**
- `DomusMind.Domain`: Add `Update`, `Delete` (tombstone or removal), `RemoveIngredient`, `UpdateIngredient` methods to `Recipe` aggregate
- `DomusMind.Application`: New slices — `UpdateRecipe`, `DeleteRecipe`, `GetRecipeDetail`, `AddRecipeIngredient`, `RemoveRecipeIngredient`, `UpdateRecipeIngredient`; update `GetFamilyRecipes` to accept optional `mealType` filter
- `DomusMind.Contracts`: New request/response records for update, delete, detail
- `DomusMind.Api`: New endpoints on `RecipesController` — `GET /api/recipes/{id}`, `PUT /api/recipes/{id}`, `DELETE /api/recipes/{id}`, ingredient sub-resource endpoints
- EF migration required if soft-delete or new column is added

**Frontend**
- New `recipe-library` feature folder
- `RecipesPage` — standalone library surface (list + filter + create)
- `RecipeDetailPanel` or page — full recipe view with ingredients
- `EditRecipeModal` or page — full edit form including ingredients
- Update `CreateRecipeModal` to expose remaining fields (ingredients, tags, `allowedMealTypes`, `isFavorite`) or route to the new full form
- Update `RecipePickerPanel` to filter by `allowedMealTypes` when slot meal type is known
