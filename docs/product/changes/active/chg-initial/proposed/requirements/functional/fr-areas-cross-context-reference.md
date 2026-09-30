---
id: FR-AREAS-CROSS-CONTEXT-REFERENCE
type: functional-requirement
title: Let other household work reference an Area
status: draft
derived-from:
- BR-AREAS-OWNERSHIP-EXCLUSIVE
verification:
- scenario-ref: SB-AREAS-TASK-REFERENCES-AREA
uses-terms:
- TERM-AREA
- TERM-TASK
- TERM-PLAN
- TERM-ROUTINE
provenance:
  source: 'openspec/specs/areas/spec.md (Requirement: Cross-Context Referencing); docs/_legacy/04_contexts/responsibilities.md'
  confidence: high
  recovered-from: documentation
---

## Requirement

Tasks, plans, routines and calendar entries MAY reference an Area for organisational context. Referencing an Area MUST NOT change its Owner or Support.

## Rationale

Areas are a categorisation anchor for the rest of the product while ownership stays in one place (observed: openspec/specs/areas/spec.md, Cross-Context Referencing; docs/_legacy/04_contexts/responsibilities.md, Boundaries With Other Contexts).
