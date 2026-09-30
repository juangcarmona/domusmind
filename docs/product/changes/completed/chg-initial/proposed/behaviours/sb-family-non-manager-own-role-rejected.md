---
id: SB-FAMILY-NON-MANAGER-OWN-ROLE-REJECTED
type: structured-behaviour
title: "A person who is not a manager cannot change their own role"
status: draft
illustrates:
  - BR-FAMILY-MANAGERS-ADMINISTER
given:
  - "A household exists with a person who is not a manager"
  - "The requester is that person"
when: "The person tries to change their own role"
then:
  - "The change is rejected"
  - "The person's role is unchanged"
uses-terms:
  - TERM-MEMBER-ROLE
  - TERM-MANAGER
provenance:
  source: "interview: product owner decision Q-0023 (E-0158)"
  confidence: high
  recovered-from: interview
---

## Intent

Establishes that the role is administrative even on a person's own record.

## Boundaries

Does not restrict changes to the person's own name or birth date.
