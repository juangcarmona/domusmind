---
id: BR-AREAS-UNIQUE-SUPPORT
type: business-rule
title: A person is listed as Support of an Area at most once
status: draft
applies-to:
- UC-AREAS-ADD-SUPPORT
uses-terms:
- TERM-AREA
- TERM-AREA-SUPPORT
provenance:
  source: openspec/specs/areas/spec.md (Secondary Owner Assignment); docs/_legacy/04_contexts/responsibilities.md (Role Consistency); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs (AssignSecondaryOwner)
  confidence: high
  recovered-from: observation
---

## Rule

The same person cannot be Support of the same Area more than once; a duplicate Support assignment is rejected.

## Rationale

Support people must be unique within the Area (observed: openspec/specs/areas/spec.md, Secondary Owner Assignment; docs/_legacy/04_contexts/responsibilities.md, Role Consistency; enforced in src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs).

## Examples

Luis already supports "School"; adding Luis again as Support is rejected (observed: openspec/specs/areas/spec.md, scenario Duplicate secondary owner is rejected).

## Exceptions

None.
