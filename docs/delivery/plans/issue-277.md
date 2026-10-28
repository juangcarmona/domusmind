# Issue #277 plan: Lighthouse flows and development BFF

## Outcome

Adopt the pinned Lighthouse engineering baseline and its optional SDLC workflow projection while preserving DomusMind's product, architecture and lifecycle authorities. Prove a development-only OIDC browser session in the existing API host. Production authentication remains local JWT, and Lighthouse agent workers remain unavailable until their dedicated runner and isolation policy are verified.

## Architecture

Keep the modular monolith and its REST API. In Aspire Development only, the API host owns OIDC Authorization Code with PKCE, a protected browser session cookie, CSRF/origin validation and mapping from the configured test identity to an active local `AuthUser`. Existing household authorization remains authoritative. The browser continues to call same-origin API routes. No product-model change or production identity migration is in scope.

Keep the existing DomusMind surface system, palette, layout and responsive behavior. Map semantic Lighthouse token roles onto DomusMind's current tokens; adopt no Lighthouse visual component or palette by default.

Keep Lighthouse workflows gated. The router must require an explicit repository setting, and agent execution must continue to require the dedicated self-hosted runner label. Do not configure the enable setting as part of this item. Runner and host-policy acceptance are separately reported evidence, not inferred from workflow compilation.

## Ordered tasks

1. Reconcile the installed Lighthouse manifest, package pins, generated projections and label mapping with DomusMind's lifecycle; keep ProductShape, ArcShape, OpenSpec and DomusMind lifecycle authoritative.
2. Add BFF policy and integration coverage for OIDC callback validation, verified email and subject mapping, cookie flags, identity mapping, same-origin enforcement, CSRF, expiration, logout and user switching.
3. Exercise a real development sign-in through Aspire, Vite and the configured Keycloak realm, then prove an authenticated API query and mutation and capture browser evidence. Use two disposable Keycloak identities mapped to separate active local `AuthUser` records to prove account switching.
4. Verify the DomusMind token mapping resolves through the design guide without replacing current surface guidance; check light/dark/system behavior, keyboard focus, reduced motion and narrow viewport behavior.
5. Run the deterministic backend, web, product, architecture, image and Lighthouse checks derived from repository CI. Keep Lighthouse agent execution disabled and record the runner acceptance as pending unless its operator evidence is available.
6. Update the existing PR with the evidence and remaining limits; do not create a second branch or PR.

## Test plan

- Backend unit/integration tests cover BFF mode restriction, session cookie properties, authorization-code callback and PKCE behavior, verified email plus configured-subject-to-email-to-local-user mapping, unknown/disabled/unverified/mismatched identities, CSRF header/form handling, missing and foreign Origin, session expiry, logout and account switching.
- A Playwright journey runs against Aspire with the actual Keycloak development realm and Vite proxy. It asserts the Secure/HttpOnly/SameSite cookie, absence of access/ID/refresh tokens from browser storage and JavaScript responses, one authenticated read, one mutation, rejected cross-origin/CSRF requests and logout.
- The browser journey verifies that the forwarded scheme and host seen by the BFF resolve to the configured same-origin UI/API boundary. It must prove a legitimate proxied mutation succeeds and a foreign `Origin` is rejected through that same path.
- For session expiry, configure a short, bounded BFF lifetime only in the disposable development test environment; after the cookie expires, the browser must receive an unauthenticated response from the real API path. For account switching, sign out identity A, sign in identity B, verify `/me` resolves to B, and verify A's session can no longer access authenticated routes.
- Existing backend and web test suites, lint, typecheck, builds, Lighthouse verification, product validation, OpenSpec citation verification, architecture checks and image smoke remain required.
- The dedicated workflow runner and its operator-owned isolation policy require their own infrastructure acceptance evidence. A green hosted CI run does not satisfy that evidence.

## Definition of Done

- Acceptance criteria: every issue criterion has linked test or browser evidence, including a real OIDC journey.
- Implementation: the BFF remains development-only, fails closed, and production authentication remains unchanged.
- Tests: all applicable backend and web tests pass with the new security boundaries covered.
- Quality: backend, web, Lighthouse, product, architecture, image and security checks pass.
- Security: cookie, CSRF, Origin, identity mapping, no-token-storage and logout findings are reviewed against source and runtime evidence.
- Architecture: changes follow this plan, preserve ADR-0002 and the modular monolith, and keep the BFF in the existing API host.
- Documentation: architecture, operational, design and PR evidence match observed behavior.
- Product/spec consistency: no product behavior changes; citations and existing specs remain valid.
- Evidence: CI and real browser evidence are linked to the PR.
- Human review: the owner retains the non-substitutable Done judgement and required forge approval.
- Limitations: production OIDC, production provisioning/recovery, workflow automation enablement and runner acceptance remain explicitly outside completion unless separately verified.

## Known deviations and risks

Implementation commits `08b5dc3` and `e206b6b` predate this FF run and the independent plan review. The proposal review is therefore retrospective; it cannot change that sequence. The current CI browser journey does not exercise the Aspire + Keycloak BFF path. The only currently configured development subject is the local test administrator; the two-identity journey requires disposable test setup and must not add production users. No production identity decision is made here.
