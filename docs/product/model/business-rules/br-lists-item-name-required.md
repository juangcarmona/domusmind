---
id: BR-LISTS-ITEM-NAME-REQUIRED
type: business-rule
title: "A list item must have a non-empty name"
status: draft
applies-to:
  - UC-LISTS-ADD-ITEM
  - UC-LISTS-UPDATE-ITEM
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "docs/_legacy/04_contexts/shared-lists-item-model.md (Invariant 1); docs/_legacy/04_contexts/shared-lists.md (Invariants: SharedListItem); src/backend/DomusMind.Domain/Lists/ValueObjects/ListItemName.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A list item's name must be non-empty at all times (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Invariant 1). The code trims it and caps it at 200 characters (observed: ListItemName.cs).

## Rationale

The name is the only base field needed to capture something to remember.

## Examples

- Adding "Milk" succeeds.
- Renaming an item to blank is rejected.

## Exceptions

None.
