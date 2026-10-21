---
id: FR-MCP-EXPOSE-HOUSEHOLD-CAPABILITIES
type: functional-requirement
title: Make selected household capabilities available to connected assistants
status: draft
derived-from:
  - UC-MCP-USE-HOUSEHOLD-CAPABILITIES
verification:
  - id: READ-ONLY-ACCESS
    scenario: A connected assistant retrieves the agenda or pending tasks for the authorized household/member scope, and receives no information outside that scope.
  - id: SAFE-HOUSEHOLD-WRITE
    scenario: A connected assistant adds a requested item to an existing list only after required details and confirmation are present, and the existing Lists rules determine the result.
  - id: NO-IMPLICIT-CAPABILITY
    scenario: A household operation not explicitly included in the supported capability set cannot be invoked through the assistant connection.
---

## Requirement

DomusMind MUST allow a household member to use a compatible external assistant to invoke a deliberately selected set of semantically meaningful household capabilities. The supported set MUST prioritize useful agenda, task, list, meal-planning, routine, area, calendar-plan, and household-member workflows based on their value, current application support, domain ownership, permission, ambiguity, confirmation needs, and delivery effort; not every candidate or API operation is required to be exposed.

Each capability MUST use the owning product area's existing information and operations, preserve its domain invariants, distinguish read-only from state-changing access, and return only data the invoking member can access. Supported writes MUST require the information and explicit member confirmation appropriate to their consequence and the connected client's ability to confirm. The client connection MUST be individually revocable and have no authority beyond the member's current household and capability access.

## Rationale

The product is more useful when members can express household questions and actions naturally, but only an explicit, domain-aware capability boundary can make this safe and maintainable. A prioritized semantic interface preserves existing ownership and avoids turning transport endpoints into a second product API.
