---
id: TERM-AGENDA-SCOPE
type: domain-term
title: "Agenda Scope"
status: draft
defined-in: "BC-CALENDAR"
synonyms:
  - "Household scope"
  - "Member scope"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-HOUSEHOLD"
  - "TERM-MEMBER"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Scope); openspec/specs/calendar/spec.md (Household Timeline Projection, Member Agenda Projection); interview: product owner decision Q-0041 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

Whose picture the Agenda shows. **Household scope** shows the household as one coordinated unit: a shared row for household-level plans, unassigned tasks and list items, and one row or section per person. **Member scope** centres one person: their tasks, the plans they take part in plus household plans without participants (decided: Q-0041), their routines, household routines they are responsible for, and their own imported external calendar entries (observed: agenda.md, Scope).

## Distinguish From

- **Agenda mode**: the time window (Day, Week, Month), chosen independently of scope (observed: agenda.md, Time Modes).
- **Household vs member ownership of data**: scope filters what is shown, it does not change who owns a record (inferred from agenda.md, Data).

## Usage

Household scope is coordination-oriented (who has what, where load concentrates, what is unassigned); Member scope is individual-clarity-oriented (a person's day, conflicts, overload). External calendar entries appear only in Member scope (observed: agenda.md, Scope; calendar spec).
