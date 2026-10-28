---
id: FR-OIDC-SIGN-IN-AND-BOOTSTRAP
type: functional-requirement
title: Initialize and sign in with authorized external identity
status: draft
derived-from:
  - UC-INITIALIZE-INSTALLATION
  - UC-SIGN-IN-WITH-OIDC
  - UC-FAMILY-ACCEPT-ACCESS-INVITATION
  - BR-OIDC-IDENTITY-BOUND-TO-ACCESS
  - CON-PRODUCTION-OIDC-SIGN-IN
verification:
  - scenario: An unauthorized visitor cannot complete first-run setup
  - scenario: The designated first administrator claims access with the configured provider and a matching verified email
  - scenario: A provider account without a valid invitation or existing binding receives no household access
  - scenario: A person with disabled access cannot use an existing or new DomusMind session
uses-terms:
  - TERM-MEMBER-ACCESS
---

## Requirement

The product MUST restrict first-run setup to the installation operator, record the first administrator's email there, and require a matching verified OIDC email before that person claims administration. It MUST authenticate production users through the configured provider and authorize household access only through the bound provider identity and current DomusMind access state. It MUST deny active sessions after access is disabled.

## Rationale

Initialization, authentication and household authorization need separate checks so a provider account or public setup page cannot take an installation.
