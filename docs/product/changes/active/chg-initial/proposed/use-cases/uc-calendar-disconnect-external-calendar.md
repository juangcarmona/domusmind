---
id: UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR
type: use-case
title: "Disconnect an external calendar"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-CALENDAR-DISCONNECT-IS-LOCAL"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS"
  - "BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Disconnection); docs/_legacy/06_interfaces/external-calendar-api.md (Disconnect connection); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs (Disconnect)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Stop showing an external calendar in DomusMind and remove DomusMind's copy of it (observed: calendar spec).

## Trigger

The person disconnects a connection in their settings.

## Preconditions

- The person owns the connection (observed: calendar spec).

## Main Flow

1. The person disconnects the connection.
2. DomusMind removes the connection, its imported entries and the locally held access (observed: calendar spec).
3. Its entries disappear from the person's Agenda immediately (observed: calendar spec).

## Alternative Flows

None recorded in the sources.

## Failure Conditions

- The connection does not exist: rejected (observed: calendar spec).
- The connection is in a sync step that cannot be interrupted: rejected (observed: external-calendar-api.md, 409).

## Postconditions

- No trace of the connection remains in the Agenda; the provider account and every plan are untouched (observed: calendar spec).
