# Obligation lifecycle refinement memory

This is a draft recommendation against accepted epic base `f857ed15ed50bb5bf8f6e08356c7d0d65c6d5094`. It does not contain owner answers or grant approval.

## Evidence and scope

Native child #358 refinement44392638537 identified missing accepted rules for allowed and forbidden obligation and occurrence actions. Its input independently verified accepted children356 and357 and the exact current epic base. The source model and impact map explicitly defer the complete obligation and occurrence lifecycle; accepted Asset archive/restore intentionally did not select those rules. Existing `BR-ASSET-RECURRENCE-DUE-STATE`, `BR-ASSET-OCCURRENCE-HISTORY`, both obligation use cases and the Assets reliability requirement provide the unchanged recurrence/history constraints.

Baseline validation:680artifacts,0errors,56existing warnings. The complete artifact inventory and structural impact reports were inspected alongside the affected model texts. Semantic review is focused on Assets and its direct product neighbors.

## Ranked decision queue

1. Temporary suspension: reactivation, retention of the pending date, permitted operations and actionable projection eligibility.
2. Cancellation: terminality and an explicit Cancelled cycle that invents no completion or skip.
3. One-time completion/skip: Finished requirement with no new cycle; recurring requirements still advance once.
4. Archived Asset guard: mutation refusal, retention of obligation states/dates and restoration without silent reactivation.

All four are unanswered. The proposed artifacts show recommended outcomes for review, not accepted owner decisions. The first question was sent to the owner; no answer is inferred from silence.

## Graph review

| Node or neighbor | Assessment | Reason |
| --- | --- | --- |
| Obligation/occurrence terms, manage/outcome use cases and obligation requirement | Affected | Their accepted prose lacks the complete action/state contract. |
| Agenda projection requirement | Affected | Lifecycle state determines whether a pending cycle is actionable; Agenda still owns only the read surface. |
| Assets recurrence/history quality requirement | Affected | Adds coverage of each permitted and forbidden source state without changing existing date guarantees. |
| Recurrence and occurrence-history rules | Checked, unchanged | Their accepted dates, explicit outcomes, bounded history and traceable-correction guarantees are preserved. |
| Asset archive/restore rule, LC-ASSET and restore use case | Checked, unchanged | This proposal supplies the previously deferred obligation guard without silently changing Asset state or copying Asset lifecycle. |
| Asset Record history requirement | Checked, unchanged | One visible durable outcome and separate traceable correction remain; correction effects and cost/record commands are outside this change. |
| Document references and grants | Checked, unchanged | No document lifecycle, grant, access or deletion decision is introduced. |
| Household, Areas, Tasks, Calendar, Lists | Checked, unchanged | Existing authorization and independently owned work remain authoritative. |
| Calendar anchors, timezone changes, dated auto-resume | Consciously excluded | Their missing decisions are not required to formalize these lifecycle actions and are not invented here. |

## Answers

None received.

## Readiness

Not ready for approval. The four named product decisions must be resolved. Nothing is approved, applied or archived; accepted model files remain unchanged.
