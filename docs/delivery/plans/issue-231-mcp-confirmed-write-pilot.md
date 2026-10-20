# Plan: add the first confirmed household write through MCP

- Work item: [#231](https://github.com/juangcarmona/domusmind/issues/231)
- Parent Epic: [#224](https://github.com/juangcarmona/domusmind/issues/224)
- Product Change: [CHG-MCP-HOUSEHOLD-INTEGRATION-001](../../product/changes/active/chg-mcp-household-integration/change.md)
- Definition PR: [#225](https://github.com/juangcarmona/domusmind/pull/225)
- Depends on merged: [#229](https://github.com/juangcarmona/domusmind/issues/229), [#230](https://github.com/juangcarmona/domusmind/issues/230)

## Outcome

Release one narrowly bounded MCP write workflow: an authenticated household member can add an item to an existing shared list only after DomusMind has issued and verified a server-side confirmation challenge. The operation reuses the existing Lists application command and remains behind an explicit write capability gate.

The pilot deliberately excludes calendar and task writes. It demonstrates the safety and authorization boundary needed before broader MCP mutations are considered.

## Product and security decisions

- The only production write capability in this child is `list_item_add`.
- The caller supplies a stable `listId`; list discovery and natural-language list-name resolution are deferred to a future Lists read capability. This avoids adding an unreviewed list-resolution policy to the first write.
- The caller supplies `name` and may supply `quantity` and `note`. The tool derives the authenticated user and household from the validated OAuth request context and accepts no identity selector.
- Client confirmation text, model intent, MCP descriptions, or a client UI are not authorization. The server issues a confirmation challenge and validates it independently.
- The confirmation challenge binds the exact actor, OAuth client, household, target list, operation payload, and idempotency identity. It is short-lived, single-use, and invalidated after successful execution or explicit cancellation.
- A replay with the same idempotency identity must not create a second item. The result policy is to return the original successful result while the original result is retained; otherwise return a safe duplicate outcome without mutation.
- The Lists application command remains authoritative for list ownership, household authorization, input validation, domain events, event logging, and persistence.

## Confirmation protocol

The production tool accepts an optional server-issued `confirmationToken` and required `idempotencyKey`.

1. A request without a valid confirmation token validates the input shape and actor context, creates a pending challenge containing a hash of the canonical operation payload, and returns `CONFIRMATION_REQUIRED` plus a human-readable action summary. It performs no application command and no persistence mutation.
2. The client presents the action summary to the household member. A follow-up request repeats the exact operation and supplies the token and idempotency key.
3. The adapter verifies token integrity, expiry, single-use state, actor/client/household/list/payload binding, and idempotency state before dispatching `AddItemToListCommand`.
4. The adapter marks the challenge consumed only as part of the successful idempotent operation boundary. A failed authorization or validation attempt does not create a list item and does not allow a changed payload to reuse the challenge.

The implementation must choose a storage mechanism that survives concurrent requests and does not put secrets or raw item content into logs. A signed token alone is insufficient if replay and result retention cannot be enforced; a server-side short-lived store is preferred unless existing infrastructure provides equivalent guarantees.

## MCP contract

The exact request/result schemas, error codes, confirmation states, idempotency rules, and audit fields are defined in [the production contract](../contracts/issue-231-mcp-confirmed-write-pilot.md).

The capability is registered only when all of the following are enabled:

- `Mcp:Capabilities:HouseholdWrites=true`
- `Mcp:OAuth:Enabled=true`
- `Mcp:RequireOAuth=true`
- the write scope accepted by the authorization configuration

The default catalog remains unchanged when the write capability is disabled or OAuth protection is incomplete.

## Architecture and boundaries

- MCP tools remain thin adapters under `src/backend/DomusMind.Mcp`.
- The adapter uses the validated MCP request identity from #229 and never trusts a household, user, member, or scope identifier supplied as tool input.
- The adapter dispatches `AddItemToListCommand` through the existing application command dispatcher. It does not access EF Core, repositories, or domain aggregates directly.
- Lists owns list and list-item semantics, household authorization, event logging, and persistence. MCP owns only the external contract, confirmation challenge, idempotency boundary, safe errors, and audit attribution.
- No generic tool, arbitrary API proxy, prompt interpreter, client-side confirmation assumption, calendar mutation, task mutation, or persistence shortcut is introduced.
- The implementation must determine whether the confirmation/idempotency store belongs inside the existing .NET host and infrastructure abstractions. A separate deployable component is not justified by this pilot unless existing runtime constraints demonstrate otherwise.

## Safety requirements

- Validate UTF-8 string lengths, blank values, list ID format, idempotency key format and length, and confirmation token format before any write path.
- Return safe structured errors for invalid input, missing/ambiguous confirmation, access denial, expired/replayed challenges, duplicate operation identities, and unexpected failures.
- Never include confirmation tokens, raw prompts, raw item content, or authorization internals in logs or error messages.
- Enforce current household membership and OAuth grant validity at confirmation time, not only at challenge creation time.
- Preserve cancellation and ensure a cancelled or denied operation does not mutate the list.
- Record fixed capability name, trusted actor/client/household IDs, confirmation outcome, idempotency outcome, and final result class in the security audit event.
- Protect against prompt injection and untrusted external content by treating all tool input as untrusted structured data and by refusing fields outside the contract.
- Define concurrency behavior for two confirmations using the same challenge or idempotency key; at most one list item may be created.

## Ordered implementation tasks

1. Confirm and document the operation/confirmation contract, write gate, scope, error codes, and idempotency policy in this plan and the contract artifact.
2. Add the confirmation challenge abstraction and infrastructure-backed lifecycle with expiry, binding, single-use consumption, cancellation, and concurrent replay protection.
3. Add the production `list_item_add` MCP tool and register it only behind the write and OAuth gates.
4. Dispatch `AddItemToListCommand` with the validated household/user identity and map its response to a minimized MCP result.
5. Add capability-level audit events that distinguish challenge creation, confirmation rejection, idempotent replay, and successful/failed mutation without sensitive payloads.
6. Add production-path tests for confirmation, authorization, idempotency, replay, concurrency, cancellation, error safety, no writes before confirmation, and application command reuse.
7. Add an HTTPS OAuth integration journey using the official .NET MCP client that performs challenge creation, explicit confirmation, successful list mutation, and replay protection. Do not claim hosted-client compatibility.
8. Update operator/client documentation with the write scope, confirmation contract, supported operation, exclusions, and rollout flag. Keep the capability disabled by default.

## Test plan

### Contract and composition

- The default catalog remains unchanged with the write flag disabled.
- With all required gates enabled, exactly `list_item_add` is registered in addition to the already released read tools.
- The schema contains only `listId`, `name`, `quantity`, `note`, `idempotencyKey`, and optional `confirmationToken`; no user, member, household, or arbitrary command fields exist.
- Calendar, task, list-management, and generic write tools remain absent.

### Confirmation and idempotency

- The first valid request returns `CONFIRMATION_REQUIRED` and leaves database state and event log unchanged.
- A valid confirmation produces one list item through `AddItemToListCommand`.
- Changed payload, actor, client, household, list, or idempotency key invalidates the challenge.
- Expired, cancelled, consumed, malformed, and concurrently reused challenges produce no duplicate mutation.
- Replaying a completed idempotency identity returns the documented original/duplicate result without a second item.

### Authorization and safety

- A member cannot target another household's list, even with a caller-supplied list ID or extra JSON fields.
- Disabled accounts, revoked grants, removed membership, and failed OAuth/resource validation cannot confirm a pending operation.
- Blank/oversized/invalid fields, cancellation, unknown lists, and application validation errors are safe and non-mutating.
- Audit events contain only allowlisted identifiers and result classes; no token, prompt, item content, or exception details are recorded.

### Remote evidence

- A custom official .NET MCP client completes protected-resource discovery, OAuth PKCE, token exchange, tool discovery, challenge creation, explicit confirmation, successful add, and idempotent replay over loopback HTTPS/Streamable HTTP.
- The evidence records the client/version, transport, protocol, certificate assumptions, and unsupported hosted-client profiles.

## Open decisions requiring owner review

- Approve the single operation `list_item_add` as the first mutation and defer calendar/task writes.
- Approve the two-step server-side challenge model and the requirement that `idempotencyKey` be supplied by the client.
- Approve stable `listId` targeting for this child, with list discovery deferred.
- Approve short-lived challenge/result retention limits after the implementation identifies the existing infrastructure abstraction.

## Definition of Done for this plan

- Every #231 acceptance criterion has deterministic evidence or an explicit reviewed limitation.
- The only write path is the confirmed `list_item_add` capability behind the OAuth, scope, and default-off write gates.
- No MCP code bypasses Lists application ownership, household authorization, domain invariants, event logging, or persistence boundaries.
- Confirmation, idempotency, replay, concurrency, cancellation, audit, and safe-error behavior are proven through production-path tests.
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
