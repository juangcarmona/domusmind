---
id: FR-AREAS-CREATE-AREA
type: functional-requirement
title: Create a named Area in the household
status: draft
derived-from:
- UC-AREAS-CREATE-AREA
- BR-AREAS-NAME-REQUIRED
- BR-AREAS-OWNERSHIP-OPTIONAL
verification:
- scenario-ref: SB-AREAS-CREATE-AREA
uses-terms:
- TERM-AREA
provenance:
  source: 'openspec/specs/areas/spec.md (Requirement: Area Creation); docs/_legacy/00_product/surfaces/areas.md (Interaction)'
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let the household create an Area with a name, scoped to that household. Owner and Support MUST be optional at creation, and an Area created without them MUST have no Owner.

## Rationale

Adding an Area stays lightweight so households start using Areas without setup burden (observed: openspec/specs/areas/spec.md, Area Creation; docs/_legacy/00_product/surfaces/areas.md, Interaction).
