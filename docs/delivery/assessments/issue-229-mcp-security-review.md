# MCP OAuth security review evidence (#229)

Date: 2026-10-20  
Status: Draft implementation evidence  
Scope: OAuth/OIDC protection of the DomusMind MCP resource; write tools remain disabled.

## Evidence completed in this slice

- OAuth authorization-code flow requires PKCE, exact redirect URIs, an exact MCP `resource`, explicit consent, and the `mcp.read` scope.
- Access-token validation is issuer-bound, resource/audience-bound, lifetime-aware, scope-aware, and backed by OpenIddict token-entry validation.
- Every MCP request resolves the subject to the current DomusMind account and re-checks current household access before the transport continues.
- Account disablement, household-access removal, authorization revocation, refresh-token reuse, authorization-code replay, invalid bearer material, wrong resource requests, and rate limiting are covered by integration tests.
- Consent withdrawal accepts one validated OpenIddict authorization identifier and revokes only tokens belonging to that authorization. It does not enumerate or revoke every token for a user.
- MCP security audit events are structured and redacted: operation, authenticated user/household when known, client, scope, result, safe reason, status, and trace identifier. Tokens, authorization headers, codes, verifiers, and payloads are never logged.
- The MCP endpoint has a bounded fixed-window rate limit, partitioned by remote address, with a safe 429 result and an audit event.

## Threat review

| Threat | Boundary | Evidence / residual risk |
| --- | --- | --- |
| Token substitution or wrong resource | OpenIddict validation plus exact `aud`/resource checks | Wrong-resource and invalid-bearer tests; production issuer/key rotation remains an operational gate. |
| Confused deputy or cross-household access | Server-side subject mapping and current `IFamilyAuthorizationService` check | Disabled-account and removed-membership tests; member-specific tools remain disabled until handler ownership is proven. |
| Authorization-code replay | OpenIddict single-use authorization codes | Replay test; provider behavior remains pinned to the tested OpenIddict version. |
| Refresh-token reuse | Rotating refresh-token flow and token-entry validation | Reuse test; client interoperability must be tested against real remote clients before release. |
| Consent overreach | Explicit consent, read-only scope, exact authorization identifier on withdrawal | Grant-specific revocation test; household selection is bound into the token and cannot be changed by a tool argument. |
| Prompt injection or untrusted household content | MCP tools remain empty in production and application handlers remain the domain boundary | Future tool delivery must threat-test content-origin handling before exposing data or writes. |
| Abuse and resource exhaustion | MCP fixed-window rate limiter and bounded protocol surface | Local rate-limit test; production limits and upstream protection still require deployment tuning. |
| Audit leakage | Structured event allowlist and redacted logging | No secrets or raw token material are recorded. Durable audit retention is intentionally deferred until an audit-storage contract is approved. |

## Client interoperability matrix

| Client profile | Evidence in this slice | Decision |
| --- | --- | --- |
| RFC-compliant custom HTTPS client | End-to-end test covers login, consent, PKCE, code exchange, refresh, MCP request and revocation | Supported contract for this slice. |
| ChatGPT remote MCP connector | No external account or deployment credentials available in this test environment | Not claimed; production release gate. |
| Claude / Claude Code remote MCP | No external account or deployment credentials available in this test environment | Not claimed; production release gate. |
| Local STDIO client | Separate transport and credential boundary | Deferred; not enabled by this child. |

## Release gates that remain visible

This child establishes the in-process authorization boundary and testable contract. It does not claim production interoperability with hosted AI clients, durable audit retention, production certificate rotation/backup, or production MCP tool exposure. Those remain explicit gates for the follow-up interoperability and operational-hardening work in the Epic.
