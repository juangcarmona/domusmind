# Plan: Prove the initial read-only MCP capabilities with test identity (#228)

## Outcome

Prove a bounded household-timeline and member-directory journey over the new MCP transport, using only synthetic fixtures and test-only tool registration. This validates the adapter's application-query seam; it does not authorize production exposure.

## Context and constraints

- Epic: #224; draft Product Change `CHG-MCP-HOUSEHOLD-INTEGRATION-001` remains the product intent.
- Foundation #227 is merged into `feat/224-mcp-household-assistants`; PR #268 is closed as merged.
- The existing application queries `GetFamilyTimelineQuery` and `GetFamilyMembersQuery` are family-scoped and call `IFamilyAuthorizationService`.
- `GetMemberAgendaQueryHandler` verifies access to `FamilyId` but does not prove the supplied `MemberId` belongs to that family. Do not register or call it from the MCP proof. Member-specific agenda is deferred until that authorization gap is addressed and tested.
- No complete Tasks-owned list query exists in the current application surface. Do not model agenda entries as a complete pending-task list; record this gap and leave tasks out of the tool catalog.
- Production `DomusMindMcpServiceCollectionExtensions` must continue to register an empty tool catalog. MCP test tools are added only by the API test host.
- The adapter must dispatch existing typed queries and must not reference persistence, Domain, or EF types directly.
- Do not use real user credentials, production/self-hosted data, an external MCP client, OAuth/OIDC, or write operations.

## Proposed design

Extend the API test host's existing official-SDK client harness with two test-only, read-only tools:

1. `agenda_get`: accepts an optional bounded ISO date range and runs `GetFamilyTimelineQuery` with a synthetic family and requester identity supplied by the test fixture, never by model-controlled household/user identifiers. It returns only the timeline fields required by the test journey, with a deterministic date interpretation and a documented maximum window.
2. `household_members_list`: takes no identity arguments and runs `GetFamilyMembersQuery` with the same synthetic fixture identity. Map the response to member ID, display name and role only; omit linked-account IDs, email, birth date, invitation/access status and other account metadata.

The tools use `IQueryDispatcher` and typed Application query contracts. The test harness supplies a dispatcher backed by the existing query handlers and isolated synthetic persistence/test doubles where feasible; if full handler composition proves disproportionately invasive, keep dispatcher integration tests separate from existing handler tests and explicitly record that boundary rather than claiming a production integration test. No production MCP registration, controller route, schema, or domain behavior changes are required.

The SDK client tests must demonstrate tool discovery and representative calls over the real in-process Streamable HTTP endpoint. Negative cases cover malformed/reversed/oversized date windows, cancellation, safe structured errors, no mutations, and the fact that production registration still lists no tools. The pending-task query is documented as deferred because there is no complete Tasks-owned read contract. The member-agenda authorization gap is called out in PR limitations and remains a prerequisite for any future member-specific operation.

## Ordered implementation tasks

1. Confirm query contracts, existing handler authorization tests, test-host seams, date semantics and current task-query gap against the source.
2. Add test-only fixture context and capability handlers for household timeline and minimal member resolution; ensure no household/user identity is accepted from tool input.
3. Wire the tools only into the API test host and exercise discovery/calls with the official MCP client. Preserve an empty production catalog.
4. Add bounds, ambiguity, cancellation, error-scrubbing, privacy-field exclusion, and no-side-effect tests; reuse existing query handlers/contracts rather than adding a new domain model.
5. Update the plan with any justified deviation, document Tasks/member-agenda exclusions in the PR, run the complete required verification and capture runtime/API transcript evidence.

## Test and verification impact

- Focused `DomusMind.Api.Tests` MCP tests must cover tool discovery, success and failure paths, actual typed query dispatch, synthetic identity binding, minimal response projection, cancellation and production-empty registration.
- Existing `GetFamilyTimelineQueryHandlerTests` and `GetFamilyMembersQueryHandlerTests` remain the owning tests for query authorization and projections; add handler changes only if implementation demonstrates a specific necessary gap within this issue's test-only scope.
- Run the full Release solution build/test and all CI-defined backend, docs, ProductShape/OpenSpec/citation, public-site and web checks that the workflow requires. Confirm the newly added MCP tests appear in totals.
- Drive the built API/MCP endpoint using only synthetic test identity. Capture a sanitized request/response transcript and verify it contains no real household data or account metadata.
- The PR remains explicitly non-production: no exposed household capability until #229 authorization is complete, and no member-specific agenda while the cross-family/member-ID gap remains.

## Definition of Done

- Acceptance criteria: each criterion has test or inspected evidence; no data is exposed outside synthetic fixture scope.
- Implementation completeness: planned tasks are done or explicitly dropped with a reason; no production tool registration.
- Tests: relevant xUnit suite passes and includes new MCP tests.
- Quality: Release `dotnet build`/`dotnet test`, `npm run build`, `npm run lint`, `npm run test`, and public-site build pass where the CI definitions apply.
- Security: secret/dependency scans and CodeQL results reviewed; test identity/data isolation and empty production catalog demonstrated.
- Architecture: MCP tools are thin typed-query adapters; no persistence access or bypass of domain authorization.
- Documentation/spec consistency: plan, PR limitations and this issue's exclusions are accurate; do not alter canonical domain specs for a test-only proof.
- Evidence: CI links and runtime/API transcript are attached to the PR and durable FF evidence.
- Human review: owner judgement remains required for #228; this cannot be inherited from #227.
- Known limitations: pending Tasks query and member-agenda authorization gap remain explicit; no unsupported client compatibility claim.

## Deviations / decision log

None at planning time. If a real query-handler integration cannot be composed without production identity or unrelated persistence changes, report the constraint and revise the plan before implementation; do not silently substitute a mock and claim end-to-end Application behavior.
