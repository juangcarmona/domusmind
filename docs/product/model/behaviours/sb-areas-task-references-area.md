---
id: SB-AREAS-TASK-REFERENCES-AREA
type: structured-behaviour
title: A task linked to an Area does not change its ownership
status: draft
illustrates:
- BR-AREAS-OWNERSHIP-EXCLUSIVE
when: A task is created with a reference to an Area
then:
- The task records the Area as its organisational context
- The Area's Owner and Support are unchanged
uses-terms:
- TERM-AREA
- TERM-TASK
provenance:
  source: 'openspec/specs/areas/spec.md (scenario: A task references an Area)'
  confidence: high
  recovered-from: documentation
---

## Intent

Pins that referencing an Area is categorisation only.

## Boundaries

Does not assert who becomes the task's assignee.
