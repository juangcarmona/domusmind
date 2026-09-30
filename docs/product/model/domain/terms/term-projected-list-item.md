---
id: TERM-PROJECTED-LIST-ITEM
type: domain-term
title: "Projected List Item"
status: draft
defined-in: BC-LISTS
synonyms:
  - "list-item Agenda entry"
  - "AgendaItemProjection"
uses-terms:
  - TERM-LIST-ITEM
  - TERM-AGENDA
  - TERM-ITEM-TEMPORAL-FIELDS
provenance:
  source: "openspec/specs/lists/spec.md (Agenda Projection); docs/_legacy/04_contexts/shared-lists-item-model.md (Projection Rules); docs/_legacy/00_product/surfaces/lists.md (Relationship with Agenda, UX Grammar: Edit path)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

The appearance of a list item with temporal fields in the Agenda, as a distinct entry type with a visible list-origin cue. It is read-only: selecting it offers only "Open in Lists" (observed: openspec/specs/lists/spec.md, Agenda Projection; docs/_legacy/00_product/surfaces/lists.md, Edit path).

## Distinguish From

- **Task or plan in the Agenda**: a projected list item is visually distinct from both and is never merged or deduplicated with a task covering the same subject (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Conflict with tasks).
- **Plan list cue**: a plan linked to a list shows a compact cue (list name, unchecked count); that cue is not a projection of the items (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Plan-linked temporal list item).

## Usage

Assembled for the Agenda by reading list items; Lists keeps ownership of the item (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Ownership of Behavior).
