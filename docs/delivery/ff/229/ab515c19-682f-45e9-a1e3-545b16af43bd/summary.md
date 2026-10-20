# Fast Forward run: issue #229

- Run ID: `ab515c19-682f-45e9-a1e3-545b16af43bd`
- Status: running; refinement passed and the corrected proposal is approved; implementation is starting
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

Independent proposal review round 2 approved exact corrected plan head `000ae3971a9e45c0204ef359631fb0e45689884d`. The implementation stage can begin; the Draft PR remains documentation-only until implementation work is added.
