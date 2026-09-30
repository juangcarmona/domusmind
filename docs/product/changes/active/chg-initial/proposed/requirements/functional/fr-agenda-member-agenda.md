---
id: FR-AGENDA-MEMBER-AGENDA
type: functional-requirement
title: "Show one person's temporal picture"
status: draft
derived-from:
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
  - "BR-CALENDAR-EXTERNAL-ENTRY-VISIBILITY"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-OWNER-SCOPE-ONLY"
  - "BR-LISTS-EDIT-ONLY-IN-LISTS"
  - "BR-AGENDA-PROJECTION-READ-ONLY"
  - "BR-AGENDA-MEMBER-SCOPE-PLANS"
verification:
  - scenario-ref: "SB-AGENDA-MEMBER-INCLUDES-IMPORTED"
  - scenario-ref: "SB-AGENDA-HOUSEHOLD-EXCLUDES-EXTERNAL"
  - scenario-ref: "SB-AGENDA-MEMBER-LIST-ITEMS-READ-ONLY"
  - scenario-ref: "SB-AGENDA-MEMBER-HOUSEHOLD-PLAN-WITHOUT-PARTICIPANTS"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-HOUSEHOLD-TIMELINE"
  - "TERM-AGENDA-SCOPE"
  - "TERM-PLAN"
  - "TERM-TASK"
  - "TERM-ROUTINE"
  - "TERM-LIST-ITEM"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "openspec/specs/calendar/spec.md (Member Agenda Projection); docs/_legacy/00_product/surfaces/agenda.md (Scope: Member); docs/_legacy/06_interfaces/external-calendar-api.md (View Agenda in member scope); interview: product owner decision Q-0041 (E-0158); interview: product owner decision Q-0053 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

For a requested date window the product MUST show, in a person's Member scope, plans they take part in plus household plans without participants (decided: Q-0041), tasks assigned to them, routines for them or the household, temporal list items and the external entries of that person's own active connections. An external entry MUST be shown only when its connection is active, its calendar selected, it lies within the horizon and it is not deleted, and MUST carry a source label and be read-only. Building this view MUST NOT create or change any record (observed: calendar spec).

Undecided, deferred (Q-0053): how Member scope behaves for a single-person household is left to a later change; no source states the behaviour.

## Rationale

Gives each person clarity on their own time, including commitments that live outside DomusMind (observed: agenda.md, Member scope).
