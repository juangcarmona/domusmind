---
id: "TERM-MEAL-SLOT"
type: "domain-term"
title: "Meal Slot"
status: "draft"
defined-in: "BC-MEAL-PLANNING"
synonyms:
  - "MealSlot"
  - "slot"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-MEAL-TYPE"
  - "TERM-MEAL-SOURCE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Meal Slot Assignment); docs/_legacy/04_contexts/meal-planning.md (Internal Entities: MealSlot); docs/_legacy/03_domain/ubiquitous-language.md (Meal Slot); src/backend/DomusMind.Domain/MealPlanning/Entities/MealSlot.cs; interview: product owner decision Q-0040 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

A single meal position within a Meal Plan, identified by day of week and Meal Type. It carries a Meal Source (and a recipe reference or free text when the source needs one), optional notes, an optional flag (a non-binding meal, common for snacks) and a locked flag (the slot is stable and must not change until unlocked) (observed: openspec/specs/meal-planning/spec.md, Meal Slot Assignment; src/backend/DomusMind.Domain/MealPlanning/Entities/MealSlot.cs). A slot with nothing assigned has source Unplanned; the slot itself always exists.

## Distinguish From

- Plan (TERM-PLAN): a timed calendar item; a meal slot is a non-timed household entry and never becomes a Plan.
- Slot template: the slot shape inside a Weekly Template, which mirrors a Meal Slot but belongs to the template.

## Usage

Each cell of the weekly grid is one Meal Slot (observed: docs/_legacy/00_product/surfaces/meal-planning.md, Core Layout). Slots are the unit that is assigned, cleared, copied and projected into the Agenda.
