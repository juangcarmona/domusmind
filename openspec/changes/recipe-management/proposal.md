<!-- pdac-scope: cited -->

## Why

Recipe is a first-class aggregate in the Meal Planning context, but its lifecycle is incomplete. Creation exists; update, delete, and ingredient editing do not. The only surface entry point is the slot inspector inside the weekly meal plan — there is no way to browse, view, or manage the recipe library independently. Several spec-stated requirements (recipe update, delete guard, `allowedMealTypes` filtering) are unimplemented. Completing the recipe lifecycle unblocks shopping list accuracy, reuse through templates, and a coherent recipe library experience.

<!-- pdac:cite id="BC-MEAL-PLANNING" digest="sha256:3e2d211a42325e9e719480512210f6ec9c5255f4fa647518cae3598e2bd6e835" -->

<!-- pdac:cite id="TERM-RECIPE" digest="sha256:a0a6852e2fe6cc29247e1d0cf8e2159cea630e420d1f40adc5458c02a4e7fcd1" -->

## What Changes

- Add `UpdateRecipe` capability: name, description, times, servings, tags, `allowedMealTypes`, `isFavorite`
- Add `DeleteRecipe` capability: guarded by active-plan-slot check
- Add `GetRecipeDetail` capability: returns full recipe including ingredients
- Add `AddIngredient` / `RemoveIngredient` / `UpdateIngredient` capabilities on a recipe (post-create mutation)
- Enforce `allowedMealTypes` filtering in `GetFamilyRecipes` when a slot context is provided
- Complete the `CreateRecipe` UI form: expose ingredients, tags, `allowedMealTypes`, `isFavorite`
- Add a standalone recipe library surface in the web app (browse, view, create, edit, delete)

<!-- pdac:cite id="FR-RECIPES-UPDATE" digest="sha256:b9a2910eede27b053d5a6ef69a9353774eca4f2410fadc76647c818b50d12fbd" -->

<!-- pdac:cite id="UC-RECIPES-UPDATE-RECIPE" digest="sha256:6825e998420a1234fc86e999215f082e17c4796a7ea709d40d20818b02b0a8bd" -->

<!-- pdac:cite id="BR-RECIPES-NAME-UNIQUE" digest="sha256:566fe524fa65c59602ade22e42e52cf55d27b6682382bf6c92db1e86f0710150" -->

<!-- pdac:cite id="FR-RECIPES-DELETE" digest="sha256:cb195db1bf181dfd62617a60604b4438d2ea9d476d78dbb62fe8c68fc96089a6" -->

<!-- pdac:cite id="UC-RECIPES-DELETE-RECIPE" digest="sha256:b9dabcfacb713eefa11031b17c4992a5eb43a220c2eae3662a9b5bb96de11a85" -->

<!-- pdac:cite id="BR-RECIPES-DELETE-GUARD" digest="sha256:974103e77810c33d6bd1ecae8266284a454470810f64eb426758fbcda52b8e89" -->

<!-- pdac:cite id="FR-RECIPES-DETAIL" digest="sha256:0e0d422c5299e2fd97d969a06cb652814c18c14c9c7ae459bf3c964d29caca3b" -->

<!-- pdac:cite id="UC-RECIPES-VIEW-RECIPE" digest="sha256:06c252bac16722b4a109c9bcc28fce527edc4e44d5f22e09c222efa889df90ca" -->

<!-- pdac:cite id="FR-RECIPES-INGREDIENTS" digest="sha256:7059e97cd2eeaaf68396610bb1f2b4c6a382615c1491df62a94506dce8e70a71" -->

<!-- pdac:cite id="UC-RECIPES-MANAGE-INGREDIENTS" digest="sha256:e46fd68c79fa8d3e690b955ba261bc0853f23fa28a9ae42853f1017faf7fb9cd" -->

<!-- pdac:cite id="BR-RECIPES-INGREDIENT-NAME-UNIQUE" digest="sha256:e2c4c7915e08a7cd51d1c20b7af3bb4776ead0f8073329d3856d0724f9e7e7f1" -->

<!-- pdac:cite id="BR-RECIPES-ALLOWED-MEAL-TYPES" digest="sha256:4b2522790207b70224c50687e2a59dc8261512ae7154781dfcf7185f2986f20f" -->

<!-- pdac:cite id="UC-RECIPES-CREATE-RECIPE" digest="sha256:c9aba5e8213215812c9767138d04130ab2fa2875d493c7a72f060f8cbd2b6e35" -->

<!-- pdac:cite id="FR-RECIPES-LIBRARY" digest="sha256:702c8c7996f89cc2fd0f678d4a163db664aae0ca384e742133ad8cdd9001bcfc" -->

<!-- pdac:cite id="FR-RECIPES-LIBRARY-SURFACE" digest="sha256:39fb69cb69311e5b14ddf29498d7f94a462cc0540eac56770c15680feccf6a5f" -->

<!-- pdac:cite id="UC-RECIPES-BROWSE-LIBRARY" digest="sha256:457336010457babd21f0b02fee26cef2f00e543726236d9e802629ed8f98833f" -->

Out of scope for this change:
- Redefining the Meal Plan or slot assignment model
- Recipe search beyond name-based filtering
- Recipe import/export
- Per-member recipe ownership (recipes are always household-scoped)

<!-- pdac:cite id="BR-MEALS-HOUSEHOLD-SCOPED-MEALS" digest="sha256:0b82c4ba89f1570e491171ecd6cbf6cd8b98be75e2608073314111316ae03578" -->

## Capabilities

### New Capabilities

- `recipe-management`: Full lifecycle management of a household recipe — update, delete (with guard), detail retrieval, and ingredient post-create mutation. Includes the web app recipe library surface.

### Modified Capabilities

- `meal-planning`: Add `allowedMealTypes` filtering to `GetFamilyRecipes` when a slot context is provided. No other meal plan behavior changes.

<!-- pdac:cite id="BR-RECIPES-ALLOWED-MEAL-TYPES" digest="sha256:4b2522790207b70224c50687e2a59dc8261512ae7154781dfcf7185f2986f20f" -->

<!-- pdac:cite id="UC-MEALS-UPDATE-SLOT" digest="sha256:717756771462a30e271a209fda42ebbdbad3244f7544420f05079851a42bd301" -->

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
