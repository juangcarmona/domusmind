---
id: BR-FAMILY-MEMBER-LIFECYCLE
type: business-rule
title: "People move through planned, active, inactive and archived"
status: draft
applies-to:
  - UC-FAMILY-MANAGE-MEMBER-LIFECYCLE
uses-terms:
  - TERM-MEMBER
provenance:
  source: "docs/_legacy/04_contexts/family-member-management.md (MemberStatus, MembershipPeriod, Invariants, Group B)"
  confidence: low
  recovered-from: documentation
---

## Rule

Planned: V1.1. A person is Planned, Active, Inactive or Archived. Allowed moves: Planned to Active, Planned to Archived, Active to Inactive, Active to Archived, Inactive to Archived. An inactive person cannot be reactivated; a returning person gets a new record. An archived person cannot be changed; an inactive person cannot receive new task assignments or be added to plans. A guest must have an end date for their stay, and an end date must follow its start date (observed: family-member-management.md, MemberStatus, MembershipPeriod, Invariants).

## Rationale

Households change over time (au pairs, guests, people moving out) and history should be kept rather than deleted (inferred).

## Examples

- An au pair arrives: Planned to Active.
- A guest is added without an end date: rejected.

## Exceptions

Expiry at the end date is manual in V1.1; automatic expiry is planned: V2 (observed: family-member-management.md, MembershipPeriod).
