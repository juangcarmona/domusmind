---
id: BR-FAMILY-BIRTH-DATE-IN-PAST
type: business-rule
title: "A birth date must be in the past"
status: draft
applies-to:
  - UC-FAMILY-UPDATE-MEMBER-DETAILS
uses-terms:
  - TERM-MEMBER
provenance:
  source: "openspec/specs/family/spec.md (Member Core Details Update); docs/_legacy/04_contexts/family.md (Commands: UpdateMember)"
  confidence: high
  recovered-from: documentation
---

## Rule

When a person's birth date is given, it must be in the past (observed: openspec/specs/family/spec.md, Member Core Details Update).

## Rationale

A future birth date cannot describe someone who is already in the household (inferred).

## Examples

- A birth date of 2015-04-02 is accepted.
- A birth date next year is rejected with a validation error (observed: scenario "Birth date in the future is rejected").

## Exceptions

The birth date is optional; leaving it out is always allowed.
