---
id: UC-FAMILY-ACCEPT-ACCESS-INVITATION
type: use-case
title: Accept an invitation to use DomusMind
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-OIDC-IDENTITY-BOUND-TO-ACCESS
uses-terms:
  - TERM-MEMBER-ACCESS
---

## Goal

Gain access to the household person for whom the invitation was issued.

## Trigger

The invited person opens the single-use invitation link shared by the manager.

## Preconditions

The invitation is valid and pending, and the person has authenticated with the configured OIDC provider.

## Main Flow

1. DomusMind checks that the provider has verified the person's email and that it matches the invitation.
2. The person accepts the invitation.
3. DomusMind binds the provider's issuer and account identifier to that household person's access.
4. Access becomes Active and the invitation can no longer be used.

## Alternative Flows

None.

## Failure Conditions

A revoked, replaced or already used link is rejected. An unverified or mismatched email, or a provider identity already bound to another person, is rejected.

## Postconditions

The person may use DomusMind as the intended household person.
