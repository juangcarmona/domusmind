---
id: QR-WEB-MOBILE-SAME-PRODUCT
type: quality-requirement
title: "Mobile is the same product in a smaller frame"
status: draft
quality-attribute: "usability"
applies-to:
  - "UC-WEB-MOVE-BETWEEN-SURFACES"
  - "UC-LISTS-OPEN-LIST"
  - "UC-AREAS-REVIEW-OWNERSHIP"
  - "UC-WEB-MANAGE-SETTINGS"
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
verification:
  - scenario: "At phone width, every capability available on desktop in Agenda, Lists, Areas and Settings is reachable, detail opens as a bottom sheet or pushed section, and creation uses a floating action (observed: web-app spec, Mobile Behavior)."
uses-terms:
  - "TERM-AGENDA"
  - "TERM-LIST"
  - "TERM-AREA"
provenance:
  source: "openspec/specs/web-app/spec.md (Mobile Behavior, App Shell and Navigation); docs/_legacy/00_product/surface-system.md (Shell Model: Mobile, Responsive Rules, Anti-Patterns); docs/_legacy/00_product/experience.md (Success Criteria)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The mobile web app MUST present the same product logic as desktop in collapsed form: compact navigation, compressed headers, swipe-based date navigation on the Agenda, bottom sheets for contextual detail, a floating action for creation, and content given the highest priority. Mobile MUST NOT change the product model or remove capabilities (observed: openspec/specs/web-app/spec.md, Mobile Behavior; docs/_legacy/00_product/surface-system.md, Responsive Rules, Anti-Patterns "mobile redesigns that change the product model").

## Measurement

Walk every use case this requirement applies to at phone width and at desktop width. Pass = every step available on desktop is available on mobile (100% parity) and every item detail opens as a bottom sheet or pushed section rather than a new page (inferred method). The Agenda area measures its own parity in QR-AGENDA-MOBILE-PARITY.
