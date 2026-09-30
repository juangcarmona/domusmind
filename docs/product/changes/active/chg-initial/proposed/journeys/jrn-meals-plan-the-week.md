---
id: "JRN-MEALS-PLAN-THE-WEEK"
type: "journey"
title: "Plan the household's week of meals and shopping"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
steps:
  - use-case: "UC-MEALS-VIEW-PLAN"
  - use-case: "UC-MEALS-COPY-PREVIOUS-WEEK"
  - use-case: "UC-MEALS-UPDATE-SLOT"
  - use-case: "UC-MEALS-PROMOTE-PLAN"
  - use-case: "UC-MEALS-DERIVE-SHOPPING-LIST"
  - use-case: "UC-MEALS-SEE-MEALS-IN-AGENDA"
provenance:
  source: "docs/_legacy/00_product/surfaces/meal-planning.md (Purpose, Header, Surface State Model, Success Criteria); openspec/specs/meal-planning/spec.md (Purpose); docs/_legacy/04_contexts/meal-planning.md (Purpose); interview: product owner decision Q-0033 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intended Outcome

The household knows what it is eating this week and has a shared shopping list for it, with reuse doing most of the work (observed: docs/_legacy/00_product/surfaces/meal-planning.md, Purpose).

## Entry Conditions

A household member opens Meal Planning on a week (usually the current or next week).

## Journey Narrative

1. The member opens the week and sees what is already decided and what is missing (UC-MEALS-VIEW-PLAN).
2. If the week has no plan, the member reuses last week (UC-MEALS-COPY-PREVIOUS-WEEK).
3. The member adjusts individual slots: recipes, free-text meals, eating out, leftovers, or leaving a slot open (UC-MEALS-UPDATE-SLOT).
4. The member makes the plan the household's working plan (UC-MEALS-PROMOTE-PLAN).
5. The member generates the shopping list, which appears in Lists (UC-MEALS-DERIVE-SHOPPING-LIST).
6. During the week, everyone sees each day's meals in the Agenda (UC-MEALS-SEE-MEALS-IN-AGENDA).

The activation step is an explicit household action (decided: Q-0033); the legacy surface does not yet show an activate action.

## Variants and Branches

- Step 2 alternatives: start from scratch (UC-MEALS-CREATE-PLAN) or apply a template (UC-MEALS-APPLY-TEMPLATE, shown as "coming soon" on the legacy surface).
- Step 2 when last week has no plan: a recoverable notice; the member starts from scratch or applies a template.
- Step 3: recipes missing from the library are added on the fly (UC-RECIPES-CREATE-RECIPE).
- Step 5 when the plan has no recipe slots: no list can be generated.

## Completion Conditions

Every slot the household cares about is decided (others can stay Unplanned or optional), the plan is the working plan, and a shopping list exists in Lists.
