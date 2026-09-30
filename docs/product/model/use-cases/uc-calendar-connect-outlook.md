---
id: UC-CALENDAR-CONNECT-OUTLOOK
type: use-case
title: "Connect an Outlook account"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-CALENDAR-ONE-CONNECTION-PER-ACCOUNT"
  - "BR-CALENDAR-OUTLOOK-REQUIRED-ACCESS"
  - "BR-CALENDAR-SUPPORTED-SYNC-HORIZONS"
  - "BR-CALENDAR-DEFAULT-REFRESH-HOURLY"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS"
  - "BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-SYNC-HORIZON"
provenance:
  source: "openspec/specs/calendar/spec.md (Outlook Account Connection); docs/_legacy/04_contexts/calendar.md (ExternalCalendarConnection, Provider and Access Model); docs/_legacy/06_interfaces/external-calendar-api.md (Connect Outlook account); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs (Connect)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

See one's Outlook calendar alongside household life in one's own Agenda, without retyping it (inferred from calendar spec, Purpose).

## Trigger

The person chooses to connect Outlook from their settings (observed: external-calendar-api.md, Settings/Profile).

## Preconditions

- The person is a household member with access to DomusMind (observed: calendar spec).
- The person has completed Microsoft's delegated sign-in and consent (observed: calendar spec).

## Main Flow

1. The person signs in to their Microsoft account and grants access.
2. DomusMind checks calendar read and continued access were granted and that the person has no active connection to that account.
3. DomusMind creates the connection for that person, discovers their provider calendars and may preselect the default one (observed: external-calendar-api.md).
4. DomusMind applies the default settings: yesterday to 90 days ahead, scheduled refresh on every 60 minutes; the connection waits for its first sync (observed: calendar spec).

## Alternative Flows

- 1a. The person gives the connection a label such as "Work Outlook" (observed: external-calendar-api.md).

## Failure Conditions

- Required access was not granted: the connection is rejected (observed: calendar spec).
- The person already has an active connection to that account: rejected (observed: calendar spec).
- The authorization is invalid: rejected (observed: external-calendar-api.md).

## Postconditions

- An external calendar connection exists for the person, pending its initial sync. No plan was created; entries arrive with the next sync (observed: calendar spec).
