---
id: SB-AGENDA-MEMBER-INCLUDES-IMPORTED
type: structured-behaviour
title: "A member's Agenda shows their plans and their Outlook entries"
status: draft
illustrates:
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
  - "BR-CALENDAR-EXTERNAL-ENTRY-VISIBILITY"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY"
given:
  - "a member participates in a plan"
  - "the member has an active Outlook connection with entries on that date"
when: "the member's Agenda is requested for that date"
then:
  - "both the plan and the external entries appear"
  - "the external entries carry a source label and are read-only"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-HOUSEHOLD-TIMELINE"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
  - "TERM-PLAN-PARTICIPANT"
provenance:
  source: "openspec/specs/calendar/spec.md (Member Agenda Projection: Member agenda includes personal events and imported entries)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that a person sees household and external commitments together.

## Boundaries

None.
