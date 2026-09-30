---
id: "TERM-WEEKLY-TEMPLATE"
type: "domain-term"
title: "Weekly Template"
status: "draft"
defined-in: "BC-MEAL-PLANNING"
synonyms:
  - "WeeklyTemplate"
  - "template"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-MEAL-SLOT"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Weekly Templates, Apply Weekly Template); docs/_legacy/04_contexts/meal-planning.md (Aggregate Roots: WeeklyTemplate); docs/_legacy/03_domain/ubiquitous-language.md (Weekly Template); src/backend/DomusMind.Domain/MealPlanning/Entities/WeeklyTemplate.cs; interview: product owner decision Q-0037 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Definition

A named, reusable weekly meal pattern belonging to the household: a set of slot assignments (day and meal type, with the same source and metadata as meal slots), possibly covering fewer than all slots (observed: openspec/specs/meal-planning/spec.md, Weekly Templates).

## Distinguish From

- Meal Plan (TERM-MEAL-PLAN): a template is not tied to a week and is never mutated by plans created from it.
- Copy from previous week: ad-hoc reuse of a specific past plan, not a named pattern (observed: openspec/specs/meal-planning/spec.md, Copy from Previous Week).

## Usage

Applied to a target week to create a pre-filled meal plan. The legacy surface shows "Apply template" as disabled with a "coming soon" tooltip (observed: docs/_legacy/00_product/surfaces/meal-planning.md, Surface State Model), so the surface lags the product. Weekly templates are current product at medium confidence, flagged for the Meal Planning refinement change (decided: Q-0037).
