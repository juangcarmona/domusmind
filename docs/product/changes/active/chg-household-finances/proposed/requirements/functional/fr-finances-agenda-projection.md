---
id: FR-FINANCES-AGENDA-PROJECTION
type: functional-requirement
title: Project authorized upcoming payment due dates into Agenda
status: draft
derived-from:
  - UC-FINANCES-REVIEW-UPCOMING-AND-SPENDING
  - BR-FINANCES-AGENDA-PROJECTION
verification:
  - scenario: "An authorized upcoming Finance due occurrence appears as a read-only Agenda projection with safe navigation back to Finances."
  - scenario: "Recording payment or editing a commitment is performed through Finances and never creates or changes a Task, Plan or List item."
  - scenario: "A member not authorized to see a commitment cannot infer its existence or private amount through Agenda entries, counts or labels."
uses-terms:
  - TERM-AGENDA
  - TERM-FINANCIAL-DUE-OCCURRENCE
---
## Requirement

The Agenda MUST be able to display authorized upcoming Finance due occurrences as read-only entries that navigate to Finances. The projection MUST NOT create or mutate source records or execution/planning records, and MUST obey the same household and financial privacy rules as the source query.

## Rationale

Payment dates are temporally useful to see with the household's other upcoming commitments while remaining financially owned and edited in Finances.
