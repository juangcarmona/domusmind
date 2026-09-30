---
id: UC-FAMILY-RESET-MEMBER-ACCESS
type: use-case
title: "Reset a person's password"
status: draft
primary-actor: ACT-HOUSEHOLD-MANAGER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-MANAGERS-ADMINISTER
  - BR-FAMILY-MANAGER-KEEPS-OWN-ACCESS
uses-terms:
  - TERM-MEMBER-ACCESS
  - TERM-MANAGER
provenance:
  source: "openspec/specs/family/spec.md (Member Access Provisioning); docs/_legacy/04_contexts/family-member-management.md (Phase 1 slice set, Permission rules, MemberAccessStatus)"
  confidence: medium
  recovered-from: documentation
---

## Goal

A person who cannot sign in gets a new temporary password (observed: openspec/specs/family/spec.md, "reset system access"; family-member-management.md, regenerate-member-password).

## Trigger

A person forgot their password or never received it.

## Preconditions

The person has an account and the actor is a manager other than the person (observed: family-member-management.md, Permission rules, "manager, not self").

## Main Flow

1. The manager chooses to reset the person's password.
2. DomusMind issues a new temporary password.
3. The person must change it at their next sign-in: their access shows Password Reset Required if they have signed in before, otherwise Invited or Provisioned.

## Alternative Flows

None known.

## Failure Conditions

- The actor is not a manager, or is resetting their own password: rejected.

## Postconditions

The person can sign in with the temporary password and must change it.

The spec has no scenario for reset; the details come from legacy prose (observed: family-member-management.md), so confidence is medium.
