---
id: UC-FAMILY-ASSIGN-HOUSEHOLD-ROLE
type: use-case
title: "Assign a person's household role"
status: draft
primary-actor: ACT-HOUSEHOLD-MANAGER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-KIND-ROLE-COMPATIBILITY
uses-terms:
  - TERM-HOUSEHOLD-ROLE
  - TERM-MEMBER
provenance:
  source: "docs/_legacy/04_contexts/family-member-management.md (HouseholdRole, Account linking, Group D)"
  confidence: low
  recovered-from: documentation
---

## Goal

Planned: V1.1. Each person has a permission level fitting their place in the household, and sign-in accounts can be linked or unlinked (observed: family-member-management.md, Group D).

## Trigger

A manager wants to widen or narrow what a person can do.

## Preconditions

The actor is a manager and the person exists.

## Main Flow

1. The manager chooses a household role for the person.
2. DomusMind checks the role fits the person's kind and applies it.

## Alternative Flows

- The manager links or unlinks a sign-in account for the person; one account links to at most one person in a household, and children cannot be linked in V1.1 (observed: family-member-management.md, Account linking).

## Failure Conditions

- The role does not fit the person's kind: rejected.
- The actor is not a manager: rejected.

## Postconditions

The person's permissions follow the new household role. Roadmap proposal only.
