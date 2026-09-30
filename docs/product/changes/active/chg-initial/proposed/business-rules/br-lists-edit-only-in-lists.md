---
id: BR-LISTS-EDIT-ONLY-IN-LISTS
type: business-rule
title: "Projected list items are read-only in the Agenda"
status: draft
applies-to:
  - UC-LISTS-SEE-ITEMS-IN-AGENDA
  - UC-AGENDA-INSPECT-ENTRY
  - UC-AGENDA-VIEW-MEMBER-AGENDA
  - UC-AGENDA-VIEW-HOUSEHOLD-AGENDA
uses-terms:
  - TERM-PROJECTED-LIST-ITEM
  - TERM-LIST-ITEM
  - TERM-AGENDA
provenance:
  source: "openspec/specs/lists/spec.md (Agenda Projection); openspec/specs/calendar/spec.md (Member Agenda Projection, list items not editable scenario); openspec/specs/web-app/spec.md (Agenda Projected List Items); docs/_legacy/00_product/surfaces/lists.md (Relationship with Agenda, UX Grammar: Edit path); docs/_legacy/00_product/surfaces/agenda.md (Projected List Items, UX Grammar); docs/_legacy/04_contexts/shared-lists-item-model.md (Ownership of Behavior); docs/_legacy/03_domain/context-map.md (Temporal Item Projection); docs/_legacy/03_domain/ubiquitous-language.md (Projection)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A list item appearing in the Agenda cannot be edited, checked, rescheduled or deleted there. Selecting it shows a read-only view (title, due date, checked state, list name) with the single action "Open in Lists"; all edits go through the Lists surface (observed: docs/_legacy/00_product/surfaces/lists.md, Edit path; openspec/specs/lists/spec.md, Agenda Projection; openspec/specs/calendar/spec.md, Member Agenda Projection; docs/_legacy/00_product/surfaces/agenda.md, "This is non-negotiable"; openspec/specs/web-app/spec.md, Agenda Projected List Items).

## Rationale

Lists owns the item; the Agenda only projects it. Editing from the Agenda would break the ownership model (observed: docs/_legacy/00_product/surfaces/lists.md, Edit path; docs/_legacy/00_product/surfaces/agenda.md).

## Examples

- Tapping a projected "Buy stamps" item in the Agenda offers Open in Lists, not an edit form.
- Selecting "Buy birthday present" in the Agenda shows read-only detail with "Open in Lists" (observed: agenda.md).

## Exceptions

None.
