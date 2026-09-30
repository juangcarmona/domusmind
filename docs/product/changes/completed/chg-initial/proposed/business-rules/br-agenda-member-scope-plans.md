---
id: BR-AGENDA-MEMBER-SCOPE-PLANS
type: business-rule
title: "A person's Agenda shows their plans and household plans without participants"
status: draft
applies-to:
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-PLAN"
  - "TERM-PLAN-PARTICIPANT"
provenance:
  source: "openspec/specs/calendar/spec.md (Member Agenda Projection: participates or is a household plan); docs/_legacy/00_product/surfaces/agenda.md (Scope: Member); interview: product owner decision Q-0041 (E-0158)"
  confidence: "high"
  recovered-from: "interview"
---

## Rule

In a person's Member-scope Agenda, the plans shown are the plans that person takes part in plus household plans that have no participants (decided: Q-0041).

## Rationale

Reconciles the openspec, which includes household plans in the member projection, with the Agenda surface, which shows shared state only when it concerns the person (observed: calendar spec; agenda.md, Scope: Member; decided: Q-0041).

## Examples

- A plan with no participants appears in every person's Member-scope Agenda (decided: Q-0041).
- A plan whose only participant is another person does not appear in this person's Member-scope Agenda (decided: Q-0041).

## Exceptions

None.
