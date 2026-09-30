---
id: SB-LISTS-CLEAR-TIMING
type: structured-behaviour
title: "Clearing timing takes an item out of the Agenda and keeps the rest"
status: draft
illustrates:
  - UC-LISTS-CLEAR-ITEM-SCHEDULE
  - BR-LISTS-AGENDA-PROJECTION
given:
  - "a list item has a due date and a reminder"
when: "a person clears the item's timing"
then:
  - "the item no longer appears in the Agenda"
  - "its name, importance, checked state and other fields are unchanged"
uses-terms:
  - TERM-ITEM-TEMPORAL-FIELDS
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Temporal Clearing; Scenario: Household clears temporal fields from an item)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Clearing is atomic and limited to temporal fields.

## Boundaries

None.
