---
id: "FR-MEALS-COPY-PREVIOUS-WEEK"
type: "functional-requirement"
title: "Copy the previous week's plan"
status: "draft"
derived-from:
  - "UC-MEALS-COPY-PREVIOUS-WEEK"
  - "BR-MEALS-COPY-EXCLUDES-SHOPPING-LIST"
verification:
  - scenario-ref: "SB-MEALS-COPY-PREVIOUS-WEEK"
  - scenario-ref: "SB-MEALS-COPY-NO-PREVIOUS-PLAN"
uses-terms:
  - "TERM-MEAL-PLAN"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Requirement: Copy from Previous Week); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs (CopyFromPlan)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household create a Draft plan for a target week by copying the slots of the preceding week's plan (or an explicitly chosen plan), without its shopping list reference; it MUST report a recoverable "no previous plan" outcome when there is no source, and MUST return the existing plan when the target week already has one (observed: openspec/specs/meal-planning/spec.md, Copy from Previous Week).

## Rationale

Ad-hoc reuse is the most common way households plan (observed: surface, Success Criteria "reuse dominates").
