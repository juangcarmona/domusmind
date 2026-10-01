<!-- pdac-scope: cited -->

# Meal Planning Specification

## Purpose

Meal Planning is the household's weekly food coordination capability. It answers what meals are planned for each day and meal type, what the household usually eats, and what needs to be purchased to cook those meals.

A meal plan represents the household's intended meals for one calendar week. Recipes form the household's reusable ingredient library. Weekly templates capture recurring patterns for fast reuse. Shopping lists are derived from assigned recipes and created in the Lists context.

Meal Planning is scoped to the household — not to individual members — and is optimized for low-effort reuse over from-scratch planning.

<!-- pdac:cite id="BC-MEAL-PLANNING" digest="sha256:3e2d211a42325e9e719480512210f6ec9c5255f4fa647518cae3598e2bd6e835" -->

<!-- pdac:cite id="BR-MEALS-HOUSEHOLD-SCOPED-MEALS" digest="sha256:0b82c4ba89f1570e491171ecd6cbf6cd8b98be75e2608073314111316ae03578" -->

<!-- pdac:cite id="JRN-MEALS-PLAN-THE-WEEK" digest="sha256:8a5e4f5dc8677bdcb01c98b79c3667423193e5ce5b188a870ba7340bf230c389" -->

<!-- pdac:cite id="QR-MEALS-LOW-EFFORT-PLANNING" digest="sha256:4791efc078efb88dccaddd5efad2ef16055fe6c61c8bd618f7c331bf3d0ec3da" -->

<!-- pdac:cite id="TERM-MEAL-PLAN" digest="sha256:2c36b02dbd255e1ac153a444a95b6f8d7355b6e3089be3ada49cd3dad0820655" -->

<!-- pdac:cite id="TERM-RECIPE" digest="sha256:a0a6852e2fe6cc29247e1d0cf8e2159cea630e420d1f40adc5458c02a4e7fcd1" -->

<!-- pdac:cite id="TERM-WEEKLY-TEMPLATE" digest="sha256:156bfda95386163b06873898df7db200ef7e0f0e7baab114e37d6b0d4963d7af" -->

<!-- pdac:cite id="TERM-SHOPPING-LIST-DERIVATION" digest="sha256:b33b4e40ecae229e5851c7ba275f246ff64fe44b475d5be3d79481dcb18fd802" -->

---

## Requirements

### Requirement: Meal Plan Creation

A household SHALL be able to create a meal plan for any given calendar week.

A meal plan is always scoped to one family and one week. The week is defined by its start date, which must align with the household's configured first day of week and spans exactly seven days.

On creation, the full slot grid is materialized: one slot per day of week × per meal type (Breakfast, MidMorningSnack, Lunch, AfternoonSnack, Dinner), totalling 35 slots. All slots start as Unplanned.

A new meal plan is created in Draft status.

Only one Active plan may exist per family per week.

<!-- pdac:cite id="FR-MEALS-CREATE-PLAN" digest="sha256:2f8f41b21e548a7d878c13ae6e409668d8ac35a551021b67f7803b6a8c1696e8" -->

<!-- pdac:cite id="UC-MEALS-CREATE-PLAN" digest="sha256:90229296d130334f0312ab047c9cde02a46b9037833d181e32f0cf220f57f52a" -->

<!-- pdac:cite id="BR-MEALS-FULL-SLOT-GRID" digest="sha256:9284b58a711674cab442d4b027a35df1afd178ce648c47a2abf9a5627e35d3c7" -->

<!-- pdac:cite id="BR-MEALS-WEEK-DEFINITION" digest="sha256:e56b5b1091b3e0f23fb0a34acc76fbdbf582492ac9f97249ec2589712feef07c" -->

<!-- pdac:cite id="BR-MEALS-ONE-PLAN-PER-WEEK" digest="sha256:33f78b7ccfa17bb1ce989cfe645563df04874c9652751ff9ff35af38b43980e4" -->

<!-- pdac:cite id="BR-MEALS-PLAN-LIFECYCLE" digest="sha256:0fe85a5dcd5dc774757a21d49f082cfa1d682a691a7effd635884f893ed1a82d" -->

<!-- pdac:cite id="TERM-MEAL-SLOT" digest="sha256:e1022c9568cb86661d3811030bac6e6296518e04db2b37748c98176abc39a01a" -->

<!-- pdac:cite id="TERM-MEAL-TYPE" digest="sha256:f22d542a1f5beb9048900ce2c4360011043e7f8c3b16f58294387cbb56c116cc" -->

<!-- pdac:cite id="SB-MEALS-CREATE-PLAN-FOR-WEEK" digest="sha256:ea84622871238a8bacb2a1a50aaee6bb1434a31f6e7784bbcc44deb29b800271" -->

<!-- pdac:cite id="SB-MEALS-CREATE-PLAN-EXISTING-WEEK" digest="sha256:7c71b93857cbdef272de8a00517545b64f8b668160a6ca3e4fe225bba2f38ea8" -->

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

<!-- pdac:cite id="FR-MEALS-PLAN-LIFECYCLE" digest="sha256:3e17a946d9445ec9a37bdc8bd76127f5b7eabacbffba92e794ec0b88ab04d220" -->

<!-- pdac:cite id="UC-MEALS-PROMOTE-PLAN" digest="sha256:cd089a6ab40e0e2f21f77d12dc706f62787ec58ecd4dee1947683a8c95258d7a" -->

<!-- pdac:cite id="BR-MEALS-PLAN-LIFECYCLE" digest="sha256:0fe85a5dcd5dc774757a21d49f082cfa1d682a691a7effd635884f893ed1a82d" -->

<!-- pdac:cite id="TERM-MEAL-PLAN-STATUS" digest="sha256:c2ccc55684994f91de79b4de0675ac21b7feb923fe92f5477780c1d4ffcf2f05" -->

<!-- pdac:cite id="SB-MEALS-PROMOTE-TO-ACTIVE" digest="sha256:23323c98b838fe88998219eba385bfbb5573244a0663a28f5c9d859871babb27" -->

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

<!-- pdac:cite id="FR-MEALS-SLOT-ASSIGNMENT" digest="sha256:bdd7b16540e780306ee6d7a6787ee483f0b7d4ae2a781b473fc1847d91edf487" -->

<!-- pdac:cite id="UC-MEALS-UPDATE-SLOT" digest="sha256:717756771462a30e271a209fda42ebbdbad3244f7544420f05079851a42bd301" -->

<!-- pdac:cite id="BR-MEALS-LOCKED-SLOT" digest="sha256:679838c99e1dd8907940112c9a2d2655c6a48c798f85e6bcc858a429f1309879" -->

<!-- pdac:cite id="BR-MEALS-SLOT-SOURCE-CONSISTENCY" digest="sha256:03a34f37b2869c29bd8b77693533f108cf4583a5307698f03a8306b85bf4df42" -->

<!-- pdac:cite id="BR-MEALS-FULL-SLOT-GRID" digest="sha256:9284b58a711674cab442d4b027a35df1afd178ce648c47a2abf9a5627e35d3c7" -->

<!-- pdac:cite id="BR-MEALS-PLAN-LIFECYCLE" digest="sha256:0fe85a5dcd5dc774757a21d49f082cfa1d682a691a7effd635884f893ed1a82d" -->

<!-- pdac:cite id="TERM-MEAL-SLOT" digest="sha256:e1022c9568cb86661d3811030bac6e6296518e04db2b37748c98176abc39a01a" -->

<!-- pdac:cite id="TERM-MEAL-SOURCE" digest="sha256:7656aa69693865849d0cc5d725175254399d26175a51c72d983e3ddfbaf0657c" -->

<!-- pdac:cite id="SB-MEALS-ASSIGN-RECIPE-TO-SLOT" digest="sha256:032e468fc5297acb1121b41068358e535a7a18b47f0fbab049861bbc97832b50" -->

<!-- pdac:cite id="SB-MEALS-CLEAR-SLOT" digest="sha256:9ab179639c7385b523ae8562492d3d87488993e4500faefbee15f608491d2b49" -->

<!-- pdac:cite id="SB-MEALS-LOCKED-SLOT-REJECTED" digest="sha256:1c8460ad00e627f29831b3551f6b2548cffe9458209cebc46ca25a730aa1b93a" -->

<!-- pdac:cite id="SB-MEALS-UNLOCK-AND-CHANGE-SLOT" digest="sha256:d671d74a5abd011a69c732a2af6434f448ee76302b9953b2110d51ab37e6db9f" -->

<!-- pdac:cite id="SB-MEALS-COMPLETED-PLAN-REJECTS-CHANGE" digest="sha256:748b452ae3bcdd5e414ec9507ce1658d18855f7f9a084b8123b5bb9831c396c1" -->

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

<!-- pdac:cite id="FR-MEALS-VIEW-PLAN" digest="sha256:48a9c335929f16e463648cb2d7c6614ac4bea9d8787546a45f5ae54419fa7182" -->

<!-- pdac:cite id="UC-MEALS-VIEW-PLAN" digest="sha256:84705ec7d88371f7bec6fe27e892952749481a0a4c2558f90bd5fa8dd9ee800c" -->

<!-- pdac:cite id="BR-MEALS-MEAL-TYPE-ORDER" digest="sha256:95b12cbd508b2e4794aec496141af92f6a7e858840436b249bce0dec118276a9" -->

<!-- pdac:cite id="SB-MEALS-VIEW-WEEK-ORDERED" digest="sha256:ba847ad6166aaef57f1e483bee2e2822560dd7e9b75f1104c20c1924366aa7be" -->

<!-- pdac:cite id="SB-MEALS-VIEW-RECIPE-SLOT-DETAILS" digest="sha256:9f9c04d90b99a8b2acec8a8a8bd45c36da7149dfc2ad2cc4b56ea524731b6a40" -->

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

<!-- pdac:cite id="FR-RECIPES-LIBRARY" digest="sha256:702c8c7996f89cc2fd0f678d4a163db664aae0ca384e742133ad8cdd9001bcfc" -->

<!-- pdac:cite id="UC-RECIPES-CREATE-RECIPE" digest="sha256:c9aba5e8213215812c9767138d04130ab2fa2875d493c7a72f060f8cbd2b6e35" -->

<!-- pdac:cite id="UC-RECIPES-BROWSE-LIBRARY" digest="sha256:457336010457babd21f0b02fee26cef2f00e543726236d9e802629ed8f98833f" -->

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

<!-- pdac:cite id="SB-RECIPES-ADD-TO-LIBRARY" digest="sha256:996027c711dbc9b8672ea360f56f03685cdee2486d8f76b2ac57e756a75f6abb" -->

<!-- pdac:cite id="SB-RECIPES-DUPLICATE-NAME-REJECTED" digest="sha256:93b9280f5809d13a6db3abc8c9c0926093f881c3b335733479ae11f28d0ae3bb" -->

<!-- pdac:cite id="SB-RECIPES-RESTRICTED-MEAL-TYPE-HIDDEN" digest="sha256:9b4c3469f79df26d76e65bff3e37928f17658561178468cfd92c5017edaf4653" -->

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

<!-- pdac:cite id="FR-MEALS-WEEKLY-TEMPLATES" digest="sha256:3f289e4c8bceb3b2d6f20572c5c40559ec350888e2f1a582ab378ec1ce95bf32" -->

<!-- pdac:cite id="UC-MEALS-CREATE-TEMPLATE" digest="sha256:fde61ac64d4360498beb08890137a72cda38af8c08813c1bfc700021fffc943a" -->

<!-- pdac:cite id="UC-MEALS-UPDATE-TEMPLATE" digest="sha256:581e4601dd6ad2ad141fc08f86be5e68e97e87d62c79067db2c4b08994e20fa1" -->

<!-- pdac:cite id="BR-MEALS-TEMPLATE-NAME-UNIQUE" digest="sha256:971364e46b10d13f7a261b28d94dc71be2a70b5de50d084246a959414f39a75f" -->

<!-- pdac:cite id="TERM-WEEKLY-TEMPLATE" digest="sha256:156bfda95386163b06873898df7db200ef7e0f0e7baab114e37d6b0d4963d7af" -->

<!-- pdac:cite id="SB-MEALS-CREATE-TEMPLATE" digest="sha256:8a24875c2fd4a6d21fcafd244f254009b113f362a99dda75651e93f08d8b72c7" -->

<!-- pdac:cite id="SB-MEALS-TEMPLATE-DUPLICATE-NAME-REJECTED" digest="sha256:7917eb781000d9d22b45ce38d5bb526e613c3a2194e514afa8d82c8ce8e4a482" -->

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

<!-- pdac:cite id="FR-MEALS-APPLY-TEMPLATE" digest="sha256:dd0cf3384f7b3f143fd0b5a0917b8c3b9e44726ec441a21ec4e40cbc13b893c7" -->

<!-- pdac:cite id="UC-MEALS-APPLY-TEMPLATE" digest="sha256:4e52ed74581ae767a0cd1e482b0fd72e47135573e27809473fd8cbc0a08e4d73" -->

<!-- pdac:cite id="BR-MEALS-TEMPLATE-SNAPSHOT" digest="sha256:51758a705775463e55f2847c9f201e4db76c34bf5794e3d7d3ac1a006b31f058" -->

<!-- pdac:cite id="BR-MEALS-ONE-PLAN-PER-WEEK" digest="sha256:33f78b7ccfa17bb1ce989cfe645563df04874c9652751ff9ff35af38b43980e4" -->

<!-- pdac:cite id="SB-MEALS-APPLY-TEMPLATE-EMPTY-WEEK" digest="sha256:cb8f93dfacaff4b5b16e00c3d5e6231e974153be59e93a50e51605bac3a26812" -->

<!-- pdac:cite id="SB-MEALS-APPLY-TEMPLATE-DELETED-RECIPE" digest="sha256:07500678227787c1cb7ddd1c9ab8c9ca03d7dc8b441fd5fdd70473b6c14e5a2b" -->

<!-- pdac:cite id="SB-MEALS-APPLY-TEMPLATE-EXISTING-WEEK" digest="sha256:77e899378f73c5d3a120c7b2a99f6f8d90b91bce07a468f4971632bc0ecdeebf" -->

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

<!-- pdac:cite id="FR-MEALS-COPY-PREVIOUS-WEEK" digest="sha256:033b3430be56c3c1f4e147ba355d3f64847d5ec0efd7b5d3ca0c2816cce697bb" -->

<!-- pdac:cite id="UC-MEALS-COPY-PREVIOUS-WEEK" digest="sha256:de1df289dda02ed7e59d0207cd8bb128c726ac8bb367dbd3d356e6c779edce55" -->

<!-- pdac:cite id="BR-MEALS-COPY-EXCLUDES-SHOPPING-LIST" digest="sha256:468c20678fbd91cbd71882086f740dc5d5295a7f81c568e0d90cadc1475423d2" -->

<!-- pdac:cite id="BR-MEALS-ONE-PLAN-PER-WEEK" digest="sha256:33f78b7ccfa17bb1ce989cfe645563df04874c9652751ff9ff35af38b43980e4" -->

<!-- pdac:cite id="SB-MEALS-COPY-PREVIOUS-WEEK" digest="sha256:41cb2816f1ac59fc130f9c885b9f2110d1a0e61bc6381dbc4ba700eca1575332" -->

<!-- pdac:cite id="SB-MEALS-COPY-NO-PREVIOUS-PLAN" digest="sha256:3233c22157a745903d9aa3409ffe8289f436622e98047779293516579a0caa0c" -->

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

<!-- pdac:cite id="FR-MEALS-SHOPPING-LIST-DERIVATION" digest="sha256:bb9dc8626edf5e2bca7ec7a3d6de5d6d06eba9856b28540ee7b09f50c9ca1030" -->

<!-- pdac:cite id="UC-MEALS-DERIVE-SHOPPING-LIST" digest="sha256:725ca067d0627c4778a7b2255e912140d1b0a676e6d7c38b5230b4bb5a5a8515" -->

<!-- pdac:cite id="BR-MEALS-SHOPPING-LIST-REQUIRES-RECIPE" digest="sha256:70c83fa014c2bb4eac1481cd105c3ca00d9f8c07e1b79d70d18907c0f4dad97e" -->

<!-- pdac:cite id="BR-MEALS-SHOPPING-LIST-CONSOLIDATION" digest="sha256:18621048d247b368f03bd298723acce625e2abe5798623b6f1de9f9b9b619873" -->

<!-- pdac:cite id="BR-MEALS-SHOPPING-LIST-NEW-EACH-TIME" digest="sha256:66bbb87ee56438054657395a8491fb49c3b1c8b4ded3b5451c8c3933b50f08e1" -->

<!-- pdac:cite id="TERM-SHOPPING-LIST-DERIVATION" digest="sha256:b33b4e40ecae229e5851c7ba275f246ff64fe44b475d5be3d79481dcb18fd802" -->

<!-- pdac:cite id="SB-MEALS-SHOPPING-LIST-FROM-PLAN" digest="sha256:5fce46ba76e46703b9a8bcabc3d66937592ea41ce29587ea458cbbdccc04d8cc" -->

<!-- pdac:cite id="SB-MEALS-SHOPPING-LIST-RE-REQUEST" digest="sha256:f01e18c9d385300d401d3b464de0cdd2311bdeefe93e0050a54916fc1f2ea2ad" -->

<!-- pdac:cite id="SB-MEALS-SHOPPING-LIST-NO-RECIPES" digest="sha256:37af68c42dee3dec4e79b5d9a0faa59b3db3224b3f34be35ec8c871a3d787a42" -->

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

<!-- pdac:cite id="FR-MEALS-AGENDA-PROJECTION" digest="sha256:14542a51f00845c388cd4e159f50674eca6d349656eacac4935fa63b064cd244" -->

<!-- pdac:cite id="UC-MEALS-SEE-MEALS-IN-AGENDA" digest="sha256:818716e263b257f7532b94022abf4464ddf4fe54e02b4e9e95de01b5d73ac668" -->

<!-- pdac:cite id="BR-MEALS-AGENDA-PROJECTION" digest="sha256:1b3f980477d116ba46ef6addf9e8a1b6aaeeefe5de1560bb9097a092c6b6aa6d" -->

<!-- pdac:cite id="BR-AGENDA-PROJECTION-READ-ONLY" digest="sha256:652b5a6581672dfbabf8e5da42d010cc34d51bbcbf5b2bc73fac9273bb631337" -->

<!-- pdac:cite id="SB-MEALS-AGENDA-SHOWS-ASSIGNED-MEAL" digest="sha256:924a7853fae063e0f6713b40a2f463060e02971c1a2e1d696a7189d5161879bc" -->

<!-- pdac:cite id="SB-MEALS-AGENDA-HIDES-EMPTY-SLOT" digest="sha256:0571354f6d33fda0a0c17ce28e02892fd8e5bf0fd4f97cff7cf952815fe7e0df" -->

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
