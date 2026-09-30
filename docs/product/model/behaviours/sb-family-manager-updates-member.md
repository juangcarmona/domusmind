---
id: SB-FAMILY-MANAGER-UPDATES-MEMBER
type: structured-behaviour
title: "A manager updates a person's name and role"
status: draft
illustrates:
  - UC-FAMILY-UPDATE-MEMBER-DETAILS
given:
  - "A household exists with a person"
  - "The requester is a manager"
when: "The person's name, role or birth date is updated with valid values"
then:
  - "The person's record shows the new values"
uses-terms:
  - TERM-MANAGER
  - TERM-MEMBER
provenance:
  source: "openspec/specs/family/spec.md (Member Core Details Update, scenario \"Manager updates a member's name and role\")"
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes managers maintain core details.

## Boundaries

None.
