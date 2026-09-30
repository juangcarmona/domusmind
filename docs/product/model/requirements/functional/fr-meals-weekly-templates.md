---
id: "FR-MEALS-WEEKLY-TEMPLATES"
type: "functional-requirement"
title: "Weekly meal templates"
status: "draft"
derived-from:
  - "UC-MEALS-CREATE-TEMPLATE"
  - "UC-MEALS-UPDATE-TEMPLATE"
  - "BR-MEALS-TEMPLATE-NAME-UNIQUE"
verification:
  - scenario-ref: "SB-MEALS-CREATE-TEMPLATE"
  - scenario-ref: "SB-MEALS-TEMPLATE-DUPLICATE-NAME-REJECTED"
uses-terms:
  - "TERM-WEEKLY-TEMPLATE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Requirement: Weekly Templates); src/backend/DomusMind.Domain/MealPlanning/Entities/WeeklyTemplate.cs; interview: product owner decision Q-0037 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household create named weekly templates with unique names, holding any subset of slot assignments with the same sources and flags as meal slots, and SHOULD let it update them afterwards (observed: openspec/specs/meal-planning/spec.md, Weekly Templates).

## Rationale

Captures the household's usual week for fast reuse. Medium: the legacy surface marks templates "coming soon" and template update is absent from the domain code. Weekly templates are current product at medium confidence, flagged for the Meal Planning refinement change (decided: Q-0037).
