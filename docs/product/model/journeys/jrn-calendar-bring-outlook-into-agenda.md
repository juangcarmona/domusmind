---
id: JRN-CALENDAR-BRING-OUTLOOK-INTO-AGENDA
type: journey
title: "Bring my Outlook calendar into my Agenda"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
steps:
  - use-case: "UC-CALENDAR-CONNECT-OUTLOOK"
  - use-case: "UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR"
  - use-case: "UC-CALENDAR-SYNC-EXTERNAL-CALENDAR"
  - use-case: "UC-AGENDA-VIEW-MEMBER-AGENDA"
  - use-case: "UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS"
provenance:
  source: "openspec/specs/calendar/spec.md (Outlook Account Connection through Member Agenda Projection); docs/_legacy/06_interfaces/external-calendar-api.md (Endpoint Catalog); docs/_legacy/00_product/surfaces/agenda.md (External calendar entry rules)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intended Outcome

A person sees their Outlook commitments next to their household plans, tasks and routines in their own Agenda, kept current automatically, without those commitments leaking into the household picture (observed: calendar spec; agenda.md).

## Entry Conditions

- The person is a household member with access to DomusMind and a Microsoft account with a calendar (observed: calendar spec).

## Journey Narrative

1. The person connects their Outlook account from settings, granting read and continued access; the connection starts with the default horizon of 90 days and hourly refresh (observed: calendar spec).
2. They choose which Outlook calendars to include and optionally change the horizon (observed: calendar spec).
3. The first sync imports entries from the selected calendars; the person can also press "Sync now" (observed: calendar spec, "initial data import occurs through a subsequent sync").
4. They open their Member-scope Agenda and see Outlook entries, labelled and read-only, beside their plans and tasks (observed: calendar spec).
5. From then on the Calendar Sync Scheduler refreshes the connection hourly, with catch-up when they sign in or open their Agenda with stale data (observed: calendar spec).

## Variants and Branches

- Required access not granted, or the account already connected: the journey stops at step 1 (observed: calendar spec).
- Authorization later expires: syncs fail and the connection shows it until the person reconnects (inferred from external-calendar-api.md and ExternalCalendarConnection.cs).
- The person disconnects: every imported entry disappears immediately (observed: calendar spec).
- Who triggers the first sync (the person or automatically after connecting) is not specified (inferred gap).

## Completion Conditions

The person's Member-scope Agenda shows current, read-only Outlook entries for the window, and the household scope still shows none of them (observed: calendar spec).
