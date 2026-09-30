---
id: SB-LISTS-BROWSE-ACTIVE
type: structured-behaviour
title: "The list switcher shows active lists with unchecked counts"
status: draft
illustrates:
  - UC-LISTS-BROWSE-LISTS
given:
  - "a household has several active lists"
when: "a person asks for the household's lists"
then:
  - "every active list is returned with its name and number of unchecked items"
  - "archived lists are not included"
uses-terms:
  - TERM-LIST
  - TERM-ARCHIVED-LIST
provenance:
  source: "openspec/specs/lists/spec.md (Requirement: List Retrieval; Scenario: Household views the list switcher)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Counts are visible before a list is opened.

## Boundaries

None.
