---
id: CON-FINANCES-OWNERSHIP-AND-INTEGRATION
type: constraint
title: Financial coordination preserves context ownership
status: draft
applies-to:
  - BC-HOUSEHOLD-FINANCES
uses-terms:
  - TERM-HOUSEHOLD
  - TERM-AGENDA
  - TERM-TASK
  - TERM-PLAN
  - TERM-FINANCIAL-COMMITMENT
  - TERM-EXPENSE
  - TERM-PAYMENT-RECORD
---
## Constraint

Household Finances owns commitments, due/payment outcomes, expenses and finance summaries. Household owns identity/membership, Areas owns responsibility structures, Assets owns asset identity and maintenance meaning, Documents owns files/metadata/access, Calendar owns Plans, Tasks owns Tasks/Routines, and Agenda remains the unified read projection. Cross-context links preserve those owners and require the target's authorization; they do not copy target records or transfer lifecycle.

A Financial Commitment's due date is expected financial state, not a Task, Plan, List item or generic Obligation. Agenda may project authorized due state read-only, but payment or commitment changes occur through Finances. No background or user-flow behavior automatically creates execution/planning records.

## Rationale

Household coordination depends on a coherent financial view without making time, execution, responsibility, document storage, asset maintenance and actual cost one indistinguishable domain.

## Consequences

- The Finance module uses its own command/query/application slices and remains inside the existing modular-monolith architecture unless later evidence supports a different boundary.
- Expense-to-Asset, Finance-to-Documents, and Finance-to-Agenda links are explicit contracts; their target owners retain identity and access policy.
- Asset Record cost and Finance Expense ownership must be aligned with the separate Assets Product Change before either integration is implemented.
- Recurrence may reuse proven date-calculation mechanics, but financial cycles keep their domain rules and do not become Tasks Routines or recurring Calendar Plans.
- No new bank adapter, generic recurrence aggregate, generic Provider/Contract context or accounting ledger is justified by this change alone.
