---
id: FR-AGENDA-ENTRY-TYPES-DISTINGUISHABLE
type: functional-requirement
title: "Make every Agenda entry type recognisable"
status: draft
derived-from:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
  - "BR-AGENDA-PROJECTION-READ-ONLY"
verification:
  - scenario: "In a collapsed board row, a projected list item still shows its list name or list icon and a star or diamond marker, never a task checkbox (observed: agenda.md, UX Grammar)."
  - scenario: "A checked list item still due in the window stays visible, struck through with a done marker, in the de-emphasized section (observed: agenda.md, Checked state)."
uses-terms:
  - "TERM-AGENDA"
  - "TERM-PLAN"
  - "TERM-TASK"
  - "TERM-ROUTINE"
  - "TERM-LIST-ITEM"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Item grammar, UX Grammar - List Items in Agenda, Checked state)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST make overdue tasks, tasks, plans (with time), routines, projected list items with and without importance, imported external entries and completed items visually distinct in every Agenda view without legends. Every projected list item MUST show a list-origin cue, even in collapsed rows. Completed tasks and checked list items still in the window MUST remain visible but de-emphasized (observed: agenda.md).

## Rationale

People must always know whether an entry is a plan, a task or something from a list (observed: agenda.md).
