---
id: BR-LISTS-IMPORTANCE-BINARY
type: business-rule
title: "Importance is a binary, independent flag"
status: draft
applies-to:
  - UC-LISTS-SET-ITEM-IMPORTANCE
uses-terms:
  - TERM-ITEM-IMPORTANCE
provenance:
  source: "openspec/specs/lists/spec.md (Item Importance); docs/_legacy/04_contexts/shared-lists-item-model.md (Group 3, Invariant 5); docs/_legacy/04_contexts/shared-lists.md (Invariants)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Importance is either set or not set; it is never a score. Setting it to its current value succeeds without change. It does not affect temporal fields, checked state or Agenda eligibility (observed: openspec/specs/lists/spec.md, Item Importance).

## Rationale

Keeps lists lightweight and distinct from task prioritisation.

## Examples

- Starring an already starred item succeeds and leaves it starred.

## Exceptions

None.
