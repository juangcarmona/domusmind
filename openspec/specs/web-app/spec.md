# Web App Specification

<!-- pdac-scope: cited -->

## Purpose

The DomusMind web app is the primary household operational interface. Its primary surfaces are Agenda, Lists, Areas, Meal Planning, and Settings; household Chats also use the shared shell.

The web app makes the household understandable — what is happening, what needs attention, who owns what, and what should be remembered — without requiring members to coordinate across separate tools.

All surfaces share the same shell, layout grammar, visual tone, and interaction model. The product must feel like one system, not a collection of disconnected screens.

<!-- pdac:cite id="FR-WEB-APP-SHELL" digest="sha256:841b3571594a0d027a82d7f0a21afe531f98089c711f04cc9afc0a4f641c3b72" -->
<!-- pdac:cite id="FR-CHAT-MANAGE-THREADS" digest="sha256:ca38e0324cedc282b3ad87f3f482eb85bf0772b6aeb509468f9b18a4559a7418" -->
<!-- pdac:cite id="CON-PRODUCT-CURRENT-SCOPE" digest="sha256:7de869c937a3553053b5d5fce34c1e5db69426f8fef9fa4dc79e01475837ecf9" -->

<!-- pdac:cite id="UC-WEB-MOVE-BETWEEN-SURFACES" digest="sha256:e7adb55bd3adfc47814fc7d124dd249e3284480ea0dcb269fc0e6d12ccf5b3bb" -->

<!-- pdac:cite id="JRN-PRODUCT-WHAT-MATTERS-TODAY" digest="sha256:f362293fcb9ea9d94e0e798ad0d8bfbcc4fd315f5bb4e703996753fa297d53f1" -->

---

## Requirements

### Requirement: App Shell and Navigation

The web app SHALL present a persistent shell that is consistent across all surfaces.

The shell has the following zones:
- left navigation rail (desktop)
- compact page header
- main content canvas
- optional right contextual inspector

On mobile, the left rail collapses into a compact navigation pattern (drawer or bottom strip). The main content fills the screen. Contextual detail is presented as a bottom sheet or pushed detail section.

The navigation gives access to each primary surface, including Chats for household conversations and explicit @dm questions. The current surface is indicated in the navigation.

<!-- pdac:cite id="FR-WEB-APP-SHELL" digest="sha256:841b3571594a0d027a82d7f0a21afe531f98089c711f04cc9afc0a4f641c3b72" -->

<!-- pdac:cite id="UC-WEB-MOVE-BETWEEN-SURFACES" digest="sha256:e7adb55bd3adfc47814fc7d124dd249e3284480ea0dcb269fc0e6d12ccf5b3bb" -->

<!-- pdac:cite id="QR-WEB-MOBILE-SAME-PRODUCT" digest="sha256:0b38079d5c55a3bcc863ca9b9903dc959910db3df187576c9c9522546b605047" -->

<!-- pdac:cite id="SB-WEB-NAVIGATE-AGENDA-TO-LISTS" digest="sha256:303e18c78fda63501c895e2f4a6363ee25af876e7578457fe19099932cd9a6fc" -->

#### Scenario: User navigates from Agenda to Lists

- GIVEN the user is on Agenda
- WHEN the user selects Lists in the navigation
- THEN the Lists surface opens in the main content canvas
- AND the shell (navigation, header) remains visible and consistent

#### Scenario: User navigates to Meal Planning

- GIVEN the user is on another primary surface
- WHEN the user selects Meal Planning in the navigation
- THEN the Meal Planning surface opens in the main content canvas
- AND the shared shell remains visible

---

### Requirement: Surface Axis Separation

The primary surfaces SHALL maintain strict ownership boundaries. No surface owns another surface's entities, and no semantic collapse is permitted between them.

| Surface | Owns |
|---|---|
| Agenda | Time — the unified temporal read surface |
| Lists | Household execution containers — capture, flexible execution, time reference |
| Areas | Ownership — who is accountable for what |
| Meal Planning | Household meal plans, recipes, and shopping-list derivation |
| Settings | Configuration — member preferences and integrations |
| Chats | Household conversation history and explicitly requested assistant responses |

Tasks own a structured execution lifecycle. Areas own accountability. Lists own capture. Agenda reads from all sources but writes to none of them.

<!-- pdac:cite id="CON-PRODUCT-SEPARATE-AXES" digest="sha256:f0adb53673dfbfd7051791182578f9ee2f44e0a37b7699c16258854579342a13" -->

<!-- pdac:cite id="BR-AGENDA-PROJECTION-READ-ONLY" digest="sha256:a3c72a044c52c99808fa103720f30e8737d55fc1f7c9b6d7cc78e56c7539359c" -->

<!-- pdac:cite id="BR-WEB-SETTINGS-STAYS-CONFIGURATION" digest="sha256:359161ae6d2ae57e66e5f0e4d264593cac97a51760fdf377c0018076b62037f7" -->

<!-- pdac:cite id="BR-LISTS-ITEM-IS-NOT-A-TASK" digest="sha256:3d1bef6b9be254d8683460f8e32fdc7e3dfe406720927975e5ec5dbc646f1faa" -->

<!-- pdac:cite id="BC-MEAL-PLANNING" digest="sha256:bb3de4faf9d7ae3434d0db4180f005236ef81bc3ab84875aaa364e97ac3e66bd" -->

<!-- pdac:cite id="BC-CHATS" digest="sha256:8bee3e6eae0eef994be3523b72a983cd4c98e8b3aa1d365da90fd7de7d24283f" -->

#### Scenario: Agenda projects source records without taking ownership

- GIVEN a temporal List Item exists in Lists
- WHEN it appears in Agenda
- THEN it remains a List Item owned by Lists
- AND Agenda does not change the item

---

### Requirement: Agenda Default Entry State

Agenda SHALL open with a defined default state that answers the primary operational question: "what matters today?"

Default entry state:
- Scope: Household
- Mode: Day
- Date: today

Switching to Week mode preserves the current scope. Switching to a member scope preserves the current mode and date.

<!-- pdac:cite id="FR-AGENDA-DEFAULT-ENTRY" digest="sha256:1604ac8dfd3867fe9ccb2e2be3168fd1d65b9369bc06f8e4e3995d436aec4285" -->

<!-- pdac:cite id="BR-AGENDA-DEFAULT-ENTRY-STATE" digest="sha256:1aa3ff1187ac63bfd50169b370b3ef9a9254b43966d9eab80ffdd4f14b50b05b" -->

<!-- pdac:cite id="BR-AGENDA-SCOPE-SWITCH-KEEPS-CONTEXT" digest="sha256:f68c9943d8de6de62b06d780fbc59c1fae84a59edcf711676048ed6961acc740" -->

<!-- pdac:cite id="QR-PRODUCT-TODAY-AT-A-GLANCE" digest="sha256:5de2ebf4d1b4d156a4c05bd53d8bb4aa3145206cf6ad9669f9aeb2a0f85ecb37" -->

<!-- pdac:cite id="JRN-PRODUCT-WHAT-MATTERS-TODAY" digest="sha256:f362293fcb9ea9d94e0e798ad0d8bfbcc4fd315f5bb4e703996753fa297d53f1" -->

#### Scenario: User opens the app on a weekday

- GIVEN the app is freshly opened
- WHEN Agenda loads
- THEN the surface shows the Household scope, Day mode, and today's date
- AND no user configuration is required to reach this state

---

### Requirement: Agenda Scope

Agenda SHALL support two scopes: Household and Member.

**Household scope** shows the coordinated household picture — all members together, shared plans, household routines, household-level list projections, and unassigned tasks.

**Member scope** shows one specific member's temporal reality — their tasks; Plans they participate in and household Plans without participants; their routines and household routines they are responsible for; and their imported external calendar entries when within the active date window.

Switching to a member scope is triggered by clicking a member identity (avatar or name). The current date and mode are preserved when switching scope.

<!-- pdac:cite id="TERM-AGENDA-SCOPE" digest="sha256:7d64b169305cffdcfae55f7c66209f15ca27c3d00065ff7123253fc234d394e9" -->

<!-- pdac:cite id="FR-AGENDA-SCOPE-AND-MODE-SWITCHING" digest="sha256:048418adbe18b051f9c7ba0573518322e4aaea8bb073ea0f61881f13ad7bed4f" -->

<!-- pdac:cite id="UC-AGENDA-NAVIGATE" digest="sha256:27b7f4ebd25e3ca6b1236b8dc8caf2400a60011a10cdfe7873317a63228b970e" -->

<!-- pdac:cite id="UC-AGENDA-VIEW-HOUSEHOLD-AGENDA" digest="sha256:06da1c10928a2e14a7b2ace4e421c5d68123aa381fe516d913f405bbf4a88893" -->

<!-- pdac:cite id="UC-AGENDA-VIEW-MEMBER-AGENDA" digest="sha256:500f25f0376043d38af118a8e6824e8c032a27831f7aa01719c04fda98dc2c44" -->

<!-- pdac:cite id="BR-AGENDA-SCOPE-SWITCH-KEEPS-CONTEXT" digest="sha256:f68c9943d8de6de62b06d780fbc59c1fae84a59edcf711676048ed6961acc740" -->

<!-- pdac:cite id="BR-AGENDA-MEMBER-SCOPE-PLANS" digest="sha256:fb1cc45142f9871212ec0c144e92003d0cdeeb427ce92397612c38ca1f2a175a" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-OWNER-SCOPE-ONLY" digest="sha256:c1f6a0090e071a7cbca68eb2f1b200e68156542ace4fe9a8ca0223f3aa71df32" -->

<!-- pdac:cite id="SB-AGENDA-MEMBER-HOUSEHOLD-PLAN-WITHOUT-PARTICIPANTS" digest="sha256:d8d590d90b5ca72952ef0cb73e71e4fe022c50e4fe547cf21da0150a1eb8af53" -->

#### Scenario: User switches from Household to a member scope

- GIVEN the user is in Household scope, Week mode
- WHEN the user clicks a member's avatar
- THEN the surface switches to that member's scope
- AND the mode remains Week and the date window is unchanged

#### Scenario: Member scope includes unassigned household Plans only

- GIVEN the household has one Plan without participants
- AND another Plan whose only participant is a different member
- WHEN this member's Agenda is shown
- THEN the Plan without participants appears
- AND the Plan for the other member does not appear

---

### Requirement: Agenda Time Modes

Agenda SHALL support three time modes: Day, Week, and Month.

**Day mode** presents different layouts depending on scope:
- Household + Day = Board: a shared household row plus one row per member, optimized for scanning the full household at a glance
- Member + Day = Timeline: an hour-slot timeline for one member, with plans positioned by time and tasks/routines in a compact non-timed section

**Week mode** shows a 7-day window beginning on the household's configured first day of week. Plans appear as time blocks (timed) or day-lane items (untimed). Routines appear in a recurring lane. Tasks appear in a compact task lane.

**Month mode** shows a calendar grid. Each day cell shows entry counts and presence indicators. Month is a navigation and load-awareness surface, not a primary editing surface. Tapping a day cell switches to Day mode for that date.

The mode toggle is accessible without scrolling, on both desktop and mobile.

<!-- pdac:cite id="TERM-AGENDA-MODE" digest="sha256:c6b80ef7bcb2266ea8b3c50eb6c9954d594714d921a8102973bca252ca48e2ac" -->

<!-- pdac:cite id="FR-AGENDA-SCOPE-AND-MODE-SWITCHING" digest="sha256:048418adbe18b051f9c7ba0573518322e4aaea8bb073ea0f61881f13ad7bed4f" -->

<!-- pdac:cite id="FR-AGENDA-HOUSEHOLD-DAY-BOARD" digest="sha256:7dc7600fd06cbc97077be1acbf7443e02ebe635838a27b212f6c39cd1ca76dd9" -->

<!-- pdac:cite id="FR-AGENDA-MEMBER-DAY-TIMELINE" digest="sha256:dba2518b34ec42da83ed0197b47cb2db345d57c2ba76b32d86e58ebc3724f564" -->

<!-- pdac:cite id="FR-AGENDA-WEEK-VIEW" digest="sha256:dfda5d1785c059561d2d320f299c6a2d9643d9c243aaad224f7daad10ba9533c" -->

<!-- pdac:cite id="FR-AGENDA-MONTH-VIEW" digest="sha256:cde10b68fc76275f7d57907ea6c5cb9ca1ea590eea1ed8d7c55859232d262895" -->

<!-- pdac:cite id="BR-AGENDA-WEEK-STARTS-ON-HOUSEHOLD-FIRST-DAY" digest="sha256:07bbb5ad779c66a8f0349ef52a91c6631c76d0496a8e55ee752c72f68cbed000" -->

#### Scenario: User taps a date in Month mode

- GIVEN the user is in Month mode
- WHEN the user taps a day cell
- THEN the surface switches to Day mode for that date
- AND the scope is preserved

---

### Requirement: Agenda Item Priority Ordering

Agenda SHALL display items within a day or cell in a defined priority order.

Order within any day or cell:
1. Overdue items
2. Tasks due on this date
3. Projected list items due on this date (unchecked, with importance)
4. Plans (by start time ascending; untimed plans after timed)
5. Routines
6. Projected list items due on this date (unchecked, without importance)
7. Completed and checked items

Completed and checked items remain visible but de-emphasized, not removed.

<!-- pdac:cite id="BR-AGENDA-ENTRY-PRIORITY-ORDER" digest="sha256:4bb5b5f53d3f74ab929768074734785133a92a7ea30c940f17bb0e353e63f1ba" -->

<!-- pdac:cite id="FR-WEB-COUNTS-AND-COMPRESSED-COMPLETION" digest="sha256:a222ff4110e33f64b652398e78b61a103ec013957a8844561515563763bb1df8" -->

#### Scenario: Agenda items follow the defined priority order

- GIVEN a day contains overdue items, tasks, important List Items, timed and untimed Plans, Routines, other List Items, and completed items
- WHEN the Agenda displays that day
- THEN it shows them in that priority order
- AND completed items remain visible but de-emphasized

---

### Requirement: Agenda Selection and Inspection

Selecting any item in Agenda SHALL open its detail without navigating away from the surface.

- Desktop: the item opens in the right inspector panel. The surrounding content remains visible.
- Mobile: the item opens in a bottom sheet.

Deselecting closes the inspector or bottom sheet without any navigation.

Agenda does not navigate to a separate page for item inspection or creation.

<!-- pdac:cite id="FR-AGENDA-INSPECT-ENTRY" digest="sha256:88cfacfed507d3405a1a85b00cbfe2c0ec803dbfe319068a94d21d1131a946c7" -->

<!-- pdac:cite id="UC-AGENDA-INSPECT-ENTRY" digest="sha256:abdaa6bbeb066db2f71154961df7e4f405a409fd37fe290987d679f710f4d1b6" -->

<!-- pdac:cite id="FR-AGENDA-CREATE-IN-CONTEXT" digest="sha256:b56239d0d179c502a7db9f9bb8de51df57c077abfaa1b724312f23700fe84dee" -->

<!-- pdac:cite id="FR-WEB-CONTEXTUAL-DETAIL-PATTERNS" digest="sha256:5cbddd4d738a319aba075ce6c69082423619f352188faf139f892270cbd4c425" -->

#### Scenario: User selects a plan on desktop

- GIVEN the user is viewing Agenda in Day mode on desktop
- WHEN the user clicks a plan entry
- THEN the plan detail opens in the right inspector panel
- AND the day board or timeline remains visible behind the inspector

#### Scenario: User deselects an item

- GIVEN an item is selected and the inspector is open
- WHEN the user dismisses the inspector
- THEN the inspector closes
- AND the Agenda view is unchanged

---

### Requirement: Agenda Projected List Items

List items with temporal fields (due date, reminder, or repeat) SHALL project into Agenda as a distinct entry type.

Agenda does not own list items. Agenda projects them. The write owner is always Lists.

Projected list items are visually distinguishable from tasks and plans in all Agenda views. Each projected item carries a list-origin cue (list name or list icon). This cue must be visible even in collapsed row states.

Projected list items:
- are not editable from Agenda
- open in a read-only inspector with a single `Open in Lists` action
- appear in both Household and Member scope (list items are household-scoped in V1)
- in Household + Day: appear in the shared household row, not in individual member rows
- in Member scope: appear in the non-timed section alongside tasks and routines

<!-- pdac:cite id="BR-AGENDA-PROJECTION-READ-ONLY" digest="sha256:a3c72a044c52c99808fa103720f30e8737d55fc1f7c9b6d7cc78e56c7539359c" -->

<!-- pdac:cite id="BR-LISTS-AGENDA-PROJECTION" digest="sha256:043eef067ee7db97dd23b7dc3dc2e7c9b7c52b4c9f5aefc6e3d2066abd179350" -->

<!-- pdac:cite id="BR-AGENDA-LIST-ITEMS-HOUSEHOLD-ROW" digest="sha256:a1346bfc71bcc50424d839ee9a9779cedf52284197c98dc35ea8edf265b2a16a" -->

<!-- pdac:cite id="FR-LISTS-OPEN-IN-LISTS-FROM-AGENDA" digest="sha256:e00b832b3463117967eef9ea29223bba3e49587a1243aeaa56140b49577a0d6f" -->

<!-- pdac:cite id="FR-AGENDA-ENTRY-TYPES-DISTINGUISHABLE" digest="sha256:153e467b9bce79cc6090ee57380de4fb8ddeb255069088d495aff8cb73080757" -->

<!-- pdac:cite id="TERM-PROJECTED-LIST-ITEM" digest="sha256:1d7dee71c8e9ef330968975a9a7b48e15b547f1b823286d462ab6b5ed059078f" -->

<!-- pdac:cite id="UC-LISTS-SEE-ITEMS-IN-AGENDA" digest="sha256:19f437a80c20b0934963cfd836845c4f7d45d3caa9e31b73984860dfd63cc900" -->

<!-- pdac:cite id="SB-AGENDA-MEMBER-LIST-ITEMS-READ-ONLY" digest="sha256:8231ad3d04776f037c212779854491d81a971d0eca1fd320e617a373d65c93df" -->

#### Scenario: User selects a projected list item in Agenda

- GIVEN a list item has a due date and appears in today's Agenda
- WHEN the user selects it
- THEN a read-only inspector opens showing the item's title, due date, list origin, and checked state
- AND a single `Open in Lists` action is available
- AND no edit controls are present

#### Scenario: User attempts to edit a projected list item in Agenda

- GIVEN a projected list item is selected in the Agenda inspector
- WHEN the user uses the `Open in Lists` action
- THEN the user is navigated to the item in the Lists surface
- AND the item is editable there

---

### Requirement: Agenda External Calendar Entries

Imported external calendar entries SHALL appear in Agenda in Member scope only.

External entries (Outlook) appear read-only. They carry a visible source cue (e.g., `Outlook`). Selecting an external entry opens read-only detail. The detail may offer an `Open in Outlook` action.

External entries are never converted to editable household plans.

External entries are omitted from Agenda when they fall outside the selected date window, even if cached locally.

<!-- pdac:cite id="FR-AGENDA-EXTERNAL-ENTRY-PRESENTATION" digest="sha256:ba4ab90479187bd041f38b0cd6513153f5eaea9d0560f49cf127e125696370b0" -->

<!-- pdac:cite id="TERM-EXTERNAL-CALENDAR-ENTRY" digest="sha256:a544452b97f1df5874ba287953f6e9ad857c23c3dd1902fc5f3d22bca412ca6b" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-OWNER-SCOPE-ONLY" digest="sha256:c1f6a0090e071a7cbca68eb2f1b200e68156542ace4fe9a8ca0223f3aa71df32" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY" digest="sha256:0a46d8c63e8a5ca97a1757e7ed3e0528c2c10779a63621783793131b2bf55522" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRY-VISIBILITY" digest="sha256:949e939555ba5883283a4fde1554e44e12a29571db306d54222f01114f3a782d" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS" digest="sha256:f538589cc3d3dd54bfc8539b59e38f13f87b49547cf25e651e8ce2906810583e" -->

<!-- pdac:cite id="SB-AGENDA-MEMBER-INCLUDES-IMPORTED" digest="sha256:2e6e2071df4c87715883ec209e11c2f221c09046119a4bc5b393f3a1644c8370" -->

<!-- pdac:cite id="SB-AGENDA-HOUSEHOLD-EXCLUDES-EXTERNAL" digest="sha256:7783dc44b8b6ab700b8fb8b48ee763fe3e9b904ff78b9b37b7169da7ebad11a5" -->

#### Scenario: External entry appears in Member scope

- GIVEN a member has an active Outlook connection with imported entries
- WHEN the user views that member's Day timeline
- THEN imported Outlook entries appear with a visible source cue
- AND they are not presented with edit affordances

#### Scenario: External entry is absent from Household scope

- GIVEN a member has imported Outlook entries
- WHEN the user views Agenda in Household scope
- THEN no imported Outlook entries are shown

---

### Requirement: Agenda Plan–List Reference Cue

When a plan has an associated list, Agenda SHALL surface a compact reference cue on the plan entry.

The cue shows the list name and the current unchecked item count. Selecting the cue navigates to the full list in the Lists surface.

Agenda does not expand list items inline as plan content. Agenda does not convert list items to tasks. The cue and the temporal projection of list items are two independent mechanisms.

<!-- pdac:cite id="FR-LISTS-PLAN-LIST-CUE" digest="sha256:81912e77e4934b36cf350a287e610201ff95a73d7e1f8ae8ccb3e773e748e352" -->

<!-- pdac:cite id="BR-LISTS-PLAN-LINK-DOES-NOT-PROJECT" digest="sha256:2bdc7fe6d5bc3727361258ac8d5fd7e22f16d3056002e48b7eaa663a2ff1c150" -->

<!-- pdac:cite id="BR-LISTS-CONTEXT-LINKS-INFORMATIONAL" digest="sha256:c5d604a88b3d0a26c3005ee116c6145fd451a9fd4e2f1b56f4e48f0c5fdd7f16" -->

<!-- pdac:cite id="BR-LISTS-ITEM-IS-NOT-A-TASK" digest="sha256:3d1bef6b9be254d8683460f8e32fdc7e3dfe406720927975e5ec5dbc646f1faa" -->

#### Scenario: Plan cue opens its linked List

- GIVEN a Plan has an associated List with unchecked items
- WHEN the Plan appears in Agenda
- THEN its cue shows the List name and unchecked item count
- AND selecting the cue opens that List in Lists
- AND the List Items are not expanded as Plan content

---

### Requirement: Lists Surface Structure

The Lists surface SHALL provide a persistent split layout: a list switcher and an active list working area.

The list switcher shows all household lists with their name, unchecked count, and optional area or plan context cue. Switching lists preserves shell context. The active list is visually clear in the switcher.

The active list shows unchecked items first, completed items collapsed and accessible. A quick add bar is always visible. Item selection opens the inspector (desktop) or bottom sheet (mobile).

Inspector sections for capabilities not set on an item are collapsed by default. Empty sections do not show placeholder fields, but are accessible when the user needs to add that capability.

On desktop: switcher pane + active list pane + optional inspector (three-column).
On mobile: active list fills the screen; list switcher is accessible via drawer or sheet.

<!-- pdac:cite id="FR-LISTS-SURFACE-LAYOUT" digest="sha256:d7b14e7e9cb7fef6eacf885efb2916a5d26d67d61d790653afac224b49548034" -->

<!-- pdac:cite id="FR-LISTS-BROWSE-LISTS" digest="sha256:5b9e3226a32138d68f98657ee01c2530bcbf39f097e41b55e7a18c8b6f8efc26" -->

<!-- pdac:cite id="FR-LISTS-OPEN-LIST" digest="sha256:7b22d78453a3e7e8c72f95e9391ab646c664da7e5c9afe7b39a3948efa9e6bfd" -->

<!-- pdac:cite id="UC-LISTS-BROWSE-LISTS" digest="sha256:3edc8eb98802d84347ef8ae624a29dc866c5dcf0939db22a0067288b42f87f7b" -->

<!-- pdac:cite id="UC-LISTS-OPEN-LIST" digest="sha256:8e1a65110f288baa5f7fb827197590037cb9e7255c9cb1d61ac0c85d656cfa10" -->

<!-- pdac:cite id="FR-WEB-COUNTS-AND-COMPRESSED-COMPLETION" digest="sha256:a222ff4110e33f64b652398e78b61a103ec013957a8844561515563763bb1df8" -->

<!-- pdac:cite id="SB-LISTS-OPEN-UNCHECKED-FIRST" digest="sha256:7bd0a97f8e052ca3c996fde32163ba3062bf5079a9189747164b7cfabae33def" -->

#### Scenario: User switches between lists

- GIVEN the user is on Lists with a list selected
- WHEN the user taps another list in the switcher
- THEN the new list opens in the active list pane
- AND the switcher remains visible on desktop

---

### Requirement: List Item Capability Model

List items SHALL support a progressive capability model. Not every item requires every capability.

**Base (always present):**
- title
- checked state (toggle)

**Optional capabilities:**

| Capability | Fields | Effect |
|---|---|---|
| Importance | starred flag | item is visually prioritized in the list |
| Temporal | due date, reminder, repeat | item becomes eligible for Agenda projection |

Temporal fields are each independently optional. Due date alone, reminder alone, or repeat alone is sufficient for Agenda projection. No temporal field requires another as a prerequisite.

A plain item with a title only is fully valid. A starred item with all three temporal fields is also valid.

Items do not carry an assignee, a status system beyond checked/unchecked, comments, attachments, or steps.

<!-- pdac:cite id="TERM-LIST-ITEM" digest="sha256:c6112cdc3c6f6573d978a595cec94145f47269189357e3e50257d39667ca56ee" -->

<!-- pdac:cite id="TERM-ITEM-IMPORTANCE" digest="sha256:e3bbf50c0811e62b653decd8f04508de22754a2f836fd939ffa7cd4fe1ac0de7" -->

<!-- pdac:cite id="TERM-ITEM-TEMPORAL-FIELDS" digest="sha256:15498c7a4246e94ae298569f73d3f6cfa6efdbc366631baae6fe03a5fc8b8578" -->

<!-- pdac:cite id="FR-LISTS-ITEM-IMPORTANCE" digest="sha256:ce302425272a44f0fe0719f0589804bcc94e294a07d4e4b486ad7b19f90ad394" -->

<!-- pdac:cite id="FR-LISTS-SET-ITEM-TIMING" digest="sha256:b3cbffce65b3e20f553f712ea2729544a94c6e4a9bc888619b60e7a58eb41c14" -->

<!-- pdac:cite id="BR-LISTS-IMPORTANCE-BINARY" digest="sha256:b779b624287684ff2c848bbf1152cd268f2d4d8e0849e3fe391010817143c4d2" -->

<!-- pdac:cite id="BR-LISTS-TEMPORAL-FIELDS-INDEPENDENT" digest="sha256:88fa4dda19ba3d6cd4e9e92b1d43d08fb0d96717d76e1428747adf70df3e55bb" -->

<!-- pdac:cite id="CON-LISTS-ITEM-CAPABILITY-BOUNDARY" digest="sha256:f0b4e3bfe38b1249e7261996f8b21420e271b1c5ee0d349e7a42264f23bdfe9e" -->

#### Scenario: List Item works without optional capabilities

- WHEN a member creates a List Item with only a title
- THEN the item is valid and can be checked or unchecked

#### Scenario: Temporal capabilities are independent

- GIVEN a List Item has only a due date, only a reminder, or only a repeat rule
- WHEN the item is saved
- THEN the selected temporal capability is valid without requiring another temporal field

---

### Requirement: Lists Temporal Projection to Agenda

List items with any temporal field SHALL project into the Agenda surface as a distinct entry type.

This is a non-negotiable cross-surface capability. The write model stays divided: Lists owns the item. Agenda projects it. No entity crosses a context boundary.

Clearing all temporal fields from an item removes it from Agenda projection immediately.

When a repeat rule is set alongside a due date, the due date acts as the anchor for the current recurrence.

<!-- pdac:cite id="FR-LISTS-AGENDA-PROJECTION" digest="sha256:b94162c9eb42af755596bd3448c69f727b83d6dd2a707631568d90ea10453d50" -->

<!-- pdac:cite id="BR-LISTS-AGENDA-PROJECTION" digest="sha256:043eef067ee7db97dd23b7dc3dc2e7c9b7c52b4c9f5aefc6e3d2066abd179350" -->

<!-- pdac:cite id="BR-LISTS-TEMPORAL-FIELDS-INDEPENDENT" digest="sha256:88fa4dda19ba3d6cd4e9e92b1d43d08fb0d96717d76e1428747adf70df3e55bb" -->

<!-- pdac:cite id="CON-PRODUCT-SEPARATE-AXES" digest="sha256:f0adb53673dfbfd7051791182578f9ee2f44e0a37b7699c16258854579342a13" -->

<!-- pdac:cite id="FR-LISTS-CLEAR-ITEM-TIMING" digest="sha256:befbf601bbf68f6cda4e54ee4af1a8642208b44c15c1692005c7da958f2b3a50" -->

<!-- pdac:cite id="SB-LISTS-CLEARED-ITEM-LEAVES-AGENDA" digest="sha256:435869f319b3f053ea27df3bc2eb5b8d6f9dd5a0bf910fbe2201071d409a806b" -->

#### Scenario: List Item projects while it has temporal information

- GIVEN a List Item has at least one temporal field
- WHEN the Agenda is requested
- THEN the item appears as a distinct List Item entry

#### Scenario: Clearing temporal information removes the projection

- GIVEN a List Item appears in Agenda
- WHEN all of its temporal fields are cleared
- THEN it no longer appears in Agenda

---

### Requirement: List Creation

Creating a list SHALL require only a name. No other metadata is required at creation time.

A list may be created from the Lists surface, from the detail of a plan, or from the context of an Area. Linking to a plan or Area is always optional and may be done after creation.

Item capture SHALL not require a modal. Quick add must support sequential entry without interruption. Focus returns for repeated capture.

<!-- pdac:cite id="FR-LISTS-CREATE-LIST" digest="sha256:25c9cc803d305300a243539bd99da341cbda24a290f61af4894dbd02f91b0e6d" -->

<!-- pdac:cite id="UC-LISTS-CREATE-LIST" digest="sha256:82df9acf755674420aae2f412b71663853188987c855c12931e78c38a4db8f4d" -->

<!-- pdac:cite id="BR-LISTS-LIST-NAME-REQUIRED" digest="sha256:95049208f08bebfb977ee6a6bad5f34e2ceef9fbd21bff7aadfc78cdf7fd6d29" -->

<!-- pdac:cite id="BR-LISTS-CONTEXT-LINKS-INFORMATIONAL" digest="sha256:c5d604a88b3d0a26c3005ee116c6145fd451a9fd4e2f1b56f4e48f0c5fdd7f16" -->

<!-- pdac:cite id="FR-LISTS-ADD-ITEM" digest="sha256:31ca6047288c0e531133f611eda3ba2b8c142fc5d991cc0015103929474a2ff3" -->

<!-- pdac:cite id="QR-LISTS-FRICTIONLESS-CAPTURE" digest="sha256:c49275adaa16e46f1d1613af96372c572b5562e38912ce8db7693be665d6e4ec" -->

<!-- pdac:cite id="SB-LISTS-CREATE-NAME-ONLY" digest="sha256:67ca544ab5591dbd2d620615cf09d68da36f4a8da202529356e9f571083ec2ed" -->

<!-- pdac:cite id="SB-LISTS-SEQUENTIAL-CAPTURE" digest="sha256:f71db1393adbc83b54644275fa8d6f276a304a59ac74ea138432bd12ca5bfa44" -->

#### Scenario: User creates a list with a name only

- GIVEN the user is on the Lists surface
- WHEN the user creates a new list with a name and no other fields
- THEN the list is created and becomes available in the switcher
- AND the user can immediately begin adding items

---

### Requirement: List Lifecycle

A list follows a defined lifecycle: created → active use → rested → archived.

Items are consumable. Lists are designed for reuse across uses.

Completed items are de-emphasized behind a collapse toggle (`Completed (N)`) but remain accessible. They are not removed.

Archived lists are no longer in active use but are retained.

<!-- pdac:cite id="JRN-LISTS-REUSE-A-LIST" digest="sha256:b6207a0738ce655272ca0575c950bc6a5b5fc830ca0e9dc116365b2e9eff73dc" -->

<!-- pdac:cite id="TERM-ARCHIVED-LIST" digest="sha256:3283e585a3a3a503e6af9b70f17f75c99dcc79f5e3a0b7ad58cbaa29bf5ed860" -->

<!-- pdac:cite id="TERM-CHECKED-ITEM" digest="sha256:92b2028e2e875bd9b994751aa25de16c9928b4204734ca2cfe7a77e1ff1bd94b" -->

<!-- pdac:cite id="FR-LISTS-ARCHIVE-LIST" digest="sha256:050b27be17e6b043e91241e56988e8099bc62e18ff07fe964842e197bfe11477" -->

<!-- pdac:cite id="UC-LISTS-ARCHIVE-LIST" digest="sha256:1b1904e7a6583bce2e708950e1b87060cdf5c7ac0ae7432036226211825a0cde" -->

<!-- pdac:cite id="FR-WEB-COUNTS-AND-COMPRESSED-COMPLETION" digest="sha256:a222ff4110e33f64b652398e78b61a103ec013957a8844561515563763bb1df8" -->

#### Scenario: Completed List Items remain accessible

- GIVEN a List has completed items
- WHEN the completed section is collapsed
- THEN the items remain available behind the Completed count

#### Scenario: Archived List remains retained and read-only

- GIVEN a List is archived
- WHEN a member views the List
- THEN the List remains retained
- AND its items cannot be changed until the List is restored

---

### Requirement: Areas Surface

The Areas surface SHALL display household ownership structure as a dense, scannable list ordered to make gaps visible first.

Default ordering:
1. Unowned Areas (no primary owner)
2. Partially assigned Areas (primary owner present, no secondary owner)
3. Fully assigned Areas
4. Archived Areas (only shown when the archived filter is applied)

Each Area row shows: Area name, primary owner or a gap indicator if unowned, and support members if any.

Selecting an Area opens the inspector (desktop) or bottom sheet/pushed section (mobile). There is no separate full-detail page for normal area inspection on desktop.

The inspector shows: Area identity (name and color cue), owner with inline change affordance, support members with add/remove affordance, related work counts (open tasks, plans, routines, linked lists), and explicit creation entry points (New task, New routine, New plan — each pre-filled with the current Area context).

Clicking an owner or supporter name in the inspector navigates to that person's Agenda context.

There is no separate full-detail page for Areas. A direct URL to an area detail page redirects to the Areas surface with that area's selection restored.

<!-- pdac:cite id="FR-AREAS-OWNERSHIP-VISIBILITY" digest="sha256:acf5743c30edd22cc0badaaf5bc4d6998fb46c2e71dc2dc84452b7557a4b14d8" -->

<!-- pdac:cite id="UC-AREAS-REVIEW-OWNERSHIP" digest="sha256:bf641773cb1723b92c2eb3e20bfebb013ee1be7d3722c06df94cc97e56cc69a0" -->

<!-- pdac:cite id="TERM-OWNERSHIP-GAP" digest="sha256:6315a8195c46d7e4cae41ff65fcaf5a4a777c8d964661988b850cdc6102c4434" -->

<!-- pdac:cite id="FR-AREAS-LINKED-WORK" digest="sha256:b867edbec0a1c2482f5d7e1bd2d88ca1a3a65107d38bb0e15884ae1228db21e6" -->

<!-- pdac:cite id="FR-AREAS-CREATE-WORK-FROM-AREA" digest="sha256:fadb351fdb99292d3b4d7c5f42c31cef12b77d1ef1a8c0248b8831914c661273" -->

<!-- pdac:cite id="FR-AREAS-AREA-COLOR" digest="sha256:ee51f587835984eb7de4d74edffaa0626f0caeaf06e3001e8949419e76b50df8" -->

<!-- pdac:cite id="QR-AREAS-GAPS-AT-A-GLANCE" digest="sha256:f1a55dc43455210a0ed6dd85f11f80d67e68d984acd0d84e65e8f392fce5829f" -->

<!-- pdac:cite id="FR-WEB-CONTEXTUAL-DETAIL-PATTERNS" digest="sha256:5cbddd4d738a319aba075ce6c69082423619f352188faf139f892270cbd4c425" -->

<!-- pdac:cite id="SB-AREAS-UNOWNED-FIRST" digest="sha256:47b6a5b5b96fe924545ba8d7fba1408ed5eaff98d3404ddb129ff5d09dd29b41" -->

<!-- pdac:cite id="SB-AREAS-GAP-INDICATOR" digest="sha256:3ddabbf8e8bd488cc243d27a98468f3b9bf74559e1f0092ad6d2b8848b4e6a6f" -->

#### Scenario: User views Areas with an unowned Area

- GIVEN a household has one Area with no primary owner
- AND other Areas have owners assigned
- WHEN the user opens the Areas surface
- THEN the unowned Area appears at the top of the list
- AND a gap indicator is shown where the owner would appear

#### Scenario: User selects an Area on desktop

- GIVEN the user is on the Areas surface
- WHEN the user clicks an Area row
- THEN the Area inspector opens on the right
- AND the Areas list remains visible on the left

---

### Requirement: Settings Surface

Settings SHALL be the low-frequency configuration surface. It does not duplicate operational functionality present in other surfaces.

Settings has three primary sections:
- **Profile** — member identity, account details, calendar connections
- **Household** — household-level preferences, including assistant provider configuration for household managers
- **Preferences** — personal defaults

Settings opens to the Profile section by default.

Outlook calendar connections are managed in Profile. Each member manages their own connections.

Settings SHALL NOT handle: editing of native household plans, browsing of imported entries as a calendar-like surface, provider write-back, or administrative control of another member's calendar connections in phase 1.

<!-- pdac:cite id="FR-ASSISTANT-CONFIGURE-PROVIDERS" digest="sha256:f6d31fa396855bedd343863cb7e1485c49ad0cde33f074bf702852432a9c1f7c" -->
<!-- pdac:cite id="UC-ASSISTANT-CONFIGURE-PROVIDERS" digest="sha256:4368dc7f90d60dfc87ce6d3313e2865ec897854ece9e4cbc1f064356c53a4820" -->
<!-- pdac:cite id="FR-WEB-SETTINGS-SURFACE" digest="sha256:ded16890d8ee378ace6a8f2e67f815920b569ec20eef0904e177614a0eecb44d" -->

<!-- pdac:cite id="UC-WEB-MANAGE-SETTINGS" digest="sha256:7e168c902564d7bc6f54495ca764cc3cb6e7976fb327cba962a3176fad2ce07a" -->

<!-- pdac:cite id="BR-WEB-SETTINGS-STAYS-CONFIGURATION" digest="sha256:359161ae6d2ae57e66e5f0e4d264593cac97a51760fdf377c0018076b62037f7" -->

<!-- pdac:cite id="BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS" digest="sha256:a3c89a729bbc1b61d0b8104ceacce71dc29a6e915ab2a90834c869b72c0a9da8" -->

<!-- pdac:cite id="SB-WEB-SETTINGS-OPENS-ON-PROFILE" digest="sha256:d1d2c1d3321d62776397d526c6fb7c76cdd39fd92371b07a34d4359ffef62aec" -->

#### Scenario: User opens Settings

- GIVEN the user navigates to Settings
- THEN the Profile section is displayed by default
- AND the calendar connections section is visible without deep navigation

---

### Requirement: Outlook Calendar Connection Management

A member SHALL be able to connect, configure, sync, and disconnect their Outlook calendar from Settings / Profile.

**Connect:** launches a provider auth flow. On return, the new connection appears and is configurable immediately (calendar selection, sync horizon).

**Sync now:** triggers manual sync for a single connection. Progress is shown inline. Success or failure is surfaced inline without leaving Settings.

**Sync calendars:** available when multiple connections exist. Dispatches sync for all connections simultaneously. Shows aggregate progress at section level without hiding per-connection failure state.

**Disconnect:** requires an explicit confirm step. Confirmation must make the outcome clear: imported Outlook entries will disappear from Agenda; native household plans are unaffected.

Each connection row shows: provider label, account email, included calendar count, sync horizon, last sync time, and current status.

<!-- pdac:cite id="FR-WEB-CALENDAR-CONNECTIONS-SECTION" digest="sha256:ff32c9662d1c32f4f33d0046caa1ca98db89157e0c27e2d6bd763622bd665f0a" -->

<!-- pdac:cite id="FR-WEB-CONFIRM-DISCONNECT" digest="sha256:e938c89cca90929913990eaba3bb556b2e6f9a12b53eca14efadb0763dc382c3" -->

<!-- pdac:cite id="FR-CALENDAR-SHOW-CONNECTION-STATUS" digest="sha256:ebea42ca547efc3df187cfb4e96c667c5f0421336e08dec667a693dbcb2b7403" -->

<!-- pdac:cite id="FR-CALENDAR-SYNC-ALL-MY-CALENDARS" digest="sha256:00d6307f6e04781deae485202279fadb2c0840b08267d96f71e1d8c40f3fc62b" -->

<!-- pdac:cite id="UC-CALENDAR-CONNECT-OUTLOOK" digest="sha256:2b5979801bddd1324f3c23b0958b000afe550e98e17a11ce792c2ea54235c034" -->

<!-- pdac:cite id="UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR" digest="sha256:eaf278bf6b3a77c0dc9ff934b444ab82b5aa23dac9c882f3c16ed698f1f410ff" -->

<!-- pdac:cite id="UC-CALENDAR-SYNC-EXTERNAL-CALENDAR" digest="sha256:20397f50049cb36f540d4e65b7989c0f38be858535349e86daf4790319b37fe4" -->

<!-- pdac:cite id="UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR" digest="sha256:3a3e5fa7533474185fbda7d556bb496ff71feb16302a5e219ff587eba5a386bc" -->

<!-- pdac:cite id="BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS" digest="sha256:a3c89a729bbc1b61d0b8104ceacce71dc29a6e915ab2a90834c869b72c0a9da8" -->

<!-- pdac:cite id="SB-WEB-DISCONNECT-CONFIRMATION" digest="sha256:dcb36d5105f87047c7c19261610758db1fa39e201836df71e895f525029d3f1e" -->

#### Scenario: User disconnects an Outlook connection

- GIVEN a member has an active Outlook connection
- WHEN the member selects Disconnect
- THEN a confirmation step is shown
- AND the confirmation explains that imported Outlook entries will be removed from Agenda
- AND upon confirmation the connection is removed and imported entries are no longer shown in Agenda

---

### Requirement: Inspector and Modal Usage

The web app SHALL use consistent patterns for contextual detail across all surfaces.

**Inspector (right panel on desktop / bottom sheet on mobile):**
- used when inspecting one selected item
- used for lightweight editing
- surrounding context remains visible
- default pattern for item detail in all surfaces

**Modal:**
- used for destructive actions requiring confirmation
- used for short interruptive flows that must be completed or cancelled before continuing
- not the default for item inspection

**Full-page navigation:**
- used when moving to a distinct work context
- used when a flow requires depth or sustained focus

Contextual creation actions (add plan, add task, add list item) stay close to the current surface. Creation does not require navigating to a separate page.

<!-- pdac:cite id="FR-WEB-CONTEXTUAL-DETAIL-PATTERNS" digest="sha256:5cbddd4d738a319aba075ce6c69082423619f352188faf139f892270cbd4c425" -->

<!-- pdac:cite id="FR-WEB-LOCAL-CAPTURE" digest="sha256:e5ac1c8084079d226d134ec0f16ac26e57e15e716b4925c8070db85b000ddc12" -->

<!-- pdac:cite id="QR-PRODUCT-CAPTURE-EASIER-THAN-REMEMBERING" digest="sha256:8481194f2c3fb06e4c6d24ab85b116e0f6f08200e1ece492593e2eb554a4e8ac" -->

#### Scenario: Selecting an item uses contextual detail

- GIVEN a member selects an item on desktop
- WHEN its details open
- THEN they appear in the right inspector while the surrounding surface remains visible
- AND on mobile the same details appear in a bottom sheet or pushed section

#### Scenario: Destructive action asks for confirmation

- WHEN a member starts a destructive action
- THEN a modal asks for confirmation before the action proceeds

---

### Requirement: Mobile Behavior

The mobile web app SHALL present the same product logic as desktop in a collapsed form.

- Left navigation rail collapses to a compact navigation pattern
- Page headers compress; date navigation on Agenda becomes swipe-based on the canvas
- Contextual detail is presented as a bottom sheet
- Creation flows use a FAB where the desktop uses a compact header action
- Content priority is highest — chrome is compressed

Mobile must feel like the same product at a smaller scale, not a different product.

<!-- pdac:cite id="QR-WEB-MOBILE-SAME-PRODUCT" digest="sha256:0b38079d5c55a3bcc863ca9b9903dc959910db3df187576c9c9522546b605047" -->

<!-- pdac:cite id="QR-AGENDA-MOBILE-PARITY" digest="sha256:7096274135e4808dfaec9a30523645cebc0e11bfad842b073078f08052633187" -->

<!-- pdac:cite id="QR-WEB-CONTENT-OVER-CHROME" digest="sha256:2c69639ca2b2671411675f4b9c68fef916c646f7ab3c3d45b6d5514629aca541" -->

<!-- pdac:cite id="FR-WEB-LOCAL-CAPTURE" digest="sha256:e5ac1c8084079d226d134ec0f16ac26e57e15e716b4925c8070db85b000ddc12" -->

#### Scenario: Mobile keeps the same product behavior

- GIVEN a member uses the app on a mobile device
- WHEN they navigate, inspect an item, and create an item
- THEN navigation uses the compact pattern, contextual detail uses a bottom sheet or pushed section, and creation uses the corresponding FAB
- AND the same product behavior remains available as on desktop

---

## Notes

### Meal Planning surface

Meal Planning is a current primary surface in the shared shell (decided: Q-0051). The web-app requirement includes its navigation entry.

### Tasks surface

Tasks are referenced throughout Agenda as a primary data source (tasks project into Agenda). However, no Tasks surface spec was available in the source material. Task surface behavior is unspecced. This spec does not cover a Tasks surface.

### Agenda catch-up sync timing

The agenda spec states "stale connections may trigger a lightweight catch-up sync when Agenda opens." The conditions defining a "stale" connection and the exact trigger point are not fully specified.

### Member scope availability

Member scope in Agenda requires a household with multiple members. Single-member households or the configured scope defaults for them are not specified.

---

## Source References

- `docs/design/surface-system.md`
- `docs/product/model/` (formerly docs/00_product/experience.md)
- `docs/design/surfaces/agenda.md`
- `docs/design/surfaces/areas.md`
- `docs/design/surfaces/lists.md`
- `docs/design/surfaces/meal-planning.md`
- `docs/design/surfaces/settings.md`
