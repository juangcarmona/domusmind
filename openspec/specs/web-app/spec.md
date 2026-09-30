# Web App Specification

<!-- pdac-scope: cited -->

## Purpose

The DomusMind web app is the primary household operational interface. It presents one shared product shell containing multiple operational surfaces: Agenda, Lists, Areas, and Settings, with Meal Planning as an additional surface (see Notes).

The web app makes the household understandable — what is happening, what needs attention, who owns what, and what should be remembered — without requiring members to coordinate across separate tools.

All surfaces share the same shell, layout grammar, visual tone, and interaction model. The product must feel like one system, not a collection of disconnected screens.

<!-- pdac:cite id="FR-WEB-APP-SHELL" digest="sha256:31085ec615794bf88151dee377bbfb2f5021f7a11dfbb4fbeb53ab6028cefadf" -->

<!-- pdac:cite id="UC-WEB-MOVE-BETWEEN-SURFACES" digest="sha256:644e9cb653039be248abe495399211a5757cec36a5e4296cdd494098705e05f1" -->

<!-- pdac:cite id="JRN-PRODUCT-WHAT-MATTERS-TODAY" digest="sha256:d3b34ef815ae22176d06373d245649d511cfd136c57ef2d7859598fdaf22b37f" -->

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

The navigation gives access to each primary surface. The current surface is indicated in the navigation.

<!-- pdac:cite id="FR-WEB-APP-SHELL" digest="sha256:31085ec615794bf88151dee377bbfb2f5021f7a11dfbb4fbeb53ab6028cefadf" -->

<!-- pdac:cite id="UC-WEB-MOVE-BETWEEN-SURFACES" digest="sha256:644e9cb653039be248abe495399211a5757cec36a5e4296cdd494098705e05f1" -->

<!-- pdac:cite id="QR-WEB-MOBILE-SAME-PRODUCT" digest="sha256:c318bd0291b8badc6c509bd6dbdc22cf069487aa03b5d15714210312ac261e5d" -->

<!-- pdac:cite id="SB-WEB-NAVIGATE-AGENDA-TO-LISTS" digest="sha256:4877188ce2b000393bad98518b314be0b3d6f97c8df41bae43e6e4db0021fe34" -->

#### Scenario: User navigates from Agenda to Lists

- GIVEN the user is on Agenda
- WHEN the user selects Lists in the navigation
- THEN the Lists surface opens in the main content canvas
- AND the shell (navigation, header) remains visible and consistent

---

### Requirement: Surface Axis Separation

The four primary surfaces SHALL maintain strict ownership boundaries. No surface owns another surface's entities, and no semantic collapse is permitted between them.

| Surface | Owns |
|---|---|
| Agenda | Time — the unified temporal read surface |
| Lists | Household execution containers — capture, flexible execution, time reference |
| Areas | Ownership — who is accountable for what |
| Settings | Configuration — member preferences and integrations |

Tasks own a structured execution lifecycle. Areas own accountability. Lists own capture. Agenda reads from all sources but writes to none of them.

<!-- pdac:cite id="CON-PRODUCT-SEPARATE-AXES" digest="sha256:740fa2cc41d90ec6afb099c5f97fa44e9092af8d83cd68291f807262387a44b0" -->

<!-- pdac:cite id="BR-AGENDA-PROJECTION-READ-ONLY" digest="sha256:652b5a6581672dfbabf8e5da42d010cc34d51bbcbf5b2bc73fac9273bb631337" -->

<!-- pdac:cite id="BR-WEB-SETTINGS-STAYS-CONFIGURATION" digest="sha256:d39e22599e6932f15dec9d9328e42cd39b17254968c267480466de3dcfcafcdf" -->

<!-- pdac:cite id="BR-LISTS-ITEM-IS-NOT-A-TASK" digest="sha256:cb2e2207883d40101d5b4ef7b33d79dc2c444d62b2b66c80c5e887b5e938b061" -->

---

### Requirement: Agenda Default Entry State

Agenda SHALL open with a defined default state that answers the primary operational question: "what matters today?"

Default entry state:
- Scope: Household
- Mode: Day
- Date: today

Switching to Week mode preserves the current scope. Switching to a member scope preserves the current mode and date.

<!-- pdac:cite id="FR-AGENDA-DEFAULT-ENTRY" digest="sha256:3c9ce83b74e0fd104a25d173790954085d2b95317ca31b828692b301e8f9d637" -->

<!-- pdac:cite id="BR-AGENDA-DEFAULT-ENTRY-STATE" digest="sha256:0791c2a66b0379b6ba16ca0b57ca61f989fdc63e7733ba3a2ff578cbac1c1e20" -->

<!-- pdac:cite id="BR-AGENDA-SCOPE-SWITCH-KEEPS-CONTEXT" digest="sha256:e02f53667c4612164319e6c05f980c64ff758014bac4d99ab0730dd716c2e5ad" -->

<!-- pdac:cite id="QR-PRODUCT-TODAY-AT-A-GLANCE" digest="sha256:659932b0b836c008118516caa8647d05dc133c1e12c31114f6df3a49527420fa" -->

<!-- pdac:cite id="JRN-PRODUCT-WHAT-MATTERS-TODAY" digest="sha256:d3b34ef815ae22176d06373d245649d511cfd136c57ef2d7859598fdaf22b37f" -->

#### Scenario: User opens the app on a weekday

- GIVEN the app is freshly opened
- WHEN Agenda loads
- THEN the surface shows the Household scope, Day mode, and today's date
- AND no user configuration is required to reach this state

---

### Requirement: Agenda Scope

Agenda SHALL support two scopes: Household and Member.

**Household scope** shows the coordinated household picture — all members together, shared plans, household routines, household-level list projections, and unassigned tasks.

**Member scope** shows one specific member's temporal reality — their tasks, plans, and routines; household plans they participate in; household routines they are responsible for; and their imported external calendar entries when within the active date window.

Switching to a member scope is triggered by clicking a member identity (avatar or name). The current date and mode are preserved when switching scope.

<!-- pdac:cite id="TERM-AGENDA-SCOPE" digest="sha256:7253db64d83dac0a1595a1f2c629b59e27d467f8677afcfa330f9ebf3cdaafc9" -->

<!-- pdac:cite id="FR-AGENDA-SCOPE-AND-MODE-SWITCHING" digest="sha256:eab963a157c87e65ff18629754491c6e96fcd5dc75d0810d4c37bd4d503d1ac4" -->

<!-- pdac:cite id="UC-AGENDA-NAVIGATE" digest="sha256:532ae62a754386ab4c7166dc2db3856434a5538c9c39242114b854af029b8657" -->

<!-- pdac:cite id="UC-AGENDA-VIEW-HOUSEHOLD-AGENDA" digest="sha256:3aa05500b8b3f5f223447760dc3b2d73fded5269ea3925ae117d4c0d93be4318" -->

<!-- pdac:cite id="UC-AGENDA-VIEW-MEMBER-AGENDA" digest="sha256:2c870adbc231b6b1b0c661ab8062530786d0ef7d40c29ca48d6099cd46500fbc" -->

<!-- pdac:cite id="BR-AGENDA-SCOPE-SWITCH-KEEPS-CONTEXT" digest="sha256:e02f53667c4612164319e6c05f980c64ff758014bac4d99ab0730dd716c2e5ad" -->

<!-- pdac:cite id="BR-AGENDA-MEMBER-SCOPE-PLANS" digest="sha256:687c533e32506e30c54751e420d32bc5d6ca0683d04cb534977881f133c0e45b" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-OWNER-SCOPE-ONLY" digest="sha256:b1dcb8fae830badbc584fe2a2e8b3ec117053470d9123391b2fa0d69f6bc9e69" -->

<!-- pdac-drift ids="BR-AGENDA-MEMBER-SCOPE-PLANS, TERM-AGENDA-SCOPE" summary="Spec shows only household plans the member participates in; the model (Q-0041) also shows household plans without participants in every member scope" -->

#### Scenario: User switches from Household to a member scope

- GIVEN the user is in Household scope, Week mode
- WHEN the user clicks a member's avatar
- THEN the surface switches to that member's scope
- AND the mode remains Week and the date window is unchanged

---

### Requirement: Agenda Time Modes

Agenda SHALL support three time modes: Day, Week, and Month.

**Day mode** presents different layouts depending on scope:
- Household + Day = Board: a shared household row plus one row per member, optimized for scanning the full household at a glance
- Member + Day = Timeline: an hour-slot timeline for one member, with plans positioned by time and tasks/routines in a compact non-timed section

**Week mode** shows a 7-day window beginning on the household's configured first day of week. Plans appear as time blocks (timed) or day-lane items (untimed). Routines appear in a recurring lane. Tasks appear in a compact task lane.

**Month mode** shows a calendar grid. Each day cell shows entry counts and presence indicators. Month is a navigation and load-awareness surface, not a primary editing surface. Tapping a day cell switches to Day mode for that date.

The mode toggle is accessible without scrolling, on both desktop and mobile.

<!-- pdac:cite id="TERM-AGENDA-MODE" digest="sha256:4962f471f0c6157f565f83459b8e453d86120d9cae4dc19bb268fd100b51ee7f" -->

<!-- pdac:cite id="FR-AGENDA-SCOPE-AND-MODE-SWITCHING" digest="sha256:eab963a157c87e65ff18629754491c6e96fcd5dc75d0810d4c37bd4d503d1ac4" -->

<!-- pdac:cite id="FR-AGENDA-HOUSEHOLD-DAY-BOARD" digest="sha256:aad8ac860b5c014e2498e3fe01c0212b99a00a47add18b47f0b95e678e1f172f" -->

<!-- pdac:cite id="FR-AGENDA-MEMBER-DAY-TIMELINE" digest="sha256:0b3a0835f85644202dcaf98da711dfe478f50290928e502f91e214b6d920e281" -->

<!-- pdac:cite id="FR-AGENDA-WEEK-VIEW" digest="sha256:1a914a6129591596301447031ecea6b1c4abc658087a9b8594143738bd32c9c6" -->

<!-- pdac:cite id="FR-AGENDA-MONTH-VIEW" digest="sha256:5fe78463a472773e56734ec2d0cbe27136fc0962720ae9b10de24334f6816b5c" -->

<!-- pdac:cite id="BR-AGENDA-WEEK-STARTS-ON-HOUSEHOLD-FIRST-DAY" digest="sha256:f921026b204a43ad8570d08eccf2e0e0a692c1c858b28e148b0219feea0982f0" -->

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

<!-- pdac:cite id="BR-AGENDA-ENTRY-PRIORITY-ORDER" digest="sha256:99c2693fee545eedd4a3feb29da8d89e78edb05d8c68bba4afffc5073078cac6" -->

<!-- pdac:cite id="FR-WEB-COUNTS-AND-COMPRESSED-COMPLETION" digest="sha256:75b1699d75bf1b4494c8a72fc588b0fbf1813618f8d5f608c61e14d2dda3a3fd" -->

---

### Requirement: Agenda Selection and Inspection

Selecting any item in Agenda SHALL open its detail without navigating away from the surface.

- Desktop: the item opens in the right inspector panel. The surrounding content remains visible.
- Mobile: the item opens in a bottom sheet.

Deselecting closes the inspector or bottom sheet without any navigation.

Agenda does not navigate to a separate page for item inspection or creation.

<!-- pdac:cite id="FR-AGENDA-INSPECT-ENTRY" digest="sha256:938a73fbc4f9919aaad58faa9f0ca6e1171804afd79b8eedeca4caf80c954f99" -->

<!-- pdac:cite id="UC-AGENDA-INSPECT-ENTRY" digest="sha256:fbb3eb9a54bd0ebdbad83db2c1025a28fac4c3866d7107035614c0fc2fd05b6b" -->

<!-- pdac:cite id="FR-AGENDA-CREATE-IN-CONTEXT" digest="sha256:761445cdb4f2654e5f0a8e539b58a1f2eac384bf5fa779a161e789b7278e483d" -->

<!-- pdac:cite id="FR-WEB-CONTEXTUAL-DETAIL-PATTERNS" digest="sha256:7d0eea19745ff772c67fad4b9248424244669368f6c113c10884eb9b45cc48ef" -->

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

<!-- pdac:cite id="BR-AGENDA-PROJECTION-READ-ONLY" digest="sha256:652b5a6581672dfbabf8e5da42d010cc34d51bbcbf5b2bc73fac9273bb631337" -->

<!-- pdac:cite id="BR-LISTS-AGENDA-PROJECTION" digest="sha256:07f2a39c4da3143f7ba1dc285acb840d8a121020a2c68adf87783c23b1e92af9" -->

<!-- pdac:cite id="BR-AGENDA-LIST-ITEMS-HOUSEHOLD-ROW" digest="sha256:9abd5f15ec082a5ca1bc132d65e0efd3e9a2948830531dfbd514e38020b7b56e" -->

<!-- pdac:cite id="FR-LISTS-OPEN-IN-LISTS-FROM-AGENDA" digest="sha256:8eadf3f69912d9fb73e1526b622556cf379395868b04ab4084ef8d08d43829c3" -->

<!-- pdac:cite id="FR-AGENDA-ENTRY-TYPES-DISTINGUISHABLE" digest="sha256:a44de0d8278f8bf9ee5f21c1620d9b9787284d49a0c97f6ce37839310e52fd2a" -->

<!-- pdac:cite id="TERM-PROJECTED-LIST-ITEM" digest="sha256:c8ace0e5be8d059740bfd3edd63eb5f8b101c2687ffcccbc7f918c9e5a864fd7" -->

<!-- pdac:cite id="UC-LISTS-SEE-ITEMS-IN-AGENDA" digest="sha256:ee7a292721be6b4b2df1d3de830f3918089d760b22e49883e906b03b657c7e80" -->

<!-- pdac:cite id="SB-AGENDA-MEMBER-LIST-ITEMS-READ-ONLY" digest="sha256:aca5be88f2c84c717d71bed6e7b84dc226ebd267d0bfc2a2e7f5c05cf78f54c6" -->

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

<!-- pdac:cite id="FR-AGENDA-EXTERNAL-ENTRY-PRESENTATION" digest="sha256:866645ac5e44faca42776b0ad94a13c249431c370fbe3d8139e2f6136fa23503" -->

<!-- pdac:cite id="TERM-EXTERNAL-CALENDAR-ENTRY" digest="sha256:9ef123050d8fc32bd7755be77ace1bab03fe0009c4583a0bdced3aff9aba6272" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-OWNER-SCOPE-ONLY" digest="sha256:b1dcb8fae830badbc584fe2a2e8b3ec117053470d9123391b2fa0d69f6bc9e69" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY" digest="sha256:a3d8d4afcfc23492b745fa37ac5b103ae76a7bc1d214456aeddbe2f57473ceea" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRY-VISIBILITY" digest="sha256:c0b22c61a193c5202c7877f77b3b86a5f0251afabebafa9367623f63d3a4cf42" -->

<!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS" digest="sha256:2b2ea1de6c0f9a84d96f94d20087467fa7a198f197da80a85748fb145816fe03" -->

<!-- pdac:cite id="SB-AGENDA-MEMBER-INCLUDES-IMPORTED" digest="sha256:b36d86ff2957c14ce9476a747fe05e0a8cb48b3c83c913066bf01400c78789cd" -->

<!-- pdac:cite id="SB-AGENDA-HOUSEHOLD-EXCLUDES-EXTERNAL" digest="sha256:314f199dbf9e633b3422db2e43359e1936f4c0fd61fd8c4ba4a18e76c2d3b2ea" -->

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

<!-- pdac:cite id="FR-LISTS-PLAN-LIST-CUE" digest="sha256:010654cd32b56f0f85d189f4d5f6953f1de930e44cd0037cd5722c0a461f49cc" -->

<!-- pdac:cite id="BR-LISTS-PLAN-LINK-DOES-NOT-PROJECT" digest="sha256:4c9723864e62308e6b5a0068872b52fcafd549eebd8837c69d9eef568c10ea42" -->

<!-- pdac:cite id="BR-LISTS-CONTEXT-LINKS-INFORMATIONAL" digest="sha256:75037c1fcced9208b8cd1c668a98f37b7d742dea8c8f447a482e15bf7e3c76f6" -->

<!-- pdac:cite id="BR-LISTS-ITEM-IS-NOT-A-TASK" digest="sha256:cb2e2207883d40101d5b4ef7b33d79dc2c444d62b2b66c80c5e887b5e938b061" -->

---

### Requirement: Lists Surface Structure

The Lists surface SHALL provide a persistent split layout: a list switcher and an active list working area.

The list switcher shows all household lists with their name, unchecked count, and optional area or plan context cue. Switching lists preserves shell context. The active list is visually clear in the switcher.

The active list shows unchecked items first, completed items collapsed and accessible. A quick add bar is always visible. Item selection opens the inspector (desktop) or bottom sheet (mobile).

Inspector sections for capabilities not set on an item are collapsed by default. Empty sections do not show placeholder fields, but are accessible when the user needs to add that capability.

On desktop: switcher pane + active list pane + optional inspector (three-column).
On mobile: active list fills the screen; list switcher is accessible via drawer or sheet.

<!-- pdac:cite id="FR-LISTS-SURFACE-LAYOUT" digest="sha256:dacab172d564533ea1cf10e7c4f5a056f7b3a59bca0b2d479e473213f5be44f6" -->

<!-- pdac:cite id="FR-LISTS-BROWSE-LISTS" digest="sha256:dc7bf80f44b5e99a69a0c2449b160a2d5bca9746fb817cffc1e7e28d04c24228" -->

<!-- pdac:cite id="FR-LISTS-OPEN-LIST" digest="sha256:2adf8e6854602e5d6029b6cb889800220fed57d862de2bbfa07136611deb8053" -->

<!-- pdac:cite id="UC-LISTS-BROWSE-LISTS" digest="sha256:119aa4f67efbc350613fa0f77e48134406a41ffc0e0c53b729e73f3e57d15508" -->

<!-- pdac:cite id="UC-LISTS-OPEN-LIST" digest="sha256:245d5390e63e0281cb1cb5c29ef1a9fa25ab39fc2c4cf54b51240b6d5f571dfe" -->

<!-- pdac:cite id="FR-WEB-COUNTS-AND-COMPRESSED-COMPLETION" digest="sha256:75b1699d75bf1b4494c8a72fc588b0fbf1813618f8d5f608c61e14d2dda3a3fd" -->

<!-- pdac:cite id="SB-LISTS-OPEN-UNCHECKED-FIRST" digest="sha256:6c1e8e0624f0b3efc8eb12a39bc8297df35fd52943cee0474d2bbaacd422f779" -->

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

<!-- pdac:cite id="TERM-LIST-ITEM" digest="sha256:904520339922c3589e43d6ea243a61938b6f5304cb1e30cc8c8b7bffff92aa2e" -->

<!-- pdac:cite id="TERM-ITEM-IMPORTANCE" digest="sha256:10b6e633466cc85d1aee7ab7f7d74e519279a86581c1f715e2cb4956e1437a10" -->

<!-- pdac:cite id="TERM-ITEM-TEMPORAL-FIELDS" digest="sha256:138ea408865568c2d484dcdef52b9395e05a4f103ce7fc9462e9deead72e484d" -->

<!-- pdac:cite id="FR-LISTS-ITEM-IMPORTANCE" digest="sha256:6f2ad263ba0ae54f8a682c9578ec553165f84da6bc547ed31a224ab021d09834" -->

<!-- pdac:cite id="FR-LISTS-SET-ITEM-TIMING" digest="sha256:d7ebe6861d147b98d3f0b647ac8ab6123a20cea7ff52e516210fe306593e9a93" -->

<!-- pdac:cite id="BR-LISTS-IMPORTANCE-BINARY" digest="sha256:8f9276f3e7b87e7f2ad2c59437da11a5ec0481cdaa7f97e520c6f8a91a8fe6c3" -->

<!-- pdac:cite id="BR-LISTS-TEMPORAL-FIELDS-INDEPENDENT" digest="sha256:40002c49ac40b5b30be14b9d66152d74c7c6db734817ff70ba4976c4849778fe" -->

<!-- pdac:cite id="CON-LISTS-ITEM-CAPABILITY-BOUNDARY" digest="sha256:057cef9585d6cb8ab3cba66f55cdb76637abe9b9bb9342a997232b58dba53e15" -->

---

### Requirement: Lists Temporal Projection to Agenda

List items with any temporal field SHALL project into the Agenda surface as a distinct entry type.

This is a non-negotiable cross-surface capability. The write model stays divided: Lists owns the item. Agenda projects it. No entity crosses a context boundary.

Clearing all temporal fields from an item removes it from Agenda projection immediately.

When a repeat rule is set alongside a due date, the due date acts as the anchor for the current recurrence.

<!-- pdac:cite id="FR-LISTS-AGENDA-PROJECTION" digest="sha256:97074fff7f6f05b914ce4619df4ea615d6d3b8198c29f7a9c85b9cd0a95ffe0b" -->

<!-- pdac:cite id="BR-LISTS-AGENDA-PROJECTION" digest="sha256:07f2a39c4da3143f7ba1dc285acb840d8a121020a2c68adf87783c23b1e92af9" -->

<!-- pdac:cite id="BR-LISTS-TEMPORAL-FIELDS-INDEPENDENT" digest="sha256:40002c49ac40b5b30be14b9d66152d74c7c6db734817ff70ba4976c4849778fe" -->

<!-- pdac:cite id="CON-PRODUCT-SEPARATE-AXES" digest="sha256:740fa2cc41d90ec6afb099c5f97fa44e9092af8d83cd68291f807262387a44b0" -->

<!-- pdac:cite id="FR-LISTS-CLEAR-ITEM-TIMING" digest="sha256:dac218145bbf6f152b37ca13e0c90761046bf9141f5cec589137e62d80d0ebda" -->

<!-- pdac:cite id="SB-LISTS-CLEARED-ITEM-LEAVES-AGENDA" digest="sha256:9f66ef5bca4eecb07050a78ed725f41873bd84e1422ad2ae6d4cd783dc29c0ff" -->

---

### Requirement: List Creation

Creating a list SHALL require only a name. No other metadata is required at creation time.

A list may be created from the Lists surface, from the detail of a plan, or from the context of an Area. Linking to a plan or Area is always optional and may be done after creation.

Item capture SHALL not require a modal. Quick add must support sequential entry without interruption. Focus returns for repeated capture.

<!-- pdac:cite id="FR-LISTS-CREATE-LIST" digest="sha256:40e9d54e5314ad2e475d7fce3b2a85fa548e4b4928390b1a4cbb5c3889b68c8c" -->

<!-- pdac:cite id="UC-LISTS-CREATE-LIST" digest="sha256:375c978ff484265047fac5dc6656ddafb9b13474b61d356ce67388071a75ae5e" -->

<!-- pdac:cite id="BR-LISTS-LIST-NAME-REQUIRED" digest="sha256:e8b8f128499fe155698642fddd3002344b0ec947bece7644a30f3d8f54a7b752" -->

<!-- pdac:cite id="BR-LISTS-CONTEXT-LINKS-INFORMATIONAL" digest="sha256:75037c1fcced9208b8cd1c668a98f37b7d742dea8c8f447a482e15bf7e3c76f6" -->

<!-- pdac:cite id="FR-LISTS-ADD-ITEM" digest="sha256:e8aa749e760b1deb27d59fd5f3e997342ccfd52438cffb1f96e79479275a3ce5" -->

<!-- pdac:cite id="QR-LISTS-FRICTIONLESS-CAPTURE" digest="sha256:54f0abaea8ee6e4683dca671179356b0dc68aeaeab8353b550bfefc66212698c" -->

<!-- pdac:cite id="SB-LISTS-CREATE-NAME-ONLY" digest="sha256:debfbf3036eda107b3f1f195d25d5023ed3fa3554f5f9da2bb9f76da13caa8ab" -->

<!-- pdac:cite id="SB-LISTS-SEQUENTIAL-CAPTURE" digest="sha256:084f626099f7e5566efe9ecbb65bda5c6a31d63d03ea1c9fed0045de260fe92c" -->

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

<!-- pdac:cite id="JRN-LISTS-REUSE-A-LIST" digest="sha256:e59043d8f633b8e68bbfff954803e5f97c46167d42a022ac5b11b7b875603d10" -->

<!-- pdac:cite id="TERM-ARCHIVED-LIST" digest="sha256:71bd5c8a326250a271fca746551c3e38d203d4318eb29bc9ee02dc6b1487b85d" -->

<!-- pdac:cite id="TERM-CHECKED-ITEM" digest="sha256:5556c31585a82994143d5a49b60e7d1c8d9780aede68115e79e8fbaee6499246" -->

<!-- pdac:cite id="FR-LISTS-ARCHIVE-LIST" digest="sha256:7204dcb449c16d9bf95653de6d5dab3140f60ad07df5dcc3a31ae877b6e27e57" -->

<!-- pdac:cite id="UC-LISTS-ARCHIVE-LIST" digest="sha256:2395fb97234a2de067a6f26ada8345758c18ccf3ef081d97c9d20a9b04fa6afe" -->

<!-- pdac:cite id="FR-WEB-COUNTS-AND-COMPRESSED-COMPLETION" digest="sha256:75b1699d75bf1b4494c8a72fc588b0fbf1813618f8d5f608c61e14d2dda3a3fd" -->

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

<!-- pdac:cite id="FR-AREAS-OWNERSHIP-VISIBILITY" digest="sha256:87da19a0776edd63c305dbcc30a7e51ec98fe949c762e217df6f17d284c745b4" -->

<!-- pdac:cite id="UC-AREAS-REVIEW-OWNERSHIP" digest="sha256:fe5c941fe5760bc287bf7e51f2e59e21e0215853396e8f5760ada0ae99d35acd" -->

<!-- pdac:cite id="TERM-OWNERSHIP-GAP" digest="sha256:e403011bc8751c3eb99fa08906c90b4172f9e0f8e7eb8d4e49761a40ac843531" -->

<!-- pdac:cite id="FR-AREAS-LINKED-WORK" digest="sha256:848cee830481740fcd7579c934393814a7aebadcd3af422ff9e190ae01a55a7b" -->

<!-- pdac:cite id="FR-AREAS-CREATE-WORK-FROM-AREA" digest="sha256:8067db8e87e5852fb22b4d5af029609a8ed981602c384ea5777205f1f1e270bf" -->

<!-- pdac:cite id="FR-AREAS-AREA-COLOR" digest="sha256:72d580077f39e5fc9bf6a006d109b15b03b7aa06ace7fafde3c4ce3883aa94ca" -->

<!-- pdac:cite id="QR-AREAS-GAPS-AT-A-GLANCE" digest="sha256:f1a55dc43455210a0ed6dd85f11f80d67e68d984acd0d84e65e8f392fce5829f" -->

<!-- pdac:cite id="FR-WEB-CONTEXTUAL-DETAIL-PATTERNS" digest="sha256:7d0eea19745ff772c67fad4b9248424244669368f6c113c10884eb9b45cc48ef" -->

<!-- pdac:cite id="SB-AREAS-UNOWNED-FIRST" digest="sha256:9c21be8ac399f55a749f9b93986be95559b578f293922b375be1768afff351be" -->

<!-- pdac:cite id="SB-AREAS-GAP-INDICATOR" digest="sha256:797e4f31f1fca1a42437dcc02b8d5cf61763383fe4d34fb9e1f5a2e5e692a9e4" -->

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

Phase 1 Settings has three primary sections:
- **Profile** — member identity, account details, calendar connections
- **Household** — household-level preferences
- **Preferences** — personal defaults

Settings opens to the Profile section by default.

Outlook calendar connections are managed in Profile. Each member manages their own connections.

Settings SHALL NOT handle: editing of native household plans, browsing of imported entries as a calendar-like surface, provider write-back, or administrative control of another member's calendar connections in phase 1.

<!-- pdac:cite id="FR-WEB-SETTINGS-SURFACE" digest="sha256:d6bc7cc0dd3fe7d5338d0bb8e836467cde256b6feea2fce0c789204c1d22fc79" -->

<!-- pdac:cite id="UC-WEB-MANAGE-SETTINGS" digest="sha256:9dbb90084c9135c6603878370913e153c97318b79ae85bbb5106eab623292734" -->

<!-- pdac:cite id="BR-WEB-SETTINGS-STAYS-CONFIGURATION" digest="sha256:d39e22599e6932f15dec9d9328e42cd39b17254968c267480466de3dcfcafcdf" -->

<!-- pdac:cite id="BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS" digest="sha256:92737f241a94bb8c8663837766c0035e4f8c64b12cc5409bef30289fb4dab848" -->

<!-- pdac:cite id="SB-WEB-SETTINGS-OPENS-ON-PROFILE" digest="sha256:d499111e5c7daf319d71a4a482457abc74d8a19a56cea38f9683c4ca8dca9a91" -->

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

<!-- pdac:cite id="FR-WEB-CALENDAR-CONNECTIONS-SECTION" digest="sha256:1fb3cc1de52ffa3afa91f266978c9221423a19f961597d080d9e2aae2aeafeea" -->

<!-- pdac:cite id="FR-WEB-CONFIRM-DISCONNECT" digest="sha256:cb0f0d3acd4db0feede258fdaeaacdf07fce5b3742780859627354cae6002205" -->

<!-- pdac:cite id="FR-CALENDAR-SHOW-CONNECTION-STATUS" digest="sha256:90493f6dec63621093e6aaffdcfec1673e691583f48157237781964253fb8ca4" -->

<!-- pdac:cite id="FR-CALENDAR-SYNC-ALL-MY-CALENDARS" digest="sha256:584a41b95599d2173f6c7e358a2c04873010db89917b1dec55c10fbb0eac240f" -->

<!-- pdac:cite id="UC-CALENDAR-CONNECT-OUTLOOK" digest="sha256:f8c4aff4fa188eacc0a4014f41ab98b85f94446cca0daab97e36a8a67c4f1a5f" -->

<!-- pdac:cite id="UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR" digest="sha256:2cfd9b4b0f71c7e62dcd49d1fda91cdbee19a21a49023c4c8eab24d6dbb9c3a0" -->

<!-- pdac:cite id="UC-CALENDAR-SYNC-EXTERNAL-CALENDAR" digest="sha256:bbecb1046bb1b9c284c6f69ed6a02879f41210f364542e9538d7bb63cd99045e" -->

<!-- pdac:cite id="UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR" digest="sha256:2e005d84005460d3bb48da21ccbd69a57c65964dc32c774bae93df627c8bb21b" -->

<!-- pdac:cite id="BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS" digest="sha256:92737f241a94bb8c8663837766c0035e4f8c64b12cc5409bef30289fb4dab848" -->

<!-- pdac:cite id="SB-WEB-DISCONNECT-CONFIRMATION" digest="sha256:0828a6c9065b8fe44e71d0e60f8076426aba49c9af1f84bc319e558663649b0f" -->

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

<!-- pdac:cite id="FR-WEB-CONTEXTUAL-DETAIL-PATTERNS" digest="sha256:7d0eea19745ff772c67fad4b9248424244669368f6c113c10884eb9b45cc48ef" -->

<!-- pdac:cite id="FR-WEB-LOCAL-CAPTURE" digest="sha256:a43d6d379267bfaaa54145e61b18a92e07c6f02dda63ef57e4d4ebfd82a33b47" -->

<!-- pdac:cite id="QR-PRODUCT-CAPTURE-EASIER-THAN-REMEMBERING" digest="sha256:a4c482905ac1c2e5c6fac9a61e6d1449d28a4d682cc4bd43a452d80a79944b8b" -->

---

### Requirement: Mobile Behavior

The mobile web app SHALL present the same product logic as desktop in a collapsed form.

- Left navigation rail collapses to a compact navigation pattern
- Page headers compress; date navigation on Agenda becomes swipe-based on the canvas
- Contextual detail is presented as a bottom sheet
- Creation flows use a FAB where the desktop uses a compact header action
- Content priority is highest — chrome is compressed

Mobile must feel like the same product at a smaller scale, not a different product.

<!-- pdac:cite id="QR-WEB-MOBILE-SAME-PRODUCT" digest="sha256:c318bd0291b8badc6c509bd6dbdc22cf069487aa03b5d15714210312ac261e5d" -->

<!-- pdac:cite id="QR-AGENDA-MOBILE-PARITY" digest="sha256:995a78a704051071eda6e66154994c53bc729905ddec7dd9473da8592b779fe7" -->

<!-- pdac:cite id="QR-WEB-CONTENT-OVER-CHROME" digest="sha256:d77b33426ebc338ec4fdfd1e9a075c38ca7867793bb6aabdb9b62c71a7d54ff4" -->

<!-- pdac:cite id="FR-WEB-LOCAL-CAPTURE" digest="sha256:a43d6d379267bfaaa54145e61b18a92e07c6f02dda63ef57e4d4ebfd82a33b47" -->

---

## Notes

### Meal Planning surface

The Meal Planning surface spec (`docs/design/surfaces/meal-planning.md`) is fully specified and describes a complete surface. However, the domain context document (`docs/_legacy/04_contexts/meal-planning.md`) marks Meal Planning as a **V2 bounded context**, not part of the V1 core. Whether Meal Planning is included as a V1 navigation entry in the web app is not definitively resolved in the source material. It is excluded from the navigation requirements above until its V1 inclusion is confirmed.

<!-- pdac-drift ids="FR-WEB-APP-SHELL, UC-WEB-MOVE-BETWEEN-SURFACES" summary="Spec excludes Meal Planning from navigation pending V1 status and names four primary surfaces; the model (Q-0001, Q-0051) makes Meal Planning current product and a navigable primary surface" -->

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
