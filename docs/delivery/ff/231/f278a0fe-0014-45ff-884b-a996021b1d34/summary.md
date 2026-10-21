# FF run f278a0fe-0014-45ff-884b-a996021b1d34

- Work item: #231
- Branch: `feat/231-confirmed-mcp-household-writes`
- Proposal head: `9cafa64e45eaa747c357d4a3277072abf1afaeaa`
- Implementation head: `53623d6`
- Draft PR: [#272](https://github.com/juangcarmona/domusmind/pull/272)
- Status: awaiting integration

The issue reached Ready, the proposal artifacts were created, and the independent proposal review approved the final head after three bounded correction rounds. The approved pilot is implemented on the child branch as a confirmed `list_item_add` MCP write. The implementation keeps MCP thin, reuses the existing list command, requires `mcp.write`, binds approval and execution to household/list authorization, supports explicit confirmation, idempotency, expiry, cancellation, bounded result retention, post-commit notification, structured outcomes, an explicit production invocation boundary for untrusted tool arguments, and attributable approval/idempotency audit outcomes including malformed-call rejection events.

Verification on `53623d6`:

- Release build: passed.
- Release tests: 890 passed, 5 skipped PostgreSQL integration tests because `DOMUSMIND_TEST_POSTGRES` is not enabled in this environment. The two new skips are the real list-handler/event-log success and rollback transaction tests.
- Hosted backend CI: passed, including the PostgreSQL-backed persistence tests ([run 39521465654](https://github.com/juangcarmona/domusmind/actions/runs/39521465654)). The isolated PostgreSQL fixtures now apply EF migrations instead of relying on `EnsureCreatedAsync`, which is not reliable when each test uses a separate schema in one database.
- Markdown lint: 0 issues.
- `git diff --check`: passed.
- EF pending-model check: no changes since the last migration.
- Local production PostgreSQL and remote HTTPS write journey: not exercised; the environment has no enabled PostgreSQL integration gate or deployed MCP endpoint.

The stacked Draft PR targets the Epic branch rather than `main`, so normal pull-request workflows do not auto-report checks. Manual backend, documentation, and CodeQL workflow dispatches passed ([backend run 39521465654](https://github.com/juangcarmona/domusmind/actions/runs/39521465654), [CodeQL run 39521468558](https://github.com/juangcarmona/domusmind/actions/runs/39521468558)).

The final independent Done-gate review approved `b32d831` with no P0/P1 findings. The run is awaiting the separate protected integration gate; PR #272 remains Draft and stacked on the Epic branch. Non-blocking P2s are retained: malformed-call audit identity resolution can throw when the accessor is scoped, and the deployed HTTPS approval/write journey remains unexercised.

Known inherited warnings remain documented rather than silently treated as resolved: Kiota has a high-severity package advisory, MessagePack has a moderate-severity advisory, and the EF tool is older than the runtime. The next independent done-gate review is the next FF step; integration remains a separate protected gate.
