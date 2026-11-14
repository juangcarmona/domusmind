---
id: LC-MEAL-PLAN
type: domain-lifecycle
title: Meal Plan lifecycle
status: active
subject: TERM-MEAL-PLAN
states:
  - id: DRAFT
    title: Draft
    initial: true
  - id: ACTIVE
    title: Active
  - id: COMPLETED
    title: Completed
    terminal: true
transitions:
  - id: ACTIVATE
    title: Make the plan the working plan
    from:
      - DRAFT
    to: ACTIVE
    trigger: A household member explicitly activates the Draft plan
    initiated-by:
      - ACT-HOUSEHOLD-MEMBER
    governed-by:
      - BR-MEALS-PLAN-LIFECYCLE
      - BR-MEALS-ONE-PLAN-PER-WEEK
    realized-by:
      - UC-MEALS-PROMOTE-PLAN
  - id: COMPLETE
    title: Finish the meal-planning week
    from:
      - ACTIVE
    to: COMPLETED
    trigger: A household member chooses Finish week
    initiated-by:
      - ACT-HOUSEHOLD-MEMBER
    governed-by:
      - BR-MEALS-PLAN-LIFECYCLE
    realized-by:
      - UC-MEALS-COMPLETE-PLAN
provenance:
  source: 'TERM-MEAL-PLAN-STATUS; BR-MEALS-PLAN-LIFECYCLE; product owner decision Q-0033 (explicit activation); product owner, 2026-11-14: "Las tres recomendadas" (CHG-STATE-MODEL-COMPLETION)'
  confidence: high
  recovered-from: interview
---

## Purpose

Distinguish a week being prepared, a working meal plan and a week the household has closed.

## Invariants

A household has at most one meal plan for a week. New plans start Draft. Draft and Active plans allow slot changes under the existing slot rules. Completed plans are read-only and cannot reopen.

## State Semantics

Draft is preparation. Active is the household's working plan for its week. Completed records a week explicitly closed by a household member.

## Transition Semantics

Activation and completion are separate member decisions. Finish week moves an Active plan to Completed. A week boundary changes neither state; completion never runs automatically.

## Boundaries

Completion preserves the meal plan and its history. Existing Lists remain owned by Lists. Calendar Plans and Tasks have separate lifecycles. This defines accepted product intent; the inspected MealPlan class does not yet implement activation or completion commands.
