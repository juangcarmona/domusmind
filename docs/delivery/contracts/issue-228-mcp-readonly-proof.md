# MCP read-only proof contract (#228)

This is a test-only interoperability contract for #228. It is not a production MCP API commitment. Tools described here are registered only in the API test host and use synthetic fixtures. The production MCP catalog remains empty until authentication and household authorization are delivered and approved in #229.

## `agenda_get`

Description: Read the synthetic household timeline for an explicit, bounded calendar-date range.

Input:

```json
{
  "from": "2026-10-20",
  "to": "2026-10-31"
}
```

Input schema (`additionalProperties: false`):

```json
{
  "type": "object",
  "properties": {
    "from": { "type": "string", "format": "date" },
    "to": { "type": "string", "format": "date" }
  },
  "required": ["from", "to"],
  "additionalProperties": false
}
```

- Both fields are required strings in exact `yyyy-MM-dd` form, representing Gregorian calendar dates. No locale-based parsing or timezone inference is permitted.
- `from` and `to` are inclusive. The interval may cover no more than 31 calendar dates.
- Missing/partial bounds, invalid or ambiguous dates, reversed ranges, and ranges longer than 31 dates return a structured invalid-input error.
- Household and requester identity come only from the synthetic test fixture, never from tool input.
- Structured result schema (`additionalProperties: false` at every object level):

```json
{
  "type": "object",
  "properties": {
    "events": {
      "type": "array",
      "items": {
        "type": "object",
        "properties": {
          "title": { "type": "string" },
          "date": { "type": "string", "format": "date" },
          "time": { "type": ["string", "null"] },
          "endDate": { "type": ["string", "null"], "format": "date" },
          "endTime": { "type": ["string", "null"] },
          "status": { "type": "string" },
          "participants": {
            "type": "array",
            "items": {
              "type": "object",
              "properties": { "displayName": { "type": "string" } },
              "required": ["displayName"],
              "additionalProperties": false
            }
          }
        },
        "required": ["title", "date", "time", "endDate", "endTime", "status", "participants"],
        "additionalProperties": false
      }
    },
    "listItems": {
      "type": "array",
      "items": {
        "type": "object",
        "properties": {
          "listName": { "type": "string" },
          "itemName": { "type": "string" },
          "dueDate": { "type": ["string", "null"], "format": "date" },
          "reminder": { "type": ["string", "null"], "format": "date-time" },
          "checked": { "type": "boolean" },
          "importance": { "type": "boolean" }
        },
        "required": ["listName", "itemName", "dueDate", "reminder", "checked", "importance"],
        "additionalProperties": false
      }
    },
    "notes": {
      "type": "array",
      "items": {
        "type": "object",
        "properties": {
          "text": { "type": "string" },
          "date": { "type": "string", "format": "date" },
          "createdAtUtc": { "type": "string", "format": "date-time" }
        },
        "required": ["text", "date", "createdAtUtc"],
        "additionalProperties": false
      }
    }
  },
  "required": ["events", "listItems", "notes"],
  "additionalProperties": false
}
```

The projection intentionally omits calendar/list/note persistence IDs, area IDs, event color, list-item IDs, and all account metadata. It includes note text and item names because those are the content of the requested timeline/list read; fixtures remain synthetic.

## `household_members_list`

Description: List the members of the synthetic household associated with the test identity.

Input: no fields. Household and requester identity come only from the test fixture.

Input schema: `{ "type": "object", "properties": {}, "required": [], "additionalProperties": false }`.

Each result contains only:

- `memberId`: stable member identifier, used to distinguish people with the same display name.
- `name`: display name.
- `role`: household role, if present in the existing query result.

Linked-account identifiers, email addresses, birth dates, invitation/access state, and other account metadata are excluded. Duplicate display names remain distinct; the tool does not accept a name as a selector or resolve a person by name.

Structured result schema (`additionalProperties: false`):

```json
{
  "type": "object",
  "properties": {
    "members": {
      "type": "array",
      "items": {
        "type": "object",
        "properties": {
          "memberId": { "type": "string", "format": "uuid" },
          "name": { "type": "string" },
          "role": { "type": "string" }
        },
        "required": ["memberId", "name", "role"],
        "additionalProperties": false
      }
    }
  },
  "required": ["members"],
  "additionalProperties": false
}
```

`name` is the effective display name (`PreferredName` when present, otherwise `Name`) from the existing directory query. `memberId` is retained solely to distinguish same-name members; no name-based lookup is supported.

## Errors and privacy

Invalid input is reported with MCP `isError: true` and structured error content matching this schema:

```json
{
  "type": "object",
  "properties": {
    "code": { "type": "string", "enum": ["INVALID_INPUT", "ACCESS_DENIED", "INTERNAL_ERROR"] },
    "message": { "type": "string" }
  },
  "required": ["code", "message"],
  "additionalProperties": false
}
```

Messages are safe, stable summaries and never include exception messages, stack traces, storage details, identifiers or fixture internals. Cancellation is propagated rather than converted into a successful result. Tool output and logs used as evidence contain synthetic data only.

## Example journeys and exclusions

- “What is happening at home from October 20 through October 31?” calls `agenda_get` with explicit dates.
- “Who is in the household?” calls `household_members_list`.
- “What tasks are pending?” is not supported: no complete Tasks-owned read query is available for this proof.
- “What does Juanito have scheduled?” is not supported: member-specific agenda access is excluded because the current query handler does not establish that the requested member belongs to the authorized household.

Tests must compare both registered tool names and descriptions, input schemas, and output schemas with this contract. They must validate success response shapes and reject unexpected properties, exercise both example journeys, and verify output-field minimization and the exclusions above. The structured error envelope must expose only a stable error code and safe message; tests must assert it reveals no exception or storage details.
