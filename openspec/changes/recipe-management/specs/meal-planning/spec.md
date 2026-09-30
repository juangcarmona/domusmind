<!-- pdac-scope: cited -->

## MODIFIED Requirements

### Requirement: Recipe Library

A household SHALL maintain a library of recipes scoped to the family.

A recipe has a name (unique within the family), optional description, optional preparation and cook times, optional servings count, an ingredient list, optional tags for classification, an optional set of allowed meal types indicating which meal types the recipe is appropriate for, and an `isFavorite` flag.

Ingredients within a recipe must have unique names (to support deduplication during shopping list derivation). Ingredient quantity and unit are optional. When both preparation and cook times are provided, total time is derived as their sum; if either is absent, total time is unset.

A recipe may not be deleted if it is currently referenced by any slot in an Active meal plan. Draft and Completed plans do not block deletion.

Recipes may be updated after creation; ingredients can be added, updated, or removed individually after creation.

When browsing recipes for assignment to a slot, the household SHALL only be shown recipes that are compatible with the slot's meal type. A recipe is compatible when its `allowedMealTypes` list is empty (no restriction) or contains the target meal type.

<!-- pdac:cite id="FR-RECIPES-LIBRARY" digest="sha256:702c8c7996f89cc2fd0f678d4a163db664aae0ca384e742133ad8cdd9001bcfc" -->

<!-- pdac:cite id="UC-RECIPES-CREATE-RECIPE" digest="sha256:c9aba5e8213215812c9767138d04130ab2fa2875d493c7a72f060f8cbd2b6e35" -->

<!-- pdac:cite id="UC-RECIPES-BROWSE-LIBRARY" digest="sha256:457336010457babd21f0b02fee26cef2f00e543726236d9e802629ed8f98833f" -->

<!-- pdac:cite id="UC-MEALS-UPDATE-SLOT" digest="sha256:717756771462a30e271a209fda42ebbdbad3244f7544420f05079851a42bd301" -->

<!-- pdac:cite id="BR-RECIPES-NAME-UNIQUE" digest="sha256:566fe524fa65c59602ade22e42e52cf55d27b6682382bf6c92db1e86f0710150" -->

<!-- pdac:cite id="BR-RECIPES-INGREDIENT-NAME-UNIQUE" digest="sha256:e2c4c7915e08a7cd51d1c20b7af3bb4776ead0f8073329d3856d0724f9e7e7f1" -->

<!-- pdac:cite id="BR-RECIPES-TOTAL-TIME" digest="sha256:077e3fa243bd6ee0b3f2495bcd15b8321145f05a98359628b0716a7e2a8d3340" -->

<!-- pdac:cite id="BR-RECIPES-ALLOWED-MEAL-TYPES" digest="sha256:4b2522790207b70224c50687e2a59dc8261512ae7154781dfcf7185f2986f20f" -->

<!-- pdac:cite id="FR-RECIPES-DELETE" digest="sha256:cb195db1bf181dfd62617a60604b4438d2ea9d476d78dbb62fe8c68fc96089a6" -->

<!-- pdac:cite id="UC-RECIPES-DELETE-RECIPE" digest="sha256:b9dabcfacb713eefa11031b17c4992a5eb43a220c2eae3662a9b5bb96de11a85" -->

<!-- pdac:cite id="BR-RECIPES-DELETE-GUARD" digest="sha256:974103e77810c33d6bd1ecae8266284a454470810f64eb426758fbcda52b8e89" -->

<!-- pdac:cite id="FR-RECIPES-UPDATE" digest="sha256:b9a2910eede27b053d5a6ef69a9353774eca4f2410fadc76647c818b50d12fbd" -->

<!-- pdac:cite id="UC-RECIPES-UPDATE-RECIPE" digest="sha256:6825e998420a1234fc86e999215f082e17c4796a7ea709d40d20818b02b0a8bd" -->

<!-- pdac:cite id="FR-RECIPES-INGREDIENTS" digest="sha256:7059e97cd2eeaaf68396610bb1f2b4c6a382615c1491df62a94506dce8e70a71" -->

<!-- pdac:cite id="UC-RECIPES-MANAGE-INGREDIENTS" digest="sha256:e46fd68c79fa8d3e690b955ba261bc0853f23fa28a9ae42853f1017faf7fb9cd" -->

<!-- pdac:cite id="TERM-RECIPE" digest="sha256:a0a6852e2fe6cc29247e1d0cf8e2159cea630e420d1f40adc5458c02a4e7fcd1" -->

<!-- pdac:cite id="TERM-INGREDIENT" digest="sha256:856986e747f9963a266837a7aced64b419cb5e0e31b02af7503dcd6785aecbe5" -->

<!-- pdac:cite id="TERM-MEAL-TYPE" digest="sha256:f22d542a1f5beb9048900ce2c4360011043e7f8c3b16f58294387cbb56c116cc" -->

<!-- pdac:cite id="SB-RECIPES-ADD-TO-LIBRARY" digest="sha256:996027c711dbc9b8672ea360f56f03685cdee2486d8f76b2ac57e756a75f6abb" -->

<!-- pdac:cite id="SB-RECIPES-DUPLICATE-NAME-REJECTED" digest="sha256:93b9280f5809d13a6db3abc8c9c0926093f881c3b335733479ae11f28d0ae3bb" -->

<!-- pdac:cite id="SB-RECIPES-RESTRICTED-MEAL-TYPE-HIDDEN" digest="sha256:9b4c3469f79df26d76e65bff3e37928f17658561178468cfd92c5017edaf4653" -->

<!-- pdac:cite id="SB-RECIPES-UNRESTRICTED-SHOWN-EVERYWHERE" digest="sha256:6c6ab3201ffeb3504d2e154fe01220c165fd5cdb4585ec8de058053bcf759bb8" -->

#### Scenario: Household adds a recipe to the library

- GIVEN a family exists
- WHEN the household creates a recipe with a unique name
- THEN the recipe is added to the household's recipe library
- AND it becomes available for assignment to meal slots

#### Scenario: Household attempts to create a recipe with a duplicate name

- GIVEN a recipe named "Pasta Bolognese" already exists in the family's library
- WHEN the household creates another recipe with the same name
- THEN the creation is rejected
- AND the existing recipe is unchanged

#### Scenario: Recipe is assigned to allowed meal types

- GIVEN a recipe with `allowedMealTypes = [Dinner]`
- WHEN the household browses recipes for a Breakfast slot
- THEN the recipe is not presented as a valid option for that slot

#### Scenario: Recipe with no allowedMealTypes restriction is shown for any slot

- GIVEN a recipe with an empty `allowedMealTypes` list
- WHEN the household browses recipes for any meal type slot
- THEN the recipe is presented as a valid option
