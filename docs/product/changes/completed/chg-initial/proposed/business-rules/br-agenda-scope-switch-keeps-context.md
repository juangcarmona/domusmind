---
id: BR-AGENDA-SCOPE-SWITCH-KEEPS-CONTEXT
type: business-rule
title: "Switching scope keeps the mode and date"
status: draft
applies-to:
  - "UC-AGENDA-NAVIGATE"
uses-terms:
  - "TERM-AGENDA-SCOPE"
  - "TERM-AGENDA-MODE"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Default Entry State, Click Semantics)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

Switching to a person's scope, or back to Household, preserves the current mode and date; switching mode keeps the current scope (observed: agenda.md).

## Rationale

Lets people compare the household and one person over the same window (inferred).

## Examples

- Viewing next Wednesday's week in Household scope and tapping Ana shows Ana's week for the same dates (observed: agenda.md).

## Exceptions

None.
