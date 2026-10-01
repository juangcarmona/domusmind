---
id: FR-WEB-SETTINGS-SURFACE
type: functional-requirement
title: "Settings with Profile, Household and Preferences sections"
status: draft
derived-from:
  - "UC-WEB-MANAGE-SETTINGS"
  - "UC-FAMILY-UPDATE-MEMBER-DETAILS"
  - "UC-FAMILY-CONFIGURE-HOUSEHOLD-SETTINGS"
  - "BR-WEB-SETTINGS-STAYS-CONFIGURATION"
verification:
  - scenario-ref: "SB-WEB-SETTINGS-OPENS-ON-PROFILE"
uses-terms:
  - "TERM-MEMBER-PROFILE"
  - "TERM-HOUSEHOLD-SETTINGS"
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
provenance:
  source: "openspec/specs/web-app/spec.md (Settings Surface); docs/_legacy/00_product/surfaces/settings.md (Shell, Information Architecture, Default View, Entry Points); interview: product owner decision Q-0054 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST offer a Settings surface, inside the standard shell, with three sections: **Profile** (the person's identity, account details and calendar connections), **Household** (household-level preferences) and **Preferences** (reserved for personal defaults). Settings MUST open on Profile, with the calendar connections section visible without deep navigation (observed: openspec/specs/web-app/spec.md, Settings Surface; docs/_legacy/00_product/surfaces/settings.md, Information Architecture, Default View). Settings MUST be reachable from the navigation, from the account menu and from Agenda links when calendar setup is missing or needs attention (observed: docs/_legacy/00_product/surfaces/settings.md, Entry Points).

The Preferences section is reserved and empty in the baseline; no requirement applies to its content until it is defined (decided: Q-0054; observed: docs/_legacy/00_product/surfaces/settings.md, "personal defaults and future settings seams").

## Rationale

Configuration needs a calm home of its own so that operational surfaces stay clean (observed: experience.md, Surface Roles: Settings).
