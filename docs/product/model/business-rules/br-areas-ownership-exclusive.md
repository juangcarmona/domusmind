---
id: BR-AREAS-OWNERSHIP-EXCLUSIVE
type: business-rule
title: Only Areas changes ownership
status: draft
applies-to:
- BC-RESPONSIBILITIES
uses-terms:
- TERM-AREA
- TERM-AREA-OWNER
- TERM-AREA-SUPPORT
provenance:
  source: "openspec/specs/areas/spec.md (Purpose, Cross-Context Referencing); docs/_legacy/04_contexts/responsibilities.md (Ownership Boundary); interview: product owner decision Q-0040 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Rule

Only the Areas (Responsibilities) part of the product may change who owns or supports an Area. Tasks, plans, routines and lists may reference an Area but never change its ownership. Meal plans do not reference an Area (decided: Q-0040).

## Rationale

Keeps accountability in one place and prevents operational work from silently rewriting it (observed: openspec/specs/areas/spec.md, Purpose and Cross-Context Referencing; docs/_legacy/04_contexts/responsibilities.md, Ownership Boundary).

## Examples

Creating a task in the "School" Area records the Area on the task and leaves the Area's Owner and Support unchanged (observed: openspec/specs/areas/spec.md, scenario A task references an Area).

## Exceptions

None.
