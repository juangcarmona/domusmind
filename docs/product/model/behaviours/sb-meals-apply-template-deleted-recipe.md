---
id: "SB-MEALS-APPLY-TEMPLATE-DELETED-RECIPE"
type: "structured-behaviour"
title: "A template referencing a deleted recipe cannot be applied"
status: "draft"
illustrates:
  - "UC-MEALS-APPLY-TEMPLATE"
  - "BR-MEALS-TEMPLATE-SNAPSHOT"
given:
  - "a weekly template has a slot referencing a recipe"
  - "that recipe has since been deleted from the household library"
when: "a person applies the template to a target week"
then:
  - "the application fails"
  - "no meal plan is created"
uses-terms:
  - "TERM-WEEKLY-TEMPLATE"
  - "TERM-RECIPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Template references a deleted recipe)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intent

Plans are never created with dangling recipes.

## Boundaries

None.
