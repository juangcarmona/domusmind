---
id: BR-WEB-SETTINGS-STAYS-CONFIGURATION
type: business-rule
title: "Settings holds configuration only"
status: draft
applies-to:
  - "UC-WEB-MANAGE-SETTINGS"
uses-terms:
  - "TERM-PLAN"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
provenance:
  source: "openspec/specs/web-app/spec.md (Settings Surface); docs/_legacy/00_product/surfaces/settings.md (Role, Information Architecture, Cross-Surface Relationship, Permissions and Ownership, Non-Goals)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Settings is the low-frequency configuration surface and does not duplicate operational functionality: it does not edit plans, does not show imported calendar entries as a calendar, performs no write-back to a provider, and in phase 1 gives no one control over another person's calendar connections. Imported entries are inspected in the Agenda, not in Settings (observed: openspec/specs/web-app/spec.md, Settings Surface "SHALL NOT handle"; docs/_legacy/00_product/surfaces/settings.md, Non-Goals, Cross-Surface Relationship).

## Rationale

Operational surfaces stay the place where household state is read and changed; Settings must not become a dumping ground or a second agenda (observed: docs/_legacy/00_product/surfaces/settings.md, Information Architecture, Cross-Surface Relationship).

## Examples

- A person who wants to see tomorrow's Outlook meetings goes to their Agenda scope, not to Settings.
- The calendar connections section shows sync status, not the imported entries themselves.

## Exceptions

A later household-administrator override of someone else's connections would have to be explicit and auditable; it is not part of phase 1 (observed: docs/_legacy/00_product/surfaces/settings.md, Permissions and Ownership).
