---
id: TERM-CHECKED-ITEM
type: domain-term
title: "Checked Item"
status: draft
defined-in: BC-LISTS
synonyms:
  - "completed item"
  - "handled item"
uses-terms:
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Item Toggle); docs/_legacy/04_contexts/shared-lists-item-model.md (Toggling checked state, Invariants 6-7); docs/_legacy/00_product/surfaces/lists.md (Completed Items)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

A list item that has been handled for the current use of the list. Checking is reversible: unchecking makes the item relevant again for the next use. A checked item stays in the list and keeps all its other fields (observed: openspec/specs/lists/spec.md, Item Toggle).

## Distinguish From

- **Removed item**: removal deletes the item permanently; checking keeps it (observed: openspec/specs/lists/spec.md, Item Removal).
- **Completed task**: a task has a completion lifecycle; a checked item is a binary, reversible state (observed: docs/_legacy/04_contexts/shared-lists.md, Design Notes).

## Usage

On the Lists surface checked items are struck through, de-emphasised and collapsed under "Completed (N)"; in the Agenda they stay visible, de-emphasised (observed: docs/_legacy/00_product/surfaces/lists.md, Completed Items, UX Grammar; openspec/specs/lists/spec.md, Agenda Projection).
