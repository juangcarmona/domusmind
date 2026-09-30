---
id: TERM-ARCHIVED-LIST
type: domain-term
title: "Archived List"
status: draft
defined-in: BC-LISTS
synonyms:
  - "archived"
uses-terms:
  - TERM-LIST
provenance:
  source: "openspec/specs/lists/spec.md (List Archive, List Restore); docs/_legacy/00_product/surfaces/lists.md (Lifecycle); src/backend/DomusMind.Domain/Lists/SharedList.cs (Archive, Restore); interview: product owner decision Q-0026 (E-0158); interview: product owner decision Q-0031 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

A list taken out of the household's active collection because it is no longer in active use, with all its data preserved. It can be restored to active use unchanged (observed: openspec/specs/lists/spec.md, List Archive, List Restore).

## Distinguish From

- **Rested list**: a list whose items remain ready for the next use is still active; resting is a phase of use, archiving removes it from the active collection (observed: docs/_legacy/00_product/surfaces/lists.md, Lifecycle).
- **Deleted list**: permanently deleting a list is also current product; a deleted list and its items are gone and cannot be restored, whereas an archived list keeps all its data (decided: Q-0026; observed: src/backend/DomusMind.Domain/Lists/SharedList.cs, Delete).

## Usage

Archived lists are excluded from the default list of the household's lists (observed: openspec/specs/lists/spec.md, List Retrieval). An archived list is read-only: no adding, checking or editing items until it is restored (decided: Q-0031).
