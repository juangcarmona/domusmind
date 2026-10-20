# Production confirmed-write MCP contract (#231)

This contract defines the first write capability after the authorized read-only release. It is available only when `Mcp:Capabilities:HouseholdWrites=true`, `Mcp:OAuth:Enabled=true`, `Mcp:RequireOAuth=true`, and the matching write scope is enabled. The write flag defaults to false. The capability is not a generic API gateway.

## `list_item_add`

Purpose: add one item to an existing household-shared list after explicit server-verified confirmation.

### Input

```json
{
  "listId": "00000000-0000-0000-0000-000000000000",
  "name": "Milk",
  "quantity": "2",
  "note": "Whole milk",
  "idempotencyKey": "client-operation-2026-10-20-001",
  "confirmationToken": "opaque-server-issued-token"
}
```

Required: `listId`, `name`, and `idempotencyKey`. Optional: `quantity`, `note`, and `confirmationToken`. Exact maximum lengths and token syntax are implementation-contract details and must be fixed before implementation. No household, user, member, scope, command, prompt, or persistence selector is accepted.

The server derives the authenticated user, client, and household from the validated OAuth request context. The stable list ID is supplied by the caller; this child does not expose list discovery or name-based resolution.

### Unconfirmed result

Without a valid confirmation token, the tool must not invoke `AddItemToListCommand` or mutate the database. It returns a structured result equivalent to:

```json
{
  "status": "CONFIRMATION_REQUIRED",
  "confirmationToken": "opaque-server-issued-token",
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

The action summary is reviewable by a human. It contains no hidden fields and no token in audit logs. The exact response may redact or omit the token from client-visible text while retaining it as a structured field.

### Confirmed result

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

The result is a minimized projection of `AddItemToListResponse`. It does not expose internal event, account, authorization, or persistence details beyond the item/list identifiers needed for client reconciliation.

## Confirmation and idempotency rules

- A confirmation challenge is bound to the authenticated subject, client ID, household, list ID, canonical payload, and idempotency key.
- The challenge expires after a short configured lifetime and is single-use.
- A changed payload, actor, client, household, list, or idempotency key returns `CONFIRMATION_INVALID` and does not write.
- A consumed or concurrently reused challenge cannot create another item.
- A completed idempotency key returns the documented original result or `DUPLICATE_OPERATION`; it never silently creates a second item.
- Token contents are opaque to clients, are not logged, and are protected at rest if persisted.
- Current OAuth validity and household membership are checked again at confirmation time.

## Errors

Structured tool errors contain only `code` and `message` plus, where explicitly required, a safe `status` field. Initial codes are:

- `INVALID_INPUT`
- `ACCESS_DENIED`
- `CONFIRMATION_REQUIRED`
- `CONFIRMATION_INVALID`
- `DUPLICATE_OPERATION`
- `INTERNAL_ERROR`

Messages never include tokens, prompts, raw item content, SQL, exception types, storage details, or unrelated household data. Cancellation propagates and does not become a successful write.

## Audit

The adapter records fixed capability name `list_item_add`, trusted subject/client/household identifiers, correlation, confirmation outcome, idempotency outcome, and final result class. It does not record confirmation tokens, raw prompts, item names, quantity, notes, request headers, or response content.

## Exclusions

This contract does not expose calendar or task writes, list creation/update/deletion, item toggling, temporal fields, importance, repeat rules, list discovery, arbitrary CRUD, resources, prompts, or a generic command proxy. Hosted ChatGPT/Claude compatibility is not claimed by this child.
