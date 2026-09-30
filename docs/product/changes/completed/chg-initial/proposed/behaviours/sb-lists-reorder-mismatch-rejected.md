---
id: SB-LISTS-REORDER-MISMATCH-REJECTED
type: structured-behaviour
title: "A reorder that omits an item is rejected"
status: draft
illustrates:
  - UC-LISTS-REORDER-ITEMS
  - BR-LISTS-REORDER-FULL-SET
given:
  - "a list has three items"
when: "a person provides a new order naming only two of them"
then:
  - "the reorder is rejected"
  - "the current order is preserved"
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Reorder; Scenario: Reorder with mismatched item set is rejected)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Reorder is a full replacement of exactly the current items.

## Boundaries

None.
