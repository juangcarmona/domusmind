# FF run f278a0fe-0014-45ff-884b-a996021b1d34

- Work item: #231
- Branch: `feat/231-confirmed-mcp-household-writes`
- Proposal head: `9cafa64e45eaa747c357d4a3277072abf1afaeaa`
- Final branch head: `18b4acbdd682f813cdea097fa49e849716c226c8` (implementation `4983052cf6eb93c132b4f6dea53111ef959d53c4` plus evidence/test hardening).
- Draft PR: [#272](https://github.com/juangcarmona/domusmind/pull/272)
- Status: complete

The issue reached Ready, the proposal artifacts were created, and the independent proposal review approved the final head after three bounded correction rounds. The approved pilot is implemented on the child branch as a confirmed `list_item_add` MCP write. The implementation keeps MCP thin, reuses the existing list command, requires `mcp.write`, binds approval and execution to household/list authorization, supports explicit confirmation, idempotency, expiry, cancellation, bounded result retention, post-commit notification, structured outcomes, an explicit production invocation boundary for untrusted tool arguments, and attributable approval/idempotency audit outcomes including malformed-call rejection events.

Verification on `4983052cf6eb93c132b4f6dea53111ef959d53c4`:

- Release build: passed.
- Release tests: 892 passed, 7 skipped PostgreSQL integration tests because `DOMUSMIND_TEST_POSTGRES` is not enabled in this environment. The new concurrency/cancellation tests passed separately against a real local PostgreSQL instance.
- Hosted backend CI: passed, including the PostgreSQL-backed persistence tests ([run 39544619958](https://github.com/juangcarmona/domusmind/actions/runs/39544619958)).
- Production approval boundary tests: MCP bearer credentials are rejected by the cookie-authenticated endpoint, and cookie-authenticated approval without an antiforgery token is rejected before the coordinator is called.
- Markdown lint: 0 issues.
- `git diff --check`: passed.
- EF pending-model check: no changes since the last migration.
- Local production PostgreSQL and remote HTTPS write journey: not exercised; the environment has no enabled PostgreSQL integration gate or deployed MCP endpoint.

The stacked Draft PR targets the Epic branch rather than `main`, so normal pull-request workflows do not auto-report checks. Manual backend, documentation, and CodeQL workflow dispatches passed ([backend run 39544619958](https://github.com/juangcarmona/domusmind/actions/runs/39544619958), [docs run 39544620588](https://github.com/juangcarmona/domusmind/actions/runs/39544620588), [CodeQL run 39543474327](https://github.com/juangcarmona/domusmind/actions/runs/39543474327)). The C# and JavaScript analyses are `1993844069` and `1993825060`; the branch has zero open code-scanning alerts after the documented false-positive disposition.

The integration audit at `39ec02d` requested corrections for CodeQL disposition evidence, production-path concurrency/cancellation and cookie/CSRF-boundary evidence, and scoped identity resolution in malformed-call auditing. Those corrections are present in `4983052`; the final review’s P2 request for explicit scope-validation coverage is present on `18b4acb`, and the PR verification text now reports the hosted result accurately. The final independent Done-gate review approved `18b4acb` with no P0/P1 findings. PR #272 merged into the Epic branch as `ae2d3754ffbd5ef3c1bc4cd554edfe2b432eecc5`; issue #231 is closed with `status:done`.

Known inherited warnings remain documented rather than silently treated as resolved: Kiota has a high-severity package advisory, MessagePack has a moderate-severity advisory, and the EF tool is older than the runtime. The independent review found no P0/P1 defects; integration remains a separate protected gate.

Closes #231

Refs #224, #229, #230
