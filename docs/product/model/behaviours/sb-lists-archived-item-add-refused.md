---
id: SB-LISTS-ARCHIVED-ITEM-ADD-REFUSED
type: structured-behaviour
title: An archived List refuses a new item
status: active
illustrates:
  - BR-LISTS-ARCHIVED-READ-ONLY
  - UC-LISTS-ADD-ITEM
given:
  - a list has been archived
when: A person tries to add an item to the archived List.
then:
  - the change is rejected
  - the list's items stay as they were when it was archived
uses-terms:
  - TERM-ARCHIVED-LIST
  - TERM-LIST-ITEM
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: Item Addition); interview: product owner decision Q-0031 (E-0158)"
  confidence: high
  recovered-from: interview
---

## Intent

Keep an archived checklist intact until the household restores it.

## Boundaries

The example concerns the stated action and context. Other actions have their own examples.
