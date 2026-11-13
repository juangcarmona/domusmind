<!-- pdac-scope: cited -->

# Meal Planning Specification

## Purpose

Meal Planning is the household's weekly food coordination capability. It answers what meals are planned for each day and meal type, what the household usually eats, and what needs to be purchased to cook those meals.

A meal plan represents the household's intended meals for one calendar week. Recipes form the household's reusable ingredient library. Weekly templates capture recurring patterns for fast reuse. Shopping lists are derived from assigned recipes and created in the Lists context.

Meal Planning is scoped to the household — not to individual members — and is optimized for low-effort reuse over from-scratch planning.

<!-- pdac:cite id="BC-MEAL-PLANNING" digest="sha256:bb3de4faf9d7ae3434d0db4180f005236ef81bc3ab84875aaa364e97ac3e66bd" -->

<!-- pdac:cite id="BR-MEALS-HOUSEHOLD-SCOPED-MEALS" digest="sha256:ef16950ff7825bfa0a1af1d5492432640c6f8ab77987aa3bf0fd4b20ca6aaa26" -->

<!-- pdac:cite id="JRN-MEALS-PLAN-THE-WEEK" digest="sha256:1ca92afdde559b5b4dc74423673bace6d67c62a826e7474e5321f033cb15b887" -->

<!-- pdac:cite id="QR-MEALS-LOW-EFFORT-PLANNING" digest="sha256:4791efc078efb88dccaddd5efad2ef16055fe6c61c8bd618f7c331bf3d0ec3da" -->

<!-- pdac:cite id="TERM-MEAL-PLAN" digest="sha256:8c7a85c9b75cec9a89206b938f18e09ff3ad0e373eef27c41e164c3068d4b886" -->

<!-- pdac:cite id="TERM-RECIPE" digest="sha256:3c619b3dae05589a60edac9f0ed0b63a78a963b1070a73cb6c4c5b76e9a570b4" -->

<!-- pdac:cite id="TERM-WEEKLY-TEMPLATE" digest="sha256:1c5972bfe3fcdc9462f102729b7f57cc8a9ca7da47ea8d1b4511e6119d325b62" -->

<!-- pdac:cite id="TERM-SHOPPING-LIST-DERIVATION" digest="sha256:a398f0ca78bcfa341bc725cf81fbc6d5aefc7023d819b0535fbfac4f59e0f07f" -->

---

## Requirements

### Requirement: Meal Plan Creation

A household SHALL be able to create a meal plan for any given calendar week.

A meal plan is always scoped to one family and one week. The week is defined by its start date, which must align with the household's configured first day of week and spans exactly seven days.

On creation, the full slot grid is materialized: one slot per day of week × per meal type (Breakfast, MidMorningSnack, Lunch, AfternoonSnack, Dinner), totalling 35 slots. All slots start as Unplanned.

A new meal plan is created in Draft status.

Only one Active plan may exist per family per week.

<!-- pdac:cite id="FR-MEALS-CREATE-PLAN" digest="sha256:b4ea95c07c8b56290b7d3d92e67a7ebc82290e2b8bef4e5314d0a17c726b1dd5" -->

<!-- pdac:cite id="UC-MEALS-CREATE-PLAN" digest="sha256:66ac90fb2f2596dc8da7299347b9a99411dfe406c769f49eeb2d5a0f409b15bd" -->

<!-- pdac:cite id="BR-MEALS-FULL-SLOT-GRID" digest="sha256:9e8aba8fb1cf802ae62731e77d2aefcd1c341fc52511fa39403944e4105cea93" -->

<!-- pdac:cite id="BR-MEALS-WEEK-DEFINITION" digest="sha256:96e848fe098b66954584cfe940defa059796b6349c8f7ef84f13792838a783e2" -->

<!-- pdac:cite id="BR-MEALS-ONE-PLAN-PER-WEEK" digest="sha256:79b515e0a731c9cccbf80f2561e44b2a485e421fafad348fb14da69c8af34163" -->

<!-- pdac:cite id="BR-MEALS-PLAN-LIFECYCLE" digest="sha256:2ef77d2a9bddb9b70267efcf0437b4da02dd0564d335132a699692f30f349e7f" -->

<!-- pdac:cite id="TERM-MEAL-SLOT" digest="sha256:9d2b20a39139b8f48ecd3b251fcba7a961e4004669fb2512051859cd7d9038a8" -->

<!-- pdac:cite id="TERM-MEAL-TYPE" digest="sha256:02ab71683e9a86620e98abdcd7109fec5a50cba3baed85d430dc84f32f3b64c3" -->

<!-- pdac:cite id="SB-MEALS-CREATE-PLAN-FOR-WEEK" digest="sha256:cdb8a8e3b9a811c32f06a2b2871448d8b464fac176dde6e9ddef21c6a9949401" -->

<!-- pdac:cite id="SB-MEALS-CREATE-PLAN-EXISTING-WEEK" digest="sha256:ddf128c1f5e3c1e0f407ce9d3a49e3aa6ec73372882465893e2b6ea4764b2670" -->

<!-- pdac-drift ids="BR-MEALS-ONE-PLAN-PER-WEEK, FR-MEALS-CREATE-PLAN" summary="Spec limits only Active plans to one per family and week; the model allows at most one meal plan of any status per week, so a Draft and an Active plan never coexist (decided: Q-0034)" -->

#### Scenario: Household creates a meal plan for the current week

- GIVEN a family exists with a configured first day of week
- WHEN the household creates a meal plan for a valid week start date
- THEN a meal plan is created in Draft status for that family and week
- AND all 35 slots (7 days × 5 meal types) are initialized as Unplanned

#### Scenario: A meal plan already exists for the target week

- GIVEN a meal plan already exists for the family and week
- WHEN the household attempts to create another plan for the same week
- THEN the existing plan is returned
- AND no duplicate plan is created

---

### Requirement: Meal Plan Lifecycle

A meal plan progresses through a defined lifecycle: Draft → Active → Completed.

A Draft plan is being set up. It is not yet the household's working plan for the week. An Active plan is the household's working plan for the week. A Completed plan is read-only and represents a past week.

Transition to Active must occur before the plan is treated as the household's working plan. Transition to Completed prevents all mutation.

<!-- pdac:cite id="FR-MEALS-PLAN-LIFECYCLE" digest="sha256:0cb76be9f01cd720fbec9478b35e99182d96992ed93c628407a46407e9dc3bb4" -->

<!-- pdac:cite id="UC-MEALS-PROMOTE-PLAN" digest="sha256:b26eecfdff35bb0f581dd955e0960ccc97969c86f451c03d7779b57679eb3c1d" -->

<!-- pdac:cite id="BR-MEALS-PLAN-LIFECYCLE" digest="sha256:2ef77d2a9bddb9b70267efcf0437b4da02dd0564d335132a699692f30f349e7f" -->

<!-- pdac:cite id="TERM-MEAL-PLAN-STATUS" digest="sha256:8771ab7cdffe66d265ba863bfd83723d03cd29ba69b9bbf968ccc7637f42f397" -->

<!-- pdac:cite id="SB-MEALS-PROMOTE-TO-ACTIVE" digest="sha256:d8cf84525ff2e7dea7470908615529bd84dc380dde0c07ed538d52427150b9f3" -->

#### Scenario: Plan is promoted from Draft to Active

- GIVEN a meal plan exists in Draft status
- WHEN the household promotes it to Active
- THEN the plan becomes the Active plan for that family and week
- AND no other Active plan may exist for the same family and week

---

### Requirement: Meal Slot Assignment

A household SHALL be able to assign or clear any slot in a meal plan.

A slot is identified by its day of week and meal type. Each slot carries a source type:

- **Recipe** — references a recipe from the household library
- **FreeText** — a free-text label (e.g., "pasta night")
- **External** — a meal outside the household (school lunch, restaurant)
- **Leftovers** — designated as leftovers from a prior meal
- **Unplanned** — explicitly left open

Each slot may also carry optional notes, an `isOptional` flag (non-binding meal, common for snacks), and an `isLocked` flag (slot is stable and should not change).

Slot structure is fixed at plan creation. This operation updates content only, not structure.

<!-- pdac:cite id="FR-MEALS-SLOT-ASSIGNMENT" digest="sha256:7c02ee787691996b2cafed0e60c636487f906536247d3f5960e2beb8c18d786b" -->

<!-- pdac:cite id="UC-MEALS-UPDATE-SLOT" digest="sha256:a659fa4c170ec938919501335736ec5ea87ea35c81cc207d793db62e5a6c9d74" -->

<!-- pdac:cite id="BR-MEALS-LOCKED-SLOT" digest="sha256:42078088f75a852c312c635a114bdd36a4a8f9dd99ebc945abb6eee2412f3cfd" -->

<!-- pdac:cite id="BR-MEALS-SLOT-SOURCE-CONSISTENCY" digest="sha256:01a7eacd0c659a25a57c818911d9f22659e7431a262a8d271ecca389033516ad" -->

<!-- pdac:cite id="BR-MEALS-FULL-SLOT-GRID" digest="sha256:9e8aba8fb1cf802ae62731e77d2aefcd1c341fc52511fa39403944e4105cea93" -->

<!-- pdac:cite id="BR-MEALS-PLAN-LIFECYCLE" digest="sha256:2ef77d2a9bddb9b70267efcf0437b4da02dd0564d335132a699692f30f349e7f" -->

<!-- pdac:cite id="TERM-MEAL-SLOT" digest="sha256:9d2b20a39139b8f48ecd3b251fcba7a961e4004669fb2512051859cd7d9038a8" -->

<!-- pdac:cite id="TERM-MEAL-SOURCE" digest="sha256:56aae694f8d61a548820236be6b3ad413dfd25881d95ca3a9fb37465c74794a7" -->

<!-- pdac:cite id="SB-MEALS-ASSIGN-RECIPE-DRAFT" digest="sha256:3507447df3332c09a80b1f84599da4cb8d954bf2d6119d45db76ed876bb9e606" -->
<!-- pdac:cite id="SB-MEALS-ASSIGN-RECIPE-ACTIVE" digest="sha256:5dabd0c7cbd087636db467f44c2ec264d5633513a8b47d41617478bf31a2bbc6" -->

<!-- pdac:cite id="SB-MEALS-CLEAR-SLOT" digest="sha256:d98aa580c0172fd881adb4cd60f116bc6f71bf115a0d043e156e600226d7fc4b" -->

<!-- pdac:cite id="SB-MEALS-LOCKED-SLOT-REJECTED" digest="sha256:1dfe17e272d3c09200f41ccab0e6bf219c3e770ef7232c987e9f927643e8fc4c" -->

<!-- pdac:cite id="SB-MEALS-UNLOCK-AND-CHANGE-SLOT" digest="sha256:fd3d0381f1f1d61e63e257240f436d852cc75a7e9a41b3b5785a4919fbce5f24" -->

<!-- pdac:cite id="SB-MEALS-COMPLETED-PLAN-REJECTS-CHANGE" digest="sha256:e72b100827c44739ff2100673f0adbfaeb76ccba7ecca3fc5ee1aa76343b35fd" -->

#### Scenario: Household assigns a recipe to a slot

- GIVEN a meal plan in Draft or Active status exists
- AND a recipe from the household library is available
- WHEN the household assigns the recipe to a specific day and meal type
- THEN the slot's source type is set to Recipe with the recipe reference
- AND the slot is updated within the plan

#### Scenario: Household clears a slot

- GIVEN a meal plan with a populated slot
- WHEN the household clears that slot
- THEN the slot's source type is set to Unplanned
- AND any previous recipe reference or free text is removed
- AND the slot remains in the grid

#### Scenario: Household attempts to mutate a locked slot

- GIVEN a slot with `isLocked = true`
- WHEN the household submits a content change without unlocking the slot
- THEN the update is rejected
- AND the slot content is unchanged

#### Scenario: Household unlocks and mutates a locked slot

- GIVEN a slot with `isLocked = true`
- WHEN the household submits a content change that includes `isLocked = false`
- THEN the slot is unlocked and the content change is applied in the same operation

#### Scenario: Household attempts to mutate a Completed plan

- GIVEN a meal plan in Completed status
- WHEN the household attempts to update any slot
- THEN the update is rejected
- AND the plan remains unchanged

---

### Requirement: Meal Plan Viewing

A household SHALL be able to retrieve the full detail of a meal plan for any week.

The response includes all 35 slots in day-then-meal-type order, including Unplanned slots. For Recipe slots, recipe metadata is included inline.

The plan may be retrieved by plan identifier or by family + week start.

<!-- pdac:cite id="FR-MEALS-VIEW-PLAN" digest="sha256:e5cc4ed01313a4b447c75dd5ced3068f866ec6b6187aecf21a0beb19222cd404" -->

<!-- pdac:cite id="UC-MEALS-VIEW-PLAN" digest="sha256:3ef4f20651e6ffa416f797c98f3ed02aa81698cf7921d3102f4feaa6f347959b" -->

<!-- pdac:cite id="BR-MEALS-MEAL-TYPE-ORDER" digest="sha256:9d61e77141750d98778a1c4f1abafbd707dc0787c27148cedb24b5d2b0a2a429" -->

<!-- pdac:cite id="SB-MEALS-VIEW-WEEK-ORDERED" digest="sha256:568230584a7dabd3640ba2eb7131d1fcf37fda3d12c53011d9136d475eb8bb18" -->

<!-- pdac:cite id="SB-MEALS-VIEW-RECIPE-SLOT-DETAILS" digest="sha256:fdeefa683b681a069d124ef05223f9730f017fde1d62ea5c8e6b34f469ec85ac" -->

#### Scenario: Household views the current week's meal plan

- GIVEN an Active meal plan exists for the current week
- WHEN the household retrieves it by family and week start
- THEN all 35 slots are returned ordered by day (from the household's first day of week) then by meal type
- AND Unplanned slots are included in the response

#### Scenario: Household views a plan with recipe slots

- GIVEN a meal plan with one or more Recipe-type slots
- WHEN the household retrieves the plan
- THEN each Recipe slot includes the recipe name, servings, and time metadata inline

---

### Requirement: Recipe Library

A household SHALL maintain a library of recipes scoped to the family.

A recipe has a name (unique within the family), optional description, optional preparation and cook times, optional servings count, an ingredient list, optional tags for classification, an optional set of allowed meal types indicating which meal types the recipe is appropriate for, and an `isFavorite` flag.

Ingredients within a recipe must have unique names (to support deduplication during shopping list derivation). Ingredient quantity and unit are optional. When both preparation and cook times are provided, total time is derived as their sum; if either is absent, total time is unset.

A recipe may not be deleted if it is currently referenced by any active meal plan slot.

Recipes may be updated after creation; ingredients can be added or modified.

<!-- pdac:cite id="FR-RECIPES-LIBRARY" digest="sha256:1340dae095e80b3c9ea2f47bb1780af2e6ae2acbd28baeb5750cd532bd02d34d" -->

<!-- pdac:cite id="UC-RECIPES-CREATE-RECIPE" digest="sha256:b1982c4e6063006dac46caf79a882dbb237450836e34795ea7dd1751b8ce02a8" -->

<!-- pdac:cite id="UC-RECIPES-BROWSE-LIBRARY" digest="sha256:fbcff790cf18adf9b802721d066aafff3e01dcd239d62fb5ff197f8e629837e9" -->

<!-- pdac:cite id="BR-RECIPES-NAME-UNIQUE" digest="sha256:54fe88bd63ffb8100bf3dbb97bf05c0275a1dfcc89645b9659c013783a93e7a8" -->

<!-- pdac:cite id="BR-RECIPES-INGREDIENT-NAME-UNIQUE" digest="sha256:49bf51a252a84807602bd53fcbbe8754840772a71957f7108738501f3cc4e0a0" -->

<!-- pdac:cite id="BR-RECIPES-TOTAL-TIME" digest="sha256:6a507e94619fa4a98cb67c168864e0acf6e037852123b0283d56a0b350766ff1" -->

<!-- pdac:cite id="BR-RECIPES-ALLOWED-MEAL-TYPES" digest="sha256:d5576dfc67764576f0e2d0f681e4c56655eb6be09ff5e5ed95b3c1fc87241f3a" -->

<!-- pdac:cite id="FR-RECIPES-DELETE" digest="sha256:7ad724d99da2ba8bd340eedddcf5d47e338caef5ccfecbf0a3628252da8e6015" -->

<!-- pdac:cite id="UC-RECIPES-DELETE-RECIPE" digest="sha256:47bb5bbb4aaa4ff7c6a788ca533d974aa3fa9561177752b8ca6dfc7c55ff8b56" -->

<!-- pdac:cite id="BR-RECIPES-DELETE-GUARD" digest="sha256:2e15467fd305be37d291774e1e09606ef41448d1713b4f98d5ff2cbce2ada86e" -->

<!-- pdac:cite id="FR-RECIPES-UPDATE" digest="sha256:beed777fe2618300d42fcb98eb5e6193fc8c62aa1e05a00957e43ed27ed57d83" -->

<!-- pdac:cite id="UC-RECIPES-UPDATE-RECIPE" digest="sha256:0fb0ec3cf0b3ef1ad1159d45b722e6be18e00d694f7290a93b9a29f5a26c27f5" -->

<!-- pdac:cite id="FR-RECIPES-INGREDIENTS" digest="sha256:82e9ab23dc511501d2a8f51222e43dc395c8f99365dd238e91dabebae50b745b" -->

<!-- pdac:cite id="UC-RECIPES-MANAGE-INGREDIENTS" digest="sha256:1aa4b2786d2d9f085881e2c21e1b3622f14f13f9ea52a1813f0a93e319712f00" -->

<!-- pdac:cite id="TERM-RECIPE" digest="sha256:3c619b3dae05589a60edac9f0ed0b63a78a963b1070a73cb6c4c5b76e9a570b4" -->

<!-- pdac:cite id="TERM-INGREDIENT" digest="sha256:e70048c508c2ac43cbd9797dc538c97f9240f658fc0cffcd73c937c64c1586d8" -->

<!-- pdac:cite id="SB-RECIPES-ADD-TO-LIBRARY" digest="sha256:f02f677488f7b736528b92f93c5e40a54e38d6ade8505ae78324eb1f3791ed04" -->

<!-- pdac:cite id="SB-RECIPES-DUPLICATE-NAME-REJECTED" digest="sha256:7ce386f0fa1bf75160900e054f7400b4009413d1207a94052909f4042b93592a" -->

<!-- pdac:cite id="SB-RECIPES-RESTRICTED-MEAL-TYPE-HIDDEN" digest="sha256:5722d8a4be68d567d3d6e61116b25eb872ae1b469a58676624eddb0fe5a387e6" -->

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

---

### Requirement: Weekly Templates

A household SHALL be able to create named, reusable weekly meal patterns.

A weekly template captures a set of slot assignments (day + meal type combinations) with the same source type and metadata as meal slots. Template names must be unique within the family. Templates may have fewer than 35 slots defined; unrepresented slots default to Unplanned when the template is applied.

Templates may be updated after creation; slot assignments can be added or modified.

<!-- pdac:cite id="FR-MEALS-WEEKLY-TEMPLATES" digest="sha256:2eba7b562fe2c1e7c81c462a0a3279f0bf8becd39cb4f147516761100f64dece" -->

<!-- pdac:cite id="UC-MEALS-CREATE-TEMPLATE" digest="sha256:a815c4bccb4dbc71768fb42f3b5cac822eda8cc8ceb42580d152f01364c5d068" -->

<!-- pdac:cite id="UC-MEALS-UPDATE-TEMPLATE" digest="sha256:581e4601dd6ad2ad141fc08f86be5e68e97e87d62c79067db2c4b08994e20fa1" -->

<!-- pdac:cite id="BR-MEALS-TEMPLATE-NAME-UNIQUE" digest="sha256:80e26b29a89dc804d1fa24bfd2d2c6a7a8a661ba3dbafd314aaaec18917f8d90" -->

<!-- pdac:cite id="TERM-WEEKLY-TEMPLATE" digest="sha256:1c5972bfe3fcdc9462f102729b7f57cc8a9ca7da47ea8d1b4511e6119d325b62" -->

<!-- pdac:cite id="SB-MEALS-CREATE-TEMPLATE" digest="sha256:b098dd5c4e44ec2e6efba492a27e9cc9723812f7f2fc43427da59a06690bfae7" -->

<!-- pdac:cite id="SB-MEALS-TEMPLATE-DUPLICATE-NAME-REJECTED" digest="sha256:d59adc6867efd50812a1f3a854803a323103fed1cdf9ed6a4511afa6f1acc751" -->

#### Scenario: Household creates a weekly template

- GIVEN a family exists
- WHEN the household creates a template with a unique name and optional slot assignments
- THEN the template is saved to the family's template library
- AND it becomes available to apply to future weeks

#### Scenario: Household attempts to create a template with a duplicate name

- GIVEN a template named "Standard week" already exists
- WHEN the household creates another template with the same name
- THEN the creation is rejected

---

### Requirement: Apply Weekly Template

A household SHALL be able to create a new meal plan for a target week pre-populated from an existing weekly template.

Template application creates a new meal plan with slots copied from the template. Slots not represented in the template default to Unplanned. The plan records which template was applied. Template application is a snapshot — subsequent changes to the template do not affect already-created plans.

Recipe references within the template must be valid at the time of application. If a recipe referenced by the template has been deleted, the application fails.

If a plan already exists for the target week, the existing plan is returned and the template is not re-applied.

After a template is applied, individual slots may still be modified.

<!-- pdac:cite id="FR-MEALS-APPLY-TEMPLATE" digest="sha256:bd1bff7939cc76096b2e6a88d0364fe00ea1e877ac55e8909033abd163b49597" -->

<!-- pdac:cite id="UC-MEALS-APPLY-TEMPLATE" digest="sha256:2d62181e6fc6fe0dedc0d942713adf63104020c991ef3db8b32acc114d53ce66" -->

<!-- pdac:cite id="BR-MEALS-TEMPLATE-SNAPSHOT" digest="sha256:796a58fc2dbc6b961509b790f8c39fdd2fa9062d5844b804f99e054ff2466639" -->

<!-- pdac:cite id="BR-MEALS-ONE-PLAN-PER-WEEK" digest="sha256:79b515e0a731c9cccbf80f2561e44b2a485e421fafad348fb14da69c8af34163" -->

<!-- pdac:cite id="SB-MEALS-APPLY-TEMPLATE-EMPTY-WEEK" digest="sha256:60cfa7964f9a06a5898414ac9de2eb95c146f1bb44397b58c26e3fa980dfa45c" -->

<!-- pdac:cite id="SB-MEALS-APPLY-TEMPLATE-DELETED-RECIPE" digest="sha256:28262f1a33526ec57c83279b5c9e40104afa0e8966855980e1d295cd16edf5d1" -->

<!-- pdac:cite id="SB-MEALS-APPLY-TEMPLATE-EXISTING-WEEK" digest="sha256:ad2d3e00904d05e58fc37d3489f927009b424dd3713089a323acbf2ce739a1ea" -->

#### Scenario: Household applies a template to an empty week

- GIVEN a weekly template exists and no plan exists for the target week
- WHEN the household applies the template to a target week
- THEN a meal plan is created in Draft status with slots populated from the template
- AND slots not covered by the template are Unplanned
- AND the plan records a reference to the applied template

#### Scenario: Template references a deleted recipe

- GIVEN a weekly template with a slot that references a recipe
- AND that recipe has since been deleted from the household library
- WHEN the household applies the template to a target week
- THEN the application fails
- AND no meal plan is created

#### Scenario: A plan already exists for the target week

- GIVEN a meal plan already exists for the target family and week
- WHEN the household applies a template to the same week
- THEN the existing plan is returned
- AND the template is not re-applied

---

### Requirement: Copy from Previous Week

A household SHALL be able to create a new meal plan for a target week by cloning the slot assignments from a previous week's plan.

The source is either an explicitly provided plan or the plan for the immediately preceding week. The copy carries over slot source types, recipe references, free text, notes, and flags. Shopping list references are never transferred — derivation starts fresh for the new week.

If no source plan exists for the preceding week, the operation returns a recoverable outcome (not a hard failure) and the household can proceed to create a plan from scratch or apply a template. If a plan already exists for the target week, the existing plan is returned.

This operation is distinct from applying a template. Templates are named, reusable patterns; copy-from-week is ad-hoc reuse.

<!-- pdac:cite id="FR-MEALS-COPY-PREVIOUS-WEEK" digest="sha256:74b8f1780a8074d9b9b7ece1f173baeea40d5271577fd12b92eb5b34ae71bab6" -->

<!-- pdac:cite id="UC-MEALS-COPY-PREVIOUS-WEEK" digest="sha256:1afa37619a41616c6389f594c9cb4364ff6e6bdacc7c5744d3e15fbc0ae62ec6" -->

<!-- pdac:cite id="BR-MEALS-COPY-EXCLUDES-SHOPPING-LIST" digest="sha256:cbba531e4d36c7e9ce8717af57727391a51c6ae791eef0959b3a43e95afbe036" -->

<!-- pdac:cite id="BR-MEALS-ONE-PLAN-PER-WEEK" digest="sha256:79b515e0a731c9cccbf80f2561e44b2a485e421fafad348fb14da69c8af34163" -->

<!-- pdac:cite id="SB-MEALS-COPY-PREVIOUS-WEEK" digest="sha256:6b66af89bc9d08aec95c5a2191cf81d11aae9e7c7313d70b477f4750cbbad269" -->

<!-- pdac:cite id="SB-MEALS-COPY-NO-PREVIOUS-PLAN" digest="sha256:2664cab7e6941b3671f4baae2347617a20428b02f30f5c5b7dcff1e5b101d661" -->

#### Scenario: Household copies the previous week's plan

- GIVEN a meal plan exists for the preceding week
- AND no plan exists for the target week
- WHEN the household copies the previous week
- THEN a new meal plan is created in Draft status for the target week
- AND all slot assignments from the source plan are copied
- AND shopping list references from the source plan are not carried over

#### Scenario: No source plan exists for the preceding week

- GIVEN no meal plan exists for the preceding week
- WHEN the household attempts to copy the previous week
- THEN the operation returns a recoverable "no previous plan" outcome
- AND the household can create a plan from scratch or apply a template

---

### Requirement: Shopping List Derivation

A household SHALL be able to derive a shopping list from a meal plan's recipe assignments.

Derivation consolidates all ingredients from all Recipe-type slots in the plan. Ingredients are deduplicated by name (case-insensitive) within the same unit. Ingredients with differing units are kept as separate items.

Each derivation request creates a new shopping list in the Lists context. Previous shopping lists derived from the same plan are not mutated. The meal plan records a reference to the most recently derived shopping list and increments a version counter on each derivation.

The resulting shopping list is a first-class List in the Lists context. Meal Planning does not own it after creation and does not receive feedback when items are checked off.

At least one Recipe-type slot must be assigned in the plan for derivation to proceed.

<!-- pdac:cite id="FR-MEALS-SHOPPING-LIST-DERIVATION" digest="sha256:b80be8ac1ed40f2e58ff5d426c5fcdff0c93c3826ffc5907d595f6349287635b" -->

<!-- pdac:cite id="UC-MEALS-DERIVE-SHOPPING-LIST" digest="sha256:303aa9be7ed639ffbf4f8e4d202984c84439d3ddc61c1facc3c10a00f5bafc7f" -->

<!-- pdac:cite id="BR-MEALS-SHOPPING-LIST-REQUIRES-RECIPE" digest="sha256:f3fe88b6db2c19f271054f44d9a3b59b72294f96d60393aac13e36a996c2f9b8" -->

<!-- pdac:cite id="BR-MEALS-SHOPPING-LIST-CONSOLIDATION" digest="sha256:f0adea452cccbb8dce850f9bb2466376978361834fb832ee97668e8b8f772c8f" -->

<!-- pdac:cite id="BR-MEALS-SHOPPING-LIST-NEW-EACH-TIME" digest="sha256:8799a109481010d97e95891dd9e7e523839db1adc0b59770f05f44bd40b7ac70" -->

<!-- pdac:cite id="TERM-SHOPPING-LIST-DERIVATION" digest="sha256:a398f0ca78bcfa341bc725cf81fbc6d5aefc7023d819b0535fbfac4f59e0f07f" -->

<!-- pdac:cite id="SB-MEALS-SHOPPING-LIST-FROM-PLAN" digest="sha256:e2d159d0fbbc1552242ef9913ddbd004ebd136a6b122a05bcfe8f35a1d35518a" -->

<!-- pdac:cite id="SB-MEALS-SHOPPING-LIST-RE-REQUEST" digest="sha256:156cfd06d47fc2b2946bb88004b230ba889d7f2408aeeea7433f0fd192e9195f" -->

<!-- pdac:cite id="SB-MEALS-SHOPPING-LIST-NO-RECIPES" digest="sha256:894109c2d98337fa2ba7d2d0213dcb89382fc6bfc60b897fe938054eae4ee329" -->

#### Scenario: Household requests a shopping list from an active plan

- GIVEN a meal plan with at least one Recipe-type slot
- AND the recipe has at least one ingredient
- WHEN the household requests a shopping list
- THEN a shopping list is created in the Lists context with one item per consolidated ingredient
- AND the meal plan records the shopping list reference and increments its derivation version

#### Scenario: Household re-requests a shopping list

- GIVEN a meal plan that already has a shopping list reference
- WHEN the household requests a new shopping list
- THEN a new shopping list is created
- AND the previous shopping list is not modified
- AND the meal plan's derivation version increments

#### Scenario: Plan has no recipe slots assigned

- GIVEN a meal plan with no Recipe-type slots
- WHEN the household requests a shopping list
- THEN the request is rejected
- AND no shopping list is created

---

### Requirement: Agenda Projection

Meal slots project into the Agenda surface as non-timed household-level entries.

Slots with `mealSourceType = Unplanned` and no notes are not projected. Projected slots are read-only in the Agenda — editing occurs only in the Meal Planning surface.

<!-- pdac:cite id="FR-MEALS-AGENDA-PROJECTION" digest="sha256:c647b93ceb2e80493b6ea0d5df2ab49ac174e5623a111df1a57cf0e03633561c" -->

<!-- pdac:cite id="UC-MEALS-SEE-MEALS-IN-AGENDA" digest="sha256:08bdb9e168b619bde9e0966da74899d2f183db8ae80f1ee965546224c112d3d9" -->

<!-- pdac:cite id="BR-MEALS-AGENDA-PROJECTION" digest="sha256:fecf2e8fbd7fdf7674ba5f7d8870727b2d53f87d2da15c73e0bed19ffb4af998" -->

<!-- pdac:cite id="BR-AGENDA-PROJECTION-READ-ONLY" digest="sha256:a3c72a044c52c99808fa103720f30e8737d55fc1f7c9b6d7cc78e56c7539359c" -->

<!-- pdac:cite id="SB-MEALS-AGENDA-SHOWS-ASSIGNED-MEAL" digest="sha256:a40268ca794a6a5d985fec6059d791c737a26fe70c48296e704d6ebe00a429d9" -->

<!-- pdac:cite id="SB-MEALS-AGENDA-HIDES-EMPTY-SLOT" digest="sha256:166d44d656e04d272674ff758073da6fbbcbcb60a625daa7d67a2c863e3efef6" -->

#### Scenario: Assigned meal slot appears in the Agenda

- GIVEN a meal plan with a Recipe-type slot on a specific day
- WHEN the household views the Agenda for that day
- THEN the meal slot appears as a non-timed household entry
- AND it is visually distinct from Calendar Events and Tasks

#### Scenario: Unplanned slot without notes is excluded from Agenda

- GIVEN a slot with `mealSourceType = Unplanned` and no notes
- WHEN the household views the Agenda for that day
- THEN the slot does not appear

---

## Notes

### Ambiguity: Active plan promotion timing

The context document states a plan may be promoted to Active "explicitly, or automatically when the week begins." The automatic promotion rule is marked as TBD. The current spec treats promotion as explicit only.

<!-- pdac-drift ids="BR-MEALS-PLAN-LIFECYCLE, FR-MEALS-PLAN-LIFECYCLE, UC-MEALS-PROMOTE-PLAN" summary="Note keeps automatic promotion when the week begins as an open TBD; the model decided activation is an explicit household action only and automatic promotion is not current product (decided: Q-0033)" -->

### Resolved: Mutation on Draft vs. Active plans

Both Draft and Active plans allow slot mutation. Only Completed plans prevent mutation. The source `update-meal-slot` feature spec contained a contradictory precondition; the intent documented in the same file ("Draft plans allow mutation; Completed plans do not") is treated as the authoritative statement.

### Future behavior: Completed transition

The `Completed` status is defined in the domain model to prevent modeling debt but its transition trigger is not yet specified. It is included in the spec as a lifecycle state; scenarios for Completed transition are intentionally absent.

### Partial specification: Shopping list unit consolidation

The consolidation rule for mismatched units (e.g., "500g flour" and "2 cups flour") results in separate list items. Consolidated quantity calculation when units match is referenced but not fully specified.

---

## Source References

- `docs/04_contexts/meal-planning.md`
- `00_product/surfaces/meal-planning.md`
- `specs/features/meal-planning/create-meal-plan.md`
- `specs/features/meal-planning/view-meal-plan.md`
- `specs/features/meal-planning/update-meal-slot.md`
- `specs/features/meal-planning/create-recipe.md`
- `specs/features/meal-planning/create-weekly-template.md`
- `specs/features/meal-planning/apply-weekly-template.md`
- `specs/features/meal-planning/copy-meal-plan-from-previous-week.md`
- `specs/features/meal-planning/request-shopping-list.md`
