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

Input schema (the only declared parameters are `from` and `to`):

```json
{
  "type": "object",
  "properties": {
    "from": { "type": "string" },
    "to": { "type": "string" }
  },
  "required": ["from", "to"]
}
```

- Both fields are required strings in exact `yyyy-MM-dd` form, representing Gregorian calendar dates. No locale-based parsing or timezone inference is permitted.
- `from` and `to` are inclusive. The interval may cover no more than 31 calendar dates.
- Missing/partial bounds fail MCP SDK argument binding safely. Invalid or ambiguous date strings, reversed ranges, and ranges longer than 31 dates return a structured application invalid-input error.
- Household and requester identity come only from the synthetic test fixture, never from tool input.
- Structured result schema. It advertises the exact projection properties; the SDK-generated schema does not include `additionalProperties`, so contract tests also assert that runtime responses contain exactly these fields:

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
              "required": ["displayName"]
            }
          }
        },
        "required": ["title", "date", "time", "endDate", "endTime", "status", "participants"]
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
        "required": ["listName", "itemName", "dueDate", "reminder", "checked", "importance"]
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
        "required": ["text", "date", "createdAtUtc"]
      }
    }
  },
  "required": ["events", "listItems", "notes"]
}
```

The projection intentionally omits calendar/list/note persistence IDs, area IDs, event color, list-item IDs, and all account metadata. It includes note text and item names because those are the content of the requested timeline/list read; fixtures remain synthetic.

## `household_members_list`

Description: List the members of the synthetic household associated with the test identity.

Input: no fields. Household and requester identity come only from the test fixture.

Input schema: `{ "type": "object", "properties": {}, "required": [] }`. The tool has no caller-supplied identity or selector inputs.

Each result contains only:

- `memberId`: stable member identifier, used to distinguish people with the same display name.
- `name`: display name.
- `role`: household role, if present in the existing query result.

Linked-account identifiers, email addresses, birth dates, invitation/access state, and other account metadata are excluded. Duplicate display names remain distinct; the tool does not accept a name as a selector or resolve a person by name.

Structured result schema:

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
        "required": ["memberId", "name", "role"]
      }
    }
  },
  "required": ["members"]
}
```

`name` is the effective display name (`PreferredName` when present, otherwise `Name`) from the existing directory query. `memberId` is retained solely to distinguish same-name members; no name-based lookup is supported.

## Errors and privacy

Invalid date/range input after both string parameters bind is reported with MCP `isError: true` and structured error content matching this schema:

```json
{
  "type": "object",
  "properties": {
    "code": { "type": "string", "enum": ["INVALID_INPUT", "ACCESS_DENIED", "INTERNAL_ERROR"] },
    "message": { "type": "string" }
  },
  "required": ["code", "message"]
}
```

Messages are safe, stable summaries and never include exception messages, stack traces, storage details, identifiers or fixture internals. Cancellation is propagated rather than converted into a successful result. Tool output and logs used as evidence contain synthetic data only.

The MCP SDK performs argument binding before invoking the tool method. Missing required properties and JSON values that cannot be bound to the declared string parameters are therefore returned by the SDK as safe `isError` tool results, rather than the application `INVALID_INPUT` structured envelope. Tests exercise missing, partial, null, and wrong-type inputs through the endpoint and assert failure without leaking exception details. Once both string parameters bind, invalid date syntax and range semantics use the structured `INVALID_INPUT` shape above.

## Example journeys and exclusions

- “What is happening at home from October 20 through October 31?” calls `agenda_get` with explicit dates.
- “Who is in the household?” calls `household_members_list`.
- “What tasks are pending?” is not supported: no complete Tasks-owned read query is available for this proof.
- “What does Juanito have scheduled?” is not supported: member-specific agenda access is excluded because the current query handler does not establish that the requested member belongs to the authorized household.

Tests must compare both registered tool names and descriptions, input schema property/required sets, and complete nested output-schema property/required/nullability/format constraints with this contract. The MCP SDK's generated schemas do not advertise `additionalProperties: false`; tests must also submit an unrecognized household identity property and prove it cannot change fixture-bound identity, and assert successful runtime responses contain exactly the listed output fields. They exercise both example journeys and verify output minimization and the exclusions above. The structured error envelope exposes only a stable error code and safe message; tests assert it reveals no exception or storage details.
