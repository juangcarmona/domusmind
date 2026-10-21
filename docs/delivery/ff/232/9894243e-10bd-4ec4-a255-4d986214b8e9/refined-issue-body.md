## Outcome

Expand DomusMind's MCP capability catalog only where a high-value household workflow is supported by a clear owning-context contract, permission model, and safe interaction. The result must be a coherent, independently reviewable capability slice, not a mechanical exposure of every API endpoint.

## Actors and context

- A household member using a connected AI assistant to ask about lists, meals, routines, recipes, responsibilities, tasks, or plans.
- A household member who owns or is responsible for private information, such as a private list or assigned area.
- The owning DomusMind context that remains responsible for the capability's rules, authorization, persistence, and audit semantics.
- DomusMind operators who need to understand supported client behavior, failure modes, and deferred capabilities.

## Scope

Reassess the P2/P3 candidates in Epic #224: household/private list reads, meal-plan reads, routines, recipe/meal-plan actions, responsibility-area insights, and additional task/calendar operations. Select a coherent subset based on household value, existing application support, authorization clarity, ambiguity, confirmation risk, complexity, and client usefulness.

For every selected capability, identify the natural-language interaction, owning context, existing or required application query/command, read/write classification, household/member/private visibility, ambiguity handling, confirmation requirement, idempotency behavior, audit outcome, and tests. Record deferred candidates and why they are deferred.

## Dependencies

Issues #229 and #230 provide the authorized MCP read path. Issue #231 provides the confirmed-write and audit safety boundary. Each new capability depends on an accepted owner-context contract and any required OpenSpec impact. No capability may bypass household/member authorization or access persistence directly from an MCP tool.

## Acceptance Criteria

- [ ] Every selected capability has an evidence-based household interaction, owner context, and existing or explicitly identified application query/command.
- [ ] Selected reads enforce household and member visibility, including private-list semantics and empty/not-found behavior.
- [ ] Selected writes meet the confirmed-write requirements: authorization, clarification, confirmation, idempotency, structured errors, and audit attribution.
- [ ] Existing domain specifications remain canonical; only focused MCP contracts or traceability amendments are added where necessary.
- [ ] Deferred candidates and reasons are recorded; endpoint count is not used as a proxy for product value.
- [ ] MCP Resources or Prompts are added only if a demonstrated client journey justifies their permission, caching, lifecycle, and interoperability costs.
- [ ] Unit, application, authorization, contract, and integration tests cover every selected capability and its negative paths.
- [ ] The resulting branch and Draft PR contain no unrelated product or domain changes.

## Affected behavior and quality expectations

The MCP adapter gains additional thin, semantically meaningful tools while existing application commands, queries, domain invariants, household authorization, structured error envelopes, and audit rules remain authoritative. Date/week interpretation must use the household/member locale and timezone. Ambiguous members, lists, dates, plans, recipes, and assignments must be clarified before a state-changing operation. Private data must not leak through aggregate results, error messages, logs, or client-visible metadata. Retries must not duplicate state changes.

## Explicit exclusions

Household administration, authentication settings, provider credentials, external-calendar connections, Inbox execution, raw chat history, unrestricted CRUD, database/resource enumeration, destructive deletes/cancellations, and generic task/routine mutation remain out of scope unless separately approved. Resources and Prompts are deferred by default.

## Unknowns and risks

- Which candidate subset offers the strongest household value after inspecting current handlers and specs?
- Does Tasks expose a safe read model for pending tasks and routine state, or is that a separate domain slice?
- Which private-list and responsibility-area visibility rules are already enforceable versus requiring an application seam?
- Which client versions support the proposed tools consistently, and do any require client-specific workarounds?
- Which selected writes need confirmation beyond the existing #231 boundary because they affect multiple aggregates or generate derived state?

## Related work and references

Epic #224; Product Change `CHG-MCP-HOUSEHOLD-INTEGRATION-001`; merged dependencies #229, #230, and #231. Relevant specifications include Lists, Meal Planning, Tasks, Areas, and the current MCP specification.
