# Fast Forward run: issue #228

- Run ID: `26ee8aa4-c1ba-4e3e-8137-ee82d6608c9a`
- Status: awaiting Juan's owner-only Done judgements; do not integrate or start #229 yet
- Branch: `feat/228-prove-readonly-mcp-capabilities`
- Starting commit: `1ce14ac910ec2ce6136c604eb699af74ef6e718b`
- Executor: GPT-5 (active Codex model)
- Independent reviewer: `gpt-6.1-sol`
- Correction limit: 3 rounds per gate
- Parent Epic: [#224](https://github.com/juangcarmona/domusmind/issues/224)
- Work item: [#228](https://github.com/juangcarmona/domusmind/issues/228)

## Refinement

Issue #228 passed Ready after adding explicit stakeholders, the synthetic-only boundary, the #229 production-auth dependency, clear acceptance evidence, and the member-agenda security gap. The current handlers for family timeline and member-directory reads check family access. `GetMemberAgendaQueryHandler` does not establish that its requested member belongs to the authorized family, so the plan excludes it. The current Tasks application surface has no complete pending-task list query; this is deferred rather than approximated from agenda data.

Pickup WIP check: zero open `status:in-progress` and zero open `status:in-review` items; configured cap is 3. Review debt for #226 and #227 was reported and does not block.

## Proposal

Plan: [`issue-228-readonly-mcp-proof.md`](../../../plans/issue-228-readonly-mcp-proof.md). Client contract: [`issue-228-mcp-readonly-proof.md`](../../../contracts/issue-228-mcp-readonly-proof.md). They specify test-host-only `agenda_get` backed by `GetFamilyTimelineQuery` and a minimal `household_members_list` backed by `GetFamilyMembersQuery`, with synthetic fixture identity, no user-supplied household/member identity, and an empty production tool catalog. Draft PR #269 contains the approved plan, these implementation tests, and sanitized transcript evidence; it remains Draft pending security/CI evidence and owner DoD judgements.

The first independent plan audit requested changes at head `6fb3fe1ecd65db499dc23198f0c6eedbbc13dec8`. Its three findings required actual handler integration with two-household synthetic isolation, exact date and duplicate-name behavior, and a named client-facing contract with schema parity tests. Correction round 1 added those constraints. The second audit at head `7103b8c4c6589793205e4b39ca4c001845dc0f5e` closed the first two and requested complete schemas plus an explicit timeline field allowlist. Correction round 2 defined both tools' full input/output schemas, minimized DTO-derived fields, and output validation/parity requirements. Independent plan review round 3 approved exact head `62b975a93916a0c18904d6475e9541cb5d2231af` before implementation.

## Implementation

Draft PR #269 now contains the test-only MCP adapter, actual Application-handler integration over isolated synthetic persistence, and sanitized transcript evidence. Production registration remains empty. The focused suite passes 9/9; Release build passes; Release solution tests pass API 33, Application 525, Domain 250, and Infrastructure 56, with 3 PostgreSQL integration tests skipped because `DOMUSMIND_TEST_POSTGRES` is unset. Existing Kiota (high) and MessagePack (moderate) advisories remain deferred under the approved #227 exception.

Independent implementation review round 1 at `713d100526a4ad22a891164b6a58400445e06d14` requested two P2 corrections: force invariant date formatting and prove it under a non-Gregorian server culture; prevent persistence writes after seeding, including committed writes. Correction round 1 added the invariant-culture scope, a `th-TH` request test with Gregorian output assertions, and a SaveChanges interceptor that rejects writes. Round 2 approved exact code head `330568b0e9d5d3c679b83cbdd5712ed8b49e4efb`; its decision is recorded in PR comment [#6294774341](https://github.com/juangcarmona/domusmind/pull/269#issuecomment-6294774341).

The separate criterion-by-criterion DoD audit found all seven issue acceptance criteria satisfied. After its initial audit, the remaining deterministic evidence was completed on code head `330568b0e9d5d3c679b83cbdd5712ed8b49e4efb`:

- [Backend CI](https://github.com/juangcarmona/domusmind/actions/runs/39450868459) succeeded, including PostgreSQL-backed tests, Release build, and API publish.
- [CodeQL](https://github.com/juangcarmona/domusmind/actions/runs/39450868200) succeeded for C# and JavaScript/TypeScript. No open code-scanning alerts or repository secret-scanning alerts were observed.
- Explicit Markdown lint of the four Product Change delivery artifacts succeeded with no findings; JSON evidence and `git diff --check` passed.
- NuGet audit confirms inherited Kiota (high) and MessagePack (moderate) advisories. GitHub Dependabot still has inherited open http-cache-semantics (high) and KaTeX (low) alerts. Their deferral was explicitly accepted when the owner approved #227; this feature adds no production dependency.

Local Release verification passed 864 tests; 3 PostgreSQL tests were skipped locally because `DOMUSMIND_TEST_POSTGRES` was unset, and all relevant PostgreSQL tests ran and passed in the linked backend CI run. The owner-only Done judgements remain: acceptance/completeness, test adequacy, security exposure, architecture conformance, documentation/spec consistency, self-review, and known limitations. PR #269 remains Draft pending those judgements. Do not integrate it into the Epic branch, mark #228 Done, or start #229 until the owner approves.
