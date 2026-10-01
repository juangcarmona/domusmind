---
id: UC-FAMILY-PROVISION-MEMBER-ACCESS
type: use-case
title: "Give a person access to DomusMind"
status: draft
primary-actor: ACT-HOUSEHOLD-MANAGER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-MANAGERS-ADMINISTER
  - BR-FAMILY-PET-RESTRICTIONS
uses-terms:
  - TERM-MEMBER-ACCESS
  - TERM-MANAGER
  - TERM-PET
provenance:
  source: "openspec/specs/family/spec.md (Member Access Provisioning); docs/_legacy/04_contexts/family-member-management.md (Phase 1 slice set, Permission rules, MemberAccessStatus); src/backend/DomusMind.Domain/Family/Family.cs (LinkMemberAccount)"
  confidence: high
  recovered-from: documentation
---

## Goal

A person who had no access gets a sign-in account linked to them and can start using DomusMind (observed: openspec/specs/family/spec.md, Member Access Provisioning).

## Trigger

A manager wants another person in the household to use DomusMind.

## Preconditions

The person has No Access, is not a pet, and the actor is a manager.

## Main Flow

1. The manager chooses to grant access to the person.
2. DomusMind links a new sign-in account to the person.
3. The person's access becomes Invited or Provisioned: they must change their password on first sign-in.

## Alternative Flows

None known.

## Failure Conditions

- The person is a pet: rejected.
- The actor is not a manager: rejected.
- The person already has an account: rejected (observed: src/backend/DomusMind.Domain/Family/Family.cs, LinkMemberAccount).

## Postconditions

The person has an account and appears as Invited or Provisioned until they sign in and change their password.
