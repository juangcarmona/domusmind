---
id: "QR-MEALS-LOW-EFFORT-PLANNING"
type: "quality-requirement"
title: "A full week can be planned in minutes"
status: "draft"
quality-attribute: "usability"
applies-to:
  - "JRN-MEALS-PLAN-THE-WEEK"
  - "UC-MEALS-COPY-PREVIOUS-WEEK"
  - "UC-MEALS-DERIVE-SHOPPING-LIST"
verification:
  - scenario: "A household member plans a full week on the Meal Planning surface in under 2 minutes."
  - scenario: "A shopping list is produced from a planned week with a single action."
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-SHOPPING-LIST-DERIVATION"
provenance:
  source: "docs/_legacy/00_product/surfaces/meal-planning.md (Success Criteria, UX Rules)"
  confidence: "low"
  recovered-from: "documentation"
---

## Requirement

Planning a full week MUST take under two minutes, reuse (template or previous week) MUST be faster than creating from scratch, shopping list derivation MUST be a single action, and the grid MUST be scannable in seconds (observed: docs/_legacy/00_product/surfaces/meal-planning.md, Success Criteria and UX Rules).

## Measurement

Time-on-task for a representative household member planning a complete week from an existing previous week or template: under 2 minutes. Number of actions from a planned week to a shopping list: 1. Measurement method is not stated in the source (inferred); confidence low because it comes from one legacy surface document.
