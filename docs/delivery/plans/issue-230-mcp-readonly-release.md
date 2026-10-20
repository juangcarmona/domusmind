# Plan: release the first authorized read-only household MCP workflows

- Work item: [#230](https://github.com/juangcarmona/domusmind/issues/230)
- Parent Epic: [#224](https://github.com/juangcarmona/domusmind/issues/224)
- Product Change: [CHG-MCP-HOUSEHOLD-INTEGRATION-001](../../product/changes/active/chg-mcp-household-integration/change.md)
- Definition PR: [#225](https://github.com/juangcarmona/domusmind/pull/225)
- Depends on merged: [#228](https://github.com/juangcarmona/domusmind/issues/228), [#229](https://github.com/juangcarmona/domusmind/issues/229)

## Outcome

Release the first externally useful DomusMind MCP read surface: an authenticated household member can ask for a bounded household agenda and resolve household member identities, receiving only authorized and deliberately minimized data. The surface is read-only, household-scoped, and available only when the MCP OAuth boundary is enabled.

## Product and security boundaries

- The MCP client is an interface to existing DomusMind capabilities, not a source of truth or a second authorization model.
- The OAuth/OIDC validation middleware from #229 remains the resource boundary. Tool code trusts only the validated request identity supplied by the server context; it accepts no user, member, or household identity arguments.
- Application query handlers remain responsible for household authorization. Tools dispatch `GetFamilyTimelineQuery` and `GetFamilyMembersQuery`; they do not access EF Core, persistence, or domain entities directly.
- `agenda_get` is a bounded date-range read of the existing unified family timeline projection. It deliberately returns only the fields in the production contract and omits persistence IDs, account metadata, and internal authorization fields.
- `household_members_list` returns only stable member ID, effective display name, and household role. It never returns email, account IDs, birth dates, invitation/access state, or management flags.
- `tasks_list` is not exposed because the Tasks context has no complete, owned pending-task query suitable for this release. The list/meal/routines expansion and member-specific agenda remain later children.
- Write tools, prompt injection defenses for action-taking, destructive actions, arbitrary CRUD, raw chat history, and MCP resources/prompts are out of scope.

## Design

### Tool registration and identity

1. Add production tools under `src/backend/DomusMind.Mcp`, not under the API test project. Register them only when `Mcp:Capabilities:HouseholdReadOnly=true`, `Mcp:OAuth:Enabled=true`, and `Mcp:RequireOAuth=true`; the capability flag defaults to false and is a separate release gate from OAuth setup. The default/disabled production catalog remains empty, so upgrading #230 cannot automatically expose household data. Enabling the capability requires the owner-approved release checklist, accepted Product Change/ADR state, production key/configuration review, and the documented HTTPS deployment gate.
2. Add a small MCP request-identity accessor that reads the validated `ClaimsPrincipal` from the current HTTP context and returns the trusted `AuthUser` subject plus the single household claim bound by #229. Reject missing or malformed context with a safe authorization error.
3. Keep the tool methods thin. They validate tool-owned input, dispatch existing typed application queries, map to the production contract, and translate known application errors to stable structured MCP errors. They must not accept identity selectors or call persistence.
4. Preserve `ReadOnly = true` metadata and stable descriptions. Do not register a pending-task placeholder or a generic query tool.

The MCP adapter records capability-level security audit events. It records only the fixed allowlisted capability name (`agenda_get` or `household_members_list`), trusted subject/client/household identifiers, correlation, and result class (`success` or `tool_error`). It never records raw arguments, response content, headers, tokens, or arbitrary request names. This supplements the existing transport audit because MCP tool errors can be returned with HTTP 200 and must remain attributable. The current official SDK registration surface does not provide a generic filter around the ordinary registered tool collection, so this release emits the same allowlisted audit event at each production tool boundary; the deviation is deliberate and covered by the tool-path tests.

### `agenda_get`

- Required `from` and `to` fields use exact Gregorian `yyyy-MM-dd` date-only values, inclusive, with a maximum 31-calendar-day window.
- The server supplies `FamilyId` and `RequestedByUserId` from the validated request identity. Tool input cannot override either value.
- Dispatch `GetFamilyTimelineQuery` and map the result to events, temporally relevant list items, and notes using the production field allowlist in the contract. Preserve invariant Gregorian date formatting independent of server culture.
- Date-only bounds use the existing handler's explicit semantics: calendar-event and note dates are compared as `DateOnly`; list-item reminders are compared against UTC start-of-day/end-of-day bounds because the current query contract stores reminder instants in UTC. No client timezone selector is accepted in this release. Tests cover reminders immediately before, at, and after the UTC boundaries so this limitation is visible rather than inferred.
- Invalid/ambiguous/reversed/overlong ranges return the stable `INVALID_INPUT` error. Query authorization failures return `ACCESS_DENIED`; unexpected failures return `INTERNAL_ERROR` without exception, storage, token, or identity details.
- The operation is cancellation-aware and must not write or mutate state.

### `household_members_list`

- Accept no caller-supplied selector or identity fields.
- Dispatch `GetFamilyMembersQuery` using the validated household/user context.
- Project `MemberDirectoryItemResponse` to `memberId`, effective display name (`PreferredName` when present, otherwise `Name`), and `role`. Preserve duplicate display names as distinct stable IDs; do not add name-based lookup.
- Return only safe structured errors and no account/private metadata.

### HTTPS client evidence

Use a custom C# MCP client built on the official .NET SDK as the release evidence client. Run the API with Kestrel on a loopback HTTPS endpoint and a test certificate, complete protected-resource discovery and OAuth Authorization Code + PKCE against the test authorization user, exchange the code for a resource-bound access token, then call both production tools through Streamable HTTP against real family, member, and calendar fixtures. The client tests prove invariant dates and minimal outputs, and prove that disabled accounts and removed household membership are denied; the existing OAuth integration journey also proves revoked consent denies the same protected endpoint.

This demonstrates the remote HTTPS contract without claiming ChatGPT or Claude compatibility. Hosted-client interoperability, local STDIO, private-network deployment, and client-specific UI behavior remain follow-up release gates.

## Ordered implementation tasks

1. Add the production contract and tool result/error models, with exact input/output schema and field allowlists documented in `docs/delivery/contracts/issue-230-mcp-readonly-release.md`.
2. Add the validated MCP request-identity accessor and safe structured error mapping inside the MCP adapter boundary.
3. Implement `agenda_get` and `household_members_list` as production read-only tools using the existing application query dispatcher and handlers.
4. Change MCP composition so the production catalog is populated only behind the separate `Mcp:Capabilities:HouseholdReadOnly` release gate plus the OAuth-required configuration gate; retain an empty catalog when any gate is disabled or incomplete.
5. Extend the OAuth integration host with isolated household/member/calendar fixtures, the real application query handlers, the production tools, and a Kestrel HTTPS official-SDK client journey. Do not use production data or credentials.
6. Add production-path contract and security tests for schemas, field minimization, identity non-overridability, disabled/removed/revoked access, invalid dates, invariant culture, cancellation, safe errors, audit redaction, no writes, and the deliberate absence of `tasks_list` and member-specific agenda. Keep the synthetic proof-tool tests separate from production registration.
7. Update the operator/client documentation and this plan with tested client/version, HTTPS assumptions, unsupported clients, and the Tasks/member-agenda limitations. Do not alter canonical domain specifications because this release reuses existing query ownership.

## Test plan

### Contract and tool tests

- Tool discovery exposes exactly `agenda_get` and `household_members_list` when the protected release configuration is enabled.
- Disabled capability release, disabled OAuth, or missing required protection leaves the production catalog empty.
- Input schemas require exactly `from` and `to` for `agenda_get` and no fields for `household_members_list`; identity properties are absent.
- Output schemas and successful responses contain exactly the documented allowlisted fields, including nullability and date formats.
- `tasks_list`, member-specific agenda, write tools, resources, and prompts are absent.

### Authorization and privacy tests

- An authorized member receives only their current household's timeline/member results through the real application handlers.
- A token for household A cannot select household B through extra input fields, malformed JSON, or prompts.
- Disabled accounts, revoked consent/tokens, and removed household membership fail before tool execution completes.
- Member output excludes account IDs, emails, birth dates, access status, manager/edit flags, and avatar metadata; agenda output excludes persistence and internal authorization IDs.
- Duplicate member display names remain distinct by stable member ID.

### Input, reliability, and side-effect tests

- Reject missing, null, wrong-type, non-ISO, invalid, ambiguous, reversed, and overlong date ranges safely.
- Preserve cancellation through the query dispatcher and do not convert cancellation into a successful response.
- Translate known authorization/validation failures into stable structured errors; unexpected failures do not leak exception or storage details.
- Prove no writes occur through either read tool using a write guard and before/after state assertions.
- Audit events identify only the two fixed capability names and distinguish success, tool-level error, denial, and failed execution without recording raw arguments or response content.
- Reminder filtering is verified immediately before, at, and after the UTC date-window boundaries.

### Remote interoperability evidence

- Complete the selected custom official-SDK client journey over real loopback HTTPS and Streamable HTTP, including metadata discovery, OAuth PKCE, token exchange, tool discovery, both calls, and safe rejection after authorization changes.
- Record client name/version, transport, protocol version, certificate/test-host assumptions, and unsupported client profiles in the PR and FF evidence.

## Definition of Done for this plan

- Every #230 acceptance criterion has deterministic test evidence or an explicit reviewed limitation.
- No production read tool bypasses the OAuth/resource boundary, current household authorization, or application handler ownership.
- The release catalog is exactly the two authorized read tools; no write or incomplete Tasks capability is exposed.
- Backend build/test, web checks required by CI, dependency/secret scan review, and CodeQL pass on the final head; any inherited advisories remain clearly documented rather than silently ignored.
- Documentation and production contract match the implementation and explain the supported HTTPS client evidence and known exclusions.
- The final PR remains explicit that ChatGPT/Claude/STDIO/private-hosted interoperability is not claimed by this slice.

## Known limitations and follow-ups

- Pending Tasks requires a complete Tasks-owned query and is deferred to a later capability child.
- Member-specific agenda requires a handler-level proof that the requested member belongs to the authorized household and remains deferred.
- The custom HTTPS client proves the protocol boundary but does not certify hosted ChatGPT, Claude, Claude Code, or self-hosted-agent behavior.
- Durable audit retention, production key management/rotation, and remote deployment are operational release gates owned by later children.

## References

- [Production read-only contract](../contracts/issue-230-mcp-readonly-release.md)
- [#228 synthetic MCP proof plan](issue-228-readonly-mcp-proof.md)
- [#229 OAuth/OIDC authorization plan](issue-229-mcp-oauth-authorization.md)
- [MCP authorization](https://modelcontextprotocol.io/specification/2026-07-28/basic/authorization)
- [MCP transports](https://modelcontextprotocol.io/specification/2026-07-28/basic/transports)
- [Official .NET MCP SDK](https://github.com/modelcontextprotocol/csharp-sdk)
- [RFC 9728 protected resource metadata](https://www.rfc-editor.org/rfc/rfc9728.html)
- [RFC 7636 PKCE](https://www.rfc-editor.org/rfc/rfc7636.html)
