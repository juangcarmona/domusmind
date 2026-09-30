---
id: FR-FAMILY-REGISTER-PET
type: functional-requirement
title: "Register pets as part of the household"
status: draft
derived-from:
  - UC-FAMILY-REGISTER-PET
  - BR-FAMILY-PET-RESTRICTIONS
  - BR-FAMILY-MANAGERS-ADMINISTER
verification:
  - scenario-ref: "SB-FAMILY-PET-ADDED"
  - scenario-ref: "SB-FAMILY-PET-ACCESS-REJECTED"
  - scenario: "A pet can be chosen as a participant of a plan such as a vet appointment"
  - scenario: "A pet cannot be chosen as a task assignee or area owner, and has no row in the Agenda's household view"
uses-terms:
  - TERM-PET
provenance:
  source: "openspec/specs/family/spec.md (Pet Registration); docs/_legacy/04_contexts/family.md (Pet); interview: product owner decision Q-0021 (E-0158); interview: product owner decision Q-0024 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a manager register a pet by adding it with the Pet role, and MUST reject the request from a person who is not a manager (decided: Q-0024). A pet MUST NOT be given access, assigned tasks or made an area owner, and MUST be left out of the Agenda's household rows and member timelines; it MAY be a plan participant. The directory MUST show pets as their own group after adults and children (observed: openspec/specs/family/spec.md, Pet Registration).

## Rationale

Pets belong to the household and appear in its plans, but do not coordinate.
