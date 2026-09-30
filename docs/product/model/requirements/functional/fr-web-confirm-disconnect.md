---
id: FR-WEB-CONFIRM-DISCONNECT
type: functional-requirement
title: "Confirm before disconnecting a calendar"
status: draft
derived-from:
  - "UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR"
  - "UC-WEB-MANAGE-SETTINGS"
verification:
  - scenario-ref: "SB-WEB-DISCONNECT-CONFIRMATION"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/web-app/spec.md (Outlook Calendar Connection Management); docs/_legacy/00_product/surfaces/settings.md (Interaction Rules: Disconnect)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

Disconnecting an Outlook connection MUST require an explicit confirmation step that states the outcome: imported Outlook entries will disappear from the Agenda and native household plans are unaffected (observed: openspec/specs/web-app/spec.md, Outlook Calendar Connection Management: Disconnect; docs/_legacy/00_product/surfaces/settings.md, Disconnect).

## Rationale

Disconnection is destructive for what the person sees in their Agenda; the confirmation prevents surprises (inferred; destructive actions use a modal, web-app spec Inspector and Modal Usage).
