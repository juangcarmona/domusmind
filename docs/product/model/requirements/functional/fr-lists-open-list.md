---
id: FR-LISTS-OPEN-LIST
type: functional-requirement
title: "Show a list's full content, unchecked first"
status: draft
derived-from:
  - UC-LISTS-OPEN-LIST
verification:
  - scenario-ref: "SB-LISTS-OPEN-UNCHECKED-FIRST"
uses-terms:
  - TERM-LIST
  - TERM-LIST-ITEM
  - TERM-CHECKED-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Detail); docs/_legacy/04_contexts/shared-lists.md (Read Models: SharedListDetail)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST show all items of a list regardless of checked state, unchecked items first and then checked items, each group in the list's display order.

Resolved conflict: an older feature spec limited the per-item detail to name, quantity, note, checked and order; the openspec and the legacy context read model include importance and temporal fields, so the full item state is shown (observed: openspec/specs/lists/spec.md, Notes: get-list-detail item fields).

## Rationale

Remaining work comes first while handled items remain accessible.
