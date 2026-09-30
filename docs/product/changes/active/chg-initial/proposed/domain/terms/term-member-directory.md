---
id: TERM-MEMBER-DIRECTORY
type: domain-term
title: "People Directory"
status: draft
defined-in: BC-FAMILY
synonyms:
  - "Member Directory"
  - "Household Roster"
uses-terms:
  - TERM-MEMBER
  - TERM-MEMBER-ROLE
  - TERM-MANAGER
  - TERM-MEMBER-ACCESS
provenance:
  source: "openspec/specs/family/spec.md (Household Member Directory); docs/_legacy/04_contexts/family-member-management.md (Sort order, MemberDirectoryItemResponse, Permission rules); interview: product owner decision Q-0021 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Definition

The household's list of its people and pets as every household member sees it. Adults come first, then children, then pets; within each group managers come first, then alphabetical by name. Each entry carries the viewer's permissions and the person's access state as decided by DomusMind (observed: openspec/specs/family/spec.md, Household Member Directory; family-member-management.md, Sort order).

## Distinguish From

- Not the Person Profile (TERM-MEMBER-PROFILE), which is a person's own contact details and is not meant to grow the directory (observed: family-member-management.md, section 8).

## Usage

The entry point for household administration: from it a manager edits people and manages their access (inferred from the per-entry "can edit" and "can grant access" indications in family-member-management.md).
