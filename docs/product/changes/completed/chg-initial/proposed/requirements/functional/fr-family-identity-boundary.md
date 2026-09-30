---
id: FR-FAMILY-IDENTITY-BOUNDARY
type: functional-requirement
title: "Keep household structure owned in one place"
status: draft
derived-from:
  - BR-FAMILY-OWNS-HOUSEHOLD-STRUCTURE
verification:
  - scenario: "Assigning a task, owning an area or joining a plan leaves the person's household details unchanged"
  - scenario: "Planning, tasks, areas and lists offer no way to add, change or remove people, pets or relationships"
uses-terms:
  - TERM-HOUSEHOLD
  - TERM-MEMBER
provenance:
  source: "openspec/specs/family/spec.md (Identity Boundary Enforcement); docs/_legacy/04_contexts/family.md (Ownership Boundary)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST make Household and Members the only place where people, pets and relationships are created, changed or removed; every other area MUST refer to them by identity only (observed: openspec/specs/family/spec.md, Identity Boundary Enforcement).

## Rationale

One source of truth for who belongs keeps the whole product consistent.
