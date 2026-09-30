---
id: "BC-MEAL-PLANNING"
type: "bounded-context"
title: "Meal Planning"
status: "draft"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Purpose); docs/_legacy/04_contexts/meal-planning.md (Purpose, Bounded Context Boundary, Integration sections); docs/_legacy/00_product/surfaces/meal-planning.md (Purpose, Role); corroborated by src/backend/DomusMind.Domain/MealPlanning/**; interview: product owner decision Q-0035 (E-0158); interview: product owner decision Q-0056 (E-0158); interview: product owner decision Q-0039 (E-0158); interview: product owner decision Q-0040 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Responsibility

Weekly food coordination for the household: which meals are planned for each day and meal type, what the household usually eats, and what needs to be bought to cook those meals (observed: openspec/specs/meal-planning/spec.md, Purpose).

It owns the identity, structure and lifecycle of meal plans, the assignment of meal slots, the household recipe library with its ingredients, weekly templates for reuse, and the derivation of the ingredient set for a week (observed: docs/_legacy/04_contexts/meal-planning.md, Bounded Context Boundary). It is optimised for low-effort reuse (previous week, templates) over planning from scratch (observed: openspec/specs/meal-planning/spec.md, Purpose; docs/_legacy/00_product/surfaces/meal-planning.md, Role).

Meal Planning is current product per recovery decision Q-0001 and is expected to be refined in a later change. The legacy context document still labels it "V2 (targeted, not part of V1 core)" (observed: docs/_legacy/04_contexts/meal-planning.md, header); that label is superseded by Q-0001. Recipe management (create, update, delete, ingredients, detail and library) is delivered current product (decided: Q-0039; observed: src/backend/DomusMind.Api/Controllers/RecipesController.cs). A meal plan carries no Area link and no "affects whole household" flag (decided: Q-0040).

## Language

The language is weekly and household-scoped: a Meal Plan covers one week, divided into Meal Slots by day and Meal Type; each slot says where the meal comes from (a Recipe, free text, eaten out, leftovers, or deliberately unplanned). Recipes are household-owned sets of Ingredients. Weekly Templates are named reusable weekly patterns. Shopping list derivation turns the recipes of a week into a new shared list (observed: docs/_legacy/04_contexts/meal-planning.md, Aggregate Roots and Internal Entities; openspec/specs/meal-planning/spec.md, Requirements).

Terms: TERM-MEAL-PLAN, TERM-MEAL-PLAN-STATUS, TERM-MEAL-SLOT, TERM-MEAL-TYPE, TERM-MEAL-SOURCE, TERM-RECIPE, TERM-INGREDIENT, TERM-WEEKLY-TEMPLATE, TERM-SHOPPING-LIST-DERIVATION.

Caution: "Plan" alone means a scheduled calendar Plan (TERM-PLAN) in household language; a Meal Plan is not a Plan and meals are never calendar Plans (observed: docs/_legacy/00_product/surfaces/meal-planning.md, Anti-Patterns: do not treat meals as calendar events).

## Boundaries

Outside this context (observed: docs/_legacy/04_contexts/meal-planning.md, What Meal Planning Is Not, Bounded Context Boundary, Context Scope Limits; docs/_legacy/00_product/surfaces/meal-planning.md, Role and Anti-Patterns):

- the shopping list container and purchase tracking (the Lists context owns every derived list from the moment it exists);
- task creation or tracking; Meal Planning never creates tasks automatically;
- time semantics and reminders (Calendar remains the source of truth for time);
- nutrition, dietary constraints, per-person dietary preferences and per-person meal assignment;
- recipe import from external sources, recipe social features, cost tracking and budgeting, pantry or inventory state (the legacy document says pantry and inventory "may become part of future adjacent contexts in V3+").

## External Relationships

- Household (BC-FAMILY): every meal plan, recipe and template belongs to exactly one household; the household's configured first day of week defines the plan week (decided: Q-0035, Q-0056; observed: docs/_legacy/04_contexts/meal-planning.md, Positioning; openspec/specs/meal-planning/spec.md, Meal Plan Creation). See TERM-HOUSEHOLD.
- Lists (BC-LISTS): requesting a shopping list creates a new shopping List (TERM-LIST) whose items (TERM-LIST-ITEM) are the consolidated ingredients; Meal Planning keeps only a reference to the latest derived list and receives no feedback when items are checked (observed: openspec/specs/meal-planning/spec.md, Shopping List Derivation; docs/_legacy/04_contexts/meal-planning.md, Integration with Lists).
- Agenda (TERM-AGENDA, defined in BC-CALENDAR): meal slots project into the Agenda as non-timed, household-level, read-only entries (observed: openspec/specs/meal-planning/spec.md, Agenda Projection).
- Tasks (BC-TASKS): people may create meal-related tasks themselves (for example "defrost chicken"); no automatic creation (observed: docs/_legacy/04_contexts/meal-planning.md, Integration with Tasks).
