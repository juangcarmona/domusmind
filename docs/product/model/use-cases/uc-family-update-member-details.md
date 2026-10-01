---
id: UC-FAMILY-UPDATE-MEMBER-DETAILS
type: use-case
title: "Edit a person, including my own profile"
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-MANAGERS-ADMINISTER
  - BR-FAMILY-BIRTH-DATE-IN-PAST
  - BR-FAMILY-PERSON-NEEDS-NAME-AND-ROLE
  - BR-FAMILY-MANAGER-MUST-BE-ADULT
uses-terms:
  - TERM-MEMBER
  - TERM-MANAGER
  - TERM-MEMBER-ROLE
  - TERM-MEMBER-PROFILE
provenance:
  source: "openspec/specs/family/spec.md (Member Core Details Update); docs/_legacy/04_contexts/family.md (Commands: UpdateMember); docs/_legacy/04_contexts/family-member-management.md (update-member-core-details); src/backend/DomusMind.Domain/Family/Family.cs (UpdateMember); interview: product owner decision Q-0022 (E-0158); interview: product owner decision Q-0023 (E-0158); openspec/specs/family/spec.md (Member Profile Update); docs/_legacy/04_contexts/family.md (Commands: UpdateMemberProfile); src/backend/DomusMind.Domain/Family/FamilyMember.cs (UpdateProfile); interview: product owner decision Q-0060 (E-0158); interview: product owner review decision (merges)"
  confidence: high
  recovered-from: documentation
---

## Goal

A person's record is correct and current: their core details (full name, role, birth date) and their profile (preferred name, phone, email, household note, avatar icon and colour) (observed: openspec/specs/family/spec.md, Member Core Details Update, Member Profile Update).

## Trigger

A person wants others to see how to call or reach them, or a manager notices a person's details are wrong or have changed, for example a child becoming an adult.

## Preconditions

The person being edited exists in the household and the actor is signed in to it. The actor is editing their own record, or is a manager editing any person's record (decided: Q-0023; observed: openspec family spec, "Managers may also update any member's profile details").

## Main Flow

1. The actor opens a person from the directory, or opens their own profile.
2. The actor changes the person's full name, birth date, preferred name, phone, email or household note, or chooses an avatar icon and colour (decided: Q-0060; observed: src/backend/DomusMind.Domain/Family/FamilyMember.cs).
3. DomusMind saves the new values.

## Alternative Flows

- A manager changes a person's role, or whether an adult is a manager; the designation is set when editing an adult, and only adults qualify (decided: Q-0022; observed: Family.cs UpdateMember).
- A person who is not a manager edits their own record: they may change their own full name, birth date and profile, but not their role (decided: Q-0023).

## Failure Conditions

- The actor is not a manager and changes another person's details or profile: rejected (decided: Q-0023; inferred from BR-FAMILY-MANAGERS-ADMINISTER).
- The actor is not a manager and changes their own role: rejected (decided: Q-0023).
- The birth date is in the future: rejected with a validation error.

## Postconditions

The person's record shows the new details everywhere in the product; the preferred name, when set, is shown instead of the person's name (decided: Q-0060; observed: FamilyMember.cs, DisplayName). Keeping several contact methods, addresses and emergency contacts per person is planned, see CHG-ROADMAP.
