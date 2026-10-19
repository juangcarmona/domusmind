# Finances Surface (Draft)

**Status:** Draft product surface specification for CHG-HOUSEHOLD-FINANCES-001. “Finances” is provisional user-facing wording.

## Purpose

Answer, with low effort: what is the household committed to paying, what is due next, what was actually spent, and what changed? The surface coordinates household commitments and costs; it is not a bank register or accounting dashboard.

## Information architecture

- A compact overview with upcoming due commitments and a small number of transparent same-currency period totals.
- A commitment list for recurring payment expectations, including expected amount/variable cue, cadence, next due date, category/provider and current status.
- An expense history list, filterable by date range, category, member/beneficiary, Asset and Area where authorized.
- A selected-row inspector for commitment terms/history or expense/payment details; contextual associations open in their owning surface.
- A local quick-capture action for a commitment or actual expense, available from the overview and relevant contextual surfaces.

## Interaction model

- Prefer dense list rows and inspector-driven editing; keep summary values subordinate to the underlying records and clearly label expected vs actual vs paid.
- Upcoming and completed/paid sections remain distinct; completed history is accessible but does not dominate the default view.
- Show due date and expected amount for upcoming items only when the member has authorization. Variable or unknown amounts are clearly labeled rather than presented as zero.
- Commitment forms capture cadence, date rule, amount policy, currency, effective date and optional renewal/cancellation details. Expense capture asks for actual amount, currency, date and concise description first; category and context are optional.
- Contextual entry from an Asset or Area preselects a validated reference but never changes or creates an Asset, obligation, Task or Calendar Plan.
- A linked Document is opened through Documents' authorized surface; Finances does not render around that context's access rules.
- Payment capture clearly distinguishes recording the due cycle as paid from recording a cost, following the accepted anti-duplication rule.

## Agenda behavior

Agenda may show a read-only, authorized due cue with safe navigation back to Finances. Amounts, provider labels and even existence may be hidden according to the member's Finance access. Mark-paid and commitment-edit interactions occur in Finances. No Agenda action creates work.

## Responsive behavior

Desktop uses a compact list and visible inspector with search, period/category filters and high-density rows. Mobile uses stacked sections and a bottom sheet or pushed detail with the same functions, prioritized due commitments and quick capture. No horizontal scrolling or reduced authorization behavior at narrow widths.

## Explicit non-goals

No account balance cards, cash-flow graph wall, budget envelopes, reconciliation status, merchant/logo catalogue, bank sync, automatic payment, forecasts, investment/tax modules or decorative dashboard cards. Do not show a single mixed-currency total.

## Product questions for review

- Is a dedicated top-level Finances surface the right household term and navigation location?
- Which small summary metrics are genuinely useful without encouraging accounting complexity?
- Which family members can see which commitments, expenses and Agenda cues?
- Should upcoming commitments default to a calendar/list toggle or a chronological list with a compact date grouping?
