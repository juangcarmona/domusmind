---
id: BR-AGENDA-WEEK-STARTS-ON-HOUSEHOLD-FIRST-DAY
type: business-rule
title: "The Agenda week starts on the household's first day of week"
status: draft
applies-to:
  - "UC-AGENDA-NAVIGATE"
uses-terms:
  - "TERM-AGENDA-MODE"
  - "TERM-HOUSEHOLD"
  - "TERM-HOUSEHOLD-SETTINGS"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Week); openspec/specs/web-app/spec.md (Agenda Time Modes)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

The Week mode window starts on the household's configured first day of week (observed: agenda.md; web-app spec, Agenda Time Modes), which is one of the household settings (see TERM-HOUSEHOLD-SETTINGS).

## Rationale

Households differ on whether the week starts on Monday or Sunday (inferred).

## Examples

- In a household configured to start on Monday, Week shows Monday to Sunday (inferred example).

## Exceptions

None. The first day of week is set through the household settings (observed: src/backend/DomusMind.Domain/Family/Family.cs, UpdateSettings, as recorded on TERM-HOUSEHOLD-SETTINGS).
