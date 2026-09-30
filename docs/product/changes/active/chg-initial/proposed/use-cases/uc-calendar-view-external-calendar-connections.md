---
id: UC-CALENDAR-VIEW-EXTERNAL-CALENDAR-CONNECTIONS
type: use-case
title: "Review external calendar connections and their status"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-SYNC-HORIZON"
provenance:
  source: "docs/_legacy/06_interfaces/external-calendar-api.md (List member connections, Get connection detail); docs/_legacy/06_interfaces/external-calendar-contract-model-catalog.md (connection Status values); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs; interview: product owner decision Q-0049 (E-0158); interview: product owner decision Q-0048 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Goal

Know which external calendars feed one's Agenda and whether they are healthy (observed: external-calendar-api.md, List member connections purpose).

## Trigger

The person opens the integrations part of their settings (observed: external-calendar-api.md, Settings/Profile).

## Preconditions

- The person is a household member with access to DomusMind.

## Main Flow

1. The person opens their connections.
2. DomusMind lists each connection with its provider, account and label, number of selected calendars, horizon, refresh settings, last successful sync, status (one of the code's connection statuses, decided: Q-0049), whether a sync is running and the last error (observed: external-calendar-api.md).
3. The person opens one connection to see its selected and available provider calendars and each calendar's sync window (observed: external-calendar-api.md, Get connection detail).

## Alternative Flows

None recorded in the sources.

## Failure Conditions

- The person asks for someone else's connection details or to act on them: denied (observed: external-calendar-api.md, 403). A household manager sees only a status summary of others' connections (see UC-CALENDAR-REVIEW-HOUSEHOLD-CONNECTION-STATUS; decided: Q-0048).

## Postconditions

- The person knows the state of their connections and can act on them (configure, sync, disconnect).
