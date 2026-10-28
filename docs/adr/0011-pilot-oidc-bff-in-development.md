---
name: pilot-oidc-bff-in-development
description: Pilot a same-origin OIDC browser session in development while retaining the accepted local-authentication production boundary.
status: Accepted
date: 2026-10-28
deciders: [Juan G. Carmona]
tags: [security, web, development]
---

# ADR-0011: Pilot an OIDC BFF in development

## Context

DomusMind's browser currently stores access and refresh tokens in `localStorage` and sends bearer tokens to the same-origin API. Lighthouse's web BFF profile expects a server-side browser session, PKCE, CSRF protection and no browser-readable tokens. [ADR-0002](0002-keep-authentication-local-and-separate-from-member-identity.md) remains the accepted production identity decision. Household access provisioning and password reset still depend on local accounts.

## Decision

We will run a development-only OIDC BFF pilot in the existing ASP.NET Core host, using a Keycloak realm supplied by Aspire. The browser receives a Secure, HttpOnly session cookie; the host handles Authorization Code with PKCE. A configured issuer subject and email must resolve to an active local `AuthUser`, preserving its internal ID and the separate household `Member`. The pilot uses test identities and one authenticated read/write journey. Production stays on local JWT authentication until a Product Change decides identity lifecycle and deployment.

## Consequences

- **Positive:** The browser can exercise the Lighthouse session boundary without replacing the household domain or production login.
- **Negative:** Development gains a Keycloak container and a second authentication mode. The pilot does not implement member provisioning or password reset through Keycloak.
- **Neutral:** Existing local users are not migrated. A production OIDC decision must address first-run setup, member access states, credential recovery and operations before changing ADR-0002.

## Alternatives considered

- **Keep browser bearer tokens:** rejected for the pilot because it cannot demonstrate the BFF security contract.
- **Require an external IdP in production now:** deferred because its product and operational effects have not been decided.
- **Add a separate BFF service:** rejected because the existing same-origin ASP.NET Core host can provide the boundary.

## References

- Related: [ADR-0002](0002-keep-authentication-local-and-separate-from-member-identity.md)
- Contract: [`docs/profiles/web-bff/`](../profiles/web-bff/README.md)
- Implementation: [`BffAuthenticationExtensions.cs`](../../src/backend/DomusMind.Api/Auth/BffAuthenticationExtensions.cs)
