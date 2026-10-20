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

When a follow-up request supplies an approved reference and the exact same payload, the operation executes. A pending reference returns `APPROVAL_PENDING`; it does not create a new approval. A cancelled, expired, unknown, malformed, mismatched, or consumed reference returns `CONFIRMATION_INVALID`; it does not create a fresh approval.

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

## Approval, authorization, and idempotency rules

- An approval record is bound to the authenticated subject, OAuth client ID, granted household, list ID, canonical payload hash, and idempotency key.
- Approval is a trusted server-observed state transition. The MCP tool cannot approve its own request.
- The approval expires after a short configured lifetime, is single-use, and is consumed atomically with successful execution.
- Current OAuth validity, `mcp.write`, account state, household membership, and target-list/granted-household equality are checked at approval and execution time.
- The Lists application boundary receives the expected granted household and rejects a target list in another household before the existing membership authorization check.
- The durable idempotency namespace is `mcp:list_item_add:v1`, unique by actor/client/granted household/key. The canonical payload hash is stored with the key.
- A completed key returns the stored minimized result while retained. An in-progress key returns `OPERATION_IN_PROGRESS`. The same key with a different payload returns `IDEMPOTENCY_CONFLICT`. No case creates a second item.
- Approval consumption, idempotency claim/completion, list mutation, Lists event log entry, and the completion result commit in one relational transaction. The shared post-commit list notification is published only after commit.
- Cancellation before commit rolls back the transaction. Cancellation after commit is reported as indeterminate/committed; retry resolves from idempotency state.
- Result retention and anti-replay tombstone retention are configured and documented before release. Cleanup never reuses a live key for a different payload.

## Errors

Structured tool errors contain `code` and a safe `message`, plus only the explicitly documented status/reference fields. Initial codes are:

- `INVALID_INPUT` — malformed schema, unknown field, invalid GUID, blank/oversized field, or SDK binding failure.
- `ACCESS_DENIED` — invalid audience, missing `mcp.write`, disabled subject, revoked grant, removed membership, or target-list/granted-household mismatch.
- `CONFIRMATION_REQUIRED` — valid request created a pending approval.
- `APPROVAL_PENDING` — the supplied approval is still awaiting the trusted human transition.
- `CONFIRMATION_INVALID` — unknown, malformed, expired, cancelled, consumed, or mismatched approval reference. The initial request with no reference is the documented `CONFIRMATION_REQUIRED` path.
- `IDEMPOTENCY_CONFLICT` — key is already bound to a different canonical payload.
- `OPERATION_IN_PROGRESS` — an equivalent execution owns the idempotency identity; retry after the documented interval.
- `DUPLICATE_OPERATION` — equivalent operation already completed but its full result is no longer returned by policy.
- `INTERNAL_ERROR` — unexpected failure, with a trace correlation that contains no sensitive details.

Messages never include approval references, tokens, prompts, raw item content, SQL, exception types, storage details, or unrelated household data. The tool does not turn cancellation into a successful write.

## Audit and notification

The adapter records fixed capability name `list_item_add`, trusted subject/client/household identifiers, correlation, approval outcome, idempotency outcome, and final result class. It does not record approval references, raw prompts, item names, quantity, notes, request headers, or response content.

A successful mutation publishes one existing list-change notification after the transaction commits. Rejected, pending, cancelled, rolled-back, and non-mutating duplicate/conflict paths publish none.

## Exclusions

This contract does not expose calendar or task writes, list creation/update/deletion, item toggling, temporal fields, importance, repeat rules, list discovery, arbitrary CRUD, resources, prompts, or a generic command proxy. Hosted ChatGPT/Claude compatibility is not claimed by this child.
