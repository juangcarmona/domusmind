---
id: BC-HOUSEHOLD-FINANCES
type: bounded-context
title: Household Finances
status: draft
---
## Responsibility

Household Finances helps a household understand its recurring financial commitments, upcoming expected payments, actual expenses and cost history. It owns commitment terms, financial due-cycle state, expense/payment facts and financial read summaries. Its purpose is household coordination and visibility, not account reconciliation or bookkeeping.

## Language

A **Financial Commitment** describes an expected payment stream or ongoing payable and its terms. A **Financial Due Occurrence** is one projected cycle of that commitment. An **Expense** is an actual cost incurred by the household. A **Payment Record** records settlement against a due occurrence and may be associated with one or more actual expense facts; whether settlement and expense are one record in simple cases remains an owner decision. “Finances” is provisional user-facing language.

## Boundaries

- Family owns household membership and member identity. Finances scopes each record to one household and applies approved member-level visibility rules.
- Finances owns commitment definitions, expected amount history, due/payment outcomes, expense facts and financial summaries.
- Assets owns Asset identity, maintenance obligations and service-history meaning. Finances may reference an Asset and associate its Expense with an asset record, but the Finance Expense is the canonical cost fact. The active Assets draft's optional Asset Record cost needs explicit owner alignment.
- Documents owns document identity, file bytes, metadata, access and lifecycle. Finances may navigate to a linked document through an accepted contract; it never stores a second document or grants document access.
- Agenda is the unified temporal read surface. It may project authorized due occurrences; it never owns or edits Financial Commitments or creates execution records.
- Calendar owns Plans, Tasks owns Tasks/Routines, Lists owns list items and Areas owns responsibility structures. References do not transfer ownership or create work implicitly.
- Providers/payees, subscriptions, policies and service contracts do not require separate aggregates or contexts for MVP. They are commitment attributes/labels and contextual document links unless product discovery proves otherwise.
- No banking, accounting, budgeting or generic Obligation context is proposed.

## External Relationships

Uses Household membership and authorization to scope access. May hold same-household references to Members, Areas and Assets. Commitment due-state can be queried by Agenda as a read-only projection. Expense and Payment Record may be related to each other and to a commitment cycle under the accepted non-duplication rule. Links to Documents require an accepted cross-context contract; linked contexts retain their identity, authorization and lifecycle.
