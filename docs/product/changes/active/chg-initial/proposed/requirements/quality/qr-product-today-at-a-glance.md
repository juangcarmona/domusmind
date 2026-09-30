---
id: QR-PRODUCT-TODAY-AT-A-GLANCE
type: quality-requirement
title: "The household understands today within seconds"
status: draft
quality-attribute: "usability"
applies-to:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "JRN-PRODUCT-WHAT-MATTERS-TODAY"
verification:
  - scenario: "A household member opens DomusMind and, without any configuration or navigation, sees today's household picture and can say what matters today, what needs attention and who is responsible (observed: web-app spec, Agenda Default Entry State scenario; experience.md, Success Criteria)."
uses-terms:
  - "TERM-AGENDA"
  - "TERM-HOUSEHOLD"
provenance:
  source: "docs/_legacy/00_product/experience.md (Purpose, Success Criteria); docs/_legacy/00_product/strategy.md (Value Proposition, Differentiators); openspec/specs/web-app/spec.md (Agenda Default Entry State)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

DomusMind MUST let a household understand what matters today in seconds: the product MUST open on today's household picture with no configuration required, and that picture MUST show what is happening, what needs attention and who is responsible without opening any detail (observed: docs/_legacy/00_product/experience.md, Purpose "understandable in seconds", Success Criteria; openspec/specs/web-app/spec.md, Agenda Default Entry State; docs/_legacy/00_product/strategy.md, Value Proposition, Differentiators "timeline-first and today-first").

## Measurement

Zero taps or settings between opening the app and today's household Agenda (observed: openspec/specs/web-app/spec.md, scenario "no user configuration is required"). In a moderated test, household members answer "what matters today and who owns it" from the default view; the sources say "in seconds" without a number, so a threshold such as 10 seconds is a proposal to confirm (inferred).
