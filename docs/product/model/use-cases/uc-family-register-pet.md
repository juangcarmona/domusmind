---
id: UC-FAMILY-REGISTER-PET
type: use-case
title: "Register a pet"
status: draft
primary-actor: ACT-HOUSEHOLD-MANAGER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-PERSON-NEEDS-NAME-AND-ROLE
  - BR-FAMILY-PET-RESTRICTIONS
  - BR-FAMILY-MANAGERS-ADMINISTER
uses-terms:
  - TERM-PET
  - TERM-MEMBER-ROLE
provenance:
  source: "openspec/specs/family/spec.md (Pet Registration); docs/_legacy/04_contexts/family.md (Pet, Commands: AddPet); interview: product owner decision Q-0024 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

The household's pet is recorded so it can take part in plans such as vet appointments (observed: openspec/specs/family/spec.md, Pet Registration).

## Trigger

The household gets a pet or wants to record one.

## Preconditions

The household exists and the actor is a manager (decided: Q-0024).

## Main Flow

1. The actor adds the pet by name with the Pet role.
2. DomusMind adds the pet to the household with its own identity.

## Alternative Flows

None known.

## Failure Conditions

- Missing name: rejected with a validation error.
- The actor is not a manager: rejected (decided: Q-0024).

## Postconditions

The pet appears in the directory's pet group and can be a plan participant. It cannot be given access, assigned tasks or own areas.
