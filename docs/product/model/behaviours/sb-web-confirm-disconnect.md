---
id: SB-WEB-CONFIRM-DISCONNECT
type: structured-behaviour
title: Confirming disconnect removes the imported entries
status: active
illustrates:
  - UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR
given:
  - A member owns an active Outlook connection.
  - The disconnection confirmation is open.
when: The member confirms disconnection.
then:
  - The connection is removed.
  - Its imported entries no longer appear in the Agenda.
  - Household Plans are unchanged.
uses-terms:
  - TERM-EXTERNAL-CALENDAR-CONNECTION
  - TERM-EXTERNAL-CALENDAR-ENTRY
provenance:
  source: "openspec/specs/web-app/spec.md (Outlook Calendar Connection Management: Scenario User disconnects an Outlook connection)"
  confidence: high
  recovered-from: documentation
covers-transition:
  lifecycle: LC-EXTERNAL-CALENDAR-CONNECTION
  transition: DISCONNECT
---

## Intent

Carry out the disconnection outcome the member has just confirmed.

## Boundaries

The example starts after the confirmation has explained the outcome.
