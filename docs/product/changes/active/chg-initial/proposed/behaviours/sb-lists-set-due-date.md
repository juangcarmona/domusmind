---
id: SB-LISTS-SET-DUE-DATE
type: structured-behaviour
title: "Setting a due date puts the item in the Agenda without making it a task"
status: draft
illustrates:
  - UC-LISTS-SCHEDULE-ITEM
  - BR-LISTS-ITEM-IS-NOT-A-TASK
given:
  - "a list item has no temporal fields"
when: "a person sets a due date on it"
then:
  - "the item becomes eligible to appear in the Agenda on that date"
  - "the item is not converted to a task"
uses-terms:
  - TERM-ITEM-TEMPORAL-FIELDS
  - TERM-TASK
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Temporal Assignment; Scenario: Household sets a due date on an item)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Timing enriches an item without changing its nature.

## Boundaries

None.
