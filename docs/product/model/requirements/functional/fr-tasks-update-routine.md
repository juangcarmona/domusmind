---
id: FR-TASKS-UPDATE-ROUTINE
type: functional-requirement
title: Update a routine going forward
status: draft
derived-from:
  - UC-TASKS-UPDATE-ROUTINE
  - BR-TASKS-VALID-ROUTINE-RECURRENCE
verification:
  - scenario-ref: "SB-TASKS-UPDATE-ROUTINE"
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-OCCURRENCE
provenance:
  source: "openspec/specs/tasks/spec.md (Requirement: Routine Update)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a person update a routine's name, recurrence, colour, scope and target people while keeping its identity; only future projections change and the routine's history stays unchanged (observed: openspec/specs/tasks/spec.md, Requirement Routine Update).

## Rationale

Routines evolve with the household.
