# Plan: add the first confirmed household write through MCP

- Work item: [#231](https://github.com/juangcarmona/domusmind/issues/231)
- Parent Epic: [#224](https://github.com/juangcarmona/domusmind/issues/224)
- Product Change: [CHG-MCP-HOUSEHOLD-INTEGRATION-001](../../product/changes/active/chg-mcp-household-integration/change.md)
- Definition PR: [#225](https://github.com/juangcarmona/domusmind/pull/225)
- Depends on merged: [#229](https://github.com/juangcarmona/domusmind/issues/229), [#230](https://github.com/juangcarmona/domusmind/issues/230)

## Outcome

Release one narrowly bounded MCP write workflow: an authenticated household member can add an item to an existing shared list only after a trusted DomusMind approval surface has approved the exact action. The operation reuses the existing Lists application command and remains behind an explicit write capability gate.

The pilot deliberately excludes calendar and task writes. It demonstrates the safety, authorization, approval, idempotency, and post-commit notification boundaries needed before broader MCP mutations are considered.

## Product and security decisions

- The only production write capability in this child is `list_item_add`.
- The caller supplies a stable `listId`; list discovery and natural-language list-name resolution are deferred to a future Lists read capability. This avoids adding an unreviewed list-resolution policy to the first write.
- The caller supplies `name` and may supply `quantity` and `note`. The tool derives the authenticated user, client, and OAuth-granted household from the validated request context and accepts no identity selector.
- Client confirmation text, model intent, MCP descriptions, a second model-controlled tool call, or a client UI are not authorization. A human approval must be observable as a state transition performed through a trusted DomusMind-authenticated approval surface.
- The first implementation exposes that approval through an application-owned approval port and a trusted first-party DomusMind surface (the existing application host may provide the HTTP endpoint/UI). The surface authenticates a household member, applies CSRF/session protections, shows the exact action summary, and records the approving subject. The MCP client cannot approve its own pending request with only its OAuth credential or an opaque request identifier.
- The approval record binds the exact actor, OAuth client, household, target list, canonical payload, and idempotency identity. It is short-lived, single-use, and invalidated after successful execution or explicit cancellation.
- The Lists application boundary must compare the target list household with the OAuth-granted household before invoking the existing membership check. A user who belongs to multiple households cannot use a grant for household A to target a list in household B.
- A replay with the same idempotency identity must not create a second item. The server retains the completed result for the configured retry window and then retains a non-replayable tombstone for the agreed anti-replay window; a changed payload under the same key is a conflict.
- The Lists application command remains authoritative for list ownership, household authorization, input validation, domain events, event logging, and persistence.

## Trusted approval protocol

The production tool accepts an optional `approvalRequestId` and required `idempotencyKey`. `approvalRequestId` is an opaque reference to a durable server-side record, not a bearer token and not proof of approval.

1. A request without an approval reference validates the exact input shape and current actor context, creates a pending approval record containing the canonical payload hash and safe review summary, and returns `CONFIRMATION_REQUIRED` plus the reference and expiry. It performs no Lists command, household-domain mutation, list-item mutation, or event-log write. Creating/updating the pending approval record is the only permitted persistence change.
2. The household member opens the trusted DomusMind approval surface, authenticates, reviews the exact summary, and explicitly approves or cancels. Approval is accepted only when the authenticated member is the OAuth-grant subject for this pilot, is still active, belongs to the bound household, and the record is unexpired and pending. The surface records the approval transition; MCP cannot perform it.
3. A follow-up MCP request repeats the exact operation and supplies the approval reference and idempotency key. The adapter verifies approval state, expiry, actor/client/household/list/payload binding, OAuth `mcp.write` scope, and idempotency state before dispatching `AddItemToListCommand`.
4. The confirmed execution coordinator claims the idempotency identity and consumes the approval record in the same relational transaction as the Lists command, event log, and completion result. A failed authorization, validation, conflict, rollback, or rejected command does not create a list item.
5. A cancellation before commit rolls back the execution. If request cancellation races after the database commit, the server reports an indeterminate/committed outcome rather than promising no mutation; a retry with the same idempotency key returns the stored result or safe duplicate outcome.

The trusted approval channel is a product and architecture decision. If the existing application host cannot provide it without assuming an unsupported frontend or deployment shape, the write capability remains disabled until an equivalent authenticated first-party channel is accepted. A signed token returned by MCP alone is explicitly insufficient.

## OAuth scope and gate

- The write scope is `mcp.write`; it is distinct from `mcp.read` and is never implied by it.
- Protected-resource metadata advertises `mcp.write` only when household writes are enabled, alongside `mcp.read`.
- The authorization server/client registration grants the named scope only with explicit client permission and explicit user consent. The consent surface identifies read and write access separately.
- The MCP resource middleware continues to validate authentication, audience, subject, current household membership, and `mcp.read` for read operations. The `list_item_add` tool additionally requires `mcp.write` at tool-call time; direct `tools/call` requests cannot bypass this check.
- Existing read-only grants cannot create or confirm writes. Requesting `mcp.write` requires a fresh/expanded explicit consent decision, and revocation invalidates both scopes.
- The production tool is registered only when all of `Mcp:Capabilities:HouseholdWrites=true`, `Mcp:OAuth:Enabled=true`, `Mcp:RequireOAuth=true`, and the validated request scope `mcp.write` are present. The default catalog remains unchanged otherwise.

## Durable idempotency and transaction boundary

Use an application-owned idempotent execution coordinator backed by the existing relational store. MCP must call the coordinator through an application abstraction; it must not query or mutate EF Core directly.

- Store a hash-only canonical payload, operation namespace `mcp:list_item_add:v1`, actor, client, granted household, status, expiry, safe minimized result, and audit identifiers in a durable operation table. Never persist tokens, prompts, or raw item content in audit fields.
- Enforce a database uniqueness boundary on `(namespace, actor, client, granted household, idempotency key)`. The canonical payload includes list ID, normalized name, quantity, and note; the same key with a different hash returns `IDEMPOTENCY_CONFLICT` without mutation.
- The coordinator claims a new identity or reads the existing state under a transaction/serializable equivalent, then dispatches the Lists command with an application-owned `GrantedHouseholdId`/expected-household constraint. The Lists handler rejects a list whose family differs from that constraint before its existing membership check.
- The coordinator, `AddItemToListCommandHandler`, `EventLogWriter`, completion result, and approval consumption share the same current EF transaction. `EventLogWriter` must join an ambient transaction rather than commit independently. A crash before commit rolls back the item, event, approval consumption, and operation claim; a response loss after commit is safe because the completed result is durable.
- Concurrent confirmations of one challenge or idempotency identity have one winner. An in-progress duplicate returns a retryable `OPERATION_IN_PROGRESS`/safe duplicate result and never dispatches a second command.
- Completed results are retained for the configured client retry window. After result expiry, retain an actor/client/household/key/payload-hash tombstone until the agreed anti-replay horizon; never reuse an expired key for a different payload. Cleanup must not remove live pending approvals or active tombstones.
- Cancellation is honored before commit. After commit, cancellation cannot undo the mutation and the response is classified as indeterminate; retry resolves from the stored idempotency record.

The initial implementation must choose concrete retention values through configuration and document them in operator guidance; the owner must approve those limits before enabling the capability.

## MCP contract

The independently testable request/result schemas, normalization, error codes, approval states, idempotency rules, audit fields, and notification obligations are defined in [the production contract](../contracts/issue-231-mcp-confirmed-write-pilot.md).

## Architecture and boundaries

- MCP tools remain thin adapters under `src/backend/DomusMind.Mcp`.
- The adapter uses the validated MCP request identity from #229 and never trusts a household, user, member, scope, or command identifier supplied as tool input.
- The adapter dispatches through application abstractions: approval coordination, idempotent execution, and `AddItemToListCommand`. It does not access EF Core, repositories, or domain aggregates directly.
- Lists owns list and list-item semantics, household authorization, event logging, and persistence. MCP owns only the external contract, approval request coordination, idempotency attribution, safe errors, and audit attribution.
- Extract the list-change publication contract from the API controller into a shared application-facing abstraction. The MCP execution coordinator publishes the list change only after the shared database transaction commits; the REST path continues to publish after its successful command completion through the same post-commit abstraction. Rejected, cancelled, rolled-back, and duplicate-without-new-mutation paths publish nothing.
- No generic tool, arbitrary API proxy, prompt interpreter, client-only confirmation assumption, calendar mutation, task mutation, or persistence shortcut is introduced.
- The existing .NET host remains the preferred deployment for this pilot. A separate deployable component is not justified unless runtime or security evidence demonstrates a need, and it must still call application contracts rather than persistence.

## Safety requirements

- Validate UTF-8/character lengths, blank values, list ID format, idempotency key format and length, and approval reference format before any domain write.
- Return safe structured errors for invalid input, missing/pending/ambiguous approval, access denial, expired/replayed/mismatched approvals, idempotency conflicts, duplicate operations, and unexpected failures.
- Never include approval references, tokens, prompts, raw item content, SQL, exception types, or storage details in logs or error messages.
- Revalidate OAuth token validity, `mcp.write`, current account state, membership, and target-list/granted-household binding at approval and execution time.
- Treat all tool input and external content as untrusted structured data; fields outside the contract are rejected and cannot select capabilities or authorization.
- Record fixed capability name, trusted actor/client/household IDs, approval outcome, idempotency outcome, and final result class in the security audit event.
- Define concurrency behavior for two approvals using the same request or idempotency key; at most one list item may be created and at most one post-commit notification is published.

## Ordered implementation tasks

1. Confirm and document the trusted approval channel, operation/confirmation contract, write scope, error codes, idempotency retention policy, and owner decisions in this plan and the contract artifact.
2. Add application abstractions and infrastructure-backed approval records, idempotency records, canonical payload hashing, transactional execution, expiry, single-use consumption, cancellation, and concurrent replay protection.
3. Extend the Lists application boundary to enforce the expected OAuth-granted household without moving authorization into MCP.
4. Add the shared post-commit list-change publication path and preserve REST notifications.
5. Add the production `list_item_add` MCP tool and register it only behind the write, OAuth, scope, and default-off gates.
6. Map the existing command response to the minimized MCP result and add allowlisted security audit events.
7. Add production-path tests for contract binding, approval, authorization, idempotency, replay, concurrency, cancellation, error safety, no domain writes before approval, no notification on rejection, one notification after commit, and application command reuse.
8. Add an HTTPS OAuth integration journey using the official .NET MCP client that performs protected-resource discovery, PKCE, `mcp.write` consent, pending approval, trusted approval, successful add, and idempotent replay over Streamable HTTP. Do not claim hosted-client compatibility.
9. Update operator/client documentation with the write scope, trusted approval path, confirmation contract, retention limits, supported operation, exclusions, and rollout flag. Keep the capability disabled by default.

## Test plan

### Contract and composition

- The default catalog remains unchanged with the write flag disabled.
- With all required gates enabled, exactly `list_item_add` is registered in addition to the released read tools.
- The schema contains only `listId`, `name`, `quantity`, `note`, `idempotencyKey`, and optional `approvalRequestId`; no user, member, household, scope, command, or arbitrary fields exist.
- `name` is trimmed, nonblank, and at most 200 characters; `quantity` is optional and at most 100; `note` is optional and at most 500; `listId` is a GUID; `idempotencyKey` is nonblank, bounded, and canonicalized; malformed/unknown fields and SDK binding errors are safe and non-mutating.
- Calendar, task, list-management, and generic write tools remain absent.

### Trusted approval and idempotency

- The first valid request returns `CONFIRMATION_REQUIRED`, creates only a pending approval record, and leaves lists, domain state, and event log unchanged.
- A client possessing only the OAuth credential and pending approval reference cannot approve or execute the operation; the trusted authenticated approval surface must transition the record first.
- Pending, cancelled, expired, malformed, unknown, mismatched, and already-consumed approval references return their specified errors and do not create a fresh challenge implicitly.
- A valid trusted approval produces one list item through `AddItemToListCommand`.
- A changed payload, actor, client, household, list, or idempotency key invalidates the approval.
- Concurrent confirmations have one transaction winner; crash/failure before commit rolls back all domain changes; response loss after commit returns the stored result on retry.
- Replaying a completed idempotency identity returns the documented original/duplicate result without a second item; a changed payload under the same identity returns `IDEMPOTENCY_CONFLICT`.

### Authorization, notification, and safety

- A member belonging to households A and B cannot use an A-bound OAuth grant to target B's list, even when the B list ID is known.
- Disabled accounts, revoked grants, removed membership, missing `mcp.write`, failed audience/resource validation, and approval cancellation cannot confirm a pending operation.
- A successful MCP mutation emits the same post-commit list-change notification as the REST path; rejected, rolled-back, cancelled, and non-mutating duplicate paths emit none.
- Audit events contain only allowlisted identifiers and result classes; no approval secret/reference, prompt, item content, or exception details are recorded.

### Remote evidence

- A custom official .NET MCP client completes protected-resource discovery, OAuth PKCE, explicit read/write consent, tool discovery, pending approval, trusted first-party approval, successful add, and idempotent replay over loopback HTTPS/Streamable HTTP.
- Evidence records the client/version, transport, approval path, certificate assumptions, retention configuration, and unsupported hosted-client profiles.

## Open decisions requiring owner review

- Approve the single operation `list_item_add` as the first mutation and defer calendar/task writes.
- Approve the trusted first-party approval channel and same-OAuth-subject approval rule; MCP client possession alone is not accepted as human confirmation.
- Approve `mcp.write` as the explicit write scope and separate consent from `mcp.read`.
- Approve stable `listId` targeting for this child, with list discovery deferred.
- Approve the initial challenge/result/tombstone retention limits after implementation proposes concrete configuration values.

## Definition of Done for this plan

- Every #231 acceptance criterion has deterministic evidence or an explicit reviewed limitation.
- The only write path is the trusted-approved `list_item_add` capability behind the OAuth, `mcp.write`, and default-off write gates.
- No MCP code bypasses Lists application ownership, household authorization, domain invariants, event logging, persistence boundaries, or shared post-commit notifications.
- Approval, idempotency, replay, concurrency, cancellation, audit, notification, and safe-error behavior are proven through production-path tests.
- Backend, web, documentation, security, and CodeQL checks pass on the final head; inherited advisories remain documented.
- The PR documents supported HTTPS evidence and does not claim ChatGPT, Claude, STDIO, or private-hosted compatibility.

## References

- [Production confirmed-write contract](../contracts/issue-231-mcp-confirmed-write-pilot.md)
- [#229 OAuth/OIDC authorization plan](issue-229-mcp-oauth-authorization.md)
- [#230 authorized read-only release plan](issue-230-mcp-readonly-release.md)
- [Lists specification](../../../openspec/specs/lists/spec.md)
- [MCP security best practices](https://modelcontextprotocol.io/docs/2026-07-28/tutorials/security/security_best_practices)
- [MCP specification](https://modelcontextprotocol.io/specification/2026-07-28)
- [Official .NET MCP SDK](https://github.com/modelcontextprotocol/csharp-sdk)
