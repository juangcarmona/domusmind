# Product model review — 13 November 2026

## What changed

The original 640 tracked model files represented 630 product artifacts and 10 directory placeholders. The graph was connected, and all 181 structured behaviours were used by requirements. The file count did not establish duplication.

The model now uses PDaC v1alpha2, six native lifecycles and separate examples for actions that had been combined. Readers can start from a household question in the [product guide](../README.md). Fourteen shared titles were reviewed and clarified, with outcome titles for requirements and action titles for use cases. FR–UC relationships remain many-to-many.

| Measure | Before | After |
| --- | ---: | ---: |
| Product artifacts | 630 | 655 |
| Authored graph edges | 2936 | 2911 |
| Connected components | 1 | 1 |
| Isolated artifacts | 0 | 0 |
| Native lifecycles | 0 | 6 |
| Structured behaviours | 181 | 199 |
| Structured behaviours without requirement references | 0 | 0 |

The increase adds state meaning and concrete examples. Six combined examples have explicit successors; no artifact was discarded to meet a file limit. The migration preserved all 630 original bodies and all 221 distinct rule/use-case associations. It replaced 206 reverse-authored links with the use case's canonical governed-by relationship, removing 202 redundant arrows. Four associations were previously authored only from the rule.

Accepted artifacts are active. Four recovered claims remain draft at low confidence. Artifact acceptance describes intent; implementation and deployment are separate facts.

## Change sequence

1. [Contract and status migration](../changes/completed/chg-model-contract-clarity/change.md): serialization, acceptance status, canonical rule links, authoring templates and native checks.
2. [Task and Inbox pilot](../changes/completed/chg-task-inbox-lifecycles/change.md): native states, transition coverage and separate proposal decisions.
3. [Coordination lifecycles](../changes/completed/chg-coordination-lifecycles/change.md): Routines, Lists, Calendar Plans and external connection health.
4. [Examples and product language](../changes/completed/chg-model-readability/change.md): complete example review, ownership handoffs, distinct requirement titles and current evidence notes.

Each batch was authored as a native Product Change, validated as an overlay, given approved status under Juan's instruction to implement this plan, and applied through the CLI in the isolated branch. The branch remains a proposal for human PR acceptance. The native unaffected ledger records reviewed neighboring artifacts with provider-generated digest pins.

## States, events and examples

| Lifecycle | States | Transitions | Review boundary |
| --- | ---: | ---: | --- |
| [LC-EXTERNAL-CALENDAR-CONNECTION](../model/domain/lifecycles/lc-external-calendar-connection.md) | 9 | 6 | Needs attention and Rehydrating retain accepted names with entry triggers unresolved. |
| [LC-INBOX-PROPOSAL](../model/domain/lifecycles/lc-inbox-proposal.md) | 4 | 3 | Approval and successful application are distinct; Executing adds no UI requirement. |
| [LC-LIST](../model/domain/lifecycles/lc-list.md) | 3 | 3 | Archive is reversible; deletion removes the record permanently. |
| [LC-PLAN](../model/domain/lifecycles/lc-plan.md) | 2 | 1 | Calendar Plans have Scheduled and Cancelled states. Meal Plans have a separate, partly deferred lifecycle. |
| [LC-ROUTINE](../model/domain/lifecycles/lc-routine.md) | 3 | 4 | Dated automatic resumption is accepted intent; implementation remains incomplete. |
| [LC-TASK](../model/domain/lifecycles/lc-task.md) | 3 | 2 | Closed Tasks stay closed. |

All 19 transition identities have at least one positive example linked by covers-transition. This checks the model relationship; it does not prove every source-state variant or delivered implementation. Refused actions keep their own examples without claiming a successful transition.

The implementation contains 71 IDomainEvent declarations. Events remain implementation evidence and can be the stimulus or result in an example. This PDaC contract has no standalone domain-event artifact. Copying each event into a new document would create another inventory to maintain without adding native meaning.

InboxProposalApproved, InboxProposalApplied, RoutinePaused, RoutineResumed, RoutineDeleted, ListArchived, ListRestored, ListDeleted and EventCancelled provide evidence for the modeled changes. The registered Inbox event handler already dispatches owning-area commands through EventLogWriter. Stale architecture prose claiming events were never dispatched is corrected; accepted ADRs are unchanged.

## Handoffs across areas

- [JRN-CHAT-REVIEW-HOUSEHOLD-ACTION](../model/journeys/jrn-chat-review-household-action.md) follows a supported household Chat request into Inbox review and Calendar execution. Staging, approval and application have distinct meanings. Private Chat content stays private.
- [JRN-MEALS-PLAN-THE-WEEK](../model/journeys/jrn-meals-plan-the-week.md) follows ingredient derivation into the generated List. Meal Planning owns the derivation and list reference; Lists owns its items and lifecycle. A new derivation leaves earlier Lists unchanged.
- [LC-INBOX-PROPOSAL](../model/domain/lifecycles/lc-inbox-proposal.md) retains the owning area's authorization and validation. Approval does not grant broader access or claim a completed action.

## Four recovered claims awaiting the owner

These artifacts retain their existing content, draft status and low confidence. Their warnings are deliberate; no guessed threshold becomes accepted intent.

| Artifact | Evidence reviewed | Exact decision needed |
| --- | --- | --- |
| [CON-PRODUCT-PHASED-ROADMAP](../model/requirements/constraints/con-product-phased-roadmap.md) | Legacy roadmap, Q-0004 and current accepted assistant/meal scope | Should V1/V1.1/V2/V3 remain a current constraint or become historical planning context? Several capabilities already cross those phases. |
| [QR-AREAS-GAPS-AT-A-GLANCE](../model/requirements/quality/qr-areas-gaps-at-a-glance.md) | Legacy Areas success criteria; no accepted numerical threshold | Accept the proposed under-10-second identification and at-most-three-interaction assignment goals, choose other values, or measure first? |
| [QR-MEALS-LOW-EFFORT-PLANNING](../model/requirements/quality/qr-meals-low-effort-planning.md) | Legacy Meal Planning success criteria; no defined measurement method | Accept under two minutes for reuse-based weekly planning and one action for derivation? Which representative setup and measurement method apply? |
| [UC-MEALS-UPDATE-TEMPLATE](../model/use-cases/uc-meals-update-template.md) | One-sentence template update intent, template-name uniqueness and snapshot rule | Confirm name/slot editing, duplicate-name refusal and unchanged existing plans, or retain this for later refinement? |

The separate active Roadmap Product Change remains proposed work. Its 52 low-confidence warnings are independent of this review.

## Remaining lifecycle and implementation questions

- Inbox failure recovery: what should members see and be able to retry after an owning-area action fails? The current relational EventLogWriter rolls back a failed dispatch transaction; the product model does not define a retry or reset policy. Executing is not a new promise of a durable UI status.
- External connection status: what product events should enter Needs attention and Rehydrating? The status catalog accepts both names, but the inspected orchestration supplies no entry trigger. Native validation reports two PRODUCT112 reachability warnings.
- Calendar authorization expiry: the sync path records AuthExpired, then its outer exception handler can record Failed. The model preserves the accepted expired-authorization distinction and flags the implementation discrepancy. This change does not fix that runtime path.
- Meal Plan completion remains deferred under Q-0033. No completion trigger was invented. Routine dated resumption remains accepted intent with an implementation gap.

## Editorial review

Humanizer 3.1.0 supplied the review for repeated framing, inflated claims and unnecessary opposition. [Taste](https://github.com/Leonxlnx/taste-skill/blob/main/skills/taste-skill/SKILL.md) supplied reader order, concrete headings and useful entry points. [Plain English](https://github.com/b1rdmania/claude-plain-english-skill/blob/main/SKILL.md) supplied direct subjects, simpler wording and scannable technical prose. Product and design authorities retain their existing roles.

The edits preserve normative MUST, SHOULD and MAY meanings, access boundaries and undecided items. The scope and household constraints lead with what the product does. Existing scope exclusions remain explicit where they set an accepted boundary. Long Inbox validation prose is laid out by proposal type. The Invited/Provisioned wording now makes clear that these label the same pending invitation; a duplicate subscription-revocation outcome is removed.

## Fourteen shared FR–UC titles reviewed

The review selected fourteen title matches between requirements and use cases. Their content was reviewed in full, and requirement titles now state the required outcome.

FR–UC relationships are many-to-many. All 161 original FR-to-UC derived-from links are unchanged: 22 FRs reference multiple UCs, and 36 UCs are referenced by multiple FRs. For example, FR-FAMILY-MANAGE-MEMBER-ACCESS references five UCs, while UC-INBOX-CAPTURE-AND-REVIEW contributes to three FRs. The title review created no new FR or UC and changed none of these links.

| Requirement | Use case | Review |
| --- | --- | --- |
| [FR-AREAS-ARCHIVE-AREA](../model/requirements/functional/fr-areas-archive-area.md) | [UC-AREAS-ARCHIVE-AREA](../model/use-cases/uc-areas-archive-area.md) | The requirement keeps retention and filtered visibility; the use case supplies actor access and reference cleanup. The stale implementation note is corrected. |
| [FR-AREAS-RENAME-AREA](../model/requirements/functional/fr-areas-rename-area.md) | [UC-AREAS-RENAME-AREA](../model/use-cases/uc-areas-rename-area.md) | The requirement permits renaming; the use case specifies household access, inline editing and name validation. |
| [FR-ASSISTANT-CONFIGURE-PROVIDERS](../model/requirements/functional/fr-assistant-configure-providers.md) | [UC-ASSISTANT-CONFIGURE-PROVIDERS](../model/use-cases/uc-assistant-configure-providers.md) | The requirement specifies one to three targets and credential protection; the use case describes manager configuration and validation. |
| [FR-CALENDAR-BACKGROUND-REFRESH](../model/requirements/functional/fr-calendar-background-refresh.md) | [UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS](../model/use-cases/uc-calendar-refresh-external-calendars.md) | The requirement specifies independent refresh and the 60-minute default; the use case describes scheduling and catch-up. Undecided interval values and staleness thresholds remain explicit. |
| [FR-CALENDAR-CANCEL-PLAN](../model/requirements/functional/fr-calendar-cancel-plan.md) | [UC-CALENDAR-CANCEL-PLAN](../model/use-cases/uc-calendar-cancel-plan.md) | The requirement states cancellation and duplicate refusal; the use case specifies closure and keeps Agenda visibility unresolved. |
| [FR-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR](../model/requirements/functional/fr-calendar-configure-external-calendar.md) | [UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR](../model/use-cases/uc-calendar-configure-external-calendar.md) | The requirement preserves selected feeds, supported horizons and optional refresh settings; the use case describes reloads, deselection and failures. |
| [FR-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR](../model/requirements/functional/fr-calendar-disconnect-external-calendar.md) | [UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR](../model/use-cases/uc-calendar-disconnect-external-calendar.md) | The requirement preserves local removal and provider isolation; the use case specifies owner access and failure conditions. |
| [FR-CALENDAR-RESCHEDULE-PLAN](../model/requirements/functional/fr-calendar-reschedule-plan.md) | [UC-CALENDAR-RESCHEDULE-PLAN](../model/use-cases/uc-calendar-reschedule-plan.md) | The requirement preserves identity, participants and reminders; the use case describes new timing and validation. |
| [FR-CALENDAR-SCHEDULE-PLAN](../model/requirements/functional/fr-calendar-schedule-plan.md) | [UC-CALENDAR-SCHEDULE-PLAN](../model/use-cases/uc-calendar-schedule-plan.md) | The requirement preserves title/start inputs and date-only or multi-day Plans; the use case supplies the capture flow and validation. |
| [FR-LISTS-DELETE-LIST](../model/requirements/functional/fr-lists-delete-list.md) | [UC-LISTS-DELETE-LIST](../model/use-cases/uc-lists-delete-list.md) | The requirement specifies permanent removal and no restoration; the use case describes the action and a missing List. |
| [FR-MEALS-APPLY-TEMPLATE](../model/requirements/functional/fr-meals-apply-template.md) | [UC-MEALS-APPLY-TEMPLATE](../model/use-cases/uc-meals-apply-template.md) | The requirement specifies a snapshot, existing-week behavior and missing-recipe refusal; the use case describes selecting and applying a template. |
| [FR-RECIPES-UPDATE](../model/requirements/functional/fr-recipes-update.md) | [UC-RECIPES-UPDATE-RECIPE](../model/use-cases/uc-recipes-update-recipe.md) | The requirement specifies full replacement and name uniqueness; the use case separates ingredient editing and describes conflicts. |
| [FR-TASKS-DELETE-ROUTINE](../model/requirements/functional/fr-tasks-delete-routine.md) | [UC-TASKS-DELETE-ROUTINE](../model/use-cases/uc-tasks-delete-routine.md) | The requirement states removal from the product; the use case describes the action. Existing Routine.Delete implementation now replaces the stale absence note. |
| [FR-TASKS-UNASSIGN-TASK](../model/requirements/functional/fr-tasks-unassign-task.md) | [UC-TASKS-UNASSIGN-TASK](../model/use-cases/uc-tasks-unassign-task.md) | The requirement states the unassigned outcome; the use case describes removing the assignee. Existing HouseholdTask.Unassign replaces the stale absence note. |

## Complete original structured-behaviour review

All 181 original examples were reviewed for a clear context, one stimulus, conjunctive outcomes, requirement references, uncertainty and lifecycle relevance. Alternatives with different stimuli or plan states have successors. Ordinary parameter choices within one action remain together when their outcome is the same. Existing evidence and confidence are preserved.

| Original example | Disposition |
| --- | --- |
| [SB-AGENDA-HOUSEHOLD-ALL-SOURCES](../model/behaviours/sb-agenda-household-all-sources.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AGENDA-HOUSEHOLD-EXCLUDES-EXTERNAL](../model/behaviours/sb-agenda-household-excludes-external.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AGENDA-HOUSEHOLD-LIST-ITEMS](../model/behaviours/sb-agenda-household-list-items.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AGENDA-MEMBER-HOUSEHOLD-PLAN-WITHOUT-PARTICIPANTS](../model/behaviours/sb-agenda-member-household-plan-without-participants.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AGENDA-MEMBER-INCLUDES-IMPORTED](../model/behaviours/sb-agenda-member-includes-imported.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AGENDA-MEMBER-LIST-ITEMS-READ-ONLY](../model/behaviours/sb-agenda-member-list-items-read-only.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AREAS-ADD-SUPPORT](../model/behaviours/sb-areas-add-support.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AREAS-ARCHIVE-AREA](../model/behaviours/sb-areas-archive-area.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AREAS-ASSIGN-FIRST-OWNER](../model/behaviours/sb-areas-assign-first-owner.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AREAS-CREATE-AREA](../model/behaviours/sb-areas-create-area.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AREAS-GAP-INDICATOR](../model/behaviours/sb-areas-gap-indicator.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AREAS-REJECT-DUPLICATE-SUPPORT](../model/behaviours/sb-areas-reject-duplicate-support.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AREAS-REJECT-OUTSIDE-OWNER](../model/behaviours/sb-areas-reject-outside-owner.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AREAS-REJECT-OUTSIDE-TRANSFER](../model/behaviours/sb-areas-reject-outside-transfer.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AREAS-REJECT-OWNER-AS-SUPPORT](../model/behaviours/sb-areas-reject-owner-as-support.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AREAS-REMOVE-SUPPORT](../model/behaviours/sb-areas-remove-support.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AREAS-RENAME-AREA](../model/behaviours/sb-areas-rename-area.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AREAS-REPLACE-OWNER](../model/behaviours/sb-areas-replace-owner.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AREAS-TASK-REFERENCES-AREA](../model/behaviours/sb-areas-task-references-area.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AREAS-TRANSFER-OWNERSHIP](../model/behaviours/sb-areas-transfer-ownership.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-AREAS-UNOWNED-FIRST](../model/behaviours/sb-areas-unowned-first.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-ADD-DUPLICATE-PARTICIPANT](../model/behaviours/sb-calendar-add-duplicate-participant.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-ADD-DUPLICATE-REMINDER](../model/behaviours/sb-calendar-add-duplicate-reminder.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-ADD-PARTICIPANT](../model/behaviours/sb-calendar-add-participant.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-ADD-PARTICIPANT-CANCELLED-PLAN](../model/behaviours/sb-calendar-add-participant-cancelled-plan.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-ADD-REMINDER](../model/behaviours/sb-calendar-add-reminder.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-CANCEL-CANCELLED-PLAN](../model/behaviours/sb-calendar-cancel-cancelled-plan.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-CANCEL-PLAN](../model/behaviours/sb-calendar-cancel-plan.md) | Retained; covers [LC-PLAN](../model/domain/lifecycles/lc-plan.md) / CANCEL. |
| [SB-CALENDAR-CATCH-UP-ON-AGENDA-OPEN](../model/behaviours/sb-calendar-catch-up-on-agenda-open.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-CHANGE-REMINDER-CANCELLED-PLAN](../model/behaviours/sb-calendar-change-reminder-cancelled-plan.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-CONFIGURE-SELECTION-AND-HORIZON](../model/behaviours/sb-calendar-configure-selection-and-horizon.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-CONNECT-DUPLICATE-ACCOUNT](../model/behaviours/sb-calendar-connect-duplicate-account.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-CONNECT-MISSING-ACCESS](../model/behaviours/sb-calendar-connect-missing-access.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-CONNECT-OUTLOOK](../model/behaviours/sb-calendar-connect-outlook.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-CREATE-SECURE-SUBSCRIPTION](../model/behaviours/sb-calendar-create-secure-subscription.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-DESELECT-FEED](../model/behaviours/sb-calendar-deselect-feed.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-DISCONNECT](../model/behaviours/sb-calendar-disconnect.md) | Retained; covers [LC-EXTERNAL-CALENDAR-CONNECTION](../model/domain/lifecycles/lc-external-calendar-connection.md) / DISCONNECT. |
| [SB-CALENDAR-DISCONNECT-UNKNOWN](../model/behaviours/sb-calendar-disconnect-unknown.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-HORIZON-CHANGE-RELOADS](../model/behaviours/sb-calendar-horizon-change-reloads.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-MANUAL-SYNC](../model/behaviours/sb-calendar-manual-sync.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-READ-SUBSCRIPTION-FEED](../model/behaviours/sb-calendar-read-subscription-feed.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-REFRESH-SKIP-FRESH](../model/behaviours/sb-calendar-refresh-skip-fresh.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-REFRESH-STALE](../model/behaviours/sb-calendar-refresh-stale.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-REMOVE-MISSING-REMINDER](../model/behaviours/sb-calendar-remove-missing-reminder.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-REMOVE-NON-PARTICIPANT](../model/behaviours/sb-calendar-remove-non-participant.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-REMOVE-PARTICIPANT](../model/behaviours/sb-calendar-remove-participant.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-REMOVE-REMINDER](../model/behaviours/sb-calendar-remove-reminder.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-RESCHEDULE-CANCELLED-PLAN](../model/behaviours/sb-calendar-reschedule-cancelled-plan.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-RESCHEDULE-INVALID-TIMES](../model/behaviours/sb-calendar-reschedule-invalid-times.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-RESCHEDULE-PLAN](../model/behaviours/sb-calendar-reschedule-plan.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-REVOKE-SECURE-SUBSCRIPTION](../model/behaviours/sb-calendar-revoke-secure-subscription.md) | Retained; wording clarified without changing the outcome. |
| [SB-CALENDAR-SCHEDULE-ALL-DAY-PLAN](../model/behaviours/sb-calendar-schedule-all-day-plan.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-SCHEDULE-PLAN-END-BEFORE-START](../model/behaviours/sb-calendar-schedule-plan-end-before-start.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-SCHEDULE-PLAN-MINIMUM](../model/behaviours/sb-calendar-schedule-plan-minimum.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-SCHEDULE-PLAN-UNKNOWN-PARTICIPANT](../model/behaviours/sb-calendar-schedule-plan-unknown-participant.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-SYNC-CONCURRENT](../model/behaviours/sb-calendar-sync-concurrent.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-SYNC-INVALID-STATE](../model/behaviours/sb-calendar-sync-invalid-state.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-SYNC-NO-FEEDS](../model/behaviours/sb-calendar-sync-no-feeds.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-CALENDAR-UNSUPPORTED-HORIZON](../model/behaviours/sb-calendar-unsupported-horizon.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-DUPLICATE-MEMBER-REJECTED](../model/behaviours/sb-family-duplicate-member-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-FUTURE-BIRTH-DATE-REJECTED](../model/behaviours/sb-family-future-birth-date-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-HOUSEHOLD-CREATED](../model/behaviours/sb-family-household-created.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-HOUSEHOLD-EMPTY-NAME-REJECTED](../model/behaviours/sb-family-household-empty-name-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-INVALID-ROLE-REJECTED](../model/behaviours/sb-family-invalid-role-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-MANAGER-DISABLES-ACCESS](../model/behaviours/sb-family-manager-disables-access.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-MANAGER-ENABLES-ACCESS](../model/behaviours/sb-family-manager-enables-access.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-MANAGER-PROVISIONS-ACCESS](../model/behaviours/sb-family-manager-provisions-access.md) | Retained; wording clarified without changing the outcome. |
| [SB-FAMILY-MANAGER-UPDATES-MEMBER](../model/behaviours/sb-family-manager-updates-member.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-MEMBER-ADDED](../model/behaviours/sb-family-member-added.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-MEMBER-UNKNOWN-HOUSEHOLD-REJECTED](../model/behaviours/sb-family-member-unknown-household-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-MEMBER-UPDATES-OWN-DETAILS](../model/behaviours/sb-family-member-updates-own-details.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-MEMBER-UPDATES-OWN-PROFILE](../model/behaviours/sb-family-member-updates-own-profile.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-NON-MANAGER-OWN-ROLE-REJECTED](../model/behaviours/sb-family-non-manager-own-role-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-NON-MANAGER-UPDATE-REJECTED](../model/behaviours/sb-family-non-manager-update-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-PET-ACCESS-REJECTED](../model/behaviours/sb-family-pet-access-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-FAMILY-PET-ADDED](../model/behaviours/sb-family-pet-added.md) | Retained; context, stimulus and outcomes remain valid. |
| SB-INBOX-CAPTURE-AND-INTERPRET | Replaced by [SB-INBOX-CAPTURE-NOTE](../model/behaviours/sb-inbox-capture-note.md), [SB-CHAT-STAGE-PLAN-PROPOSAL](../model/behaviours/sb-chat-stage-plan-proposal.md), [SB-INBOX-INTERPRET-NOTE](../model/behaviours/sb-inbox-interpret-note.md). |
| SB-INBOX-REVIEW-PROPOSAL | Replaced by [SB-INBOX-APPROVE-PROPOSAL](../model/behaviours/sb-inbox-approve-proposal.md), [SB-INBOX-APPLY-PROPOSAL](../model/behaviours/sb-inbox-apply-proposal.md), [SB-INBOX-REJECT-PROPOSAL](../model/behaviours/sb-inbox-reject-proposal.md), [SB-INBOX-UNAUTHORIZED-PROPOSAL](../model/behaviours/sb-inbox-unauthorized-proposal.md), [SB-INBOX-INVALID-PROPOSAL](../model/behaviours/sb-inbox-invalid-proposal.md). |
| [SB-LISTS-ADD-ITEM](../model/behaviours/sb-lists-add-item.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-ARCHIVE](../model/behaviours/sb-lists-archive.md) | Retained; covers [LC-LIST](../model/domain/lifecycles/lc-list.md) / ARCHIVE. |
| [SB-LISTS-ARCHIVE-TWICE-REJECTED](../model/behaviours/sb-lists-archive-twice-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| SB-LISTS-ARCHIVED-ADD-REJECTED | Replaced by [SB-LISTS-ARCHIVED-ITEM-ADD-REFUSED](../model/behaviours/sb-lists-archived-item-add-refused.md), [SB-LISTS-ARCHIVED-ITEM-CHECK-REFUSED](../model/behaviours/sb-lists-archived-item-check-refused.md). |
| [SB-LISTS-BROWSE-ACTIVE](../model/behaviours/sb-lists-browse-active.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-BROWSE-EXCLUDES-ARCHIVED](../model/behaviours/sb-lists-browse-excludes-archived.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-CHECK-ITEM](../model/behaviours/sb-lists-check-item.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-CHECKED-ITEM-STAYS-IN-AGENDA](../model/behaviours/sb-lists-checked-item-stays-in-agenda.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-CLEAR-PLAN-LINK](../model/behaviours/sb-lists-clear-plan-link.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-CLEAR-QUANTITY](../model/behaviours/sb-lists-clear-quantity.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-CLEAR-TIMING](../model/behaviours/sb-lists-clear-timing.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-CLEAR-TIMING-NOOP](../model/behaviours/sb-lists-clear-timing-noop.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-CLEARED-ITEM-LEAVES-AGENDA](../model/behaviours/sb-lists-cleared-item-leaves-agenda.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-CREATE-LINKED-TO-PLAN](../model/behaviours/sb-lists-create-linked-to-plan.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-CREATE-NAME-ONLY](../model/behaviours/sb-lists-create-name-only.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-DELETE](../model/behaviours/sb-lists-delete.md) | Retained; covers [LC-LIST](../model/domain/lifecycles/lc-list.md) / DELETE. |
| [SB-LISTS-DUE-TODAY-IN-AGENDA](../model/behaviours/sb-lists-due-today-in-agenda.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-OPEN-UNCHECKED-FIRST](../model/behaviours/sb-lists-open-unchecked-first.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-REMOVE-DATED-ITEM](../model/behaviours/sb-lists-remove-dated-item.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-REMOVE-ITEM](../model/behaviours/sb-lists-remove-item.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-RENAME](../model/behaviours/sb-lists-rename.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-RENAME-ITEM](../model/behaviours/sb-lists-rename-item.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-REORDER](../model/behaviours/sb-lists-reorder.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-REORDER-MISMATCH-REJECTED](../model/behaviours/sb-lists-reorder-mismatch-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-RESTORE](../model/behaviours/sb-lists-restore.md) | Retained; covers [LC-LIST](../model/domain/lifecycles/lc-list.md) / RESTORE. |
| [SB-LISTS-SEQUENTIAL-CAPTURE](../model/behaviours/sb-lists-sequential-capture.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-SET-DUE-DATE](../model/behaviours/sb-lists-set-due-date.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-SET-REMINDER-ONLY](../model/behaviours/sb-lists-set-reminder-only.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-SET-REPEAT-ONLY](../model/behaviours/sb-lists-set-repeat-only.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-STAR-IDEMPOTENT](../model/behaviours/sb-lists-star-idempotent.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-STAR-ITEM](../model/behaviours/sb-lists-star-item.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-LISTS-UNCHECK-ITEM](../model/behaviours/sb-lists-uncheck-item.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-AGENDA-HIDES-EMPTY-SLOT](../model/behaviours/sb-meals-agenda-hides-empty-slot.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-AGENDA-SHOWS-ASSIGNED-MEAL](../model/behaviours/sb-meals-agenda-shows-assigned-meal.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-APPLY-TEMPLATE-DELETED-RECIPE](../model/behaviours/sb-meals-apply-template-deleted-recipe.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-APPLY-TEMPLATE-EMPTY-WEEK](../model/behaviours/sb-meals-apply-template-empty-week.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-APPLY-TEMPLATE-EXISTING-WEEK](../model/behaviours/sb-meals-apply-template-existing-week.md) | Retained; context, stimulus and outcomes remain valid. |
| SB-MEALS-ASSIGN-RECIPE-TO-SLOT | Replaced by [SB-MEALS-ASSIGN-RECIPE-DRAFT](../model/behaviours/sb-meals-assign-recipe-draft.md), [SB-MEALS-ASSIGN-RECIPE-ACTIVE](../model/behaviours/sb-meals-assign-recipe-active.md). |
| [SB-MEALS-CLEAR-SLOT](../model/behaviours/sb-meals-clear-slot.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-COMPLETED-PLAN-REJECTS-CHANGE](../model/behaviours/sb-meals-completed-plan-rejects-change.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-COPY-NO-PREVIOUS-PLAN](../model/behaviours/sb-meals-copy-no-previous-plan.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-COPY-PREVIOUS-WEEK](../model/behaviours/sb-meals-copy-previous-week.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-CREATE-PLAN-EXISTING-WEEK](../model/behaviours/sb-meals-create-plan-existing-week.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-CREATE-PLAN-FOR-WEEK](../model/behaviours/sb-meals-create-plan-for-week.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-CREATE-TEMPLATE](../model/behaviours/sb-meals-create-template.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-LOCKED-SLOT-REJECTED](../model/behaviours/sb-meals-locked-slot-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-PROMOTE-TO-ACTIVE](../model/behaviours/sb-meals-promote-to-active.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-SHOPPING-LIST-FROM-PLAN](../model/behaviours/sb-meals-shopping-list-from-plan.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-SHOPPING-LIST-NO-RECIPES](../model/behaviours/sb-meals-shopping-list-no-recipes.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-SHOPPING-LIST-RE-REQUEST](../model/behaviours/sb-meals-shopping-list-re-request.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-TEMPLATE-DUPLICATE-NAME-REJECTED](../model/behaviours/sb-meals-template-duplicate-name-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-UNLOCK-AND-CHANGE-SLOT](../model/behaviours/sb-meals-unlock-and-change-slot.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-VIEW-RECIPE-SLOT-DETAILS](../model/behaviours/sb-meals-view-recipe-slot-details.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-MEALS-VIEW-WEEK-ORDERED](../model/behaviours/sb-meals-view-week-ordered.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-ADD-INGREDIENT](../model/behaviours/sb-recipes-add-ingredient.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-ADD-TO-LIBRARY](../model/behaviours/sb-recipes-add-to-library.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-BROWSE-LIBRARY](../model/behaviours/sb-recipes-browse-library.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-DELETE-BLOCKED-BY-ACTIVE-PLAN](../model/behaviours/sb-recipes-delete-blocked-by-active-plan.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-DELETE-UNREFERENCED](../model/behaviours/sb-recipes-delete-unreferenced.md) | Retained; context, stimulus and outcomes remain valid. |
| SB-RECIPES-DELETE-USED-BY-PAST-OR-DRAFT | Replaced by [SB-RECIPES-DELETE-USED-BY-DRAFT](../model/behaviours/sb-recipes-delete-used-by-draft.md), [SB-RECIPES-DELETE-USED-BY-COMPLETED](../model/behaviours/sb-recipes-delete-used-by-completed.md), [SB-RECIPES-DELETE-USED-BY-MIXED](../model/behaviours/sb-recipes-delete-used-by-mixed.md). |
| [SB-RECIPES-DUPLICATE-INGREDIENT-REJECTED](../model/behaviours/sb-recipes-duplicate-ingredient-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-DUPLICATE-NAME-REJECTED](../model/behaviours/sb-recipes-duplicate-name-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-LIBRARY-CREATE](../model/behaviours/sb-recipes-library-create.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-LIBRARY-DELETE](../model/behaviours/sb-recipes-library-delete.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-LIBRARY-EDIT](../model/behaviours/sb-recipes-library-edit.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-LIBRARY-OPEN-DETAIL](../model/behaviours/sb-recipes-library-open-detail.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-REMOVE-INGREDIENT](../model/behaviours/sb-recipes-remove-ingredient.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-RENAME-CONFLICT](../model/behaviours/sb-recipes-rename-conflict.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-RESTRICTED-MEAL-TYPE-HIDDEN](../model/behaviours/sb-recipes-restricted-meal-type-hidden.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-UNRESTRICTED-SHOWN-EVERYWHERE](../model/behaviours/sb-recipes-unrestricted-shown-everywhere.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-UPDATE](../model/behaviours/sb-recipes-update.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-UPDATE-INGREDIENT](../model/behaviours/sb-recipes-update-ingredient.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-VIEW-DETAIL](../model/behaviours/sb-recipes-view-detail.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-RECIPES-VIEW-NOT-FOUND](../model/behaviours/sb-recipes-view-not-found.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-AGENDA-HIDES-PAUSED-ROUTINE](../model/behaviours/sb-tasks-agenda-hides-paused-routine.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-AGENDA-SHOWS-DUE-TASK](../model/behaviours/sb-tasks-agenda-shows-due-task.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-AGENDA-SHOWS-ROUTINE-OCCURRENCE](../model/behaviours/sb-tasks-agenda-shows-routine-occurrence.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-ASSIGN-MEMBER](../model/behaviours/sb-tasks-assign-member.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-ASSIGN-NON-MEMBER-REJECTED](../model/behaviours/sb-tasks-assign-non-member-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-CANCEL-COMPLETED-REJECTED](../model/behaviours/sb-tasks-cancel-completed-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-CANCEL-TASK](../model/behaviours/sb-tasks-cancel-task.md) | Retained; covers [LC-TASK](../model/domain/lifecycles/lc-task.md) / CANCEL. |
| [SB-TASKS-CANCEL-TWICE-REJECTED](../model/behaviours/sb-tasks-cancel-twice-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-COMPLETE-CANCELLED-REJECTED](../model/behaviours/sb-tasks-complete-cancelled-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-COMPLETE-TASK](../model/behaviours/sb-tasks-complete-task.md) | Retained; covers [LC-TASK](../model/domain/lifecycles/lc-task.md) / COMPLETE. |
| [SB-TASKS-COMPLETE-TWICE-REJECTED](../model/behaviours/sb-tasks-complete-twice-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-CREATE-NON-MEMBER-REJECTED](../model/behaviours/sb-tasks-create-non-member-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-CREATE-TITLE-ONLY](../model/behaviours/sb-tasks-create-title-only.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-CREATE-WEEKLY-ROUTINE](../model/behaviours/sb-tasks-create-weekly-routine.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-CREATE-WITH-ASSIGNEE-DUE](../model/behaviours/sb-tasks-create-with-assignee-due.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-PAUSE-PAUSED-REJECTED](../model/behaviours/sb-tasks-pause-paused-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-PAUSE-ROUTINE](../model/behaviours/sb-tasks-pause-routine.md) | Retained; covers [LC-ROUTINE](../model/domain/lifecycles/lc-routine.md) / PAUSE. |
| [SB-TASKS-PAUSE-UNTIL-AUTO-RESUME](../model/behaviours/sb-tasks-pause-until-auto-resume.md) | Retained; covers [LC-ROUTINE](../model/domain/lifecycles/lc-routine.md) / RESUME-ON-DATE. |
| [SB-TASKS-RESCHEDULE-CANCELLED-REJECTED](../model/behaviours/sb-tasks-reschedule-cancelled-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-RESCHEDULE-COMPLETED-REJECTED](../model/behaviours/sb-tasks-reschedule-completed-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-RESCHEDULE-PENDING](../model/behaviours/sb-tasks-reschedule-pending.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-RESUME-ACTIVE-REJECTED](../model/behaviours/sb-tasks-resume-active-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-RESUME-ROUTINE](../model/behaviours/sb-tasks-resume-routine.md) | Retained; covers [LC-ROUTINE](../model/domain/lifecycles/lc-routine.md) / RESUME. |
| [SB-TASKS-ROUTINE-INVALID-RECURRENCE-REJECTED](../model/behaviours/sb-tasks-routine-invalid-recurrence-rejected.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-ROUTINE-PEOPLE-SCOPE-NEEDS-TARGETS](../model/behaviours/sb-tasks-routine-people-scope-needs-targets.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-TASKS-UPDATE-ROUTINE](../model/behaviours/sb-tasks-update-routine.md) | Retained; context, stimulus and outcomes remain valid. |
| SB-WEB-DISCONNECT-CONFIRMATION | Replaced by [SB-WEB-REQUEST-DISCONNECT](../model/behaviours/sb-web-request-disconnect.md), [SB-WEB-CONFIRM-DISCONNECT](../model/behaviours/sb-web-confirm-disconnect.md). |
| [SB-WEB-NAVIGATE-AGENDA-TO-LISTS](../model/behaviours/sb-web-navigate-agenda-to-lists.md) | Retained; context, stimulus and outcomes remain valid. |
| [SB-WEB-SETTINGS-OPENS-ON-PROFILE](../model/behaviours/sb-web-settings-opens-on-profile.md) | Retained; context, stimulus and outcomes remain valid. |

## Verification

The final branch passes npm run verify:product: native model and live-change validation, managed-integration integrity, OpenSpec validation, OpenSpec citation policy, configured citation verification and Product Explorer generation. All 915 citations are current; none is stale, tampered or unresolved. The eight stale architecture scope citations were refreshed only after reviewing and correcting their consuming prose.

Native validation has zero errors and 58 warnings: four unresolved recovered claims, 52 claims in the separate Roadmap proposal and two explicitly unresolved connection-state entry paths. The graph is one connected component with no isolated nodes and no Structured Behaviour missing a requirement reference.

Reproduce from the repository root:

```sh
npm ci --ignore-scripts
npm run verify:product
npx --no-install arcshape validate --links --citations
git diff --check
```

Open .product/generated/explorer.html to inspect the native graph. Generated files remain untracked under the existing configuration. This is a product-definition change; no application behavior, merge or deployment is claimed by these checks.
