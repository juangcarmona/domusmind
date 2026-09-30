---
id: SB-LISTS-CREATE-NAME-ONLY
type: structured-behaviour
title: "Creating a list with a name only"
status: draft
illustrates:
  - UC-LISTS-CREATE-LIST
  - BR-LISTS-LIST-NAME-REQUIRED
given:
  - "a household exists"
when: "a person creates a list with a valid name and no other details"
then:
  - "the list is created and available for use"
  - "the list has no items"
uses-terms:
  - TERM-LIST
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Creation; Scenario: Household creates a list with a name only)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

A name is the only input needed to create a list.

## Boundaries

None.
