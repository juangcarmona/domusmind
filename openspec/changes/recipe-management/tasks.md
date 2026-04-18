## 1. Domain — Recipe Aggregate Mutations

- [x] 1.1 Add `Update(name, description, prepTimeMinutes, cookTimeMinutes, servings, isFavorite, allowedMealTypes, tags)` method to `Recipe` aggregate; enforce name uniqueness at application layer
- [x] 1.2 Add `RemoveIngredient(name)` method to `Recipe` aggregate (case-insensitive match)
- [x] 1.3 Add `UpdateIngredient(name, newQuantity, newUnit)` method to `Recipe` aggregate
- [x] 1.4 Add `Delete()` method to `Recipe` aggregate that raises `RecipeDeleted` domain event

## 2. Backend — Contracts

- [x] 2.1 Add `GetRecipeDetailResponse` record with full ingredient list (`IReadOnlyList<IngredientDetail>`)
- [x] 2.2 Add `UpdateRecipeRequest` and `UpdateRecipeResponse` records
- [x] 2.3 Add `DeleteRecipeResponse` record
- [x] 2.4 Add `AddRecipeIngredientRequest` and `AddRecipeIngredientResponse` records
- [x] 2.5 Add `UpdateRecipeIngredientRequest` and `UpdateRecipeIngredientResponse` records
- [x] 2.6 Add `RemoveRecipeIngredientResponse` record
- [x] 2.7 Extend `GetFamilyRecipesQuery` with optional `MealType?` parameter; update `GetFamilyRecipesResponse` if needed

## 3. Backend — Application Slices

- [x] 3.1 Add `GetRecipeDetail` query slice (`GetRecipeDetailQuery`, `GetRecipeDetailQueryHandler`)
- [x] 3.2 Add `UpdateRecipe` command slice (`UpdateRecipeCommand`, `UpdateRecipeCommandHandler`, `UpdateRecipeValidator`)
- [x] 3.3 Add `DeleteRecipe` command slice (`DeleteRecipeCommand`, `DeleteRecipeCommandHandler`) — includes active-slot guard query
- [x] 3.4 Add `AddRecipeIngredient` command slice (`AddRecipeIngredientCommand`, `AddRecipeIngredientCommandHandler`)
- [x] 3.5 Add `UpdateRecipeIngredient` command slice (`UpdateRecipeIngredientCommand`, `UpdateRecipeIngredientCommandHandler`)
- [x] 3.6 Add `RemoveRecipeIngredient` command slice (`RemoveRecipeIngredientCommand`, `RemoveRecipeIngredientCommandHandler`)
- [x] 3.7 Update `GetFamilyRecipesQueryHandler` to filter by `MealType` when provided (empty `allowedMealTypes` OR contains the given meal type)

## 4. Backend — API

- [x] 4.1 Add `GET /api/recipes/{id}` endpoint to `RecipesController`
- [x] 4.2 Add `PUT /api/recipes/{id}` endpoint to `RecipesController`
- [x] 4.3 Add `DELETE /api/recipes/{id}` endpoint to `RecipesController`
- [x] 4.4 Add `POST /api/recipes/{id}/ingredients` endpoint to `RecipesController`
- [x] 4.5 Add `PUT /api/recipes/{id}/ingredients/{name}` endpoint to `RecipesController`
- [x] 4.6 Add `DELETE /api/recipes/{id}/ingredients/{name}` endpoint to `RecipesController`
- [x] 4.7 Add optional `mealType` query parameter to `GET /api/recipes/family/{familyId}`

## 5. Backend — Build and Tests

- [x] 5.1 Run `dotnet build` — verify no compilation errors
- [x] 5.2 Add unit tests for `Recipe.Update` covering name-change and no-op cases
- [x] 5.3 Add unit tests for `Recipe.RemoveIngredient` and `Recipe.UpdateIngredient`
- [x] 5.4 Add unit tests for `DeleteRecipe` handler — active-slot guard blocks deletion, inactive slots do not
- [x] 5.5 Add unit tests for `GetFamilyRecipes` with `mealType` filter
- [x] 5.6 Run `dotnet test` — all tests pass

## 6. Frontend — API Client and State

- [x] 6.1 Add `getRecipeDetail(recipeId)` API call in the meal planning API module
- [x] 6.2 Add `updateRecipe(...)` API call
- [x] 6.3 Add `deleteRecipe(recipeId)` API call
- [x] 6.4 Add `addIngredient(recipeId, ingredient)` API call
- [x] 6.5 Add `updateIngredient(recipeId, name, quantity, unit)` API call
- [x] 6.6 Add `removeIngredient(recipeId, name)` API call
- [x] 6.7 Create `recipeLibrarySlice` with state for recipe list, selected recipe detail, and mutation statuses
- [x] 6.8 Update `GetFamilyRecipes` call in the slot inspector to pass `mealType` when a slot is selected

## 7. Frontend — Recipe Library Surface

- [x] 7.1 Create `src/features/recipe-library/` feature folder
- [x] 7.2 Create `RecipesPage` component — recipe list with name, cook time, servings, ingredient count; "New Recipe" button
- [x] 7.3 Create `RecipeDetailPanel` component — full recipe view including ingredient list; edit and delete actions
- [x] 7.4 Create `RecipeFormModal` (or page) — full recipe creation and edit form; includes ingredients, tags, `allowedMealTypes`, `isFavorite`
- [x] 7.5 Wire delete action with confirmation and active-slot error handling (show user-friendly rejection message)
- [x] 7.6 Register `RecipesPage` route in the app router
- [x] 7.7 Add navigation entry for the recipe library surface

## 8. Frontend — Slot Inspector Update

- [x] 8.1 Update `RecipePickerPanel` to pass current slot's `mealType` to the recipes fetch; confirm filtered results display correctly
- [x] 8.2 Add inline note in `CreateRecipeModal` indicating ingredients can be added via the recipe library

## 9. Frontend — Build

- [x] 9.1 Run `npm run build` — verify no TypeScript or build errors

## 10. Corrections and Refinement

### 10.1 Backend correctness

* [x] 10.1.1 Register `GetRecipeDetailQueryHandler` in `ApplicationServices.cs`
* [x] 10.1.2 Register any other newly introduced Recipe query/command handlers missing from DI
* [x] 10.1.3 Verify `RecipesController.GetRecipeDetail` resolves correctly through `IQueryDispatcher`
* [x] 10.1.4 Add or update backend test coverage for handler registration / happy-path recipe detail resolution

### 10.2 Recipe detail loading

* [x] 10.2.1 Fix recipe selection flow so clicking a recipe loads detail into the inspector instead of failing
* [x] 10.2.2 Ensure error state is surfaced gracefully in UI if recipe detail loading fails
* [x] 10.2.3 Verify inspector state resets correctly when switching between recipes

### 10.3 UX consistency between Meal Planning and Recipes

* [x] 10.3.1 Unify recipe creation entry flow between Meal Planning and Recipes
* [x] 10.3.2 Decide one canonical creation pattern for Recipe: inspector-first or modal-first
* [x] 10.3.3 Replace the non-canonical recipe creation form so both entry points use the same form structure and fields
* [x] 10.3.4 Ensure description uses a proper multiline text area with enough vertical space
* [x] 10.3.5 Ensure form labels, spacing, and actions match current product grammar

### 10.4 Recipes surface quality

* [x] 10.4.1 Improve Recipes list density to align with Lists surface grammar
* [x] 10.4.2 Define a richer row layout for recipes:

  * title
  * compact metadata line
  * favorite cue
  * prep/cook/servings summary where available
* [x] 10.4.3 Reduce dead whitespace and weak text hierarchy in recipe rows
* [x] 10.4.4 Ensure selected recipe state is visually clear and opens inspector consistently

### 10.5 Inspector alignment

* [x] 10.5.1 Make Recipe detail use inspector as the primary desktop detail surface
* [x] 10.5.2 Ensure recipe detail layout follows the same inspector grammar as Lists / Meal Planning
* [x] 10.5.3 Include only repository-supported fields in the inspector and avoid ad-hoc UI inventions

### 10.6 Internationalization

* [x] 10.6.1 Replace untranslated strings in Recipes surface with translation keys
* [x] 10.6.2 Replace untranslated strings in recipe creation form with translation keys
* [x] 10.6.3 Replace abbreviated hardcoded metadata labels such as `"srv"` and `"ing."`
* [x] 10.6.4 Verify both existing and new recipe flows render correctly in Spanish

### 10.7 Icons and shell semantics

* [x] 10.7.1 Replace `IconMeals` with a clearer meal-planning icon
* [x] 10.7.2 Replace `IconRecipes` with a clearly distinct recipe-management icon
* [x] 10.7.3 Verify both icons remain legible at shell size and are semantically non-overlapping

### 10.8 Regression and build validation

* [x] 10.8.1 `dotnet build`
* [x] 10.8.2 `dotnet test`
* [x] 10.8.3 `npm run build`

## 11. Surface Alignment and Form Integration

### 11.1 Recipes surface must follow Lists surface grammar

* [x] 11.1.1 Review current `Recipes` surface against the implemented `Lists` surface and extract the reusable structural pattern:

  * left navigation rail
  * central working pane
  * right inspector as primary desktop detail surface
  * dense list rows
  * local creation/editing without floating detached forms
* [x] 11.1.2 Refactor `Recipes` desktop layout so it follows the same split-view grammar as `Lists`
* [x] 11.1.3 Remove the current detached lower-page form layout for recipe creation and editing

### 11.2 Recipe creation must follow the inspector-first desktop pattern

* [x] 11.2.1 Replace the current “New recipe” floating/lower form with a creation flow aligned with Lists:

  * create action from header
  * detail/edit in contextual panel or equivalent canonical form area
* [x] 11.2.2 Ensure “New recipe” does not open a disconnected form that breaks page context
* [x] 11.2.3 Make the create flow visually and structurally consistent with the rest of the product shell

### 11.3 Recipe editing must follow the same canonical form as creation

* [x] 11.3.1 Remove divergence between “New recipe” and “Edit recipe” forms
* [x] 11.3.2 Use one canonical recipe form structure for both create and edit
* [x] 11.3.3 Ensure edit happens in the same contextual pattern as detail, instead of a second disconnected form region
* [x] 11.3.4 Keep surrounding recipe list context visible while editing on desktop

### 11.4 Recipe detail + edit flow must converge on inspector usage

* [x] 11.4.1 Treat recipe detail as the primary desktop inspector experience
* [x] 11.4.2 Ensure selecting a recipe loads detail in the inspector
* [x] 11.4.3 Ensure “Edit” transitions the inspector into edit mode, instead of spawning a separate form elsewhere on the page
* [x] 11.4.4 Ensure create, inspect, and edit flows do not compete visually on the same page

### 11.5 Recipe list pane must align with Lists visual density and selection model

* [x] 11.5.1 Refine recipe rows to feel like a real working list, not loose text on canvas
* [x] 11.5.2 Introduce a clearer row layout:

  * recipe name
  * compact secondary metadata
  * favorite cue
  * stable selected-row state
* [x] 11.5.3 Ensure clicking a row selects it and opens inspector consistently
* [x] 11.5.4 Ensure empty space and weak hierarchy are reduced to match Lists surface standards

### 11.6 Meal Planning integration must stay aligned with Recipes surface

* [x] 11.6.1 Revisit recipe creation entry from Meal Planning and align it with the canonical Recipes form pattern
* [x] 11.6.2 Ensure Meal Planning does not keep a second recipe form implementation if Recipes becomes the canonical recipe-management surface
* [x] 11.6.3 Ensure recipe selection inside Meal Planning continues to work after Recipes surface/form alignment

### 11.7 Documentation update if behavior or surface contract changed

* [x] 11.7.1 Review whether the current Meal Planning OpenSpec spec or surface documentation assumes recipe creation/editing behavior that is now being changed
* [x] 11.7.2 Update Meal Planning documentation only if required by the final agreed behavior
* [x] 11.7.3 Add or refine Recipes/Meal Planning surface expectations so Recipes is clearly the canonical recipe-management surface and Meal Planning remains a recipe consumer
* [x] 11.7.4 Avoid documenting implementation details; update only surface behavior and ownership boundaries

### 11.8 Validation

* [x] 11.8.1 Manual validation:

  * open Recipes
  * select recipe
  * inspect recipe
  * edit recipe in-place
  * create recipe with canonical form
  * create/select recipe from Meal Planning
  * verify no duplicate recipe form patterns remain
* [x] 11.8.2 Confirm desktop Recipes now feels structurally aligned with Lists
* [x] 11.8.3 Confirm no documentation drift remains after the surface/form alignment changes

