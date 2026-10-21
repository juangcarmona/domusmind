---
id: UC-MCP-USE-HOUSEHOLD-CAPABILITIES
type: use-case
title: Use an external assistant to access household capabilities
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
---

## Goal

A household member uses a compatible external assistant to understand or coordinate household activity without leaving the assistant they chose.

## Trigger

The member connects an assistant and asks a household question or requests a supported household action in natural language.

## Preconditions

- The member has authenticated to DomusMind and granted the client access to the DomusMind resource.
- The member has access to the selected household and the requested capability under current household rules.
- The requested operation is in the supported, product-defined capability set.

## Main Flow

1. The member connects a compatible assistant to DomusMind and reviews the requested read and/or write access.
2. The member asks a question or requests an action, such as asking what is happening at home today, asking what tasks remain this week, adding an item to an existing list, or creating a plan for a named household member.
3. The assistant selects a supported, semantically meaningful operation and supplies the relevant household/member and time context.
4. DomusMind resolves the authenticated user to the member and household context, verifies current access and the operation's permission, and applies the existing product-area rules.
5. For a read, DomusMind returns only the information the member may access. For a write, DomusMind first resolves required details and obtains explicit confirmation where the operation or client experience requires it, then reports the outcome.

## Alternative Flows

- The assistant asks a concise follow-up when the member, household, date/time, location, list, or target item is ambiguous.
- The member grants only read access; supported reads remain available and writes are denied or require a separate grant.
- The member declines or later revokes access; the assistant can no longer access the protected household capability.
- A client that cannot use remote HTTP authorization may use a separately assessed local integration path, which does not inherit the remote OAuth assumptions.

## Failure Conditions

- Authentication, token audience, consent, scope, household membership, member access, or current domain authorization is missing, expired, revoked, or invalid; no protected data is returned and no state changes.
- The requested operation is unsupported, ambiguous, malformed, outside the member's permissions, or violates a domain invariant; DomusMind returns a structured and safe explanation without making a partial or guessed change.
- A consequential write is not confirmed, is repeated, or loses its authorization before execution; it is not applied more than once and the client receives a clear outcome.
- The client or transport is unavailable; the member receives a failure that does not expose household data, tokens, or internal persistence details.

## Postconditions

- A successful read returns an authorized, bounded result from the appropriate existing household read model.
- A successful write is performed by the appropriate existing context-owned operation and records an auditable actor/client attribution.
- An unsuccessful or unconfirmed operation has not bypassed authorization, changed household state, or disclosed protected data.
