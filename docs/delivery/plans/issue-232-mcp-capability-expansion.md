# Proposal: Expand MCP capabilities across household coordination contexts

Work item: #232  
Epic: #224  
Product Change: `CHG-MCP-HOUSEHOLD-INTEGRATION-001`  
Base: `feat/224-mcp-household-assistants`

## Proposal

Add a focused read-only MCP expansion for four household workflows that already have application-owned, authorization-aware queries:

1. `lists_get` for household-shared list summaries, with an optional selected-list detail operation only if the MCP contract keeps summary and detail semantics unambiguous.
2. `meal_plan_get` for one explicit household week.
3. `routines_get` for the household's authorized routines.
4. `areas_get` for responsibility areas and their owner context.

The tools remain thin adapters. They resolve the authenticated MCP household identity, validate bounded arguments, dispatch existing application queries, map existing response contracts to stable structured MCP output, and emit the established capability audit record. No MCP tool accesses EF Core or creates a parallel domain model.

## Evidence-based capability selection

| Candidate | Decision | Existing support | Main boundary |
| --- | --- | --- | --- |
| Household-shared lists | Select | `GetFamilyListsQuery`, `GetListDetailQuery` | Current Lists model has no private-list ownership seam; expose only authorized household-shared lists. |
| Meal plan for a week | Select | `GetMealPlansForAgendaQuery` | Require one explicit ISO week-start date; empty plan is a successful empty result. |
| Household routines | Select | `GetRoutinesByFamilyQuery` | Keep routine state and target member/area identifiers in the owning Tasks context; do not infer task occurrences. |
| Responsibility areas | Select | `GetHouseholdAreasQuery` | Read-only accountability context; owner changes remain Responsibilities commands and are deferred. |
| Pending task list | Defer | No direct task-list query/read model identified | Do not approximate it from Agenda or access persistence directly. |
| Recipes and recipe mutation | Defer | Existing Meal Planning APIs exist, but the high-value MCP contract and write confirmation are not yet bounded | Reassess with a recipe-specific interaction and output contract. |
| Meal-plan mutation or shopping-list generation | Defer | Commands exist and can create cross-context list state | Requires a separate confirmed-write design and duplicate/confirmation semantics. |
| Routine mutation/completion | Defer | Commands exist | More consequential recurrence/lifecycle semantics need their own slice. |
| Area ownership mutation and balance/overload actions | Defer | Queries and commands exist | Ownership changes are consequential and require confirmed writes; balance output needs a distinct value case. |
| MCP Resources and Prompts | Defer | No demonstrated client journey | Avoid permission, caching, invalidation, and uneven-client-support cost until a concrete workflow requires them. |

## Contract and authorization design

- Every selected tool derives household and user identity from the existing MCP request identity accessor; caller-supplied household IDs are not trusted as authorization.
- `lists_get` exposes household-shared list summaries and, when detail is selected, only that list's items after the existing list query authorizes the requesting member. Private-list semantics are explicitly out of scope until the Lists context supplies an ownership contract.
- `meal_plan_get` accepts a single `yyyy-MM-dd` `weekStart`, rejects malformed dates and dates outside the product's week boundary, and returns the existing slot projection without creating a plan.
- `routines_get` and `areas_get` are household-scoped reads. They preserve stable IDs and owning-context fields, but do not expose persistence details or unrelated private member data.
- Access-denied, invalid-input, not-found, and unexpected failures use the existing structured MCP error envelope without leaking exception text, IDs from another household, or persistence information.
- Cancellation propagates; retrying a read is safe. Read tools do not use confirmation or idempotency records because they do not change state.
- Audits identify capability, authenticated user, household, client, trace, and outcome through the existing MCP audit path.

## Architecture impact

The change is isolated to the MCP adapter and its contract/boundary tests. It reuses these canonical application queries and owning contexts:

- Lists: `GetFamilyListsQuery`, `GetListDetailQuery`.
- Meal Planning: `GetMealPlansForAgendaQuery`.
- Tasks: `GetRoutinesByFamilyQuery`.
- Responsibilities: `GetHouseholdAreasQuery`.

The existing internal mediator, domain rules, `IFamilyAuthorizationService`, structured MCP result conventions, and audit logging remain authoritative. No domain, persistence, migration, API controller, frontend, or authentication changes are planned. If a required query contract is found insufficient during implementation, stop and update this plan rather than bypassing the application layer.

## Ordered tasks

1. Add MCP-specific contract/output records and a shared read-tool mapping/error helper where needed, preserving the existing `DomusMindReadOnlyTools` behavior.
2. Implement `lists_get` with household-shared summary/detail behavior and explicit private-list exclusion.
3. Implement `meal_plan_get` with bounded week input and existing meal-plan agenda projection.
4. Implement `routines_get` with authorized household scope and stable routine fields.
5. Implement `areas_get` with authorized household scope, owner names, and archived filtering only if the MCP contract has a demonstrated need; otherwise default to active areas.
6. Register or discover the tools through the existing MCP composition path without adding a second server or transport.
7. Add application/adapter tests for success, empty results, invalid arguments, denied household access, cross-household identifiers, cancellation, structured output, and audit outcomes.
8. Add official-client HTTP boundary coverage for each selected tool and verify the complete deterministic lane.
9. Update MCP capability documentation and this proposal's deferred-candidate matrix with any implementation discoveries.

## Test plan

- Unit/contract: date/week validation, output schema shape, bounded list detail, enum/string serialization, empty results, and safe error envelopes.
- Application integration: dispatch each existing query with the authenticated household/user and verify wrong-household access is denied by the owning handler.
- MCP composition: every selected tool is discoverable, marked read-only, and does not expose persistence services or unbounded arguments.
- HTTP boundary: authorized client calls receive only their household data; unauthenticated, wrong-household, malformed, and cross-household identifiers fail safely and produce audit outcomes.
- Regression: existing agenda, member, confirmed-write, OAuth, and MCP origin/transport tests remain green.
- No database migration is expected. If implementation reveals a missing owner-context query contract, add a separate domain/application issue rather than broadening this slice silently.

## Definition of Done impact

- Acceptance: each selected tool has a representative interaction and verifiable positive/negative tests.
- Completeness: the selected/deferred matrix is reconciled with the diff; no candidate is silently dropped.
- Tests/quality: Release build and full backend tests pass; docs and MCP contract validation pass; hosted backend and CodeQL evidence is linked.
- Security: household/member authorization remains in application handlers; private-list semantics are not guessed; no token or exception leakage; SAST remains green.
- Architecture/spec: existing Lists, Meal Planning, Tasks, and Areas specs remain canonical; only MCP-specific contracts and traceability are added.
- Known limitations: private lists, pending-task read model, recipe operations, new writes, Resources, Prompts, and remote client interoperability remain deferred to later Epic children.

## Non-goals

No new domain aggregates, query read models, authentication changes, OAuth changes, MCP transport changes, writes, task generation, calendar mutation, list generation, Resources, Prompts, frontend changes, or persistence changes are included.

Closes #232

Refs #224, #229, #230, #231
