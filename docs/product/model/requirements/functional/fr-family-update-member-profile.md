---
id: FR-FAMILY-UPDATE-MEMBER-PROFILE
type: functional-requirement
title: "People update their own profile"
status: draft
derived-from:
  - UC-FAMILY-UPDATE-MEMBER-DETAILS
verification:
  - scenario-ref: "SB-FAMILY-MEMBER-UPDATES-OWN-PROFILE"
  - scenario: "A manager updates another person's phone number and the profile shows it"
uses-terms:
  - TERM-MEMBER-PROFILE
provenance:
  source: "openspec/specs/family/spec.md (Member Profile Update)"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let a person update their own preferred name, phone, email and household note, and MUST let a manager update any person's profile (observed: openspec/specs/family/spec.md, Member Profile Update).

## Rationale

People keep their own contact details current without depending on a manager.
