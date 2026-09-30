---
id: SB-FAMILY-MEMBER-UPDATES-OWN-DETAILS
type: structured-behaviour
title: "A person who is not a manager updates their own name and birth date"
status: draft
illustrates:
  - UC-FAMILY-UPDATE-MEMBER-DETAILS
  - BR-FAMILY-MANAGERS-ADMINISTER
given:
  - "A household exists with a person who is not a manager"
  - "The requester is that person"
when: "The person changes their own full name and birth date with valid values"
then:
  - "The person's record shows the new name and birth date"
uses-terms:
  - TERM-MEMBER
  - TERM-MANAGER
provenance:
  source: "interview: product owner decision Q-0023 (E-0158)"
  confidence: high
  recovered-from: interview
---

## Intent

Establishes that people keep their own name and birth date correct without a manager.

## Boundaries

Does not cover the role, which a person who is not a manager cannot change (see SB-FAMILY-NON-MANAGER-OWN-ROLE-REJECTED).
