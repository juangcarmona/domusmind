---
id: "SB-MEALS-CREATE-TEMPLATE"
type: "structured-behaviour"
title: "Creating a named template adds it to the household's templates"
status: "draft"
illustrates:
  - "UC-MEALS-CREATE-TEMPLATE"
given:
  - "a household exists"
when: "a person creates a template with a unique name and optional slot assignments"
then:
  - "the template is saved to the household's template library"
  - "it is available to apply to future weeks"
uses-terms:
  - "TERM-WEEKLY-TEMPLATE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household creates a weekly template)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intent

Templates capture the household's usual pattern.

## Boundaries

None.
