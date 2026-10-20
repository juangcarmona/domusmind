# Fast Forward run: issue #230

- Run ID: `9dc8d0df-31bc-4923-b826-dba85d57134e`
- Status: running; refinement, proposal, and corrected implementation passed; independent Done review is next
- Branch: `feat/230-release-readonly-mcp-workflows`
- Starting commit: `629a9431685d82770cb5b7e268845f78029c7d0c`
- Executor: GPT-5 (active Codex model)
- Independent reviewer: `gpt-6.1-sol`
- Correction limit: 3 rounds per gate
- Parent Epic: [#224](https://github.com/juangcarmona/domusmind/issues/224)
- Work item: [#230](https://github.com/juangcarmona/domusmind/issues/230)

The run is isolated in the dedicated issue-230 worktree. Refinement passed against the live issue and repository evidence. The proposal was corrected once and independently approved at exact plan head `dbec35e`. Pending Tasks and member-specific agenda remain explicit exclusions.

## Implementation evidence

- Implementation correction commit: `8d26128`.
- Production release is gated by `Mcp:Capabilities:HouseholdReadOnly=true` in addition to enabled OAuth and required OAuth protection; the default catalog remains empty.
- Production tools are `agenda_get` and `household_members_list` only. They dispatch existing application queries and return minimized structured contracts.
- Focused MCP/OAuth/composition suite: 34 passed.
- Full backend suite: 881 passed, 3 expected PostgreSQL integration tests skipped.
- Web app lint/test/build passed with 0 errors and inherited warnings; public-site build passed; Markdown validation passed.
- Live loopback Kestrel HTTPS test completed protected-resource metadata discovery, OAuth PKCE, resource-bound token exchange, official .NET MCP client discovery, and Streamable HTTP tool discovery.
- The SDK version used here does not expose a generic registered-tool call filter; per-tool fixed-name audit events are emitted at the two production tool boundaries and documented in the implementation plan.

The first independent Done review requested four bounded corrections: invariant date projection, real application-backed HTTPS evidence for both production tools and revoked membership states, production-path rather than proof-only test coverage, and refreshed completion evidence. Those corrections were applied in `8d26128`; five workflows succeeded against that exact implementation head:

- [Backend CI](https://github.com/juangcarmona/domusmind/actions/runs/39466842452)
- [Docs CI](https://github.com/juangcarmona/domusmind/actions/runs/39466842041)
- [Web app CI](https://github.com/juangcarmona/domusmind/actions/runs/39466842174)
- [Public site CI](https://github.com/juangcarmona/domusmind/actions/runs/39466842238)
- [CodeQL](https://github.com/juangcarmona/domusmind/actions/runs/39466842243)
