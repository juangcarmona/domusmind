---
id: SB-WEB-DISCONNECT-CONFIRMATION
type: structured-behaviour
title: "Disconnecting Outlook asks for confirmation first"
status: draft
illustrates:
  - "UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR"
given:
  - "a person has an active Outlook connection"
when: "the person selects Disconnect and confirms"
then:
  - "a confirmation step is shown before anything is removed"
  - "the confirmation explains that imported Outlook entries will be removed from the Agenda"
  - "after confirmation the connection is removed"
  - "imported entries are no longer shown in the Agenda"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "openspec/specs/web-app/spec.md (Outlook Calendar Connection Management: Scenario User disconnects an Outlook connection)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

The person knows exactly what disconnecting does before it happens.

## Boundaries

The removal itself is specified by the Calendar area (SB-CALENDAR-DISCONNECT).
