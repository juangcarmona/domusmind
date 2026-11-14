---
id: LC-MEMBER-ACCESS
type: domain-lifecycle
title: Household person access lifecycle
status: active
subject: TERM-MEMBER-ACCESS
states:
  - id: NO-ACCESS
    title: No access
    initial: true
  - id: INVITED
    title: Invited
  - id: ACTIVE
    title: Active
  - id: DISABLED
    title: Disabled
transitions:
  - id: INVITE
    title: Invite a household person
    from:
      - NO-ACCESS
    to: INVITED
    trigger: A manager creates a single-use invitation for the person and their email
    initiated-by:
      - ACT-HOUSEHOLD-MANAGER
    governed-by:
      - BR-FAMILY-MANAGERS-ADMINISTER
      - BR-FAMILY-PET-RESTRICTIONS
      - BR-OIDC-IDENTITY-BOUND-TO-ACCESS
    realized-by:
      - UC-FAMILY-PROVISION-MEMBER-ACCESS
  - id: ACCEPT
    title: Accept a valid invitation
    from:
      - INVITED
    to: ACTIVE
    trigger: The invited person accepts with a matching provider-verified email and an eligible provider identity
    initiated-by:
      - ACT-HOUSEHOLD-MEMBER
    governed-by:
      - BR-OIDC-IDENTITY-BOUND-TO-ACCESS
    realized-by:
      - UC-FAMILY-ACCEPT-ACCESS-INVITATION
  - id: REVOKE-INVITATION
    title: Revoke a pending invitation
    from:
      - INVITED
    to: NO-ACCESS
    trigger: A manager revokes the pending invitation
    initiated-by:
      - ACT-HOUSEHOLD-MANAGER
    governed-by:
      - BR-FAMILY-MANAGERS-ADMINISTER
    realized-by:
      - UC-FAMILY-MANAGE-INVITATION
  - id: DISABLE
    title: Disable application access
    from:
      - ACTIVE
    to: DISABLED
    trigger: A manager disables another person's application access
    initiated-by:
      - ACT-HOUSEHOLD-MANAGER
    governed-by:
      - BR-FAMILY-MANAGERS-ADMINISTER
      - BR-FAMILY-MANAGER-KEEPS-OWN-ACCESS
    realized-by:
      - UC-FAMILY-DISABLE-MEMBER-ACCESS
  - id: ENABLE
    title: Re-enable application access
    from:
      - DISABLED
    to: ACTIVE
    trigger: A manager re-enables the person's application access
    initiated-by:
      - ACT-HOUSEHOLD-MANAGER
    governed-by:
      - BR-FAMILY-MANAGERS-ADMINISTER
    realized-by:
      - UC-FAMILY-ENABLE-MEMBER-ACCESS
provenance:
  source: TERM-MEMBER-ACCESS; UC-FAMILY-PROVISION-MEMBER-ACCESS; UC-FAMILY-ACCEPT-ACCESS-INVITATION; UC-FAMILY-MANAGE-INVITATION; UC-FAMILY-DISABLE-MEMBER-ACCESS; UC-FAMILY-ENABLE-MEMBER-ACCESS; BR-OIDC-IDENTITY-BOUND-TO-ACCESS
  confidence: high
  recovered-from: documentation
---

## Purpose

Show how a household person gains, loses and regains application access while remaining on the household roster.

## Invariants

Managers administer invitations and application access. Pets cannot receive invitations. A manager cannot disable their own access. An invitation is single use and tied to one person and email. Accepting requires a matching provider-verified email and a provider identity not bound to another person.

## State Semantics

No access means there is no pending invitation or bound provider identity. Invited means a pending invitation exists and no provider identity is bound; Provisioned is an existing label for this same state. Active means the provider identity is bound and application access is enabled. Disabled retains the identity binding and household membership while denying existing and new sessions.

## Transition Semantics

An invitation moves the person from No access to Invited. Acceptance binds the provider identity and makes access Active. Revocation invalidates the pending link and returns to No access. Disabling and enabling preserve the existing provider binding. Replacing a pending invitation invalidates the old link and supplies a new one while the person remains Invited; it is covered by a separate example without claiming a state change.

## Boundaries

Provider credentials remain with the provider. These states describe a household person's application access. Installation setup and first-administrator designation are separate under UC-INITIALIZE-INSTALLATION and UC-SIGN-IN-WITH-OIDC; setup itself creates no household. The lifecycle introduces no invitation expiry policy, new UI labels or delivery claim.
