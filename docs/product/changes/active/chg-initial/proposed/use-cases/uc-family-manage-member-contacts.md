---
id: UC-FAMILY-MANAGE-MEMBER-CONTACTS
type: use-case
title: "Keep contact and emergency information for a person"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by: []
uses-terms:
  - TERM-MEMBER-PROFILE
  - TERM-MEMBER
provenance:
  source: "docs/_legacy/04_contexts/family-member-management.md (section 8, Groups F-G)"
  confidence: low
  recovered-from: documentation
---

## Goal

Planned: V2. The household keeps several contact methods, addresses, emergency contacts and document metadata (type, issuer, expiry, never the file) for each person (observed: family-member-management.md, section 8, Groups F-G).

## Trigger

The household wants essential information about a person in one place.

## Preconditions

The person exists.

## Main Flow

1. The actor adds or removes a contact method, address or emergency contact for the person.
2. The actor marks one contact of each type as primary.

## Alternative Flows

None known.

## Failure Conditions

None stated.

## Postconditions

The information is available on the person's profile, not in the directory (observed: family-member-management.md, section 8). Who may edit it is not stated. Roadmap proposal only.
