---
id: UC-FAMILY-ADD-MEMBER
type: use-case
title: "Add a person to the household"
status: draft
primary-actor: ACT-HOUSEHOLD-MANAGER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-PERSON-NEEDS-NAME-AND-ROLE
  - BR-FAMILY-PERSON-UNIQUE-IN-HOUSEHOLD
  - BR-FAMILY-MANAGER-MUST-BE-ADULT
  - BR-FAMILY-MANAGERS-ADMINISTER
uses-terms:
  - TERM-MEMBER
  - TERM-MEMBER-ROLE
  - TERM-HOUSEHOLD
provenance:
  source: "openspec/specs/family/spec.md (Member Addition); docs/_legacy/04_contexts/family.md (Commands: AddMember); src/backend/DomusMind.Domain/Family/Family.cs (AddMember); interview: product owner decision Q-0021 (E-0158); interview: product owner decision Q-0022 (E-0158); interview: product owner decision Q-0024 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

A new person is on the household roster and can be referred to by plans, tasks and areas (observed: openspec/specs/family/spec.md, Member Addition).

## Trigger

A person joins the household or is recorded for the first time.

## Preconditions

The household exists.

## Main Flow

1. The actor enters the person's name and chooses a role: Adult, Child or Pet (decided: Q-0021).
2. Optionally, the actor adds a birth date and notes.
3. DomusMind adds the person to the roster with an identity unique in the household.

## Alternative Flows

- The role is Pet: continue with UC-FAMILY-REGISTER-PET.
- The actor also marks an adult as manager; only adults can be managers (decided: Q-0022; observed: Family.cs).

## Failure Conditions

- Missing name or a role outside the allowed set: rejected with a validation error.
- The actor is not a manager: rejected (decided: Q-0024).
- A person with the same identity already exists: rejected.
- The household does not exist: rejected.

## Postconditions

The person appears in the directory and is available to the rest of the product.

Only managers add people (decided: Q-0024).
