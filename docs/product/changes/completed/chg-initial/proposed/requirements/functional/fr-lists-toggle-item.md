---
id: FR-LISTS-TOGGLE-ITEM
type: functional-requirement
title: "Check and uncheck items"
status: draft
derived-from:
  - UC-LISTS-TOGGLE-ITEM
  - BR-LISTS-TOGGLE-PRESERVES-ITEM
verification:
  - scenario-ref: "SB-LISTS-CHECK-ITEM"
  - scenario-ref: "SB-LISTS-UNCHECK-ITEM"
  - scenario-ref: "SB-LISTS-CHECKED-ITEM-STAYS-IN-AGENDA"
uses-terms:
  - TERM-CHECKED-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Toggle)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household toggle any item between unchecked and checked. Toggling MUST NOT remove the item, MUST NOT change its importance or temporal fields, and MUST have no effect outside the list. A checked item with temporal fields MUST keep appearing in the Agenda, de-emphasised.

## Rationale

Lists are toggle-based and reusable across uses.
