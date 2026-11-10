---
id: JRN-FINANCES-KNOW-COMING-COSTS
type: journey
title: Know the household's upcoming and historical costs
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
steps:
  - use-case: UC-FINANCES-MANAGE-COMMITMENTS
  - use-case: UC-FINANCES-RECORD-COST
  - use-case: UC-FINANCES-REVIEW-UPCOMING-AND-SPENDING
---
## Intended Outcome

The household can remember regular financial commitments, see what is due next, record actual costs without losing expected terms, and understand how spending changes over time.

## Entry Conditions

A member is authorized for the household and has a bill, subscription, insurance renewal, school fee, tax or one-off household cost to track.

## Journey Narrative

The member records a recurring electricity commitment with a variable expected amount and due cadence. Finances projects the next due cycles without creating a Task or Calendar Plan. When the bill is paid, the member records its actual expense and settlement date; the expected and actual amounts remain distinct. The household can see year-to-date electricity costs in the appropriate currency. Separately, a boiler inspection can be entered as an unscheduled Expense, linked to the boiler's Asset history only after Assets accepts the cost boundary, and associated paperwork can be opened through Documents only after that context supports the link.

## Variants and Branches

- A car-insurance commitment may be associated with the relevant Asset and its renewal obligation, while each context retains its own lifecycle.
- A member records a school/activity fee attributable to a Member or category without introducing a School context.
- A member reviews monthly or annual totals across categories and household contexts.
- Variable or changed prices are recorded at their effective dates; historical actual expenses remain immutable except through an explicit correction policy.

## Completion Conditions

Upcoming commitments, actual expense history and payment outcome are separately understandable, authorization is respected in Finances and Agenda, and a same-currency summary explains the selected period without relying on bank synchronization.
