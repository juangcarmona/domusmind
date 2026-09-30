---
id: SB-AGENDA-HOUSEHOLD-LIST-ITEMS
type: structured-behaviour
title: "Temporal list items appear as their own entry type"
status: draft
illustrates:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
given:
  - "a list item has a due date within the requested window"
when: "the household timeline is requested"
then:
  - "the list item appears marked as a list item"
  - "it is distinguishable from tasks and plans"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-HOUSEHOLD-TIMELINE"
  - "TERM-LIST-ITEM"
provenance:
  source: "openspec/specs/calendar/spec.md (Household Timeline Projection: Temporal list items project into the household timeline)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that list items never masquerade as tasks or plans.

## Boundaries

None.
