---
id: UC-FAMILY-UPDATE-MEMBER-DETAILS
type: use-case
title: "Update a person's core details"
status: draft
primary-actor: ACT-HOUSEHOLD-MANAGER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-MANAGERS-ADMINISTER
  - BR-FAMILY-BIRTH-DATE-IN-PAST
  - BR-FAMILY-PERSON-NEEDS-NAME-AND-ROLE
  - BR-FAMILY-MANAGER-MUST-BE-ADULT
uses-terms:
  - TERM-MEMBER
  - TERM-MANAGER
  - TERM-MEMBER-ROLE
provenance:
  source: "openspec/specs/family/spec.md (Member Core Details Update); docs/_legacy/04_contexts/family.md (Commands: UpdateMember); docs/_legacy/04_contexts/family-member-management.md (update-member-core-details); src/backend/DomusMind.Domain/Family/Family.cs (UpdateMember); interview: product owner decision Q-0022 (E-0158); interview: product owner decision Q-0023 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Goal

A person's name, role and birth date are correct (observed: openspec/specs/family/spec.md, Member Core Details Update).

## Trigger

A manager notices a person's details are wrong or have changed, for example a child becoming an adult.

## Preconditions

The person exists in the household. The actor is a manager, or the person themselves changing their own name or birth date (decided: Q-0023).

## Main Flow

1. The manager opens the person from the directory.
2. The manager changes the full name, role or birth date.
3. DomusMind saves the new values.

## Alternative Flows

- The manager also changes whether an adult is a manager; the designation is set when editing an adult, and only adults qualify (decided: Q-0022; observed: Family.cs UpdateMember).
- A person who is not a manager opens their own record and changes their own full name or birth date; DomusMind saves the new values (decided: Q-0023).

## Failure Conditions

- The actor is not a manager and changes another person's details: rejected.
- The actor is not a manager and changes their own role: rejected (decided: Q-0023).
- The birth date is in the future: rejected with a validation error.

## Postconditions

The person's record shows the new details everywhere in the product.
