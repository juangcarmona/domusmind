---
id: TERM-ITEM-IMPORTANCE
type: domain-term
title: "Item Importance"
status: draft
defined-in: BC-LISTS
synonyms:
  - "starred"
  - "importance flag"
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Item Importance); docs/_legacy/04_contexts/shared-lists-item-model.md (Group 3 - Importance, Invariant 5); docs/_legacy/00_product/surfaces/lists.md (Inspector: Importance, UX Grammar)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

A binary starred / not-starred flag on a list item marking it for high attention. It is never a score or ranking (observed: openspec/specs/lists/spec.md, Item Importance; docs/_legacy/04_contexts/shared-lists-item-model.md, Invariant 5).

## Distinguish From

- **Order**: an item's position carries no importance or priority meaning (observed: openspec/specs/lists/spec.md, Item Reorder).
- **Temporal fields**: importance neither requires nor affects them, nor Agenda eligibility (observed: openspec/specs/lists/spec.md, Item Importance).

## Usage

Shown as a star on the trailing edge of the row, filled when set; within an Agenda day, important unchecked items rank above plans (observed: docs/_legacy/00_product/surfaces/lists.md, UX Grammar; docs/_legacy/04_contexts/shared-lists-item-model.md, Ordering within Agenda day).
