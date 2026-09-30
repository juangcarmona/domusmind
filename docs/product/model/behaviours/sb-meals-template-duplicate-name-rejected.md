---
id: "SB-MEALS-TEMPLATE-DUPLICATE-NAME-REJECTED"
type: "structured-behaviour"
title: "Creating a template with an existing name is rejected"
status: "draft"
illustrates:
  - "UC-MEALS-CREATE-TEMPLATE"
  - "BR-MEALS-TEMPLATE-NAME-UNIQUE"
given:
  - "a template named \"Standard week\" already exists"
when: "a person creates another template with the same name"
then:
  - "the creation is rejected"
uses-terms:
  - "TERM-WEEKLY-TEMPLATE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (scenario: Household attempts to create a template with a duplicate name)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Template names are unique within a household.

## Boundaries

None.
