# Draft Surface Spec — Household Assets (working label)

Status: proposed by `CHG-HOUSEHOLD-ASSETS-001`; not accepted product terminology or implementation contract.

## Purpose

Help household members answer: what do we manage, where is it, what do we know, what needs attention, who is responsible, where is its paperwork, what has been done, and what has it cost?

This is an operational household memory surface, not an inventory-counting dashboard, accounting screen or maintenance task board.

## Depends On

- Accepted ProductShape model and `CHG-HOUSEHOLD-ASSETS-001`.
- `docs/design/surface-system.md`.
- Existing Household, Areas, Agenda, Calendar, Tasks and Lists ownership boundaries.

## Entry Points

- Main navigation using the final household-facing label.
- Agenda obligation projection opens the associated Asset and due occurrence.
- Contextual links from Areas or explicitly related Task/Calendar Plan/List detail.

## Role and Shell

Use the persistent product shell and compact page header. Desktop follows the established dense-list plus right inspector pattern. Mobile preserves the same information and actions with a collapsed list and pushed detail or bottom sheet; do not make a separate mobile workflow.

## Default View

A compact, searchable household list. Rows should prioritize name, category, location/containing property, responsible Area/member and a concise attention cue (for example next due or overdue). Parent-child relationships are visible but do not imply ownership or responsibility. Avoid decorative category cards and summary metric dashboards.

Useful filters may include category, location/parent, responsibility, due/overdue and active/archived. Search matches names and ordinary identifying details; sensitive identifiers require product/privacy review before indexing.

## Inspector

Selection opens an inspector without losing list context. It presents, in this order as available:

1. Identity, category, lifecycle and useful common details.
2. Location/containing Asset, management basis and responsible Area/member as separate facts.
3. Upcoming and overdue obligations with next action, recurrence summary and explicit completion/reschedule actions.
4. Documents and clear distinction between uploaded content and an external/authorized reference.
5. Maintenance, repair, inspection and installation history with date, provider, optional actual cost and evidence.
6. Explicitly linked Tasks, Calendar plans or Lists, labeled by their owning context.

No standalone full-detail page is assumed unless the existing surface system requires it.

## Capture and Interaction

“Add” starts with a small required set (name and broad category), then offers progressive optional details. Capture must not require a serial number, parent, member, Area or exhaustive attribute schema. Parent selection is optional and represents containment only. The final UI label remains subject to user research.

Obligation create/edit presents due rule, timezone/local date semantics, responsible reference and lifecycle clearly. Completion asks only for necessary outcome details and offers optional provider, cost, notes and supporting paperwork. Unscheduled work can be recorded without first inventing an obligation. Any linked Task or appointment is an explicit separate action.

## Agenda Relationship

Agenda displays an obligation due projection with Asset identity and a route to the obligation. It must distinguish an obligation from a Task or Calendar Plan. If a member separately creates related work, show the relationship without collapsing rows or synchronizing completion implicitly.

## Accessibility and Responsive Behavior

Preserve keyboard navigation, labeled controls, focus management in the inspector/bottom sheet, non-color-only overdue cues, readable hierarchy and consistent actions at narrow widths. Validate with browser journeys at supported desktop and mobile viewports.

## Open UX Decisions

- Household-facing label and category vocabulary.
- How a property is represented and whether location needs a reusable entity.
- Which list row metadata and attention states are most useful without creating visual noise.
- Whether document upload is MVP or an authorized reference-only first step, pending storage/security assessment.
- How much recurrence detail is legible in list and Agenda cues.
