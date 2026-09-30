---
id: FR-FAMILY-UPDATE-MEMBER-DETAILS
type: functional-requirement
title: "Update people's core details"
status: draft
derived-from:
  - UC-FAMILY-UPDATE-MEMBER-DETAILS
  - BR-FAMILY-MANAGERS-ADMINISTER
  - BR-FAMILY-BIRTH-DATE-IN-PAST
verification:
  - scenario-ref: "SB-FAMILY-MANAGER-UPDATES-MEMBER"
  - scenario-ref: "SB-FAMILY-NON-MANAGER-UPDATE-REJECTED"
  - scenario-ref: "SB-FAMILY-FUTURE-BIRTH-DATE-REJECTED"
  - scenario-ref: "SB-FAMILY-MEMBER-UPDATES-OWN-DETAILS"
  - scenario-ref: "SB-FAMILY-NON-MANAGER-OWN-ROLE-REJECTED"
uses-terms:
  - TERM-MEMBER
  - TERM-MANAGER
provenance:
  source: "openspec/specs/family/spec.md (Member Core Details Update); interview: product owner decision Q-0023 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a manager update a person's full name, role and optional birth date, and MUST let a person who is not a manager update their own full name and birth date (decided: Q-0023). It MUST reject an update by a person who is not a manager to someone else's details or to their own role (observed: openspec/specs/family/spec.md, Member Core Details Update; decided: Q-0023), and MUST reject a birth date that is not in the past.

## Rationale

The roster must stay accurate as the household changes.
