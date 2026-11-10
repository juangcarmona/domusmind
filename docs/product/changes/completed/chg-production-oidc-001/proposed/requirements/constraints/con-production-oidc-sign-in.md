---
id: CON-PRODUCTION-OIDC-SIGN-IN
type: constraint
title: Production sign-in uses a configured OIDC provider
status: draft
uses-terms:
  - TERM-MEMBER-ACCESS
---

## Constraint

Production installations MUST use one operator-configured OpenID Connect provider for authentication. DomusMind MUST NOT issue, request, reset or recover local passwords for people in these installations. Credential recovery belongs to the provider.

## Rationale

The product owner chose external authentication and a single configurable provider for production installations.

## Consequences

An installation cannot admit people until its provider is configured. Household managers control DomusMind invitations and application access without controlling provider credentials. A self-hosted provider such as Keycloak may be used but is not required.
