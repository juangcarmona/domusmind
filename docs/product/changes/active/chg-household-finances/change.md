---
id: CHG-HOUSEHOLD-FINANCES-001
type: product-change
title: Coordinate recurring commitments, payments and household spending
status: draft
base-revision: 'b4f77ad01ca87d2de33682d24ea2b7eee9e4cb15'
operations:
  add:
    - BC-HOUSEHOLD-FINANCES
    - TERM-FINANCIAL-COMMITMENT
    - TERM-FINANCIAL-DUE-OCCURRENCE
    - TERM-EXPENSE
    - TERM-PAYMENT-RECORD
    - BR-FINANCES-COMMITMENT-HISTORY
    - BR-FINANCES-EXPENSE-PAYMENT-SEPARATION
    - BR-FINANCES-AGENDA-PROJECTION
    - BR-FINANCES-CURRENCY-SAFE-SUMMARIES
    - UC-FINANCES-MANAGE-COMMITMENTS
    - UC-FINANCES-RECORD-COST
    - UC-FINANCES-REVIEW-UPCOMING-AND-SPENDING
    - JRN-FINANCES-KNOW-COMING-COSTS
    - FR-FINANCES-MANAGE-COMMITMENTS
    - FR-FINANCES-RECORD-EXPENSES-AND-PAYMENTS
    - FR-FINANCES-REVIEW-SPENDING
    - FR-FINANCES-AGENDA-PROJECTION
    - FR-FINANCES-CONTEXTUAL-ASSOCIATIONS
    - QR-FINANCES-MONEY-INTEGRITY
    - QR-FINANCES-PRIVACY
    - QR-FINANCES-HISTORY-AND-IDEMPOTENCY
    - CON-FINANCES-OWNERSHIP-AND-INTEGRATION
  modify:
    - CON-PRODUCT-CURRENT-SCOPE
    - CON-PRODUCT-HOUSEHOLD-FIRST
  remove: []
---

## Problem

DomusMind has no accepted financial coordination capability. Households cannot maintain one clear view of recurring commitments, upcoming payment dates, recorded household costs, or the history behind changes in price. Finance is currently outside product scope. Existing areas may mention finances as an Area of responsibility, but an Area does not record commitments, expenses or payments.

The product also lacks an agreed boundary between an expected commitment, a due cycle, a payment and an expense. Without it, a subscription renewal, a car-insurance payment, an asset-maintenance record and the cost recorded for that work could be duplicated or conflated. The household needs visibility and history without acquiring bank-account, bookkeeping or enterprise-accounting complexity.

## Intended Product Outcome

Household members can record the regular financial commitments they want to remember, see upcoming due dates and expected amounts, record actual household expenses and payment outcomes, and review historical costs by useful household context. A commitment represents an expected stream and terms; an expense represents an actual cost; a payment record documents settlement against an expected due cycle. Their precise persistence and matching rules remain subject to the open product decision about avoiding duplicate representations.

The proposed internal bounded context is **Household Finances**; the user-facing label is provisionally **Finances** and should be validated. It owns financial commitment definitions, expected due-cycle state, expense records, payment records and financial summaries. It may hold references to household members, Areas, Assets and Documents, but does not own their identities, responsibility structures, asset lifecycle or files. Provider/payee details begin as commitment attributes; no generic Contacts, Contracts, Banking or Accounting context is proposed.

The recommended first delivery is a manual-first MVP: manage recurring commitments and their effective-dated expected terms; project a bounded upcoming due horizon; record actual expenses and simple payment outcomes; review basic monthly/annual and category/context summaries; and navigate to related household context. Payment due dates may be projected into the read-only Agenda. They do not automatically create Tasks, Calendar Plans, List items or generic Obligations. External bank connections and automated payment are explicitly excluded.

## Rationale

The household value is knowing what has been promised, what is likely due, what was actually spent, and how those amounts changed—not reconciling balances across financial accounts. A manual-first capability works without requiring a bank, a particular country or an external financial provider.

Consumer products validate a few useful interaction patterns without defining DomusMind's architecture. Monarch presents recurring streams in calendar and list views, supports manual entry, grouping/filtering, and explicit confirmation of a due item as paid; its bank-derived detection and account integrations are not required here ([Monarch recurring expenses](https://www.monarch.com/track-recurring-bills-and-subscriptions)). YNAB's scheduled transactions make repeating costs visible ahead of time and document month-end recurrence edge cases such as a 31st falling in a shorter month ([YNAB scheduled transactions](https://support.ynab.com/scheduled-transactions-a-guide-BygrAIFA9)). DomusMind should adopt the clarity of expected-versus-actual and a reviewable recurring list, while avoiding account registers, budget envelopes, merchant detection, financial advice and bank synchronization.

The Assets Product Change (#235 / Draft PR #236) has an explicit overlap: it proposes optional actual costs on Asset Records, while this change proposes Finance as the owner of canonical household expense facts and totals. Asset history must continue to state what maintenance or inspection happened; the Expense must own the financial fact. A reference may connect them, but product owners must align the proposals before either is treated as accepted. The Documents initiative (#245 / Draft PR #246) owns files and metadata and currently has no Finance association; financial paperwork integration needs an explicit contract and must not transfer file ownership. These initiatives remain unchanged by this Product Change.

## Affected Product Areas

- New Household Finances bounded context for recurring commitment terms, due-cycle visibility, actual expenses, payment outcomes and summary queries.
- Household membership and authorization as the scope boundary; financial detail may need narrower member visibility than general household state.
- Agenda as a unified, read-only projection of selected upcoming financial due dates; Finances remains the editing owner.
- Household Assets for contextual links to assets and maintenance records, with an owner-reviewed resolution of the existing Asset Record cost overlap before integration.
- Household Documents for supporting paperwork navigation, only after its separate proposal accepts a Finance association contract; Documents retains file and access ownership.
- Areas and Household members as optional attribution/responsibility references; neither transfers its ownership rules to Finances.
- Calendar and Tasks retain their plan and execution semantics. A due payment is not a Plan or Task, and no such record is created automatically.
- A dedicated Finances surface, with compact commitment and expense lists, upcoming due cues, filters and an inspector; no enterprise accounting dashboard.
- Expected specification impact after acceptance: add an OpenSpec Finances specification; amend Agenda projection requirements for Finance-sourced due entries; define validated references for Household, Areas and any accepted Assets/Documents integrations. Calendar and Tasks specifications change only if an explicitly approved cross-context navigation contract requires it.

## Open Questions

- Is “Finances” understandable to ordinary households, or should the surface be “Bills & spending”, “Money”, or another term? Should the internal context name remain Household Finances?
- What is the canonical relationship among a payment, an expense and a commitment occurrence? Can a settled payment also be the expense record, and how can summaries avoid double-counting? Can one due cycle have multiple partial payments in the MVP?
- Which amount and due-date changes need effective-dated history, and how should a price increase affect already-projected but unpaid occurrences?
- Which commitment recurrence patterns ship first? What should happen to a monthly due date on the 29th–31st in shorter months, and how are pauses, renewals, cancellations and missed cycles represented?
- Which currencies are supported in the MVP? Is there a household default currency? The safe baseline is currency-tagged amounts, no floating-point arithmetic, no conversion, and summaries that never add unlike currencies.
- Which financial details are visible to all household members, selected members, or only the recorder? What may children see? Can household managers access restricted information, and how do member removal and access revocation affect historical records?
- Is a payment due cycle a calculated projection until a member records an outcome, or must selected cycles be persisted? How much historical due state is worth retaining?
- Which expense attribution dimensions are useful initially: category, Asset, member/beneficiary, Area or provider? Distinguish who paid from who/what benefited.
- Should provider/payee stay as a text value for MVP, or is provider deduplication sufficiently valuable to justify a separate concept later? Are subscriptions and contracts simply kinds/attributes of a Financial Commitment?
- Does the Agenda show amounts, names or only a due cue for members with limited financial access? What horizon and completed/overdue behavior should be projected?
- How should the Assets draft's optional Asset Record actual cost be aligned with the Finance-owned Expense record, without modifying its separate Product Change/PR? Which issue owner coordinates acceptance of that boundary?
- Should the Documents initiative support associations to Financial Commitments and Expenses, and which existing or future Documents decision must land first?
- What retention/correction policy applies to recorded expenses and payment references? Can a financial fact be edited/deleted, or must corrections preserve an audit trail?

## Product Acceptance

Product owners may accept this change when they confirm:

- Finance is household coordination and historical cost visibility, not banking, accounting, budgeting or financial advice.
- Financial Commitment, due occurrence, Expense and Payment Record have distinct meanings and a non-duplicating relationship suitable for the MVP.
- Recurrence, price changes, amount/currency precision, missed/partial payment handling and historical correction are explicit enough to implement and test.
- Household/member privacy and authorization are clear across the Finances surface, Agenda projection and all contextual references.
- Assets continues to own asset identity and maintenance-history meaning while Finance owns its canonical financial fact; the separate cost-model conflict is resolved before integration work.
- Documents remains owner of file bytes, metadata and access; Finance links to supporting paperwork only through an accepted contract.
- Agenda projection is read-only and never creates Tasks, Calendar Plans, Lists or generic Obligations.
- MVP, non-goals, quality measures and specification changes are traceable to the proposed artifacts and delivery issues.
- The two modified product constraints can be integrated with concurrent Product Changes without reverting or silently absorbing their scope.

## Out of Scope

This draft does not approve or apply itself, implement application behavior, select financial data providers or account infrastructure, or modify the Assets, Documents, Roadmap, MCP or Conversations Product Changes. It does not add bank synchronization/Open Banking, account balances, automated payments, transaction import, income or transfer tracking, reconciliation, double-entry bookkeeping, tax preparation, investment management, credit scoring, financial advice, advanced budgets/forecasting, currency conversion, provider marketplace, or generic Contacts/Contracts bounded contexts. It does not make due payments into Tasks or Calendar Plans or create a generic cross-domain Obligation context. It does not assume every household member may see every amount.
