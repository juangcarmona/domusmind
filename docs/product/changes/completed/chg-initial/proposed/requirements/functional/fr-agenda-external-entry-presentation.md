---
id: FR-AGENDA-EXTERNAL-ENTRY-PRESENTATION
type: functional-requirement
title: "Present imported entries as read-only and labelled"
status: draft
derived-from:
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
  - "UC-AGENDA-INSPECT-ENTRY"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY"
  - "BR-CALENDAR-EXTERNAL-ENTRY-VISIBILITY"
verification:
  - scenario: "Ana's Outlook stand-up shows a subtle \"Outlook\" cue; selecting it opens read-only detail offering \"Open in Outlook\" (observed: agenda.md, External calendar entry rules)."
  - scenario: "An Outlook entry stored for next month does not appear in today's Day view (observed: agenda.md, entries outside the window are omitted)."
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
  - "TERM-AGENDA"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (External calendar entry rules); docs/_legacy/06_interfaces/external-calendar-api.md (View Agenda in member scope)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

Imported external entries MUST carry a subtle source cue such as "Outlook", open read-only detail that MAY offer "Open in Outlook", never become editable plans, and be omitted when outside the displayed window even if stored (observed: agenda.md).

## Rationale

Keeps a clear line between what the household controls and what comes from outside (inferred).
