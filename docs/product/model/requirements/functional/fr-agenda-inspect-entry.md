---
id: FR-AGENDA-INSPECT-ENTRY
type: functional-requirement
title: "Inspect an entry in place"
status: draft
derived-from:
  - "UC-AGENDA-INSPECT-ENTRY"
  - "BR-AGENDA-PROJECTION-READ-ONLY"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY"
verification:
  - scenario: "Selecting a plan in Week mode opens its detail beside the grid (desktop) or in a bottom sheet (mobile), with type, time, participants and reminders, and an Edit action (observed: agenda.md, Inspector)."
  - scenario: "Selecting a projected list item shows its list, title, due date or reminder, checked state, importance and note read-only, with only \"Open in Lists\" and no edit, status, date or delete controls (observed: agenda.md, Inspector content for projected list items)."
uses-terms:
  - "TERM-AGENDA"
  - "TERM-PLAN"
  - "TERM-LIST-ITEM"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Interaction Grammar: Selection, Inspector / Bottom Sheet, Editing)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

Selecting an entry MUST open its structured detail in place (side panel on desktop, bottom sheet on mobile) without navigating away, showing type or source, time range and type-specific facts. An Edit action MUST be offered only for native entries (plans, tasks, routines) and, on close, the Agenda MUST refresh the window. Projected list items MUST offer only "Open in Lists"; imported entries MUST be read-only (observed: agenda.md).

## Rationale

Understanding an entry should not require opening an editor or leaving the Agenda (observed: agenda.md).
