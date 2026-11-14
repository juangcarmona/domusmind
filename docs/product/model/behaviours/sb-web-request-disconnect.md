---
id: SB-WEB-REQUEST-DISCONNECT
type: structured-behaviour
title: Disconnect asks the member to confirm
status: active
illustrates:
  - UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR
given:
  - a person has an active Outlook connection
when: The person selects Disconnect.
then:
  - A confirmation step is shown before anything is removed.
  - The confirmation explains that imported Outlook entries will disappear from the Agenda and household Plans are unaffected.
uses-terms:
  - TERM-EXTERNAL-CALENDAR-CONNECTION
  - TERM-EXTERNAL-CALENDAR-ENTRY
provenance:
  source: "openspec/specs/web-app/spec.md (Outlook Calendar Connection Management: Scenario User disconnects an Outlook connection)"
  confidence: high
  recovered-from: documentation
---

## Intent

Explain the local data removal before the member authorizes it.

## Boundaries

The member has not confirmed removal. The Calendar area owns the disconnection itself.
