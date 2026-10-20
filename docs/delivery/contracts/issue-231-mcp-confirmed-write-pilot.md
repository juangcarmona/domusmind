# Production confirmed-write MCP contract (#231)

This contract defines the first write capability after the authorized read-only release. It is available only when `Mcp:Capabilities:HouseholdWrites=true`, `Mcp:OAuth:Enabled=true`, `Mcp:RequireOAuth=true`, and the request has the explicit `mcp.write` scope. The write flag defaults to false. The capability is not a generic API gateway.

## `list_item_add`

Purpose: add one item to an existing household-shared list after approval by a trusted authenticated DomusMind surface.

### Input schema

```json
{
  "listId": "00000000-0000-0000-0000-000000000000",
  "name": "Milk",
  "quantity": "2",
  "note": "Whole milk",
  "idempotencyKey": "client-operation-2026-10-20-001",
  "approvalRequestId": "opaque-server-issued-reference"
}
```

Required: `listId`, `name`, and `idempotencyKey`. Optional: `quantity`, `note`, and `approvalRequestId`.

- `listId` is a GUID.
- `name` is trimmed, nonblank, and at most 200 characters, matching `ListItemName`.
- `quantity`, when present, is at most 100 characters; `note`, when present, is at most 500 characters, matching the Lists persistence contract. Blank optional values are normalized to null.
- `idempotencyKey` is trimmed, nonblank, ASCII, at most 128 characters, and opaque to the server except for equality and payload binding.
- `approvalRequestId`, when present, is an opaque ASCII reference at most 128 characters. It is an identifier, not a bearer token and not evidence of human approval.
- Canonicalization trims the accepted text fields, normalizes absent/blank optional fields to null, serializes fields in the fixed order `listId`, `name`, `quantity`, `note`, and hashes the UTF-8 representation. It does not apply case folding or locale-dependent transformations.
- No household, user, member, authorization, scope, command, prompt, persistence, or arbitrary extension field is accepted. Unknown fields and SDK binding errors return `INVALID_INPUT` without creating or changing an approval record.

The server derives the authenticated user, client, granted household, and scopes from the validated OAuth request context. The stable list ID is supplied by the caller; this child does not expose list discovery or name-based resolution.

### First request and trusted approval

A valid request without `approvalRequestId` creates only a pending approval record and returns:

```json
{
  "status": "CONFIRMATION_REQUIRED",
  "approvalRequestId": "opaque-server-issued-reference",
  "expiresAt": "2026-10-20T12:00:00Z",
  "action": {
    "operation": "list_item_add",
    "listId": "00000000-0000-0000-0000-000000000000",
    "itemName": "Milk",
    "quantity": "2",
    "note": "Whole milk"
  },
  "idempotencyKey": "client-operation-2026-10-20-001"
}
```

The action summary is the exact canonical operation to be shown by the trusted DomusMind approval surface. It contains no hidden fields. The MCP response does not contain an executable approval token.

The trusted surface authenticates the same OAuth-grant subject for this pilot, applies normal session/CSRF protections, displays the summary, and records an explicit approve or cancel transition. A second MCP call, model-produced confirmation text, client UI, or possession of the OAuth credential plus `approvalRequestId` cannot transition the record.

When a follow-up request supplies an approved reference and the exact same payload, the operation executes. A pending reference returns the `ERROR` variant with `APPROVAL_PENDING`; it does not create a new approval. A cancelled, expired, unknown, malformed, mismatched, or consumed reference returns the `ERROR` variant with `CONFIRMATION_INVALID` when no completed idempotent operation exists; it does not create a fresh approval.

### Completed result schema

```json
{
  "status": "COMPLETED",
  "itemId": "00000000-0000-0000-0000-000000000000",
  "listId": "00000000-0000-0000-0000-000000000000",
  "name": "Milk",
  "checked": false,
  "quantity": "2",
  "note": "Whole milk"
}
```

`quantity` and `note` are nullable in the completed result. The result is a minimized projection of `AddItemToListResponse`; it does not expose internal event, account, authorization, or persistence details beyond the item/list identifiers needed for client reconciliation.

## Result and error variants

The tool returns one of these closed structured-content variants. `CONFIRMATION_REQUIRED` and `COMPLETED` are successful operation results and have MCP `isError=false`. Every rejected or unresolved operation uses the `ERROR` variant and has MCP `isError=true`; its `error.code` is the exact vocabulary below. `APPROVAL_PENDING` is only an error code, never a successful operation status.

```json
{
  "status": "ERROR",
  "error": {
    "code": "APPROVAL_PENDING",
    "message": "The requested operation is awaiting approval."
  }
}
```

The mapping is fixed: `CONFIRMATION_REQUIRED` returns `isError=false` with the approval reference and action summary; `COMPLETED` returns `isError=false` with the completed result; `APPROVAL_PENDING`, `CONFIRMATION_INVALID`, `ACCESS_DENIED`, `INVALID_INPUT`, `IDEMPOTENCY_CONFLICT`, `OPERATION_IN_PROGRESS`, `DUPLICATE_OPERATION`, `CANCELLED`, `OPERATION_STATUS_UNKNOWN`, and `INTERNAL_ERROR` return `isError=true` with `status=ERROR` and `{code,message}`. SDK transport/protocol errors are not remapped into a business result; tool input binding failures are mapped to `INVALID_INPUT` before any approval or domain write.

The `CANCELLED` error variant is returned when cancellation is observed before the execution transaction commits:

```json
{
  "status": "CANCELLED",
  "error": {
    "code": "CANCELLED",
    "message": "The operation was cancelled before commit."
  }
}
```

If commit succeeds while cancellation races, the server returns `COMPLETED` when the response channel remains available. If the channel is already disconnected, no response is possible; a retry with the same idempotency key returns the stored `COMPLETED` result. If infrastructure cannot determine the commit outcome, the server returns:

```json
{
  "status": "OPERATION_STATUS_UNKNOWN",
  "error": {
    "code": "OPERATION_STATUS_UNKNOWN",
    "message": "The operation outcome is unknown. Retry with the same idempotency key."
  }
}
```

This status never authorizes a new execution; the durable idempotency record resolves the outcome on retry.

## Approval, authorization, and idempotency rules

- An approval record is bound to the authenticated subject, OAuth client ID, granted household, list ID, canonical payload hash, and idempotency key.
- Approval is a trusted server-observed state transition. The MCP tool cannot approve its own request.
- The approval expires after a short configured lifetime, is single-use, and is consumed atomically with successful execution.
- Current OAuth validity, `mcp.write`, account state, household membership, and target-list/granted-household equality are checked at approval and execution time.
- The Lists application boundary receives the expected granted household and rejects a target list in another household before the existing membership authorization check.
- The durable idempotency namespace is `mcp:list_item_add:v1`, unique by actor/client/granted household/key. The canonical payload hash is stored with the key.
- After current token/audience/scope/account/household validation and canonical payload validation, idempotency lookup has precedence over approval-reference state: a completed key with the same payload returns its stored minimized result even if the supplied approval reference is now consumed or expired; a same-payload tombstone returns `DUPLICATE_OPERATION`; an in-progress key returns `OPERATION_IN_PROGRESS`; and a different payload returns `IDEMPOTENCY_CONFLICT`. These outcomes are bound to the current actor, client, granted household, namespace, and key, so they do not disclose another principal's result.
- When a pending idempotency record matches the actor, client, granted household, key, and payload hash, the adapter routes to its bound approval record: no approval reference returns the same `CONFIRMATION_REQUIRED` summary and reference; the matching reference in `Pending` state returns `ERROR/APPROVAL_PENDING`; the matching reference in `Approved` state enters the transactional execution path; and a different, malformed, cancelled, expired, unknown, mismatched, or consumed reference returns `ERROR/CONFIRMATION_INVALID`. It never creates a second approval for the same pending identity.
- Only when no completed, tombstoned, in-progress, or pending record matches does the adapter evaluate a supplied approval reference as a new execution candidate; an approved reference may create the pending execution claim, while invalid states return `ERROR/CONFIRMATION_INVALID`.
- No case creates a second item. Response-loss retries therefore return `COMPLETED` from durable state rather than failing merely because the approval was consumed.
- Approval consumption, idempotency claim/completion, list mutation, Lists event log entry, and the completion result commit in one relational transaction. The shared post-commit list notification is published only after commit.
- Cancellation before commit rolls back the transaction and returns the `CANCELLED` variant when the response channel is available. Cancellation after commit returns `COMPLETED` when possible; a disconnected client retries the same key. An infrastructure-unknown commit outcome returns `OPERATION_STATUS_UNKNOWN` and never permits a new execution.
- Result retention and anti-replay tombstone retention are configured and documented before release. Cleanup never reuses a live key for a different payload.

## Errors

Structured tool errors contain `code` and a safe `message`, plus only the explicitly documented status/reference fields. Initial codes are:

- `INVALID_INPUT` — malformed schema, unknown field, invalid GUID, blank/oversized field, or SDK binding failure.
- `ACCESS_DENIED` — invalid audience, missing `mcp.write`, disabled subject, revoked grant, removed membership, or target-list/granted-household mismatch.
- `APPROVAL_PENDING` — the supplied approval is still awaiting the trusted human transition.
- `CONFIRMATION_INVALID` — unknown, malformed, expired, cancelled, consumed, or mismatched approval reference when no completed idempotent operation takes precedence. The initial request with no reference is the documented `CONFIRMATION_REQUIRED` path.
- `IDEMPOTENCY_CONFLICT` — key is already bound to a different canonical payload.
- `OPERATION_IN_PROGRESS` — an equivalent execution owns the idempotency identity; retry after the documented interval.
- `DUPLICATE_OPERATION` — equivalent operation already completed but its full result is no longer returned by policy.
- `CANCELLED` — execution was cancelled before commit.
- `OPERATION_STATUS_UNKNOWN` — the server cannot determine whether the commit completed; retry the same idempotency key.
- `INTERNAL_ERROR` — unexpected failure, with a trace correlation that contains no sensitive details.

Messages never include approval references, tokens, prompts, raw item content, SQL, exception types, storage details, or unrelated household data. The tool does not turn cancellation into a successful write.

## Audit and notification

The adapter records fixed capability name `list_item_add`, trusted subject/client/household identifiers, correlation, approval outcome, idempotency outcome, and final result class. It does not record approval references, raw prompts, item names, quantity, notes, request headers, or response content.

A successful mutation publishes one existing list-change notification after the transaction commits. Rejected, pending, cancelled, rolled-back, and non-mutating duplicate/conflict paths publish none.

## Exclusions

This contract does not expose calendar or task writes, list creation/update/deletion, item toggling, temporal fields, importance, repeat rules, list discovery, arbitrary CRUD, resources, prompts, or a generic command proxy. Hosted ChatGPT/Claude compatibility is not claimed by this child.
