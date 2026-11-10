---
id: BR-FINANCES-AGENDA-PROJECTION
type: business-rule
title: Agenda projects authorized due dates without owning or creating work
status: draft
applies-to:
  - UC-FINANCES-REVIEW-UPCOMING-AND-SPENDING
uses-terms:
  - TERM-AGENDA
  - TERM-FINANCIAL-COMMITMENT
  - TERM-FINANCIAL-DUE-OCCURRENCE
---
## Rule

Agenda may project a due Financial Due Occurrence as read-only temporal information. The entry remains owned by Finances; recording payment or changing a commitment happens through the Finances capability. Projection never creates or modifies a Task, Calendar Plan, List item or generic Obligation.

## Rationale

The Agenda unifies temporal visibility while source contexts retain their write authority and distinct meanings.

## Examples

- An authorized member sees an upcoming insurance due cue and opens the commitment in Finances.
- Recording the payment updates the Finances projection; it does not complete a Task or Calendar Plan.
- A member without access to a private amount does not see that amount or infer the private record through Agenda counts or labels.

## Exceptions

Whether a due entry shows provider, amount, or only a generic cue depends on an approved financial-access policy and Agenda privacy contract.
