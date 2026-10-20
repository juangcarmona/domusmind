# Production read-only MCP contract (#230)

This contract defines the first production MCP capability release. It is available only when the explicit release flag `Mcp:Capabilities:HouseholdReadOnly=true`, `Mcp:OAuth:Enabled=true`, and `Mcp:RequireOAuth=true`; the release flag defaults to false and is separate from OAuth setup. Every result is scoped to the validated OAuth subject and household. It is not a generic API gateway.

## List privacy applicability

The current Lists model has no privacy or owner field; private lists are therefore not representable in this release. The `listItems` projection is limited to the existing household-shared list query and does not claim to implement private-list authorization. The corresponding #230 private-list acceptance criterion is owner-approved as deferred/N/A. A future Lists privacy implementation must define visibility semantics and add MCP-specific positive and denial tests before this contract is expanded.

## `agenda_get`

Description: Read the authorized household agenda for an explicit bounded Gregorian calendar-date range.

Input:

```json
{
  "from": "2026-10-20",
  "to": "2026-10-31"
}
```

Input schema: exactly two required string properties, `from` and `to`, in `yyyy-MM-dd` form. Both bounds are inclusive and the range is at most 31 calendar dates. No household, user, member, timezone, or locale selector is accepted. The server derives identity and uses the existing household authorization boundary.

Output schema:

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

The response omits calendar/list/note persistence IDs, area IDs, colors, participant IDs, account metadata, and authorization internals. Dates remain invariant Gregorian values regardless of server culture. Calendar-event and note dates use `DateOnly`; reminder instants are filtered using UTC start-of-day/end-of-day bounds because that is the current application query contract. No caller timezone selector is accepted.

## `household_members_list`

Description: List the members of the authorized household with stable identity and minimal display information.

Input: no properties. The caller cannot select a household, account, or member.

Output schema:

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

`name` is the effective display name, using `PreferredName` when present and otherwise `Name`. Duplicate names remain distinct through `memberId`; no name-based selector is accepted. Email, linked account IDs, birth dates, access/invitation state, management flags, and avatar metadata are excluded.

## Errors and exclusions

Tool-level errors returned after method binding use a structured result with exactly `code` and `message`. Codes are `INVALID_INPUT`, `ACCESS_DENIED`, and `INTERNAL_ERROR`. Messages are safe summaries and contain no exception, storage, token, household, or account details. The MCP SDK may reject missing, null, or wrong-type arguments during binding before the tool method runs; those failures remain `isError` binding responses and are tested for safe non-disclosure, but are not promised to have the structured `code`/`message` shape. Cancellation propagates as cancellation.

The release does not expose `tasks_list`, member-specific agenda, write tools, resources, prompts, raw database access, or arbitrary API operations. A pending-task question must be reported as unsupported until the Tasks context supplies a complete owned query.

## Authorization

The MCP middleware validates the OAuth issuer, resource/audience, lifetime, scope, current account, and household access before tool execution. Tools derive the same subject and household from the validated request context and re-use the existing application query handlers. Extra JSON properties cannot change the authorized scope.
