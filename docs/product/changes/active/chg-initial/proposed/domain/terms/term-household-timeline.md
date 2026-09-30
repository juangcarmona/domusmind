---
id: TERM-HOUSEHOLD-TIMELINE
type: domain-term
title: "Household Timeline"
status: draft
defined-in: "BC-CALENDAR"
synonyms:
  - "Family Timeline"
  - "Timeline"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-PLAN"
  - "TERM-TASK"
  - "TERM-ROUTINE"
  - "TERM-LIST-ITEM"
provenance:
  source: "openspec/specs/calendar/spec.md (Household Timeline Projection); docs/_legacy/04_contexts/calendar.md (Family Timeline read model); docs/_legacy/03_domain/ubiquitous-language.md (Timeline)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

The household-scope chronological read model for a date window. It brings together plans, tasks due within the window, projected routine occurrences and temporal list items (items with a due date, reminder or repeat that produces an occurrence in the window), and never includes imported external calendar entries (observed: calendar spec, Household Timeline Projection).

## Distinguish From

- **Member agenda**: the person-scoped projection, which additionally includes that person's external calendar entries (observed: calendar spec, Member Agenda Projection).
- **Legacy family timeline**: the older definition held plans only, ordered by time (observed: legacy calendar.md, Family Timeline). The openspec definition, spanning four sources, supersedes it.

## Usage

Feeds the Household scope of the Agenda. It is read-only and never creates or changes a plan, task, routine or list item (observed: calendar spec).
