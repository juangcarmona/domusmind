---
id: "TERM-MEAL-PLAN"
type: "domain-term"
title: "Meal Plan"
status: "draft"
defined-in: "BC-MEAL-PLANNING"
synonyms:
  - "MealPlan"
  - "weekly meal plan"
uses-terms:
  - "TERM-HOUSEHOLD"
  - "TERM-MEAL-SLOT"
  - "TERM-MEAL-PLAN-STATUS"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Purpose, Meal Plan Creation); docs/_legacy/04_contexts/meal-planning.md (Aggregate Roots: MealPlan); docs/_legacy/03_domain/ubiquitous-language.md (Meal Plan); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs; interview: product owner decision Q-0035 (E-0158); interview: product owner decision Q-0056 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

The household's intended meals for one calendar week: exactly seven days starting on the household's configured first day of week (decided: Q-0035, Q-0056), divided into a full grid of Meal Slots (one per day and meal type), with a lifecycle status (observed: openspec/specs/meal-planning/spec.md, Meal Plan Creation; src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs). It belongs to exactly one household and optionally records the Weekly Template it was created from and a reference to the most recently derived shopping list (observed: docs/_legacy/04_contexts/meal-planning.md, MealPlan).

## Distinguish From

- Plan (TERM-PLAN): a scheduled calendar item in the Calendar context. A Meal Plan is not a Plan, and its meal slots are not Plans.
- Weekly Template (TERM-WEEKLY-TEMPLATE): a reusable pattern not tied to a week; a Meal Plan is the concrete plan for one week.
- Shopping list: a List in the Lists context derived from a Meal Plan; the Meal Plan only references it.

## Usage

Shown on the Meal Planning surface as the weekly grid (observed: docs/_legacy/00_product/surfaces/meal-planning.md, Core Layout). Household-facing name is "Meal Plan" (observed: docs/_legacy/03_domain/ubiquitous-language.md). Created from scratch, from a template, or by copying the previous week.
