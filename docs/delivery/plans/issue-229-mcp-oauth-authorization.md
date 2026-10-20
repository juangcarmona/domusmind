# Plan: protect the MCP resource with OAuth/OIDC and household authorization

- Work item: [#229](https://github.com/juangcarmona/domusmind/issues/229)
- Parent Epic: [#224](https://github.com/juangcarmona/domusmind/issues/224)
- Product Change: [CHG-MCP-HOUSEHOLD-INTEGRATION-001](../../product/changes/active/chg-mcp-household-integration/change.md)
- Definition PR: [#225](https://github.com/juangcarmona/domusmind/pull/225)
- Depends on: [#226](https://github.com/juangcarmona/domusmind/issues/226), [#227](https://github.com/juangcarmona/domusmind/issues/227), [#228](https://github.com/juangcarmona/domusmind/issues/228)

## Proposal

Protect the remote MCP resource with a standards-based OAuth 2.1/OIDC boundary while preserving DomusMind's existing identity separation and handler-owned household authorization. The MCP endpoint will accept only access tokens issued for the canonical MCP resource, validate the issuer, audience, lifetime and scopes, resolve the trusted DomusMind user server-side, and invoke application capabilities that re-check current household and member authorization.

This child establishes the authentication and authorization boundary for remote read access. It does not enable MCP write tools, accept the existing local API JWT as an MCP token, pass tokens through to another service, or introduce a second domain model.

## Current evidence and constraints

- The API currently authenticates local email/password users with a DomusMind-issued JWT and refresh-token module. That module is not an OAuth authorization server or OIDC provider.
- `AuthUser` and household `FamilyMember` remain separate identities. A member can lack an account, and a user-to-household grant is persisted separately.
- `IFamilyAuthorizationService` and the application handlers are the domain authorization boundary. MCP middleware and tools must not replace their checks.
- Existing application authorization is family-scoped, with additional role and target-member rules in some handlers. A token scope is consent, not membership or role authority.
- The read-only proof in #228 is test-host-only and deliberately leaves the production MCP catalog empty. This plan does not turn it into a remote release.
- The assessment in [`issue-226-mcp-architecture-assessment.md`](../assessments/issue-226-mcp-architecture-assessment.md) recommends an in-host MCP endpoint as the starting topology, while leaving the provider/authorization-server choice open.

## Design

### Resource-server boundary

1. Host the remote MCP resource at the existing API boundary unless the threat or deployment assessment proves a separate deployable necessary.
2. Use Streamable HTTP for remote MCP transport. Keep local STDIO credentials and process isolation as a later, separate integration profile.
3. Publish protected-resource metadata for the exact MCP resource URI. Advertise authorization-server metadata using RFC 8414 and/or OIDC discovery, with no hand-maintained client assumptions in the tool layer.
4. Require an HTTPS origin in non-development environments and reject requests that do not present a valid bearer token for this resource.

### Authorization-server and client flow

1. Use Authorization Code with PKCE S256 for interactive clients.
2. Use exact redirect-URI matching, state/nonce validation where applicable, authorization-code single use, short authorization-code lifetime, and explicit consent for MCP scopes.
3. Prefer Client-Initiated Metadata Documents for clients that support it; support pre-registration where required; consider Dynamic Client Registration only after interoperability testing and with policy controls.
4. Do not promise identical behavior across ChatGPT, Claude, Claude Code, local STDIO clients, and custom agents. Record the tested client matrix in the implementation evidence.
5. Require the exact canonical MCP `resource` value in authorization and token requests, bind it into the issued access token, and reject missing, repeated, alternate, or mismatched resource values. An `aud` claim alone is not an acceptable fallback.

### Decision: in-process OpenIddict authorization server

For this child, select an authorization server hosted in-process in `DomusMind.Api`, using OpenIddict's ASP.NET Core server and validation integrations with its Entity Framework Core stores. OpenIddict is a .NET OAuth 2.0/OpenID Connect server framework with documented Authorization Code + PKCE, token storage/revocation, and ASP.NET Core integration. The existing `AuthUser` remains the authenticated resource owner; OpenIddict authorization, token and consent records are infrastructure records and do not become domain aggregates.

This decision keeps the first remote integration self-hostable, avoids a new identity-provider account-linking dependency, allows the resource server to validate token entries and current household grants in the same application boundary, and preserves the accepted in-process MCP hosting direction. It does add authorization-server security and operational responsibility to DomusMind, so production enablement requires threat review, key rotation, operator backup/recovery, and a separate ADR before credentials are used.

Rejected for this slice:

- an external provider as the default, because it adds account linking, deployment/provider availability, household-consent ownership, and self-hosted operation dependencies before DomusMind has a provider abstraction;
- a custom OAuth/OIDC implementation, because standards behavior should remain in a maintained protocol framework;
- ZITADEL as the initial contract, because its current documentation says RFC 8707 `resource` is accepted but ignored for some flows, which conflicts with this resource-bound MCP boundary unless an additional provider-specific audience contract is introduced.

The implementation must pin a currently supported OpenIddict version after dependency review and record the exact version and configuration in an ADR. No provider credentials or production keys belong in this proposal.

### Identity and domain authorization

1. Validate issuer, signature, audience/resource, `sub`, `exp`, `nbf`, `iat` where required, token type, and scopes at the resource boundary.
2. Resolve the external subject to an existing DomusMind `AuthUser`; do not manufacture a household member or trust user/member/household identifiers supplied by an AI client.
3. Bind each authorization grant and token to exactly one `(AuthUser, client, MCP resource, household, scope-set)` tuple. If a user belongs to multiple households, selecting household B requires a new explicit consent transaction; an already-issued household-A token cannot switch context through a tool argument or prompt.
4. Re-check the current user-to-household grant and application handler authorization for every operation. Preserve member-level checks and do not expose a query whose handler can accept a member identifier from another household without proving membership ownership.
5. Keep read and write scopes distinct. This child may authorize only read scope semantics; no write tool is enabled by its completion.
6. Use a five-minute MCP access-token lifetime and rotating refresh tokens with a bounded thirty-day maximum lifetime, subject to operator configuration. Enable OpenIddict token-entry validation for the in-process resource server so consent withdrawal, token revocation, account disable, and grant deletion deny the next request immediately; each call also rechecks live household/member access. Document and test the revocation bound as "before the next MCP request completes authorization".

### Security and audit controls

- Never accept or forward an access token intended for another resource.
- Never emit bearer tokens, refresh tokens, authorization codes, PKCE verifiers, client secrets, or raw authorization headers in logs, traces, errors, or audit payloads.
- Return safe `401`/`403` challenges that do not disclose whether another household or member exists.
- Add rate limits and bounded request sizes at the MCP boundary; preserve cancellation and timeouts.
- Record an operation audit entry with timestamp, correlation, authenticated user, client identifier, household context, requested operation/scope, authorization decision, result class, and failure reason category. Keep payload and secret minimization explicit.
- Use OpenTelemetry correlation without treating traces as the durable audit store.
- Threat-model phishing/redirect abuse, confused deputy, token substitution, audience confusion, prompt injection through untrusted household content, replay, enumeration, consent fixation, and authorization changes after token issuance.

## Ordered delivery tasks

1. **Authorization-server decision record**: record the selected in-process OpenIddict server/validation direction, its `AuthUser` mapping, EF Core infrastructure records, key-management boundary, token-entry validation, and rejected external-provider/custom-server alternatives. No production secrets.
2. **Protected-resource contract**: define the exact MCP resource URI, require it in authorization/token requests, publish RFC 9728 metadata and RFC 8414/OIDC discovery, define issuer/audience rules, error/challenge contract, TLS and deployment assumptions, and development test configuration.
3. **Resource-server validation**: add isolated token validation for MCP requests, with issuer/audience/resource and scope checks distinct from local API JWT validation. Keep the existing API authentication behavior unchanged.
4. **User and household authorization bridge**: map trusted OIDC subjects to `AuthUser`, resolve household access server-side, preserve handler authorization, distinguish read/write scopes, and test membership/role revocation without token passthrough.
5. **Consent and household binding**: implement explicit household selection and consent records bound to the client/resource/scope tuple; prove that a user with access to households A and B cannot reuse an A grant for B, and prove next-request denial after consent, account, or membership revocation.
6. **Client interoperability and metadata safety**: exercise at least one remote interactive client, one Claude-compatible client path, and a custom test client; use pre-registration as the default. If CIMD is enabled for a client, fetch only HTTPS metadata with bounded size/time, no credentials, no redirects, and DNS-resolution/connect checks that reject loopback, link-local, private, multicast, reserved and rebinding destinations; test every rejection. Keep STDIO separate.
7. **Security, audit, and operational hardening**: implement redacted security logs/audit records, rate limiting, safe errors, replay/revocation checks, OpenTelemetry correlation, threat tests, and operator documentation.

## Test plan

### Unit and application tests

- issuer, signature, `iss`, `aud`/resource, lifetime, token type and scope validation;
- required canonical `resource` on authorization and token requests, exact token binding, alternate-resource rejection and no audience-only fallback;
- exact redirect URI, state, PKCE S256, authorization-code replay and client binding;
- identity mapping to an existing `AuthUser`, unknown/disabled subject denial, and no member fabrication;
- family grant, role, target-member and cross-household authorization decisions;
- read/write scope separation, insufficient-scope challenges, and token/resource mismatch;
- refresh rotation, revocation, consent withdrawal, membership removal and authorization-version behavior; assert denial before the next MCP request completes authorization;
- multi-household consent binding: an A-grant cannot access B, even when the same user belongs to both households;
- audit field allowlist and secret/token redaction.

### Integration and contract tests

- protected-resource metadata and authorization-server discovery documents;
- safe 401/403 responses and `WWW-Authenticate`/metadata references;
- remote Streamable HTTP MCP handshake and authorized read-only tool call;
- cross-household/member denial with no data disclosure or side effects;
- client registration/consent paths selected by the ADR;
- OpenIddict token-entry validation and immediate next-request denial after token/grant/revocation changes;
- CIMD metadata SSRF controls, redirect rejection, private-address rejection, DNS-rebinding defense, bounded response and timeout behavior;
- rate-limit, timeout, cancellation and replay behavior.

### Security review evidence

- final threat model and residual-risk register;
- exact tested client matrix and unsupported-client notes;
- no token passthrough proof and log/trace/audit redaction review;
- review of the `GetMemberAgendaQueryHandler` family/member boundary before any member-specific capability is exposed.

## Scope boundaries

Included: remote MCP resource protection, OAuth 2.1/OIDC integration direction and selected implementation, discovery metadata, PKCE/consent, audience-bound tokens, read/write scope taxonomy, current household/member authorization, revocation semantics, security audit and interoperability evidence.

Excluded: enabling write-capable tools, adding new business capabilities, changing household/member domain ownership, accepting existing local API JWTs as MCP tokens, token passthrough, LLM/agent orchestration, provider-specific UI beyond the selected consent flow, and local STDIO credentials except documenting their separate boundary.

## Definition of Done for this plan

- All #229 acceptance criteria are covered by a verified implementation and evidence.
- Authentication, OAuth grant/consent, and DomusMind domain authorization are separately testable decisions.
- Existing API authentication and application handler ownership remain intact.
- No production write tool is enabled by this child.
- Security review explicitly covers confused deputy, audience/resource binding, revocation, cross-household access, prompt injection, and audit redaction.
- The selected provider/authorization-server direction is documented in an ADR, with unresolved risks and client limitations visible.
- CI, security analysis, contract/integration tests, and documentation validation pass on the final head.

## References

- [MCP authorization](https://modelcontextprotocol.io/specification/2026-07-28/basic/authorization)
- [MCP security best practices](https://modelcontextprotocol.io/docs/2026-07-28/tutorials/security/security_best_practices)
- [RFC 9728: OAuth 2.0 Protected Resource Metadata](https://www.rfc-editor.org/rfc/rfc9728.html)
- [RFC 8414: OAuth 2.0 Authorization Server Metadata](https://www.rfc-editor.org/rfc/rfc8414.html)
- [RFC 7636: PKCE](https://www.rfc-editor.org/rfc/rfc7636.html)
- [RFC 8707: Resource Indicators for OAuth 2.0](https://www.rfc-editor.org/rfc/rfc8707.html)
- [OpenID Connect Core](https://openid.net/specs/openid-connect-core-1_0.html)
- [OAuth 2.1 draft](https://datatracker.ietf.org/doc/draft-ietf-oauth-v2-1/)
- [Official .NET MCP SDK](https://github.com/modelcontextprotocol/csharp-sdk)
- [OpenIddict introduction](https://documentation.openiddict.com/introduction)
- [OpenIddict ASP.NET Core integration](https://documentation.openiddict.com/integrations/aspnet-core)
- [OpenIddict token storage and revocation](https://documentation.openiddict.com/configuration/token-storage.html)
- [OpenIddict PKCE enforcement](https://documentation.openiddict.com/configuration/proof-key-for-code-exchange)
- [ZITADEL resource indicator limitation](https://zitadel.com/docs/guides/integrate/token-exchange)
