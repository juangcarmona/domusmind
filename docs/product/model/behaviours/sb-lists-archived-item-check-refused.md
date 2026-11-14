---
id: SB-LISTS-ARCHIVED-ITEM-CHECK-REFUSED
type: structured-behaviour
title: An archived List refuses an item check
status: active
illustrates:
  - BR-LISTS-ARCHIVED-READ-ONLY
  - UC-LISTS-TOGGLE-ITEM
given:
  - An archived List contains an unchecked item.
when: A person tries to check that item.
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

Preserve the checked state saved when a List was archived.

## Boundaries

The example concerns the stated action and context. Other actions have their own examples.
