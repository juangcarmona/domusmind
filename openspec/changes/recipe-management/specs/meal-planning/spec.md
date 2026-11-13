<!-- pdac-scope: cited -->

## MODIFIED Requirements

### Requirement: Recipe Library

A household SHALL maintain a library of recipes scoped to the family.

A recipe has a name (unique within the family), optional description, optional preparation and cook times, optional servings count, an ingredient list, optional tags for classification, an optional set of allowed meal types indicating which meal types the recipe is appropriate for, and an `isFavorite` flag.

Ingredients within a recipe must have unique names (to support deduplication during shopping list derivation). Ingredient quantity and unit are optional. When both preparation and cook times are provided, total time is derived as their sum; if either is absent, total time is unset.

A recipe may not be deleted if it is currently referenced by any slot in an Active meal plan. Draft and Completed plans do not block deletion.

Recipes may be updated after creation; ingredients can be added, updated, or removed individually after creation.

When browsing recipes for assignment to a slot, the household SHALL only be shown recipes that are compatible with the slot's meal type. A recipe is compatible when its `allowedMealTypes` list is empty (no restriction) or contains the target meal type.

<!-- pdac:cite id="FR-RECIPES-LIBRARY" digest="sha256:1340dae095e80b3c9ea2f47bb1780af2e6ae2acbd28baeb5750cd532bd02d34d" -->

<!-- pdac:cite id="UC-RECIPES-CREATE-RECIPE" digest="sha256:b1982c4e6063006dac46caf79a882dbb237450836e34795ea7dd1751b8ce02a8" -->

<!-- pdac:cite id="UC-RECIPES-BROWSE-LIBRARY" digest="sha256:fbcff790cf18adf9b802721d066aafff3e01dcd239d62fb5ff197f8e629837e9" -->

<!-- pdac:cite id="UC-MEALS-UPDATE-SLOT" digest="sha256:a659fa4c170ec938919501335736ec5ea87ea35c81cc207d793db62e5a6c9d74" -->

<!-- pdac:cite id="BR-RECIPES-NAME-UNIQUE" digest="sha256:54fe88bd63ffb8100bf3dbb97bf05c0275a1dfcc89645b9659c013783a93e7a8" -->

<!-- pdac:cite id="BR-RECIPES-INGREDIENT-NAME-UNIQUE" digest="sha256:49bf51a252a84807602bd53fcbbe8754840772a71957f7108738501f3cc4e0a0" -->

<!-- pdac:cite id="BR-RECIPES-TOTAL-TIME" digest="sha256:6a507e94619fa4a98cb67c168864e0acf6e037852123b0283d56a0b350766ff1" -->

<!-- pdac:cite id="BR-RECIPES-ALLOWED-MEAL-TYPES" digest="sha256:d5576dfc67764576f0e2d0f681e4c56655eb6be09ff5e5ed95b3c1fc87241f3a" -->

<!-- pdac:cite id="FR-RECIPES-DELETE" digest="sha256:8c2e5784a8e8bd51c1e1709dba553bb976376fe7e3011801831423d4c2e1d63b" -->

<!-- pdac:cite id="UC-RECIPES-DELETE-RECIPE" digest="sha256:47bb5bbb4aaa4ff7c6a788ca533d974aa3fa9561177752b8ca6dfc7c55ff8b56" -->

<!-- pdac:cite id="BR-RECIPES-DELETE-GUARD" digest="sha256:2e15467fd305be37d291774e1e09606ef41448d1713b4f98d5ff2cbce2ada86e" -->

<!-- pdac:cite id="FR-RECIPES-UPDATE" digest="sha256:733871393da840075fb57813e3e5453c6a84949ea91f7a51568168e3a7d0894b" -->

<!-- pdac:cite id="UC-RECIPES-UPDATE-RECIPE" digest="sha256:0fb0ec3cf0b3ef1ad1159d45b722e6be18e00d694f7290a93b9a29f5a26c27f5" -->

<!-- pdac:cite id="FR-RECIPES-INGREDIENTS" digest="sha256:82e9ab23dc511501d2a8f51222e43dc395c8f99365dd238e91dabebae50b745b" -->

<!-- pdac:cite id="UC-RECIPES-MANAGE-INGREDIENTS" digest="sha256:1aa4b2786d2d9f085881e2c21e1b3622f14f13f9ea52a1813f0a93e319712f00" -->

<!-- pdac:cite id="TERM-RECIPE" digest="sha256:3c619b3dae05589a60edac9f0ed0b63a78a963b1070a73cb6c4c5b76e9a570b4" -->

<!-- pdac:cite id="TERM-INGREDIENT" digest="sha256:e70048c508c2ac43cbd9797dc538c97f9240f658fc0cffcd73c937c64c1586d8" -->

<!-- pdac:cite id="TERM-MEAL-TYPE" digest="sha256:02ab71683e9a86620e98abdcd7109fec5a50cba3baed85d430dc84f32f3b64c3" -->

<!-- pdac:cite id="SB-RECIPES-ADD-TO-LIBRARY" digest="sha256:f02f677488f7b736528b92f93c5e40a54e38d6ade8505ae78324eb1f3791ed04" -->

<!-- pdac:cite id="SB-RECIPES-DUPLICATE-NAME-REJECTED" digest="sha256:7ce386f0fa1bf75160900e054f7400b4009413d1207a94052909f4042b93592a" -->

<!-- pdac:cite id="SB-RECIPES-RESTRICTED-MEAL-TYPE-HIDDEN" digest="sha256:5722d8a4be68d567d3d6e61116b25eb872ae1b469a58676624eddb0fe5a387e6" -->

<!-- pdac:cite id="SB-RECIPES-UNRESTRICTED-SHOWN-EVERYWHERE" digest="sha256:3bffd9c89095b2dae237b594b20912eaa4ad5991f355b169c34a26121b0471e6" -->

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
