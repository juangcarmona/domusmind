---
id: BR-AREAS-SINGLE-OWNER
type: business-rule
title: An Area has at most one Owner
status: draft
applies-to:
- UC-AREAS-ASSIGN-OWNER
uses-terms:
- TERM-AREA
- TERM-AREA-OWNER
provenance:
  source: openspec/specs/areas/spec.md (Primary Owner Assignment, Responsibility Transfer); docs/_legacy/04_contexts/responsibilities.md (Invariants); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs
  confidence: high
  recovered-from: documentation
---

## Rule

An Area has at most one Owner at any time. Assigning an Owner when one is set replaces the previous one, and a transfer always leaves exactly one Owner.

## Rationale

A single accountable person removes ambiguity about who holds an area of household life (observed: openspec/specs/areas/spec.md, Primary Owner Assignment; docs/_legacy/04_contexts/responsibilities.md, Invariants).

## Examples

Ana owns "Food"; assigning Luis as Owner makes Luis the Owner and Ana no longer holds that role (observed: openspec/specs/areas/spec.md, scenario Primary owner is replaced).

## Exceptions

None.
