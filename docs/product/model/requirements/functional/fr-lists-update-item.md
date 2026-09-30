---
id: FR-LISTS-UPDATE-ITEM
type: functional-requirement
title: "Update an item's name, quantity and note"
status: draft
derived-from:
  - UC-LISTS-UPDATE-ITEM
  - BR-LISTS-ITEM-NAME-REQUIRED
verification:
  - scenario-ref: "SB-LISTS-RENAME-ITEM"
  - scenario-ref: "SB-LISTS-CLEAR-QUANTITY"
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Update)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household update an item's name, quantity and note. At least one MUST be provided; others MUST stay unchanged; quantity MUST be clearable. The update MUST NOT affect checked state, importance, temporal fields or position.

## Rationale

Base details are corrected often and must not disturb the item's other capabilities.
