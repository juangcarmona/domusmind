---
id: "BR-MEALS-NO-AUTOMATIC-TASKS"
type: "business-rule"
title: "Meal planning never creates tasks automatically"
status: "draft"
applies-to:
  - "BC-MEAL-PLANNING"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-TASK"
provenance:
  source: "docs/_legacy/04_contexts/meal-planning.md (What Meal Planning Is Not, Integration with Tasks)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

Planning meals never creates tasks by itself; tasks such as "prepare Thursday dinner" or "pick up groceries" exist only when a person creates them in Tasks (observed: docs/_legacy/04_contexts/meal-planning.md, Integration with Tasks).

## Rationale

Explicit household action keeps the task list trustworthy (inferred).

## Examples

Assigning "roast chicken" to Sunday dinner does not add a "defrost chicken" task.

## Exceptions

None.
