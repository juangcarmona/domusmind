---
id: BR-FAMILY-MANAGERS-ADMINISTER
type: business-rule
title: "Only managers administer the household"
status: draft
applies-to:
  - UC-FAMILY-ADD-MEMBER
  - UC-FAMILY-REGISTER-PET
  - UC-FAMILY-REMOVE-MEMBER
  - UC-FAMILY-MANAGE-RELATIONSHIPS
  - UC-FAMILY-CONFIGURE-HOUSEHOLD-SETTINGS
  - UC-FAMILY-UPDATE-MEMBER-DETAILS
  - UC-FAMILY-PROVISION-MEMBER-ACCESS
  - UC-FAMILY-DISABLE-MEMBER-ACCESS
  - UC-FAMILY-ENABLE-MEMBER-ACCESS
  - UC-FAMILY-RESET-MEMBER-ACCESS
uses-terms:
  - TERM-MANAGER
  - TERM-MEMBER-ACCESS
  - TERM-MEMBER-ROLE
  - TERM-HOUSEHOLD-SETTINGS
provenance:
  source: "openspec/specs/family/spec.md (Member Core Details Update, Member Access Provisioning); docs/_legacy/04_contexts/family-member-management.md (Permission rules); interview: product owner decision Q-0023 (E-0158); interview: product owner decision Q-0024 (E-0158); interview: product owner decision Q-0025 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Rule

Only a manager may:

- add people and pets, remove them, and manage relationships between people (decided: Q-0024);
- change the household settings: name, primary language, first day of week and date format (decided: Q-0025);
- change another person's core details (name, role, birth date), and change any person's role (observed: openspec/specs/family/spec.md, Member Core Details Update; decided: Q-0023);
- provision, disable, enable or reset another person's access (observed: openspec/specs/family/spec.md, Member Access Provisioning).

Any household member may view the directory, and a person may edit their own profile (observed: family-member-management.md, Permission rules).

## Rationale

Household administration needs clear authority so the roster, settings and access stay trustworthy (inferred).

## Examples

- A manager changes a child's role from Child to Adult.
- A non-manager trying to change another person's name is rejected (observed: scenario "Non-manager cannot update another member's core details").
- A non-manager corrects their own birth date: allowed (decided: Q-0023).
- A non-manager tries to change their own role from Child to Adult: rejected (decided: Q-0023).
- A non-manager tries to add a pet: rejected (decided: Q-0024).

## Exceptions

A person who is not a manager may change their own name and birth date, but not their own role (decided: Q-0023).
