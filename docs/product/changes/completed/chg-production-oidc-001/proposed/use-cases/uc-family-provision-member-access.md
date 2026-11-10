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
  - BR-OIDC-IDENTITY-BOUND-TO-ACCESS
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

A person with No Access receives a pending invitation to use DomusMind.

## Trigger

A manager wants another person in the household to use DomusMind.

## Preconditions

The person has No Access, is not a pet, has an email for the invitation, and the actor is a manager.

## Main Flow

1. The manager chooses to invite the person using their email.
2. DomusMind creates a single-use invitation link tied to that person and email.
3. The person's access becomes Invited or Provisioned. The manager can copy the link to share it.

## Alternative Flows

None known.

## Failure Conditions

- The person is a pet: rejected.
- The actor is not a manager: rejected.
- The person already has pending or active access: rejected.

## Postconditions

The person remains Invited or Provisioned until they accept with a matching verified OIDC identity. No provider account is created by DomusMind.
