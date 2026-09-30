---
id: CHG-ROADMAP
type: product-change
title: Propose the phased DomusMind roadmap as future product
status: draft
base-revision: '3fc578f'
operations:
  add:
    - UC-AREAS-CLEAR-OWNER
    - UC-AREAS-DESCRIBE-AREA
    - UC-AREAS-MANAGE-PARTICIPANTS
    - UC-AREAS-REACTIVATE-AREA
    - UC-CALENDAR-COMPLETE-PLAN
    - UC-CALENDAR-MANAGE-RECURRING-PLANS
    - UC-FAMILY-ASSIGN-HOUSEHOLD-ROLE
    - UC-FAMILY-MANAGE-MEMBER-CONTACTS
    - UC-FAMILY-MANAGE-MEMBER-LIFECYCLE
    - UC-FAMILY-MANAGE-RELATIONSHIPS
    - UC-FAMILY-REMOVE-MEMBER
    - UC-LISTS-DUPLICATE-ITEM
    - UC-LISTS-MOVE-ITEM
    - UC-ROADMAP-PLAN-PROPERTY-MAINTENANCE
    - UC-ROADMAP-SEE-RENEWALS-AND-DEADLINES
    - UC-ROADMAP-TRACK-HOUSEHOLD-STOCK
    - UC-ROADMAP-TRACK-IMPORTANT-DOCUMENTS
    - UC-TASKS-START-TASK
    - BR-CALENDAR-COMPLETED-PLAN-SCHEDULE-FIXED
    - BR-CALENDAR-RECURRING-PLAN-HAS-RULE
    - BR-FAMILY-KIND-ROLE-COMPATIBILITY
    - BR-FAMILY-MEMBER-LIFECYCLE
    - BR-FAMILY-RELATIONSHIP-INTEGRITY
    - TERM-AGENDA-MARKER
    - TERM-AREA-PARTICIPANT
    - TERM-HOUSEHOLD-ROLE
    - TERM-IMPORTANT-DOCUMENT
    - TERM-PROPERTY-MAINTENANCE
    - TERM-RELATIONSHIP
    - TERM-RENEWAL-DEADLINE
    - TERM-SUPPLY-STATE
    - BC-ADMINISTRATION
    - BC-DOCUMENTS
    - BC-INVENTORY
    - BC-PROPERTY
    - FR-AREAS-DEFAULT-AREAS
    - FR-AREAS-FUTURE-BALANCE-INSIGHTS
    - FR-FAMILY-MANAGE-RELATIONSHIPS
    - FR-FAMILY-REMOVE-MEMBER
    - FR-ROADMAP-AI-ASSISTED-INTERPRETATION
    - FR-ROADMAP-AUTOMATION-PIPELINES
    - FR-ROADMAP-CALENDAR-SYNCHRONIZATION
    - FR-ROADMAP-HOUSEHOLD-INSIGHTS
    - FR-ROADMAP-MESSAGING-INTEGRATIONS
    - FR-ROADMAP-QUICK-CAPTURE-AND-IMPORT
    - FR-ROADMAP-REMINDER-ROUTING
    - FR-TASKS-FUTURE-TASK-INSIGHTS
    - FR-TASKS-START-TASK
    - SB-FAMILY-DUPLICATE-RELATIONSHIP-REJECTED
    - SB-FAMILY-MEMBER-REMOVED
    - SB-FAMILY-PET-REMOVED
    - SB-FAMILY-RELATIONSHIP-ASSIGNED
    - SB-FAMILY-RELATIONSHIP-REMOVED
    - SB-FAMILY-RELATIONSHIP-UNKNOWN-MEMBER-REJECTED
    - SB-FAMILY-SELF-RELATIONSHIP-REJECTED
  modify: []
  remove: []
---

## Problem

The recovered initial baseline (CHG-INITIAL) mixed current product with the phased roadmap and with capabilities the sources name only as planned or future. Accepting them together would put future product into the baseline as if it were current, and would make the baseline's review depend on roadmap items nobody has yet shaped.

## Intended Product Outcome

A separate, reviewable proposal of DomusMind's future product, following the phased roadmap stated by CON-PRODUCT-PHASED-ROADMAP:

- V1.1 operational hardening: removing people and pets, relationships between people, a person's stay in the household, household roles and their fit with kinds of person.
- V2 household domain expansion: household administration with renewals and deadlines, important documents, property and maintenance, inventory and supply state, several contacts and emergency information per person, and recurring plans.
- V3 intelligence and integrations: AI-assisted interpretation, automation pipelines, calendar synchronisation beyond read-only import, household insights, messaging integrations, quick capture and import, and reminder routing.
- Unversioned planned capabilities recovered with low confidence: starting a task, richer task attributes and insights, completing a plan, recurring-plan rules, clearing, describing and reactivating Areas, Area participants, default Areas, responsibility balance insights, duplicating and moving list items, and Agenda markers.

Once accepted, these artifacts join the product model as the declared future of the product, still as drafts to be refined area by area.

## Rationale

Product-owner decisions:

- Q-0004 (during recovery): the whole roadmap is modelled, with V2 and V3 capabilities as low-confidence drafts.
- "roadmap-split" (review of the recovered draft, 2026-09-30): the roadmap and the low-confidence items whose body marks them as planned, future or not current product move out of CHG-INITIAL into this change, so that the initial baseline states only current product. Low-confidence items that describe current product stay in CHG-INITIAL.

This change proposes the phased roadmap (V1.1, V2, V3) as future product. It is to be accepted only after CHG-INITIAL is applied and is the baseline: its artifacts reference baseline artifacts (actors, bounded contexts, terms, use cases and rules of CHG-INITIAL), which do not exist in the product model until then. The moved artifacts are unchanged apart from their references.

## Affected Product Areas

Household and members (V1.1 membership and relationships, V2 contacts), areas of responsibility (planned Area capabilities), plans and the Agenda (recurring plans, plan completion, markers), tasks (starting tasks, task insights), shared lists (duplicate and move items), the new V2 household domains, and the V3 intelligence and integration capabilities across the product.

## Open Questions

- Q-0011: what happens to tasks assigned to a person who is removed from the household (deferred with member removal, V1.1).
- Q-0018: what happens to the Areas a removed person owns or supports (deferred with member removal, V1.1).
- Q-0059: the intent behind responsibility balance, overload detection and owner suggestion is not yet described.
- Q-0046: whether and through which channel reminders are delivered, which reminder routing (V3) depends on.
- Which of these items are kept, reshaped or dropped when each area is refined; the product owner has not yet reviewed them one by one.

## Product Acceptance

With CHG-INITIAL applied, a reviewer can read the roadmap artifacts and recognise the phased future of DomusMind: each item is attributed to its phase or marked unversioned, each is traceable to its evidence, and every low-confidence (PRODUCT111) artifact has been reviewed and either confirmed, corrected or removed. The change validates against the baseline with no errors.

## Out of Scope

The current product, which CHG-INITIAL defines. Any commitment to when a phase is delivered: this change says nothing about what is implemented, released or deployed. Architecture, technical design, API contracts and persistence.
