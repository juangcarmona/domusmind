---
id: UC-FAMILY-MANAGE-INVITATION
type: use-case
title: Manage a pending access invitation
status: draft
primary-actor: ACT-HOUSEHOLD-MANAGER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-MANAGERS-ADMINISTER
uses-terms:
  - TERM-MEMBER-ACCESS
---

## Goal

Keep an invitation usable by the intended person or revoke it.

## Trigger

A manager selects a pending invitation.

## Preconditions

The person has pending invited access and the actor is a household manager.

## Main Flow

1. The manager chooses to replace or revoke the pending invitation.
2. For replacement, DomusMind invalidates the old link and gives the manager a new single-use link to copy and share with the person at the same email.
3. For revocation, DomusMind invalidates the link and returns the person's access to No Access.

## Alternative Flows

None.

## Failure Conditions

A non-manager cannot manage invitations. An accepted, replaced or revoked link cannot be used again.

## Postconditions

Only a valid pending invitation can still be accepted.
