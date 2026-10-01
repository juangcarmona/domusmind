<!-- pdac-scope: cited -->

## ADDED Requirements

### Requirement: Recipe Detail Retrieval

A household SHALL be able to retrieve the full detail of a single recipe, including its complete ingredient list.

<!-- pdac:cite id="FR-RECIPES-DETAIL" digest="sha256:0e0d422c5299e2fd97d969a06cb652814c18c14c9c7ae459bf3c964d29caca3b" -->

<!-- pdac:cite id="UC-RECIPES-VIEW-RECIPE" digest="sha256:06c252bac16722b4a109c9bcc28fce527edc4e44d5f22e09c222efa889df90ca" -->

<!-- pdac:cite id="TERM-RECIPE" digest="sha256:a0a6852e2fe6cc29247e1d0cf8e2159cea630e420d1f40adc5458c02a4e7fcd1" -->

<!-- pdac:cite id="TERM-INGREDIENT" digest="sha256:856986e747f9963a266837a7aced64b419cb5e0e31b02af7503dcd6785aecbe5" -->

<!-- pdac:cite id="SB-RECIPES-VIEW-DETAIL" digest="sha256:4c0e9c0641beee33a9471a7d3ad9c94e58e631431ba7013dee4bc06610588e44" -->

<!-- pdac:cite id="SB-RECIPES-VIEW-NOT-FOUND" digest="sha256:450b6306b56459be1e22019eb5aeb2394c745f6eb5676a354991bfbeb640805c" -->

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

<!-- pdac:cite id="FR-RECIPES-UPDATE" digest="sha256:b9a2910eede27b053d5a6ef69a9353774eca4f2410fadc76647c818b50d12fbd" -->

<!-- pdac:cite id="UC-RECIPES-UPDATE-RECIPE" digest="sha256:6825e998420a1234fc86e999215f082e17c4796a7ea709d40d20818b02b0a8bd" -->

<!-- pdac:cite id="BR-RECIPES-NAME-UNIQUE" digest="sha256:566fe524fa65c59602ade22e42e52cf55d27b6682382bf6c92db1e86f0710150" -->

<!-- pdac:cite id="SB-RECIPES-UPDATE" digest="sha256:9ff5a3c9b90b0f4a5768de8088de3067b99fc4be5729dfc80da7c0d12d885e14" -->

<!-- pdac:cite id="SB-RECIPES-RENAME-CONFLICT" digest="sha256:c7e9008428e9a26b5f26f1d04c10bc24f039d06aaecd94fc822d857d64c2c599" -->

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

<!-- pdac:cite id="FR-RECIPES-DELETE" digest="sha256:cb195db1bf181dfd62617a60604b4438d2ea9d476d78dbb62fe8c68fc96089a6" -->

<!-- pdac:cite id="UC-RECIPES-DELETE-RECIPE" digest="sha256:b9dabcfacb713eefa11031b17c4992a5eb43a220c2eae3662a9b5bb96de11a85" -->

<!-- pdac:cite id="BR-RECIPES-DELETE-GUARD" digest="sha256:974103e77810c33d6bd1ecae8266284a454470810f64eb426758fbcda52b8e89" -->

<!-- pdac:cite id="TERM-MEAL-PLAN-STATUS" digest="sha256:c2ccc55684994f91de79b4de0675ac21b7feb923fe92f5477780c1d4ffcf2f05" -->

<!-- pdac:cite id="SB-RECIPES-DELETE-UNREFERENCED" digest="sha256:aba7f3dc6fd29b35e23f5d7de94a95cb022b427d748abd007d4f0c2ebbb1b814" -->

<!-- pdac:cite id="SB-RECIPES-DELETE-BLOCKED-BY-ACTIVE-PLAN" digest="sha256:7b3942a528d6abfa57b1a92c067c75a227d816796bfc180e68f0379ed40863ad" -->

<!-- pdac:cite id="SB-RECIPES-DELETE-USED-BY-PAST-OR-DRAFT" digest="sha256:5537a5fb57920aeab76617a570b8ad25bebbacc3e0039b183db8cf298c5a3462" -->

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

<!-- pdac:cite id="FR-RECIPES-INGREDIENTS" digest="sha256:7059e97cd2eeaaf68396610bb1f2b4c6a382615c1491df62a94506dce8e70a71" -->

<!-- pdac:cite id="UC-RECIPES-MANAGE-INGREDIENTS" digest="sha256:e46fd68c79fa8d3e690b955ba261bc0853f23fa28a9ae42853f1017faf7fb9cd" -->

<!-- pdac:cite id="BR-RECIPES-INGREDIENT-NAME-UNIQUE" digest="sha256:e2c4c7915e08a7cd51d1c20b7af3bb4776ead0f8073329d3856d0724f9e7e7f1" -->

<!-- pdac:cite id="TERM-INGREDIENT" digest="sha256:856986e747f9963a266837a7aced64b419cb5e0e31b02af7503dcd6785aecbe5" -->

<!-- pdac:cite id="SB-RECIPES-ADD-INGREDIENT" digest="sha256:4e8ef7d0c4859976b564f6400049dfdae8ffb896f40c9ceda25524e11a6f815b" -->

<!-- pdac:cite id="SB-RECIPES-DUPLICATE-INGREDIENT-REJECTED" digest="sha256:8d9610e82de358e84d91fba329124e24b61d78f63278c9b6e9a0e1ed87d212cb" -->

<!-- pdac:cite id="SB-RECIPES-UPDATE-INGREDIENT" digest="sha256:4f908d6931a4c00edd8cd5671858ec375fad06ef04f2fde0ff1c48e8a42ee1fe" -->

<!-- pdac:cite id="SB-RECIPES-REMOVE-INGREDIENT" digest="sha256:42d2ea1e4ab12f0d22b61a6cbd202f5a15beccd15802c755a183e76622eeb217" -->

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

<!-- pdac:cite id="FR-RECIPES-LIBRARY-SURFACE" digest="sha256:39fb69cb69311e5b14ddf29498d7f94a462cc0540eac56770c15680feccf6a5f" -->

<!-- pdac:cite id="UC-RECIPES-BROWSE-LIBRARY" digest="sha256:457336010457babd21f0b02fee26cef2f00e543726236d9e802629ed8f98833f" -->

<!-- pdac:cite id="UC-RECIPES-VIEW-RECIPE" digest="sha256:06c252bac16722b4a109c9bcc28fce527edc4e44d5f22e09c222efa889df90ca" -->

<!-- pdac:cite id="UC-RECIPES-CREATE-RECIPE" digest="sha256:c9aba5e8213215812c9767138d04130ab2fa2875d493c7a72f060f8cbd2b6e35" -->

<!-- pdac:cite id="UC-RECIPES-UPDATE-RECIPE" digest="sha256:6825e998420a1234fc86e999215f082e17c4796a7ea709d40d20818b02b0a8bd" -->

<!-- pdac:cite id="UC-RECIPES-DELETE-RECIPE" digest="sha256:b9dabcfacb713eefa11031b17c4992a5eb43a220c2eae3662a9b5bb96de11a85" -->

<!-- pdac:cite id="SB-RECIPES-BROWSE-LIBRARY" digest="sha256:179e040dfc23f214b1d0fa5a4cf1ac399b6b961dc41c1f16ba9bffe934f61655" -->

<!-- pdac:cite id="SB-RECIPES-LIBRARY-OPEN-DETAIL" digest="sha256:4b9c2a65127400ec0e3927f14931f8704e41c579ac881ea1961f5eeec4ba3f9a" -->

<!-- pdac:cite id="SB-RECIPES-LIBRARY-CREATE" digest="sha256:54fc4a51765646a10f218c461c27d9f29f8c6131153964ae8ac679fa5b8b50ee" -->

<!-- pdac:cite id="SB-RECIPES-LIBRARY-EDIT" digest="sha256:4b7a510e846e49cdfba63a76b9037f4d19e458ea1ca41c4230ebb10349e1577b" -->

<!-- pdac:cite id="SB-RECIPES-LIBRARY-DELETE" digest="sha256:bb61563362e958ac4d987563c936ead7d87c627d3cf59f3511fd873814883931" -->

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
