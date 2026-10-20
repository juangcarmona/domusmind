# Fast Forward run: issue #229

- Run ID: `ab515c19-682f-45e9-a1e3-545b16af43bd`
- Status: running; refinement and proposal passed; implementation correction round 1 is complete pending independent Done audit and owner judgement
- Branch: `feat/229-oauth-oidc-mcp-authorization`
- Starting commit: `3e3501e7127618d2ecd48e95608917e03c5a421b`
- Executor: GPT-5 (active Codex model)
- Independent reviewer: `gpt-6.1-sol`
- Correction limit: 3 rounds per gate
- Parent Epic: [#224](https://github.com/juangcarmona/domusmind/issues/224)
- Work item: [#229](https://github.com/juangcarmona/domusmind/issues/229)

## Refinement

Issue #229 passed Ready after confirming the current identity and authorization seams. The existing local JWT is not treated as an MCP OAuth token; `AuthUser`, household membership/grants, and application handlers remain authoritative. Provider selection is intentionally left for the proposal/ADR after interoperability and self-hosting evidence. Write tools remain excluded.

## Proposal

Plan: [`issue-229-mcp-oauth-authorization.md`](../../../plans/issue-229-mcp-oauth-authorization.md). It selects in-process OpenIddict in `DomusMind.Api`, defines the protected-resource contract, OAuth Authorization Code with PKCE, OIDC discovery, mandatory resource binding, household-bound consent, immediate next-request revocation checks, server-side identity and household authorization, audit, threat testing, and client interoperability. No application functionality or credentials are included in this proposal.

The independent review of the first proposal head `c151e6a5ec1224e1c6019925e3b31d69d1ac44e0` requested four corrections: provider/placement, consent/revocation contracts, mandatory resource binding, and CIMD SSRF/DNS-rebinding defenses. Correction round 1 addressed all four at `000ae3971a9e45c0204ef359631fb0e45689884d`; the corrected plan then passed independent review.

Independent proposal review round 2 approved exact corrected plan head `000ae3971a9e45c0204ef359631fb0e45689884d`.

## Implementation

Commits `524ee88`, `986f27e`, `2789e92`, `03ac001`, and `f6fb11d` add the in-process OpenIddict 7.7.2 server/validation boundary, EF Core authorization/token migration, proposed ADR-0012, protected-resource and OIDC discovery endpoints, exact resource registration, Authorization Code + PKCE configuration, household-bound authorization claims, an explicit antiforgery-protected consent form, complete authorization-code and refresh-token endpoints, grant-specific consent/token revocation, current household checks, exact audience enforcement, structured security audit events, MCP rate limiting, and MCP bearer enforcement. The existing local API JWT remains separate; no production MCP tools or writes are enabled.

Verification evidence:

- `dotnet restore DomusMind.slnx`: passed with inherited Kiota and MessagePack advisories.
- `dotnet build DomusMind.slnx --no-restore --configuration Release`: passed, 0 errors.
- `dotnet test DomusMind.slnx --no-build --configuration Release`: passed, 878 passed and 3 pre-existing PostgreSQL integration tests skipped.
- Focused OAuth contract suite: 14 passed, including end-to-end login, explicit consent, PKCE exchange, refresh rotation/reuse, real Streamable HTTP MCP transport, resource/audience, expiry, insufficient scope, current account/membership denial, replay, rate limiting, and revocation.
- Markdown lint for ADRs and the #229 plan: passed.
- Product citation verification: 0 errors; 59 pre-existing low-confidence draft warnings.

The independent Done audit and owner-only Done judgement remain before the branch can be merged into the Epic branch. GitHub has no workflow check runs configured for this child head; local final-head evidence is recorded above.
