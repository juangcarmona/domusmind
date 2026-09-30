---
id: BR-AREAS-ONE-HOUSEHOLD
type: business-rule
title: An Area belongs to exactly one household
status: draft
applies-to:
- BC-RESPONSIBILITIES
- UC-AREAS-CREATE-AREA
uses-terms:
- TERM-AREA
- TERM-HOUSEHOLD
provenance:
  source: openspec/specs/areas/spec.md (Purpose, Area Creation); docs/_legacy/04_contexts/responsibilities.md (Invariants); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs
  confidence: high
  recovered-from: documentation
---

## Rule

Every Area belongs to exactly one household, and keeps that household and its identity for its whole life.

## Rationale

Accountability is a household matter; Areas are scoped to the household (observed: openspec/specs/areas/spec.md, Purpose; docs/_legacy/04_contexts/responsibilities.md, Invariants, Identity and Ownership).

## Examples

A "School" Area created in the Garcia household is visible only to that household (inferred example).

## Exceptions

None.
