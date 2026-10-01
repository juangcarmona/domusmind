---
id: SB-FAMILY-DUPLICATE-MEMBER-REJECTED
type: structured-behaviour
title: "A person with an existing identity is rejected"
status: draft
illustrates:
  - BR-FAMILY-PERSON-UNIQUE-IN-HOUSEHOLD
given:
  - "A household exists with an existing person"
when: "A new person is submitted with the same identity as the existing one"
then:
  - "The person is not added"
  - "A validation error is shown"
uses-terms:
  - TERM-MEMBER
provenance:
  source: "openspec/specs/family/spec.md (Member Addition, scenario \"Member addition fails with a duplicate MemberId\")"
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes identities are unique within a household.

## Boundaries

Two people may still share the same name.
