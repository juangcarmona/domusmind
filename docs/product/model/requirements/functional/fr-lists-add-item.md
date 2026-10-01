---
id: FR-LISTS-ADD-ITEM
type: functional-requirement
title: "Add an item to an active list with only a name"
status: draft
derived-from:
  - UC-LISTS-ADD-ITEM
  - BR-LISTS-NEW-ITEM-UNCHECKED-APPENDED
  - BR-LISTS-ITEM-NAME-REQUIRED
verification:
  - scenario-ref: "SB-LISTS-ADD-ITEM"
  - scenario-ref: "SB-LISTS-SEQUENTIAL-CAPTURE"
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Addition)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household add an item to an active list with only a name, optionally with a quantity and a note. The item MUST be created unchecked in the last position. Importance and temporal fields MUST NOT be set at addition; they are applied afterwards.

## Rationale

Capture must be faster than remembering.
