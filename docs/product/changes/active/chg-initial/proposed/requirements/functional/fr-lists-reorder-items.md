---
id: FR-LISTS-REORDER-ITEMS
type: functional-requirement
title: "Set the display order of a list's items"
status: draft
derived-from:
  - UC-LISTS-REORDER-ITEMS
  - BR-LISTS-REORDER-FULL-SET
verification:
  - scenario-ref: "SB-LISTS-REORDER"
  - scenario-ref: "SB-LISTS-REORDER-MISMATCH-REJECTED"
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Reorder)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household replace a list's item order with a complete new order containing exactly the list's current items, and MUST reject any order with additions, omissions or duplicates, keeping the current order. Reordering MUST change nothing but positions.

## Rationale

Order supports scanning; it must stay consistent and meaningless beyond presentation.
