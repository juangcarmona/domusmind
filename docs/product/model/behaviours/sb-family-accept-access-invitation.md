---
id: SB-FAMILY-ACCEPT-ACCESS-INVITATION
type: structured-behaviour
title: An invited person accepts with their verified identity
status: active
illustrates:
  - UC-FAMILY-ACCEPT-ACCESS-INVITATION
  - BR-OIDC-IDENTITY-BOUND-TO-ACCESS
given:
  - A valid, pending invitation is tied to a household person and their email.
  - The person is authenticated with the configured provider, whose verified email matches the invitation.
  - The provider identity is not already bound to another person.
when: The invited person accepts the invitation.
then:
  - The provider issuer and account identifier are bound to that household person.
  - Application access becomes Active.
  - The invitation can no longer be used.
covers-transition:
  lifecycle: LC-MEMBER-ACCESS
  transition: ACCEPT
uses-terms:
  - TERM-MEMBER-ACCESS
provenance:
  source: UC-FAMILY-ACCEPT-ACCESS-INVITATION; FR-FAMILY-MANAGE-MEMBER-ACCESS; BR-OIDC-IDENTITY-BOUND-TO-ACCESS
  confidence: high
  recovered-from: documentation
---

## Intent

Give the invited person access through the identity verified for their invitation.

## Boundaries

Provider credentials and household membership keep their existing rules. The example adds no new access policy.
