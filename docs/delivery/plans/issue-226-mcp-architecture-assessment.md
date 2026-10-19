# Issue #226: MCP architecture and identity assessment

## Outcome

Produce an evidence-based assessment of MCP hosting, identity, authorization, and client interoperability that lets the product owner choose a safe foundation before household data or capabilities are exposed. This plan and its resulting assessment do not accept an architecture decision or implement functionality.

## Context and repository evidence

- DomusMind is documented as a domain-centric modular monolith with one API process and one database. The AppHost is local development orchestration; the self-hosted Compose deployment exposes one API container behind an operator-provided TLS reverse proxy.
- Accepted ADR-0002 keeps built-in local authentication inside the API deployment and deliberately separates an authenticable `User` from a household `Member`; it leaves a future federation path open but does not select an external OAuth authorization server.
- The API currently configures local JWT authentication and exposes application-level security abstractions. The assessment will verify their behavior against the source and tests, not infer authorization guarantees from names or comments.
- Initial inspection found a possible documentation/implementation mismatch: `IFamilyAuthorizationService` comments describe a deny-all placeholder, while `FamilyAuthorizationService` queries persisted `UserFamilyAccess` grants. Confirm the current wiring, callers, migration history, and effective household/member boundaries.
- The Epic’s Product Change remains draft. Its existing open identity and host questions stay open unless the product owner explicitly decides them.

Relevant source: `docs/architecture/05-building-block-view.md`, `06-runtime-view.md`, `07-deployment-view.md`, `08-crosscutting-concepts.md`, `11-risks-and-technical-debt.md`; accepted `docs/adr/0002-keep-authentication-local-and-separate-from-member-identity.md`; `src/backend/DomusMind.Api/Program.cs`; `src/backend/DomusMind.Infrastructure/Auth/`; `src/backend/DomusMind.Application/Abstractions/Security/`; and the corresponding backend tests.

## Proposed approach

Create one reviewable assessment document under `docs/delivery/assessments/` with:

1. Repository findings, evidence paths, and discrepancies/gaps.
2. A comparison of MCP-in-the-existing-API-host and a separately deployable adapter, assessed against self-hosting, attack surface/isolation, standards compliance, operations, and scaling. Distinguish documented facts from assumptions.
3. A trust-boundary and responsibility map distinguishing user authentication, OAuth grant/consent, MCP resource-server token validation, household/member domain authorization, and application command/query authorization.
4. Options for integrating an OAuth 2.1/OIDC authorization server with current identity, including a standards-based external provider and any bounded local development alternative. Do not implement an authorization server or decide the provider.
5. Explicit analysis of authorization-code flow with PKCE, MCP protected-resource and authorization-server metadata discovery, client registration approaches, audience/resource binding, read/write scopes, token expiry/refresh/revocation, current membership checks, no token passthrough, confused-deputy defenses, audit, and narrow optional PATs for trusted local development.
6. A client compatibility matrix for ChatGPT custom MCP integrations, Claude.ai, Claude Code, generic local STDIO clients, and private self-hosted remote clients. Record tested/documented product surfaces and date, distinguish normative protocol behavior from client-specific support, and avoid claiming unsupported paths.
7. A recommendation matrix with benefits, costs, risks, dependencies, migration risks, and the owner decision needed. Recommendations remain proposals; do not author an Accepted ADR or modify accepted architecture docs.

Use current official MCP, OAuth/OIDC, .NET SDK, OpenAI, and Anthropic documentation. Where client/product requirements are volatile, record the access date and source URL. Explain unresolved choices rather than picking defaults silently. Initial live lookup on 2026-10-19 confirms that the unversioned MCP specification resolves to revision `2026-07-28` and the OAuth 2.1 Internet-Draft is revision `-16` (last updated 2026-09-02); verify both again when producing the assessment.

## Boundaries and exclusions

- No application code, package changes, provider configuration, database migration, host topology change, credentials, or production exposure.
- No accepted ADR or edits to the canonical architecture, ProductShape model, or domain specifications.
- No assumption that the existing local JWT tokens are suitable as OAuth access tokens for a remote MCP resource.
- No authorization based only on an MCP scope, token claim, client confirmation UI, or a client-supplied household identifier; effective domain authorization must be evidenced.
- No token passthrough from an external identity provider to downstream tools or integrations.
- No external client is marked supported solely because it implements MCP generally; client surface, plan, auth flow, network reachability, and transport are considered separately.

## Ordered tasks

1. Trace API authentication setup, local JWT issue/validation, refresh rotation and revocation, user/member linkage, family grant creation/lookup, and representative family-scoped query/command authorization. Read the relevant tests and record discrepancies.
2. Trace self-hosted Compose, reverse-proxy forwarded-header trust/host allowlisting, local AppHost, API composition, and current single-container exposure constraints.
3. Check the current MCP specification and official .NET SDK for remote transport, OAuth resource-server duties, metadata discovery, client registration, and security requirements; distinguish normative statements from guidance.
4. Check current OAuth 2.1/OIDC and RFC 9728/RFC 8414/PKCE requirements, then compare provider/authorization-server options without choosing one.
5. Check current OpenAI and Anthropic client documentation and build the client/profile compatibility matrix; explicitly record plan, reachability, and local-vs-remote constraints.
6. Synthesize trust boundaries, options, gaps, migration/security risks, recommendations, required owner decisions, and implications for children #227–#234 and Product Change `CHG-MCP-HOUSEHOLD-INTEGRATION-001`.
7. Validate links, citations, Markdown, product/architecture integrity as applicable, and confirm the diff contains documentation only.

## Test and verification impact

- Documentation-only change: no application tests are added. Confirm the configured backend, web, and documentation CI workflows run on the PR and pass; report code-test suites as not applicable to changed behavior, not as unrun local passes.
- Run the repository’s applicable Markdown and ProductShape/citation/architecture validation commands as derived from current CI and documented tooling.
- Review every statement against a repository path or an authoritative linked source. Check links and the final diff for credentials, private tokens, and unsupported claims.
- Compare the branch with its actual base before verification; rerun all applicable checks after any correction.

## Definition of Done items

- Issue #226 acceptance criteria are demonstrated by the assessment and evidence links.
- No application behavior or unrelated initiative is changed.
- Documentation findings distinguish verified code behavior, canonical intent, external normative requirements, client-specific behavior, recommendations, and owner decisions.
- Applicable repository documentation/product integrity checks and the PR CI checks pass; no unavailable check is reported as passing.
- Security/privacy risks, limitations, and remaining decisions are explicit.
- The named developer/owner reviews the resulting assessment and decides whether to accept, reject, or defer its recommendations. This judgement is not substitutable in the current lifecycle configuration.

## Review gate and dependencies

- Parent: Epic #224.
- Product intent: draft Product Change `CHG-MCP-HOUSEHOLD-INTEGRATION-001` and Draft PR #225.
- Issue #226 has no prerequisite child issue. Its accepted recommendations or unresolved decisions gate #227 and later delivery slices as stated by those issues.
- Plan review must be independent and target this plan commit before any assessment artifact is implemented.
- Implementation must stop before integration until the owner decides the assessment’s Definition-of-Done judgements and any architecture choice needed by downstream work.

## Initial references

- [MCP specification (current revision)](https://modelcontextprotocol.io/specification/2026-07-28)
- [MCP authorization (current revision)](https://modelcontextprotocol.io/specification/2026-07-28/basic/authorization)
- [MCP transports (current revision)](https://modelcontextprotocol.io/specification/2026-07-28/basic/transports)
- [MCP security best practices](https://modelcontextprotocol.io/docs/2026-07-28/tutorials/security/security_best_practices)
- [Official .NET MCP SDK](https://github.com/modelcontextprotocol/csharp-sdk)
- [OAuth 2.1 Internet-Draft](https://datatracker.ietf.org/doc/draft-ietf-oauth-v2-1/)
- [OpenID Connect Core](https://openid.net/specs/openid-connect-core-1_0.html)
- [RFC 9728: OAuth Protected Resource Metadata](https://www.rfc-editor.org/rfc/rfc9728.html)
- [RFC 8414: OAuth Authorization Server Metadata](https://www.rfc-editor.org/rfc/rfc8414.html)
- [RFC 7636: PKCE](https://www.rfc-editor.org/rfc/rfc7636.html)
- [OpenAI custom MCP authentication guidance](https://developers.openai.com/plugins/build/auth)
- [OpenAI custom MCP server guide](https://developers.openai.com/plugins/build/mcp-server)
- [Anthropic custom remote MCP connectors](https://support.anthropic.com/en/articles/11503834-building-custom-integrations-via-remote-mcp-servers)
- [Claude Code MCP documentation](https://code.claude.com/docs/en/mcp)
- [OAuth 2.1 current Internet-Draft](https://datatracker.ietf.org/doc/draft-ietf-oauth-v2-1/)
