---
id: CHG-PRODUCTION-OIDC-001
type: product-change
title: 'Use OIDC for production sign-in and household access'
status: applied
base-revision: 'b4f77ad01ca87d2de33682d24ea2b7eee9e4cb15'
operations:
  add:
    - ACT-INSTALLATION-OPERATOR
    - UC-INITIALIZE-INSTALLATION
    - UC-SIGN-IN-WITH-OIDC
    - UC-FAMILY-ACCEPT-ACCESS-INVITATION
    - UC-FAMILY-MANAGE-INVITATION
    - BR-OIDC-IDENTITY-BOUND-TO-ACCESS
    - CON-PRODUCTION-OIDC-SIGN-IN
    - FR-OIDC-SIGN-IN-AND-BOOTSTRAP
  modify:
    - ACT-HOUSEHOLD-MANAGER
    - CON-PRODUCT-CURRENT-SCOPE
    - TERM-MEMBER-ACCESS
    - TERM-MANAGER
    - BR-FAMILY-MANAGERS-ADMINISTER
    - BR-FAMILY-MANAGER-KEEPS-OWN-ACCESS
    - UC-FAMILY-PROVISION-MEMBER-ACCESS
    - UC-FAMILY-DISABLE-MEMBER-ACCESS
    - UC-FAMILY-ENABLE-MEMBER-ACCESS
    - FR-FAMILY-MANAGE-MEMBER-ACCESS
    - SB-FAMILY-MANAGER-PROVISIONS-ACCESS
    - SB-FAMILY-MANAGER-DISABLES-ACCESS
    - JRN-PRODUCT-ONBOARD-HOUSEHOLD
  remove:
    - UC-FAMILY-RESET-MEMBER-ACCESS
---

## Problem

The accepted product definition assumes DomusMind issues passwords, requires password changes and lets household managers reset other people's passwords. It has no production rule for an external identity provider, a secure first administrator, or a person accepting an invitation with a verified external identity. Those assumptions conflict with the decision to require OIDC sign-in for production installations.

## Intended Product Outcome

Every production installation uses one operator-configured OpenID Connect provider for sign-in. The installation operator records the first administrator's email through the existing first-run setup, which is accessible only to the operator. That person becomes the first administrator only after signing in with a provider-verified matching email; the first household still follows the existing creation journey. DomusMind never asks for or recovers a local password in this mode.

A household manager grants a person access through a single-use invitation link tied to that person's email. The manager can copy and share the link. Acceptance requires a sign-in at the configured provider, a matching verified email and the invitation; only then is the provider identity bound to that person's DomusMind access. Managers may replace or revoke pending invitation links and disable or re-enable application access. Credential recovery belongs to the provider. Person and sign-in identity remain separate.

## Rationale

The development BFF pilot has shown an OIDC browser session while production still uses local credentials. The product decision makes the intended production boundary explicit before changing sign-in, household access or setup behaviour. A configurable provider preserves installation choice; Keycloak can serve as a self-hosted reference without becoming part of the household domain.

## Affected Product Areas

- Installation setup and first administrator.
- Sign-in and credential recovery.
- Person invitations, access state and the people directory.
- Household manager permissions and onboarding.

## Open Questions

None.

## Product Acceptance

- An installation cannot be claimed by someone who is not its operator, and its first administrator must prove control of the configured email through OIDC.
- A person cannot use DomusMind merely because they have an account at the provider; their access must be granted and bound to the intended household person.
- A manager can copy a single-use invitation link, see pending, active and disabled access, replace or revoke pending links and disable or re-enable application access without managing provider credentials.
- Password creation, password changes and password recovery are absent from DomusMind's production access journey.

## Out of Scope

Migration or compatibility of existing installations and accounts, automatic invitation email, the choice of a specific OIDC provider, implementation architecture, deployment, and activation of the development pilot in production are outside this Product Change.

