---
id: SB-FAMILY-MEMBER-REMOVED
type: structured-behaviour
title: "A person is removed from the household"
status: draft
illustrates:
  - UC-FAMILY-REMOVE-MEMBER
given:
  - "A household exists with at least one person"
when: "That person is removed"
then:
  - "The person is no longer part of the household roster"
uses-terms:
  - TERM-MEMBER
provenance:
  source: "openspec/specs/family/spec.md (Member Removal (V1.1), scenario \"A member is removed from the household\")"
  confidence: low
  recovered-from: documentation
---

## Intent

Planned: V1.1. Establishes removal takes a person off the roster.

## Boundaries

Does not assert anything about their tasks, plans or areas, which are handled by those areas.
