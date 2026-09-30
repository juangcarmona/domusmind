---
id: BR-LISTS-TOGGLE-PRESERVES-ITEM
type: business-rule
title: "Checking or unchecking an item changes only its checked state"
status: draft
applies-to:
  - UC-LISTS-TOGGLE-ITEM
uses-terms:
  - TERM-CHECKED-ITEM
  - TERM-ITEM-TEMPORAL-FIELDS
  - TERM-ITEM-IMPORTANCE
provenance:
  source: "openspec/specs/lists/spec.md (Item Toggle); docs/_legacy/04_contexts/shared-lists-item-model.md (Invariants 6-7, Toggling checked state)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Toggling flips an item between unchecked and checked. It never removes the item, never clears importance or temporal fields, and has no effect outside the list (observed: openspec/specs/lists/spec.md, Item Toggle; docs/_legacy/04_contexts/shared-lists-item-model.md, Invariant 7).

## Rationale

Lists are reusable; a handled item must be ready to become relevant again for the next use.

## Examples

- Checking "Milk" keeps it in the list under Completed.
- Checking a dated item keeps its due date; it stays in the Agenda de-emphasised.

## Exceptions

None.
