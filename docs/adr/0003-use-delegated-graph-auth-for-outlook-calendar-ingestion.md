---
name: use-delegated-graph-auth-for-outlook-calendar-ingestion
description: Hand the Outlook authorization code from the web client to the backend, which exchanges it with Microsoft Graph and keeps provider credentials server-side.
status: Accepted
date: 2026-04-07
deciders: [Juan G. Carmona]
tags: [backend, frontend, security, integrations, calendar]
---

# ADR-0003: Use delegated Graph auth for Outlook calendar ingestion

## Context

Phase 1 Outlook ingestion needs delegated access to a member's Microsoft account through Microsoft Graph. The interactive OAuth sign-in and consent happen in the browser, but calendar sync (manual and background catch-up) runs on the backend over time and needs long-lived provider credentials.

The design must keep provider refresh material server-side, never put provider access or refresh tokens in browser storage, respect the API-first modular-monolith boundary, and fit the existing Settings/Profile connect flow. The open question is how the browser hands the completed authorization back to the backend so that the backend owns token exchange and long-lived sync state.

This record migrates the legacy `ADR-003 - Outlook Delegated Auth Transport`, which was accepted without a recorded date; the date above is the legacy file's first commit.

## Decision

We will hand the short-lived authorization code from the web client to the backend, and the backend will exchange it and store provider credentials server-side.

The member starts the connection from Settings; the browser completes Microsoft sign-in and consent; Microsoft redirects to a DomusMind web callback route; the web client posts the authorization code and redirect URI to the API; the API exchanges the code for provider tokens and stores the refresh material server-side, encrypted at rest. The browser never stores provider tokens. The backend validates the redirect URI and correlation state before exchanging the code and rejects incomplete scope grants (missing `Calendars.Read` or `offline_access`); PKCE is used when the client registration supports it. This locks in the important boundary, browser for interactive consent and backend for durable credentials and sync, without extra transport complexity before it is needed.

## Consequences

- **Positive:** Provider refresh material stays server-side only. The Settings connect flow stays straightforward. Manual and background sync share one stored credential model, and the API contract stays explicit.
- **Negative:** The frontend callback must preserve redirect URI and correlation state correctly. The short-lived authorization code passes through the browser before handoff. A future native mobile client may need a different transport wrapper around the same backend exchange.
- **Neutral:** Checked against the code on 2026-10-01, the boundary holds: the backend exchanges the code through an MSAL confidential client and persists the serialized MSAL token cache on the connection, AES-encrypted when `MicrosoftGraph:TokenEncryptionKey` is set and only Base64-encoded when it is not, and the web app only posts the code and redirect URI. Several follow-up rules are not yet met: the API, not the browser, builds the Microsoft authorize URL; the `state` parameter carries `familyId:memberId` rather than an unguessable correlation value and is not validated before exchange; the redirect URI is only checked for presence; PKCE is not used; and granted scopes are logged but an incomplete grant is not rejected.

## Alternatives considered

- **Backend-owned OAuth callback endpoint**: deferred as a viable future refinement; it reduces browser handling of the code but adds callback routing and local/deployed redirect complexity that phase 1 does not need.
- **Browser-managed provider tokens**: rejected because it conflicts with server-owned long-lived sync, increases token exposure risk, and fits background refresh and catch-up sync poorly.

## References

- Legacy source: `docs/02_architecture/adrs/ADR-003-outlook-delegated-auth-transport.md`
- Backend: [`src/backend/DomusMind.Infrastructure/Integrations/Calendar/Microsoft/MicrosoftGraphCalendarAuthService.cs`](../../src/backend/DomusMind.Infrastructure/Integrations/Calendar/Microsoft/MicrosoftGraphCalendarAuthService.cs), [`src/backend/DomusMind.Api/Controllers/ExternalCalendarConnectionsController.cs`](../../src/backend/DomusMind.Api/Controllers/ExternalCalendarConnectionsController.cs), [`src/backend/DomusMind.Application/Features/Calendar/ConnectOutlookAccount/`](../../src/backend/DomusMind.Application/Features/Calendar/ConnectOutlookAccount/)
- Web: [`src/web/app/src/features/settings/components/ConnectOutlookPanel.tsx`](../../src/web/app/src/features/settings/components/ConnectOutlookPanel.tsx)
- Related: [ADR-0002](0002-keep-authentication-local-and-separate-from-member-identity.md)
