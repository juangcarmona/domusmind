---
id: "TERM-MEAL-PLAN-STATUS"
type: "domain-term"
title: "Meal Plan Status"
status: "draft"
defined-in: "BC-MEAL-PLANNING"
synonyms:
  - "MealPlanStatus"
  - "Draft"
  - "Active"
  - "Completed"
uses-terms:
  - "TERM-MEAL-PLAN"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Meal Plan Lifecycle, Notes); docs/_legacy/04_contexts/meal-planning.md (Plan Lifecycle); src/backend/DomusMind.Domain/MealPlanning/Enums/MealPlanStatus.cs; interview: product owner decision Q-0032 (E-0158); interview: product owner decision Q-0033 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

The lifecycle stage of a Meal Plan: Draft (being set up, not yet the household's working plan), Active (the household's working plan for the week), Completed (a past week, read-only). Plans progress Draft then Active then Completed (observed: openspec/specs/meal-planning/spec.md, Meal Plan Lifecycle).

## Distinguish From

- Task status or List item checked state: unrelated lifecycles in other contexts.
- Unplanned: a Meal Source of a slot, not a plan status.

## Usage

Governs whether slots may still change (Completed forbids it) and which plan blocks recipe deletion (Active). Draft and Active plans both accept slot changes (decided: Q-0032). A plan becomes Active only when the household explicitly activates it (decided: Q-0033). Undecided, deferred (Q-0033): what triggers Completed is left to the Meal Planning refinement change.
