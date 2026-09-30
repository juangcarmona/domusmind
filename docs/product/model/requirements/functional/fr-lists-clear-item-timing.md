---
id: FR-LISTS-CLEAR-ITEM-TIMING
type: functional-requirement
title: "Clear all timing from an item at once"
status: draft
derived-from:
  - UC-LISTS-CLEAR-ITEM-SCHEDULE
verification:
  - scenario-ref: "SB-LISTS-CLEAR-TIMING"
  - scenario-ref: "SB-LISTS-CLEAR-TIMING-NOOP"
uses-terms:
  - TERM-ITEM-TEMPORAL-FIELDS
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Temporal Clearing)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household clear due date, reminder and repeat of an item together in one action, after which the item MUST no longer appear in the Agenda. The item MUST NOT be deleted and its other fields MUST stay unchanged. Clearing an item with no timing MUST succeed without change.

## Rationale

Taking an item out of the Agenda must be one simple action.
