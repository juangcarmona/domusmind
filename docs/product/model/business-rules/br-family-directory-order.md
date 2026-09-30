---
id: BR-FAMILY-DIRECTORY-ORDER
type: business-rule
title: "The directory is ordered by group, managers first, then name"
status: draft
applies-to:
  - UC-FAMILY-VIEW-MEMBER-DIRECTORY
uses-terms:
  - TERM-MEMBER-DIRECTORY
  - TERM-MANAGER
  - TERM-MEMBER-ROLE
provenance:
  source: "openspec/specs/family/spec.md (Household Member Directory, Pet Registration); docs/_legacy/04_contexts/family-member-management.md (Sort order); interview: product owner decision Q-0021 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Rule

The people directory lists adults first, then children, then pets. Within each group managers come before others, then entries are alphabetical by name (observed: openspec/specs/family/spec.md, Household Member Directory; family-member-management.md, Sort order).

## Rationale

A predictable order puts the people who run the household at the top (inferred).

## Examples

- Ana (Adult, manager), Bruno (Adult), Diego (Child), Rex (Pet), in that order.

## Exceptions

None.
