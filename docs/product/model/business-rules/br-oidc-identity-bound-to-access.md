---
id: BR-OIDC-IDENTITY-BOUND-TO-ACCESS
type: business-rule
title: External identity grants access only after an authorized binding
status: draft
applies-to:
  - UC-INITIALIZE-INSTALLATION
  - UC-SIGN-IN-WITH-OIDC
  - UC-FAMILY-ACCEPT-ACCESS-INVITATION
uses-terms:
  - TERM-MEMBER-ACCESS
---

## Rule

DomusMind MUST bind an OIDC provider's issuer and stable account identifier to a DomusMind person only when that person accepts a valid invitation tied to a matching provider-verified email, or when the designated first administrator claims an installation with a matching provider-verified email. Later sign-ins MUST use that binding and enabled application access, not email matching alone. One provider identity cannot be bound to two people.

## Rationale

An account at the provider must not grant membership by itself or be silently reassigned when an email changes.

## Examples

- A provider account with the right email but no invitation cannot enter a household.
- A person accepts a valid invitation and then signs in with the bound provider identity.
- A later provider email change does not move the binding to another person.

## Exceptions

The first administrator's restricted setup designation substitutes for an invitation.
