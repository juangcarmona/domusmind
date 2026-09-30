---
id: SB-AGENDA-MEMBER-LIST-ITEMS-READ-ONLY
type: structured-behaviour
title: "Projected list items are not editable from the Agenda"
status: draft
illustrates:
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
  - "UC-AGENDA-INSPECT-ENTRY"
  - "BR-LISTS-EDIT-ONLY-IN-LISTS"
given:
  - "a list item with a due date appears in a member's Agenda"
when: "the member views the Agenda"
then:
  - "the list item is distinguishable from tasks and plans"
  - "it cannot be edited from the Agenda"
  - "editing it takes the member to Lists"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-HOUSEHOLD-TIMELINE"
  - "TERM-LIST-ITEM"
provenance:
  source: "openspec/specs/calendar/spec.md (Member Agenda Projection: Member agenda projected list items are not editable in-view)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that Lists stays the only place list items change.

## Boundaries

None.
