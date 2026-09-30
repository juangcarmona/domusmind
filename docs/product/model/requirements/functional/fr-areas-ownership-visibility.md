---
id: FR-AREAS-OWNERSHIP-VISIBILITY
type: functional-requirement
title: Show ownership gaps first
status: draft
derived-from:
- UC-AREAS-REVIEW-OWNERSHIP
verification:
- scenario-ref: SB-AREAS-UNOWNED-FIRST
- scenario-ref: SB-AREAS-GAP-INDICATOR
uses-terms:
- TERM-AREA
- TERM-OWNERSHIP-GAP
- TERM-AREA-OWNER
- TERM-AREA-SUPPORT
provenance:
  source: 'openspec/specs/areas/spec.md (Requirement: Ownership Visibility); docs/_legacy/00_product/surfaces/areas.md (Default View, Main List)'
  confidence: high
  recovered-from: documentation
---

## Requirement

The Areas surface MUST order Areas by default as unowned, then partially assigned, then fully assigned, then archived. Each row MUST show the Area name, its Owner or a visible gap indicator when there is none, and its Support people when present.

## Rationale

Keeps accountability gaps in view without the household searching for them (observed: openspec/specs/areas/spec.md, Ownership Visibility; docs/_legacy/00_product/surfaces/areas.md, Main List).
