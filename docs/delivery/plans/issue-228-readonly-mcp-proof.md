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

1. `agenda_get`: requires both `from` and `to` as exact `yyyy-MM-dd` calendar dates. Both bounds are inclusive, and the range may span at most 31 calendar days. Reject omitted or partial ranges, non-ISO or ambiguous dates, reversed ranges, and longer windows; do not infer a timezone from a client or machine locale. Dates are date-only values interpreted by the existing query contract, with no conversion from an instant. Run `GetFamilyTimelineQuery` with a synthetic family and requester identity supplied by the test fixture, never by model-controlled household/user identifiers. Ensure the query's formatted date strings use invariant Gregorian culture, and assert them while the test server request uses a non-Gregorian culture. Return only the timeline fields required by the test journey.
2. `household_members_list`: takes no identity arguments and runs `GetFamilyMembersQuery` with the same synthetic fixture identity. Map the response to member ID, display name and role only; omit linked-account IDs, email, birth date, invitation/access status and other account metadata. Duplicate display names remain distinct results with their stable member IDs; never resolve or select a member from a display name alone.

The tools use `IQueryDispatcher` and typed Application query contracts. The test harness must invoke the actual `GetFamilyTimelineQueryHandler` and `GetFamilyMembersQueryHandler` against an isolated EF Core in-memory database (adding a test-only package reference to `DomusMind.Api.Tests` if needed) and a test `IFamilyAuthorizationService`. Seed two synthetic households with distinct members and timeline data; bind the requester to only one household, prove its results cannot contain the other household's data, and prove a direct query for the other household is denied. Reject persistence `SaveChanges` calls after fixture seeding and verify no tracked changes remain after successful reads. Verify neither tool accepts household, user, or member identity from tool input. The test host must not use production Infrastructure registrations, connection strings, credentials, or data. If the actual handlers cannot be composed in the API test host, stop and revise this plan before coding; separate mocked-dispatcher tests do not satisfy this issue. No production MCP registration, controller route, schema, or domain behavior changes are required.

Add the client-facing contract at `docs/delivery/contracts/issue-228-mcp-readonly-proof.md`. It specifies tool names and descriptions, input/output JSON schemas (including required and nullable fields), exact date and range semantics, the explicit timeline/member field allowlists, structured error behavior, representative natural-language journeys, and explicit exclusions for pending Tasks and member-specific agenda. Tests must assert registered test-tool input/output schemas and descriptions conform to this artifact, validate successful response shapes, reject unexpected output properties, and prove unknown identity inputs cannot override fixture-bound identity.

The SDK client tests must demonstrate tool discovery and representative calls over the real in-process Streamable HTTP endpoint. Negative cases cover missing, partial, null, wrong-type, ambiguous, malformed, reversed, and oversized date ranges; Gregorian date output under a non-Gregorian server culture; server-observed cancellation; safe SDK binding errors and structured application validation errors; cross-household denial/isolation; persistence writes rejected after fixture seeding and no tracked mutations; duplicate member display names; complete structural schema/response parity; and the fact that production registration still lists no tools. The pending-task query is documented as deferred because there is no complete Tasks-owned read contract. The member-agenda authorization gap is called out in PR limitations and remains a prerequisite for any future member-specific operation.

## Ordered implementation tasks

1. Confirm query contracts, existing handler authorization tests, test-host seams, date semantics and current task-query gap against the source; prove the API test host can compose the actual query handlers with isolated synthetic persistence and authorization.
2. Write `docs/delivery/contracts/issue-228-mcp-readonly-proof.md` with tool schemas, date semantics, output minimization, errors, example journeys and exclusions.
3. Add two-household synthetic fixtures and a test dispatcher that invokes the actual timeline and member query handlers; prove allowed-household results and denial for the other household.
4. Wire the tools only into the API test host and exercise discovery/calls with the official MCP client. Preserve an empty production catalog and assert schema/description parity with the client contract.
5. Add bounds, ambiguity, cancellation, error-scrubbing, privacy-field exclusion, duplicate-name, cross-household isolation, and no-side-effect tests; reuse existing query handlers/contracts rather than adding a new domain model.
6. Update the plan with any justified deviation, document Tasks/member-agenda exclusions in the PR, run the complete required verification and capture runtime/API transcript evidence.

## Test and verification impact

- Focused `DomusMind.Api.Tests` MCP tests must cover tool discovery, success and failure paths, actual handler execution through typed query dispatch, two-household isolation and denial, synthetic identity binding, minimal response projection, contract/schema parity, cancellation and production-empty registration.
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

None at planning time. If a real query-handler integration cannot be composed without production identity or unrelated persistence changes, report the constraint and revise the plan before implementation; do not silently substitute a mock and claim end-to-end Application behavior. The 31-day maximum and required exact date bounds are proof-scope constraints, not a settled production product contract.
