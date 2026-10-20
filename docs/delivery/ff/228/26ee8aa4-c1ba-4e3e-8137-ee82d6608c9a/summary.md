# Fast Forward run: issue #228

- Run ID: `26ee8aa4-c1ba-4e3e-8137-ee82d6608c9a`
- Status: running — awaiting independent plan review
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

Plan: [`issue-228-readonly-mcp-proof.md`](../../../plans/issue-228-readonly-mcp-proof.md). It proposes test-only `agenda_get` backed by `GetFamilyTimelineQuery` and a minimal `household_members_list` backed by `GetFamilyMembersQuery`, with synthetic fixture identity, no user-supplied household/member identity, and an empty production tool catalog. No implementation has started. The plan PR and independent review decision will be added here before implementation begins.
