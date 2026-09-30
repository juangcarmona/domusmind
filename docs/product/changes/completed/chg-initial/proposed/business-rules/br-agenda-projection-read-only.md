---
id: BR-AGENDA-PROJECTION-READ-ONLY
type: business-rule
title: "Items projected into the Agenda are read-only there and owned by their source area"
status: draft
applies-to:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
  - "UC-AGENDA-INSPECT-ENTRY"
  - "UC-LISTS-SEE-ITEMS-IN-AGENDA"
  - "UC-MEALS-SEE-MEALS-IN-AGENDA"
  - "UC-TASKS-SEE-WORK-IN-AGENDA"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-HOUSEHOLD-TIMELINE"
  - "TERM-PROJECTED-LIST-ITEM"
  - "TERM-LIST-ITEM"
  - "TERM-MEAL-SLOT"
  - "TERM-ROUTINE-OCCURRENCE"
provenance:
  source: "openspec/specs/calendar/spec.md (Household Timeline Projection, Member Agenda Projection); docs/_legacy/00_product/surfaces/agenda.md (Data); docs/_legacy/03_domain/ubiquitous-language.md (Agenda); openspec/specs/lists/spec.md (Agenda Projection); openspec/specs/calendar/spec.md (Member Agenda Projection, list items not editable scenario); openspec/specs/web-app/spec.md (Agenda Projected List Items); docs/_legacy/00_product/surfaces/lists.md (Relationship with Agenda, UX Grammar: Edit path); docs/_legacy/00_product/surfaces/agenda.md (Projected List Items, UX Grammar); docs/_legacy/04_contexts/shared-lists-item-model.md (Ownership of Behavior); docs/_legacy/03_domain/context-map.md (Temporal Item Projection); docs/_legacy/03_domain/ubiquitous-language.md (Projection); openspec/specs/meal-planning/spec.md (Agenda Projection); interview: product owner review decision (merges)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

The Agenda only reads what it shows. Building the Agenda, household or member, never creates or changes a plan, task, routine, list item, meal slot or external entry, and never turns an item into a record of another area (observed: calendar spec).

Every item in the Agenda stays owned by the area it comes from, and changes to it are made in that area, never in the Agenda. Items projected from another area are read-only in the Agenda:

- A list item cannot be edited, checked, rescheduled or deleted there. Selecting it shows a read-only view (title, due date, checked state, list name) with the single action "Open in Lists"; all edits go through the Lists surface (observed: docs/_legacy/00_product/surfaces/lists.md, Edit path; openspec/specs/lists/spec.md, Agenda Projection; openspec/specs/calendar/spec.md, Member Agenda Projection; docs/_legacy/00_product/surfaces/agenda.md, "This is non-negotiable"; openspec/specs/web-app/spec.md, Agenda Projected List Items).
- A meal slot cannot be edited there; editing happens only in Meal Planning (observed: openspec/specs/meal-planning/spec.md, Agenda Projection).

What qualifies to appear in the Agenda is decided by each source area's own inclusion rule (for example BR-LISTS-AGENDA-PROJECTION, BR-MEALS-AGENDA-PROJECTION, BR-TASKS-ROUTINES-PROJECTED); this rule does not restate those criteria.

## Rationale

The write model is divided across Calendar, Tasks, Lists and Meal Planning; only the read surface is unified (observed: agenda.md, Data). Editing a projected item from the Agenda would break the ownership model (observed: docs/_legacy/00_product/surfaces/lists.md, Edit path; docs/_legacy/00_product/surfaces/agenda.md).

## Examples

- Opening the Agenda does not turn a routine occurrence into a task (inferred from agenda.md and legacy system-spec.md, routines are projected on the fly).
- Tapping a projected "Buy stamps" item in the Agenda offers Open in Lists, not an edit form.
- Selecting "Buy birthday present" in the Agenda shows read-only detail with "Open in Lists" (observed: agenda.md).
- Tuesday's "Recipe: lentil soup" dinner shows in Tuesday's Agenda but is changed in Meal Planning.

## Exceptions

Opening the Member-scope Agenda may start a catch-up sync of a stale external calendar connection; that changes imported entries, not household records (observed: calendar spec, Background Feed Refresh). Native entries (plans, tasks, routines) offer Edit from the Agenda, which opens that entry's own edit dialog; the change is still made to the record in its owning area (observed: agenda.md, Editing). Whether routine occurrences can be completed from the Agenda is undecided, deferred (Q-0057).
