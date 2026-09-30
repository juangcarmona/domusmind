---
id: SB-FAMILY-NON-MANAGER-UPDATE-REJECTED
type: structured-behaviour
title: "A non-manager cannot update another person's core details"
status: draft
illustrates:
  - BR-FAMILY-MANAGERS-ADMINISTER
given:
  - "A household exists with a person"
  - "The requester is neither a manager nor that person"
when: "A core detail update is attempted"
then:
  - "The update is rejected"
uses-terms:
  - TERM-MANAGER
provenance:
  source: "openspec/specs/family/spec.md (Member Core Details Update, scenario \"Non-manager cannot update another member's core details\"); interview: product owner decision Q-0023 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes core details are administrative.

## Boundaries

Does not cover a non-manager editing their own record: they may change their own name and birth date but not their role (decided: Q-0023; see SB-FAMILY-MEMBER-UPDATES-OWN-DETAILS and SB-FAMILY-NON-MANAGER-OWN-ROLE-REJECTED).
