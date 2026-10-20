# Fast Forward run: issue #227

- Run ID: `58591b28-0424-4eab-b1a9-9dc341adfad1`
- Status: running
- Branch: `feat/227-establish-dotnet-mcp-adapter-foundation`
- Starting commit: `320526d4e3e49e5358d571e09ff50fe7a01e6086`
- Executor: GPT-5 (active Codex model)
- Independent reviewer: `gpt-6.1-sol`
- Correction limit: 3 rounds per gate
- Parent Epic: [#224](https://github.com/juangcarmona/domusmind/issues/224)
- Work item: [#227](https://github.com/juangcarmona/domusmind/issues/227)

## Stage evidence

- `refine`: Ready. Issue #227 names affected household members, product owner/maintainers, and external MCP client users; records accepted #226 host direction; defines exclusions and six verifiable criteria.
- `propose`: Approved at `235e5e687b42f0f84344051e785312184483e375` by independent reviewer `gpt-6.1-sol` after one correction round. The revised plan accounts for Application's transitive EF/Domain graph and tests actual API route composition with SPA fallback. Authorization-server placement remains open for #229.
- `implement`: The first independent audit requested changes on head `76a3fcc`: it reproduced acceptance of path-bearing Origin values, requested real cancellation/bounds/down-level protocol tests, and identified missing CI-linked evidence. Corrections are committed locally but the independent follow-up audit and branch CI runs are pending. Fixes require exact raw-Origin/authority matching; tests cover slash/dot-segment/query Origins, actual cancellation propagation, blank/over-limit input rejection, the current protocol and `2025-11-25` handshake; SDK hybrid session mode preserves compatibility. Full local Release solution build passed; all 858 backend tests passed with PostgreSQL enabled (API 24, Domain 250, Application 525, Infrastructure 59). Web app lint passed with 44 existing warnings, 62 Vitest tests passed, web build passed; public-site build passed. Arc42 frontmatter and Markdown lint (29 docs files) passed; OpenSpec validation passed (10/10), ProductShape validation passed with 59 existing low-confidence draft warnings, integration check passed, and OpenSpec citations passed (816 current, no stale/tampered/unresolved). ProductShape architecture citations remain current (81; 59 pre-existing warnings). Live-container evidence is in [runtime.md](runtime.md): valid `tools/list` returned 200 with an empty catalog, malformed Origin returned 403, current-protocol GET returned 405 rather than SPA content, and REST ping remained 200.
- Worktree setup: dedicated worktree created from the updated Epic branch `feat/224-mcp-household-assistants` at `320526d`; the parent branch is current and has no branch protection configured (GitHub returned 404 “Branch not protected”). No local secrets or untracked configuration were needed.

## Scope boundary

Production MCP still exposes no household data or tools. The adapter keeps direct project references away from Infrastructure, Domain and EF Core; Application's transitive EF/Domain graph is explicitly disclosed. The separate OAuth/OIDC authorization slice remains a prerequisite for any production capability. Pre-existing NU1902/NU1903 package advisories and Docker/frontend build advisories remain visible; they did not fail verification.

## Independent audit

- Audit 1: `gpt-6.1-sol` requested changes on `76a3fcc66b92896da0e6b496cb4dfd4f3354327c`. It reproduced malformed Origin normalization and identified missing cancellation, bounded-input, down-level protocol, and CI/evidence proof. It found no production tools/data exposure and confirmed the dependency-boundary tests and actual route composition. The implementation corrections and added tests are pending follow-up review.
- Audit 2: pending on the corrected, pushed head. Existing high/moderate .NET dependency warnings and repository Dependabot alerts were not introduced by `DomusMind.Mcp`; they remain an owner security-disposition item under the repository DoD.
