---
id: FR-AGENDA-HOUSEHOLD-TIMELINE
type: functional-requirement
title: "Show the household's temporal picture"
status: draft
derived-from:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "BR-AGENDA-PROJECTION-READ-ONLY"
  - "BR-AGENDA-ENTRY-PRIORITY-ORDER"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-OWNER-SCOPE-ONLY"
  - "BR-LISTS-PLAN-LINK-DOES-NOT-PROJECT"
verification:
  - scenario-ref: "SB-AGENDA-HOUSEHOLD-ALL-SOURCES"
  - scenario-ref: "SB-AGENDA-HOUSEHOLD-LIST-ITEMS"
  - scenario-ref: "SB-AGENDA-HOUSEHOLD-EXCLUDES-EXTERNAL"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-HOUSEHOLD-TIMELINE"
  - "TERM-PLAN"
  - "TERM-TASK"
  - "TERM-ROUTINE"
  - "TERM-LIST-ITEM"
provenance:
  source: "openspec/specs/calendar/spec.md (Household Timeline Projection); docs/_legacy/00_product/surfaces/agenda.md (Data)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

For a requested date window the product MUST show, in Household scope, plans, tasks due in the window, projected routine occurrences and temporal list items (due date, reminder or repeat producing an occurrence in the window), ordered within each day by the Agenda priority order, and MUST NOT show imported external entries. A plan-linked list item MUST appear as its own entry. Building this view MUST NOT create or change any record (observed: calendar spec).

## Rationale

Gives the household one place to see what is happening across plans, tasks, routines and lists (observed: agenda.md, Purpose).
