---
id: FR-TASKS-CREATE-ROUTINE
type: functional-requirement
title: Define a recurring routine
status: draft
derived-from:
  - UC-TASKS-CREATE-ROUTINE
  - BR-TASKS-VALID-ROUTINE-RECURRENCE
  - BR-TASKS-PEOPLE-SCOPE-NEEDS-TARGETS
verification:
  - scenario-ref: "SB-TASKS-CREATE-WEEKLY-ROUTINE"
  - scenario-ref: "SB-TASKS-ROUTINE-INVALID-RECURRENCE-REJECTED"
  - scenario-ref: "SB-TASKS-ROUTINE-PEOPLE-SCOPE-NEEDS-TARGETS"
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-RECURRENCE
  - TERM-ROUTINE-SCOPE
provenance:
  source: "openspec/specs/tasks/spec.md (Requirement: Routine Creation); interview: product owner decision Q-0006 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a person define a routine with a name, a scope (household or specific people), a colour and a recurrence (Daily, Weekly, Monthly or Yearly with its day selectors), optionally with a time of day. A new routine MUST start active and MUST NOT produce tasks. Invalid recurrences and people scopes without target people MUST be rejected (observed: openspec/specs/tasks/spec.md, Requirement Routine Creation). The openspec also names a routine kind; routine kind is not part of the product (decided: Q-0006).

## Rationale

Recurring work should be defined once and then simply show up.
