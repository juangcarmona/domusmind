---
id: UC-SIGN-IN-WITH-OIDC
type: use-case
title: Sign in through the configured identity provider
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
governed-by:
  - BR-OIDC-IDENTITY-BOUND-TO-ACCESS
uses-terms:
  - TERM-MEMBER-ACCESS
---

## Goal

Use DomusMind with an authorized identity.

## Trigger

A person opens DomusMind and chooses to sign in.

## Preconditions

A production OIDC provider is configured.

## Main Flow

1. The person authenticates with the configured provider.
2. DomusMind identifies the provider and its account identifier.
3. DomusMind grants application access only if that identity is bound to enabled access, or if the person is the designated first administrator claiming the installation with a matching verified email.

## Alternative Flows

A person with a pending invitation follows UC-FAMILY-ACCEPT-ACCESS-INVITATION before receiving household access.

## Failure Conditions

An unrecognized, disabled or mismatched identity receives no household access. A provider account alone is insufficient.

## Postconditions

The authorized person has a DomusMind session with only their permitted access.
