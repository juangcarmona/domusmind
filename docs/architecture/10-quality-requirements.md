---
title: Quality Requirements
arc42-section: "10"
description: How the architecture realises the product's quality requirements, with evidence and known gaps.
---

# Quality Requirements

The product model owns every quality requirement and its measurement under [`docs/product/model/requirements/quality/`](../product/model/requirements/quality/). This section only states which architectural mechanism carries each one and what evidence or gap the implementation shows. The highest-priority goals are summarised in [01 Introduction and goals](01-introduction-and-goals.md).

## Quality Requirements Overview

Every quality requirement in the model is a usability or consistency requirement of the household experience. Eight of the nine have an architectural realisation. `QR-WEB-CONTENT-OVER-CHROME` (interface density) is realised only by visual design and is owned by [`docs/design/`](../design/); the architecture adds no mechanism for it.

### Today at a glance

Once a household has completed onboarding, the web client's authenticated router sends the root and every unknown route to the household Agenda, so no configuration stands between sign-in and today's picture. That picture is a query-time projection: one query handler reads plans, tasks, routines, temporal list items and areas through the shared `DbContext` and returns a single read model, so the client does not assemble the household view from several calls.

<!-- pdac:cite id="QR-PRODUCT-TODAY-AT-A-GLANCE" digest="sha256:659932b0b836c008118516caa8647d05dc133c1e12c31114f6df3a49527420fa" -->

Evidence: [`AuthedRoutes.tsx`](../../src/web/app/src/app/AuthedRoutes.tsx), [`GetWeeklyGridQueryHandler.cs`](../../src/backend/DomusMind.Application/Features/Family/GetWeeklyGrid/GetWeeklyGridQueryHandler.cs), [`GetEnrichedTimelineQueryHandler.cs`](../../src/backend/DomusMind.Application/Features/Family/GetEnrichedTimeline/GetEnrichedTimelineQueryHandler.cs).

### Mobile is the same product

There is one client for every screen size: the responsive React web app. Mobile is a layout mode chosen at runtime by a single breakpoint hook, with shared components for the mobile forms of detail (bottom sheet) and navigation, so phone and desktop call the same API, the same store and the same feature pages. No separate mobile client or mobile-specific API exists.

<!-- pdac:cite id="QR-WEB-MOBILE-SAME-PRODUCT" digest="sha256:c318bd0291b8badc6c509bd6dbdc22cf069487aa03b5d15714210312ac261e5d" -->

The Agenda follows the same mechanism: its page, scope selector and modes are one implementation rendered in two layouts.

<!-- pdac:cite id="QR-AGENDA-MOBILE-PARITY" digest="sha256:995a78a704051071eda6e66154994c53bc729905ddec7dd9473da8592b779fe7" -->

Evidence: [`useIsMobile.ts`](../../src/web/app/src/hooks/useIsMobile.ts), [`BottomSheetDetail.tsx`](../../src/web/app/src/components/BottomSheetDetail.tsx), [`AppShell.tsx`](../../src/web/app/src/components/AppShell.tsx). Gap: parity is not verified automatically; the web app has two unit test files and no end-to-end or viewport tests.

### Capture is easier than remembering

Capture commands carry only the fields their area requires as mandatory parameters and take every other detail as optional, nullable data; later enrichment goes through separate commands (for example setting an item's timing or importance). Creation is a single command per capture, issued from the surface's inline quick-add component, so the client needs no extra round trip or navigation.

<!-- pdac:cite id="QR-PRODUCT-CAPTURE-EASIER-THAN-REMEMBERING" digest="sha256:a4c482905ac1c2e5c6fac9a61e6d1449d28a4d682cc4bd43a452d80a79944b8b" -->

For lists, adding an item is one command whose only required value is the name, and the client applies later item edits (toggle, rename, timing, importance, reorder) optimistically in its store before the API confirms them.

<!-- pdac:cite id="QR-LISTS-FRICTIONLESS-CAPTURE" digest="sha256:54f0abaea8ee6e4683dca671179356b0dc68aeaeab8353b550bfefc66212698c" -->

Evidence: [`AddItemToListCommand.cs`](../../src/backend/DomusMind.Application/Features/Lists/AddItemToList/AddItemToListCommand.cs), [`CreateTaskCommand.cs`](../../src/backend/DomusMind.Application/Features/Tasks/CreateTask/CreateTaskCommand.cs), [`ScheduleEventCommand.cs`](../../src/backend/DomusMind.Application/Features/Calendar/ScheduleEvent/ScheduleEventCommand.cs), [`QuickAddBar.tsx`](../../src/web/app/src/components/QuickAddBar.tsx), [`listsSlice.ts`](../../src/web/app/src/store/listsSlice.ts).

### List changes reach the whole household

Not realised. The client fetches list state when a list is opened and holds it in its store; the API has no push channel (no SignalR, WebSocket or server-sent events) and the client does not poll, so another member's change appears only after a reload or navigation. The persisted event log could feed a push mechanism, but nothing consumes it (see [11 Risks and technical debt](11-risks-and-technical-debt.md)).

<!-- pdac:cite id="QR-LISTS-REAL-TIME-SHARING" digest="sha256:bc063af2dad51048a90d0f8d29ee7466d0d74e5ad96462900c5a476996d89455" -->

Evidence: [`ListsPage.tsx`](../../src/web/app/src/features/lists/pages/ListsPage.tsx), [`package.json`](../../src/web/app/package.json) (no real-time dependency), [`Program.cs`](../../src/backend/DomusMind.Api/Program.cs) (no hub mapped).

### Low-effort meal planning

Reuse and derivation are single server-side commands rather than client-side sequences: copying the previous week and applying a weekly template each fill a whole plan in one request, and requesting a shopping list consolidates the plan's ingredients and creates the list in one request.

<!-- pdac:cite id="QR-MEALS-LOW-EFFORT-PLANNING" digest="sha256:4791efc078efb88dccaddd5efad2ef16055fe6c61c8bd618f7c331bf3d0ec3da" -->

Evidence: [`CopyMealPlanFromPreviousWeek`](../../src/backend/DomusMind.Application/Features/MealPlanning/CopyMealPlanFromPreviousWeek/), [`ApplyWeeklyTemplate`](../../src/backend/DomusMind.Application/Features/MealPlanning/ApplyWeeklyTemplate/), [`RequestShoppingListCommandHandler.cs`](../../src/backend/DomusMind.Application/Features/MealPlanning/RequestShoppingList/RequestShoppingListCommandHandler.cs). Gap: the shopping-list command changes two aggregates in two modules (see [11 Risks and technical debt](11-risks-and-technical-debt.md)).

### Ownership gaps at a glance

The Areas surface reads one projection query that returns every area with its primary and secondary owners, so the client can mark unowned areas without per-area requests; assigning or transferring an owner is one command on the responsibility domain aggregate.

<!-- pdac:cite id="QR-AREAS-GAPS-AT-A-GLANCE" digest="sha256:f1a55dc43455210a0ed6dd85f11f80d67e68d984acd0d84e65e8f392fce5829f" -->

Evidence: [`GetHouseholdAreasQueryHandler.cs`](../../src/backend/DomusMind.Application/Features/Responsibilities/GetHouseholdAreas/GetHouseholdAreasQueryHandler.cs), [`AssignPrimaryOwner`](../../src/backend/DomusMind.Application/Features/Responsibilities/AssignPrimaryOwner/).

## Quality Scenarios

These scenarios evaluate the architecture against the requirements above; their pass criteria stay in the product model.

| Scenario | Stimulus | Architectural response | Current result |
| --- | --- | --- | --- |
| Open today | A signed-in member opens the app root | Router redirects to the Agenda; one projection query returns the household picture | Met |
| Phone-width use | A member uses the Agenda, Lists or Areas below the 768 px breakpoint | Same pages render the mobile layout over the same API and store | Met by construction; not covered by tests |
| Sequential capture | A member adds several list items in a row | One `AddItemToList` command per item, name only | Met |
| Shared list edit | Member A checks an item while member B has the list open | No server push or polling reaches B | Not met |
| Week from last week | A member copies the previous week and requests a shopping list | Two commands, each completing server-side in one request | Met; shopping-list command crosses a module boundary |
| Spot unowned areas | A member opens Areas | One projection query returns all areas with owners | Met |
