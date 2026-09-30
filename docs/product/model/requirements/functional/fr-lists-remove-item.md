---
id: FR-LISTS-REMOVE-ITEM
type: functional-requirement
title: "Permanently remove an item"
status: draft
derived-from:
  - UC-LISTS-REMOVE-ITEM
verification:
  - scenario-ref: "SB-LISTS-REMOVE-ITEM"
  - scenario-ref: "SB-LISTS-REMOVE-DATED-ITEM"
uses-terms:
  - TERM-LIST-ITEM
  - TERM-PROJECTED-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Removal)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household permanently remove an item from a list, immediately and irreversibly. A removed item with temporal fields MUST disappear from the Agenda. Removal MUST have no other effects outside the list.

## Rationale

Items that no longer belong must be removable without side effects.
