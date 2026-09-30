---
id: SB-LISTS-CLEAR-TIMING-NOOP
type: structured-behaviour
title: "Clearing timing on an item without any is safe"
status: draft
illustrates:
  - UC-LISTS-CLEAR-ITEM-SCHEDULE
given:
  - "a list item has no temporal fields"
when: "a person clears the item's timing"
then:
  - "the action succeeds without error"
  - "the item is unchanged"
uses-terms:
  - TERM-ITEM-TEMPORAL-FIELDS
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Temporal Clearing; Scenario: Clearing temporal fields on an item with none is safe)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Clearing is idempotent.

## Boundaries

None.
