# Approved state-model decisions — 14 November 2026

Juan accepted the three recommended choices with **“Las tres recomendadas”**.
The [native Product Change](../changes/completed/chg-state-model-completion/change.md)
records that answer and the exact choices. This follow-up resolves the state-model
questions from the [initial review](2026-11-13-model-clarity.md).

## Accepted behavior

| Area | Decision | Product definition |
| --- | --- | --- |
| Meal Planning | A member chooses Finish week on an Active plan. Completed is read-only and cannot reopen. Week boundaries do not close plans automatically. | [Lifecycle](../model/domain/lifecycles/lc-meal-plan.md), [use case](../model/use-cases/uc-meals-complete-plan.md), [requirement](../model/requirements/functional/fr-meals-plan-lifecycle.md) |
| Inbox | A failed, unapplied action returns its proposal to Pending with its details and failure reason. The member can correct and explicitly approve it again. There are no automatic retries, and an Applied proposal cannot execute again. | [Lifecycle](../model/domain/lifecycles/lc-inbox-proposal.md), [rule](../model/business-rules/br-inbox-review-and-execution.md), [requirement](../model/requirements/functional/fr-inbox-review.md) |
| Calendar | Seven product states remain. A full reload runs within Syncing. Failed and Authorization expired show the failure reason. Needs attention and Rehydrating are retired as independent product states. | [Lifecycle](../model/domain/lifecycles/lc-external-calendar-connection.md), [status requirement](../model/requirements/functional/fr-calendar-show-connection-status.md) |

The Meal Planning decision resolves the completion trigger deferred under Q-0033;
explicit activation remains unchanged. The Calendar decision replaces the two
extra states accepted under Q-0049. Other rules from those decisions remain valid.

## Household-person access

The new [access lifecycle](../model/domain/lifecycles/lc-member-access.md) expresses
the existing accepted rules:

- No access → Invited when a manager issues an invitation.
- Invited → Active when the person accepts with the required verified identity.
- Invited → No access when a manager revokes the invitation.
- Active → Disabled when a manager disables another person's access.
- Disabled → Active when a manager re-enables access.

Replacing an invitation leaves the person Invited and invalidates the old link.
Its example claims no state change. Provisioned remains an existing label for the
same pending-invitation state. Pets, manager self-disable protection and identity
binding retain their existing rules. Installation bootstrap remains a separate flow.

## Graph and traceability

| Measure | Before this follow-up | After |
| --- | ---: | ---: |
| Bounded contexts | 9 | 9 |
| Product artifacts | 655 | 668 |
| Native lifecycles | 6 | 8 |
| Local lifecycle states | 24 | 29 |
| Transition identities | 19 | 27 |
| Structured behaviours | 199 | 209 |
| FR-to-UC links | 161 | 162 |

All 161 existing FR-to-UC links are preserved. The one additional link connects the
existing meal lifecycle requirement to the newly approved completion use case.
Relationships remain many-to-many; no new functional requirement was created.

All 27 transition identities have at least one linked positive example. Every
Structured Behaviour has a requirement reference. The graph remains one connected
component with no isolated artifacts. These are model checks, not proof that each
behavior has been delivered in the application.

The native impact ledger records 112 reviewed dependency impacts using
provider-generated digest pins. Existing creation, editing, privacy and successful
execution examples remain valid alongside the new completion and recovery rules.

## Documentation consumers

The Meal Planning, Inbox and Calendar OpenSpec specifications now express the
accepted choices. The family specification links the existing invitation examples
to the new access lifecycle. Settings and Meal Planning design guidance describe
the seven-state catalog and Finish week interaction. Design documents are included
in the configured citation checks.

All 936 citations are current, with no stale, tampered or unresolved references.
Payloads were emitted by the native CLI after reviewing the affected consumers.

## Development work still required

This PR changes product intent and its documentation. Application source and tests
are unchanged from the preceding model PR.

### Meal completion

The inspected MealPlan class initializes Draft and guards mutation of Completed
plans, but has no activation or completion operation. Development must provide the
explicit activation and Finish week actions, preserve read-only Completed content,
and verify that a week boundary never completes a plan or enables reopening.

### Inbox recovery

The current relational event writer rolls back failed dispatches, and an Applied
proposal is already guarded against another approval. Development must ensure the
member sees the failed proposal as Pending with its details and failure reason,
can correct and approve it again, and cannot cause another execution of an Applied
proposal. InboxProposalModel currently exposes no dedicated execution-failure
reason. The product decision leaves the storage and transaction design to engineering.

### Calendar status contract

The runtime enum still contains NeedsAttention and Rehydrating. Development must
align persistence, API mapping and the UI with the seven-state product catalog,
keep full reload progress within Syncing and retain useful error details. The
existing sync exception path can overwrite AuthExpired with Failed; that behavior
must preserve the distinct authorization-expired outcome.

## Verification and remaining questions

The final branch passes `npm run verify:product`, architecture link and citation
validation, and `git diff --check`. Native validation reports zero errors and 56
warnings: four existing low-confidence recovered claims and 52 claims in the
separate Roadmap proposal. The two unreachable-calendar-state warnings are gone.

The four recovered claims retain their original native digests. Their roadmap,
usability-measurement and template-editing questions are separate from the three
decisions accepted here. Dated Routine resumption also retains its existing
implementation gap.

The Product Change was approved from the recorded owner answer and applied in the
isolated branch through the native CLI. Human PR acceptance, application delivery
and deployment remain separate steps.
