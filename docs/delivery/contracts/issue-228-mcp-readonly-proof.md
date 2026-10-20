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

- Both fields are required strings in exact `yyyy-MM-dd` form, representing Gregorian calendar dates. No locale-based parsing or timezone inference is permitted.
- `from` and `to` are inclusive. The interval may cover no more than 31 calendar dates.
- Missing/partial bounds, invalid or ambiguous dates, reversed ranges, and ranges longer than 31 dates return a structured invalid-input error.
- Household and requester identity come only from the synthetic test fixture, never from tool input.
- The result contains only the timeline item fields needed by the test journeys. It excludes internal persistence identifiers and unrelated account data.

## `household_members_list`

Description: List the members of the synthetic household associated with the test identity.

Input: no fields. Household and requester identity come only from the test fixture.

Each result contains only:

- `memberId`: stable member identifier, used to distinguish people with the same display name.
- `name`: display name.
- `role`: household role, if present in the existing query result.

Linked-account identifiers, email addresses, birth dates, invitation/access state, and other account metadata are excluded. Duplicate display names remain distinct; the tool does not accept a name as a selector or resolve a person by name.

## Errors and privacy

Invalid input is reported as a structured tool error with a stable code and safe explanation; unexpected handler failures do not disclose exception messages, stack traces, storage details, or fixture internals. Cancellation is propagated. Tool output and logs used as evidence contain synthetic data only.

## Example journeys and exclusions

- “What is happening at home from October 20 through October 31?” calls `agenda_get` with explicit dates.
- “Who is in the household?” calls `household_members_list`.
- “What tasks are pending?” is not supported: no complete Tasks-owned read query is available for this proof.
- “What does Juanito have scheduled?” is not supported: member-specific agenda access is excluded because the current query handler does not establish that the requested member belongs to the authorized household.

Tests must compare the registered tool names, descriptions, and input schemas with this contract, exercise both example journeys, and verify output-field minimization and the exclusions above.
