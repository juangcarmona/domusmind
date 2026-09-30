---
id: BR-AGENDA-ENTRY-PRIORITY-ORDER
type: business-rule
title: "Agenda entries follow a fixed priority order within a day"
status: draft
applies-to:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
  - "UC-LISTS-SEE-ITEMS-IN-AGENDA"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-PLAN"
  - "TERM-TASK"
  - "TERM-ROUTINE"
  - "TERM-LIST-ITEM"
  - "TERM-PROJECTED-LIST-ITEM"
  - "TERM-ITEM-IMPORTANCE"
provenance:
  source: "openspec/specs/calendar/spec.md (Household Timeline Projection); openspec/specs/web-app/spec.md (Agenda Item Priority Ordering); docs/_legacy/00_product/surfaces/agenda.md (Item Priority Order, Household + Day = Board); docs/_legacy/04_contexts/shared-lists-item-model.md (Ordering within Agenda day)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Within a day or cell, entries appear in this order: (1) overdue items; (2) tasks due that day; (3) unchecked list items due that day with importance; (4) plans by start time (timed before untimed); (5) routines; (6) unchecked list items due that day without importance; (7) completed tasks and checked list items, which stay visible but de-emphasised (observed: calendar spec; web-app spec, Agenda Item Priority Ordering; agenda.md, Item Priority Order; shared-lists-item-model.md, Ordering within Agenda day).

The Lists item model phrases step 1 as "overdue unchecked items of any type"; the other sources say "overdue items". Read together, checked or completed items are never overdue, since they fall under step 7 (inferred).

## Rationale

Puts what needs attention first (inferred from agenda.md, Purpose). Important items deserve attention ahead of scheduled plans; handled items drop to the bottom (observed: shared-lists-item-model.md).

## Examples

- On Monday an overdue task shows before the 09:00 dentist plan, which shows before the evening routine (inferred from the order).
- On a day with a starred "Pay school trip" item, a dentist plan and an unstarred "Buy bread" item, the order is starred item, plan, unstarred item.

## Exceptions

agenda.md's board section still lists a shorter order without list items (overdue, due today, plans, routines, completed); the full order from the openspec and agenda.md's own Item Priority Order section prevails (observed: agenda.md).
