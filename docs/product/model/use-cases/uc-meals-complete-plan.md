---
id: UC-MEALS-COMPLETE-PLAN
type: use-case
title: Finish a meal-planning week
status: active
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-MEAL-PLANNING
governed-by:
  - BR-MEALS-PLAN-LIFECYCLE
uses-terms:
  - TERM-MEAL-PLAN
  - TERM-MEAL-PLAN-STATUS
provenance:
  source: 'product owner, 2026-11-14: "Las tres recomendadas" (CHG-STATE-MODEL-COMPLETION)'
  confidence: high
  recovered-from: interview
---

## Goal

Close a working meal plan and keep it as a read-only record.

## Trigger

A household member chooses Finish week.

## Preconditions

The member may access the household and the meal plan is Active.

## Main Flow

1. The member chooses Finish week on the Active meal plan.
2. DomusMind records the plan as Completed.
3. The member can view the plan as read-only history.

## Alternative Flows

The member may leave the plan Active, including after the calendar week ends.

## Failure Conditions

A person without household access cannot complete the plan. Completed plans cannot reopen.

## Postconditions

The plan is Completed and read-only. Its existing content and history remain available.
