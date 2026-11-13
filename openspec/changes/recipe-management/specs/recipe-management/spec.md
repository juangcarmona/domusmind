<!-- pdac-scope: cited -->

## ADDED Requirements

### Requirement: Recipe Detail Retrieval

A household SHALL be able to retrieve the full detail of a single recipe, including its complete ingredient list.

<!-- pdac:cite id="FR-RECIPES-DETAIL" digest="sha256:6d8e670e1225300aac4a0a0490b6da9ac83d30d3d2ffda66dd6204fe08ab59e6" -->

<!-- pdac:cite id="UC-RECIPES-VIEW-RECIPE" digest="sha256:b66dd0aeafaea8725a3204492555ff0a40f50dcd0067c1e2844992e41f5a7c91" -->

<!-- pdac:cite id="TERM-RECIPE" digest="sha256:3c619b3dae05589a60edac9f0ed0b63a78a963b1070a73cb6c4c5b76e9a570b4" -->

<!-- pdac:cite id="TERM-INGREDIENT" digest="sha256:e70048c508c2ac43cbd9797dc538c97f9240f658fc0cffcd73c937c64c1586d8" -->

<!-- pdac:cite id="SB-RECIPES-VIEW-DETAIL" digest="sha256:44cd77c634c35dc3fde00b0148e615c467086d1a7a9b238005033d573644061e" -->

<!-- pdac:cite id="SB-RECIPES-VIEW-NOT-FOUND" digest="sha256:faf20a6d9f352eb52d59e075db65c67bee22a65bc56fef32db0097a529c894b2" -->

#### Scenario: Household retrieves a recipe by ID

- GIVEN a recipe exists in the household's library
- WHEN the household retrieves it by recipe ID
- THEN the full recipe is returned including name, description, times, servings, tags, allowedMealTypes, isFavorite, and the complete ingredient list
- AND each ingredient includes name, optional quantity, and optional unit

#### Scenario: Recipe not found

- GIVEN no recipe exists with the requested ID in the household's library
- WHEN the household attempts to retrieve it
- THEN the request returns a not-found outcome

---

### Requirement: Recipe Update

A household SHALL be able to update a recipe's metadata after creation.

Updatable fields: name, description, preparation time, cook time, servings, isFavorite, allowedMealTypes, and tags. Name must remain unique within the family. All fields are replaced in full (no partial patch).

<!-- pdac:cite id="FR-RECIPES-UPDATE" digest="sha256:733871393da840075fb57813e3e5453c6a84949ea91f7a51568168e3a7d0894b" -->

<!-- pdac:cite id="UC-RECIPES-UPDATE-RECIPE" digest="sha256:0fb0ec3cf0b3ef1ad1159d45b722e6be18e00d694f7290a93b9a29f5a26c27f5" -->

<!-- pdac:cite id="BR-RECIPES-NAME-UNIQUE" digest="sha256:54fe88bd63ffb8100bf3dbb97bf05c0275a1dfcc89645b9659c013783a93e7a8" -->

<!-- pdac:cite id="SB-RECIPES-UPDATE" digest="sha256:42fa377a20742beae8999bba3813e160b76287a3fa0f4aeb1a7911ca5ca56f94" -->

<!-- pdac:cite id="SB-RECIPES-RENAME-CONFLICT" digest="sha256:362780b5775e2e187ea275324a16a82ef6c09ffb725f8eaf706eab50f2315d11" -->

#### Scenario: Household updates a recipe

- GIVEN a recipe exists in the household's library
- WHEN the household submits updated metadata
- THEN the recipe is updated with the new values
- AND the recipe's updatedAt timestamp is refreshed

#### Scenario: Household attempts to rename a recipe to a conflicting name

- GIVEN a recipe named "Pasta Bolognese" already exists in the family's library
- AND the household attempts to rename a different recipe to "Pasta Bolognese"
- THEN the update is rejected
- AND neither recipe is changed

---

### Requirement: Recipe Deletion

A household SHALL be able to delete a recipe from the library, subject to a referential guard.

A recipe SHALL NOT be deleted if it is currently referenced by any slot in an Active meal plan. Draft or Completed plans do not block deletion.

<!-- pdac:cite id="FR-RECIPES-DELETE" digest="sha256:8c2e5784a8e8bd51c1e1709dba553bb976376fe7e3011801831423d4c2e1d63b" -->

<!-- pdac:cite id="UC-RECIPES-DELETE-RECIPE" digest="sha256:47bb5bbb4aaa4ff7c6a788ca533d974aa3fa9561177752b8ca6dfc7c55ff8b56" -->

<!-- pdac:cite id="BR-RECIPES-DELETE-GUARD" digest="sha256:2e15467fd305be37d291774e1e09606ef41448d1713b4f98d5ff2cbce2ada86e" -->

<!-- pdac:cite id="TERM-MEAL-PLAN-STATUS" digest="sha256:8771ab7cdffe66d265ba863bfd83723d03cd29ba69b9bbf968ccc7637f42f397" -->

<!-- pdac:cite id="SB-RECIPES-DELETE-UNREFERENCED" digest="sha256:53b1008265453ebea65d33af2b302c4c36f6b4a7c03ea221a33bce0807a40c3d" -->

<!-- pdac:cite id="SB-RECIPES-DELETE-BLOCKED-BY-ACTIVE-PLAN" digest="sha256:50a9c1d606a609b8e3c5a17c5ae3369c8a19c431ac92476a053da273eae03afd" -->

<!-- pdac:cite id="SB-RECIPES-DELETE-USED-BY-PAST-OR-DRAFT" digest="sha256:88c3d0fe9ecea04556fffeafad4e8f9b6a658d60cb909a6769b58a840055839b" -->

#### Scenario: Household deletes an unreferenced recipe

- GIVEN a recipe exists and is not referenced by any Active plan slot
- WHEN the household deletes it
- THEN the recipe is removed from the library
- AND it is no longer available for assignment to meal slots

#### Scenario: Household attempts to delete a recipe referenced by an Active plan

- GIVEN a recipe is referenced by at least one slot in an Active meal plan
- WHEN the household attempts to delete it
- THEN the deletion is rejected
- AND the recipe remains in the library

#### Scenario: Household deletes a recipe referenced only by a Draft or Completed plan

- GIVEN a recipe is referenced by a slot in a Draft or Completed meal plan only (no Active plans)
- WHEN the household deletes it
- THEN the recipe is removed from the library
- AND affected Draft or Completed slots retain their reference but the recipe metadata is no longer resolvable

---

### Requirement: Recipe Ingredient Management

A household SHALL be able to add, update, and remove individual ingredients from a recipe after it has been created.

Ingredient identity within a recipe is the ingredient name (case-insensitive). A recipe may not contain two ingredients with the same name.

<!-- pdac:cite id="FR-RECIPES-INGREDIENTS" digest="sha256:82e9ab23dc511501d2a8f51222e43dc395c8f99365dd238e91dabebae50b745b" -->

<!-- pdac:cite id="UC-RECIPES-MANAGE-INGREDIENTS" digest="sha256:1aa4b2786d2d9f085881e2c21e1b3622f14f13f9ea52a1813f0a93e319712f00" -->

<!-- pdac:cite id="BR-RECIPES-INGREDIENT-NAME-UNIQUE" digest="sha256:49bf51a252a84807602bd53fcbbe8754840772a71957f7108738501f3cc4e0a0" -->

<!-- pdac:cite id="TERM-INGREDIENT" digest="sha256:e70048c508c2ac43cbd9797dc538c97f9240f658fc0cffcd73c937c64c1586d8" -->

<!-- pdac:cite id="SB-RECIPES-ADD-INGREDIENT" digest="sha256:958a735b570dd857840f7259232cce57426dd4f5b52f7d99e727bfaf2f0514b3" -->

<!-- pdac:cite id="SB-RECIPES-DUPLICATE-INGREDIENT-REJECTED" digest="sha256:ada5b20b446e6a2553be7650df35de9d0467faf86c9937a51c92beb2ceae3463" -->

<!-- pdac:cite id="SB-RECIPES-UPDATE-INGREDIENT" digest="sha256:d7ed3e6bc26f9c449d03917f2b558dbf7d55a684d4991ea6151ed71d2ad2c156" -->

<!-- pdac:cite id="SB-RECIPES-REMOVE-INGREDIENT" digest="sha256:0d8e6d070db9a1b8cf40f7979fb365d8e00197bb789e8aaa23e0da8782d23b29" -->

#### Scenario: Household adds an ingredient to an existing recipe

- GIVEN a recipe exists
- WHEN the household adds a new ingredient with a unique name
- THEN the ingredient is added to the recipe
- AND the recipe's ingredient count increases by one

#### Scenario: Household attempts to add a duplicate ingredient

- GIVEN a recipe already contains an ingredient named "Olive oil"
- WHEN the household attempts to add another ingredient named "olive oil"
- THEN the operation is rejected
- AND the recipe is unchanged

#### Scenario: Household updates an ingredient's quantity or unit

- GIVEN a recipe contains an ingredient named "Flour"
- WHEN the household updates its quantity and unit
- THEN the ingredient is updated
- AND the ingredient name is unchanged

#### Scenario: Household removes an ingredient from a recipe

- GIVEN a recipe contains an ingredient
- WHEN the household removes it by name
- THEN the ingredient is removed from the recipe
- AND the recipe's ingredient count decreases by one

---

### Requirement: Recipe Library Surface

The web application SHALL provide a dedicated recipe library surface where a household can browse, view, create, edit, and delete recipes independently of the weekly meal plan view.

<!-- pdac:cite id="FR-RECIPES-LIBRARY-SURFACE" digest="sha256:c8ab313c8ada872d7cdec7c39b58bc2afef4e736fe51b286f34b398ad93b5858" -->

<!-- pdac:cite id="UC-RECIPES-BROWSE-LIBRARY" digest="sha256:fbcff790cf18adf9b802721d066aafff3e01dcd239d62fb5ff197f8e629837e9" -->

<!-- pdac:cite id="UC-RECIPES-VIEW-RECIPE" digest="sha256:b66dd0aeafaea8725a3204492555ff0a40f50dcd0067c1e2844992e41f5a7c91" -->

<!-- pdac:cite id="UC-RECIPES-CREATE-RECIPE" digest="sha256:b1982c4e6063006dac46caf79a882dbb237450836e34795ea7dd1751b8ce02a8" -->

<!-- pdac:cite id="UC-RECIPES-UPDATE-RECIPE" digest="sha256:0fb0ec3cf0b3ef1ad1159d45b722e6be18e00d694f7290a93b9a29f5a26c27f5" -->

<!-- pdac:cite id="UC-RECIPES-DELETE-RECIPE" digest="sha256:47bb5bbb4aaa4ff7c6a788ca533d974aa3fa9561177752b8ca6dfc7c55ff8b56" -->

<!-- pdac:cite id="SB-RECIPES-BROWSE-LIBRARY" digest="sha256:340058490b9cc68d3187fbacc2c1e55cd1b13df1c05331f1934a2e7155479c77" -->

<!-- pdac:cite id="SB-RECIPES-LIBRARY-OPEN-DETAIL" digest="sha256:512f06106ea6b9551e48148288d351d5b171db018c2b775de36cf80f8d549e41" -->

<!-- pdac:cite id="SB-RECIPES-LIBRARY-CREATE" digest="sha256:5619c7cf53912a2bf4403b8efcc65108a56ab5ddceba2f1e764adb5a06a614c4" -->

<!-- pdac:cite id="SB-RECIPES-LIBRARY-EDIT" digest="sha256:c87c80d5f398d30162f6080f2bfe3f76da4f0ad4e215cc866ed6b8a53f7929ea" -->

<!-- pdac:cite id="SB-RECIPES-LIBRARY-DELETE" digest="sha256:645c1ebc8500c628a2ac390c5e91abd211d650225e7a3c2ac2a59128a2262d1b" -->

#### Scenario: Household browses the recipe library

- GIVEN one or more recipes exist in the household's library
- WHEN the household navigates to the recipe library surface
- THEN all recipes are listed with their name, cook time, servings, tag count, and ingredient count
- AND the list is ordered by name

#### Scenario: Household views a recipe's full detail

- GIVEN a recipe exists in the library
- WHEN the household selects it from the list
- THEN the full recipe detail is displayed including the complete ingredient list

#### Scenario: Household creates a recipe from the library surface

- GIVEN the household is on the recipe library surface
- WHEN the household submits a new recipe with a name and optional fields including ingredients
- THEN the recipe is created and appears in the library

#### Scenario: Household edits a recipe from the library surface

- GIVEN a recipe exists in the library
- WHEN the household edits it and saves
- THEN the recipe reflects the updated values

#### Scenario: Household deletes a recipe from the library surface

- GIVEN a recipe exists and is not blocked by deletion guard
- WHEN the household deletes it from the library surface
- THEN it is removed and no longer appears in the list
