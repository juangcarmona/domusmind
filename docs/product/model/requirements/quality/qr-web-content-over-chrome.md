---
id: QR-WEB-CONTENT-OVER-CHROME
type: quality-requirement
title: "Household content dominates the interface"
status: draft
quality-attribute: "usability"
applies-to:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "UC-LISTS-OPEN-LIST"
  - "UC-AREAS-REVIEW-OWNERSHIP"
  - "UC-WEB-MANAGE-SETTINGS"
verification:
  - scenario: "On each primary surface at desktop and phone width, the page header is compact and household items occupy most of the first screen (inferred from surface-system.md, Density Rules, Success Criteria)."
uses-terms:
  - "TERM-AGENDA"
provenance:
  source: "docs/_legacy/00_product/surface-system.md (Core Direction, Visual Tone, Density Rules, Anti-Patterns, Success Criteria); docs/_legacy/00_product/experience.md (Success Criteria)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

Surfaces MUST be dense, calm and content-first: compact headers, low component height, no decorative containers or cards inside cards, restrained accents, and more useful household state in view than navigation and decoration (observed: docs/_legacy/00_product/surface-system.md, Core Direction, Density Rules, Visual Tone, Anti-Patterns; docs/_legacy/00_product/experience.md, Success Criteria "content dominates chrome").

## Measurement

On the first screen of each covered surface, measure the share of area showing household items versus headers, navigation and decoration. The sources give no number; a proposed pass threshold is that household items take the majority of the first screen on desktop and phone (inferred, to be confirmed).
