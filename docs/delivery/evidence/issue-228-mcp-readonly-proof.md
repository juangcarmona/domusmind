# #228 MCP read-only proof — sanitized transcript

Captured from the in-process Streamable HTTP MCP test host in `McpReadOnlyProofTests`. All identities and records are synthetic and stored only in an isolated EF Core InMemory database. The host binds its fixture requester and household server-side; no production connection string, real credential, or household dataset is loaded.

## Tool discovery

The official MCP client discovers exactly `agenda_get` and `household_members_list` in the test host. Their input/output schemas are asserted in `McpReadOnlyProofTests`; the production-registration test continues to assert an empty catalog.

## Bounded household timeline

Request:

```json
{
  "from": "2026-10-20",
  "to": "2026-10-31"
}
```

Sanitized structured result (fixture member IDs and nonessential data omitted by the projection):

```json
{
  "events": [
    {
      "title": "Family A appointment",
      "date": "2026-10-20",
      "time": "10:00",
      "endDate": null,
      "endTime": null,
      "status": "Scheduled",
      "participants": [{ "displayName": "Alex" }]
    },
    {
      "title": "Family A end boundary",
      "date": "2026-10-31",
      "time": "10:00",
      "endDate": null,
      "endTime": null,
      "status": "Scheduled",
      "participants": [{ "displayName": "Alex" }]
    }
  ],
  "listItems": [
    {
      "listName": "Household list",
      "itemName": "Synthetic item",
      "dueDate": "2026-10-25",
      "reminder": null,
      "checked": false,
      "importance": false
    }
  ],
  "notes": [
    {
      "text": "Family A note",
      "date": "2026-10-22",
      "createdAtUtc": "2026-10-01T00:00:00Z"
    }
  ]
}
```

The fixture also contains a Family B event/note, an out-of-range Family A event, and internal IDs/area data. Tests verify none appear in this response. A separate 31-calendar-date request succeeds and includes its final-day boundary event.

## Minimal member directory

Request: `{}`

Sanitized result:

```json
{
  "members": [
    { "memberId": "<synthetic-member-id-1>", "name": "Alex", "role": "Adult" },
    { "memberId": "<synthetic-member-id-2>", "name": "Preferred Alex", "role": "Adult" },
    { "memberId": "<synthetic-member-id-3>", "name": "Alex", "role": "Child" }
  ]
}
```

The two `Alex` entries remain distinct by member ID. Linked-account identifiers, email, birth date, access state, and all Family B members are absent.

## Invalid input and isolation

For an invalid, reversed, or overlong date range, the structured tool result is:

```json
{
  "code": "INVALID_INPUT",
  "message": "Provide valid inclusive yyyy-MM-dd dates no more than 31 calendar days apart."
}
```

The client also sends unrecognized `familyId` and `userId` values. They do not enter the tool schema or replace the fixture-bound identity. A direct Application query for the second synthetic household is denied by the test authorization service. The integration tests also verify sanitized unexpected errors, MCP cancellation propagation, exact response-field sets, and that query execution leaves no tracked state changes.
