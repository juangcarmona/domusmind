# Fast Forward run: issue #229

- Run ID: `ab515c19-682f-45e9-a1e3-545b16af43bd`
- Status: running; refinement passed and proposal is being independently reviewed
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

Plan: [`issue-229-mcp-oauth-authorization.md`](../../../plans/issue-229-mcp-oauth-authorization.md). It defines the protected-resource contract, OAuth Authorization Code with PKCE, OIDC discovery, resource/audience binding, server-side identity and household authorization, revocation, audit, threat testing, and client interoperability. No application functionality or credentials are included in this proposal commit.
