---
id: BR-AREAS-OWNERSHIP-OPTIONAL
type: business-rule
title: An Area may exist without an Owner
status: draft
applies-to:
- UC-AREAS-CREATE-AREA
uses-terms:
- TERM-AREA
- TERM-AREA-OWNER
- TERM-OWNERSHIP-GAP
provenance:
  source: openspec/specs/areas/spec.md (Area Creation); docs/_legacy/04_contexts/responsibilities.md (Lifecycle Integrity); docs/_legacy/00_product/surfaces/areas.md (Interaction); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs
  confidence: high
  recovered-from: documentation
---

## Rule

Ownership is optional when an Area is created; a new Area has no Owner unless one is given, and an unowned Area is a visible gap rather than an invalid state.

## Rationale

Adding an Area must stay lightweight (observed: docs/_legacy/00_product/surfaces/areas.md, Interaction). The legacy context allowed for Areas that require an Owner (observed: docs/_legacy/04_contexts/responsibilities.md, Lifecycle Integrity) but the spec settles on optional ownership (observed: openspec/specs/areas/spec.md, Area Creation).

## Examples

A household adds "Pets" before deciding who owns it; the Area appears as unowned at the top of the list (observed: openspec/specs/areas/spec.md, Ownership Visibility).

## Exceptions

None.
