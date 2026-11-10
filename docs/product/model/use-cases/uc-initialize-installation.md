---
id: UC-INITIALIZE-INSTALLATION
type: use-case
title: Initialize a production installation
status: draft
primary-actor: ACT-INSTALLATION-OPERATOR
supporting-actors: []
governed-by:
  - BR-OIDC-IDENTITY-BOUND-TO-ACCESS
uses-terms: []
---

## Goal

Designate the first administrator of an installation.

## Trigger

The operator opens the first-run setup.

## Preconditions

The installation is uninitialized, its OIDC provider is configured, and the actor is authorized as the installation operator.

## Main Flow

1. The operator enters the first administrator's email in the existing setup flow.
2. DomusMind records that email as the only identity eligible to claim first administration.
3. DomusMind closes first-run setup to further attempts.

## Alternative Flows

None.

## Failure Conditions

An unauthorized actor or an already initialized installation cannot complete setup.

## Postconditions

The designated person may claim first administration after OIDC sign-in with a matching verified email. No household is created by this use case.
