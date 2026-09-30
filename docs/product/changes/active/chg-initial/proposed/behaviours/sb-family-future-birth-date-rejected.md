---
id: SB-FAMILY-FUTURE-BIRTH-DATE-REJECTED
type: structured-behaviour
title: "A future birth date is rejected"
status: draft
illustrates:
  - BR-FAMILY-BIRTH-DATE-IN-PAST
when: "A person's details are submitted with a birth date in the future"
then:
  - "The update is rejected"
  - "A validation error is shown"
uses-terms:
  - TERM-MEMBER
provenance:
  source: "openspec/specs/family/spec.md (Member Core Details Update, scenario \"Birth date in the future is rejected\")"
  confidence: high
  recovered-from: documentation
---

## Intent

Establishes birth dates must be in the past.

## Boundaries

None.
