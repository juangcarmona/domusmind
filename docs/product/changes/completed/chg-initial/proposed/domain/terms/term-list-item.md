---
id: TERM-LIST-ITEM
type: domain-term
title: "List Item"
status: draft
defined-in: BC-LISTS
synonyms:
  - "Shared List Item"
  - "SharedListItem"
uses-terms:
  - TERM-LIST
  - TERM-TASK
  - TERM-ITEM-IMPORTANCE
  - TERM-ITEM-TEMPORAL-FIELDS
provenance:
  source: "openspec/specs/lists/spec.md (Purpose, Item Addition); docs/_legacy/04_contexts/shared-lists-item-model.md (Canonical Item State Shape, Capability Groups); docs/_legacy/04_contexts/shared-lists.md (SharedListItem); src/backend/DomusMind.Domain/Lists/ListItem.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

A single entry within exactly one list, with progressive capabilities: always a name and a checked state; optionally a quantity and a note; optionally importance; optionally temporal fields (due date, reminder, repeat). It has a stable position in the list's order (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Capability Groups; docs/_legacy/04_contexts/shared-lists.md, SharedListItem). A plain item with only a name is fully valid (observed: docs/_legacy/00_product/surfaces/lists.md, Item Capability Model).

## Distinguish From

- **Task**: a list item with a due date is still not a task; it has no assignee and no status beyond checked/unchecked, and is never converted into a task (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Invariants 8-9; openspec/specs/lists/spec.md, Item Temporal Assignment).
- **Projected List Item**: the read-only appearance of a list item in the Agenda, not a separate thing that can be edited (observed: openspec/specs/lists/spec.md, Agenda Projection).

## Usage

Captured through quick add with only a name, then enriched through the item inspector (observed: docs/_legacy/00_product/surfaces/lists.md, Quick Add, Inspector = Command Surface).
