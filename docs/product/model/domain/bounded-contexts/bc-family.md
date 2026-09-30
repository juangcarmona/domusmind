---
id: BC-FAMILY
type: bounded-context
title: Household and Members
status: draft
provenance:
  source: "openspec/specs/family/spec.md (Purpose, Identity Boundary Enforcement, Notes N1-N8); docs/_legacy/04_contexts/family.md (Purpose, Responsibilities, Boundaries With Other Contexts, Design Notes); docs/_legacy/04_contexts/family-member-management.md (Phase 1 scope); corroborated by src/backend/DomusMind.Domain/Family/**; interview: product owner decision Q-0021 (E-0158); interview: product owner decision Q-0022 (E-0158); interview: product owner decision Q-0024 (E-0158); interview: product owner decision Q-0025 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Responsibility

Defines the household and who belongs to it. It is the source of truth for the household's identity, its people (adults and children) and pets, each person's role, which people are household managers, each person's access to DomusMind, and, once shipped, the relationships between people (observed: openspec/specs/family/spec.md, Purpose; docs/_legacy/04_contexts/family.md, Responsibilities).

It also holds the household's own settings: its name, primary language, first day of week and date format, which are part of the baseline and changed only by managers (decided: Q-0025; observed: src/backend/DomusMind.Domain/Family/Family.cs).

The guiding question is "who belongs to this household?". What must happen for a person is answered elsewhere (observed: docs/_legacy/04_contexts/family.md, Design Notes).

## Language

Small and strict identity language: Household, Member (Person), Member Role, Manager, Pet, Relationship, Member Access, Member Directory and Member Profile. The legacy documents and the code say "Family" for the household and "Member" for a person; in the product these are "Household" and "Person" (observed: docs/_legacy/04_contexts/family.md, Ubiquitous Language Notes; recovery translation table).

The legacy language warns against ambiguous synonyms for a person such as "user", "account member", "profile", "contact" or "participant" unless another context owns them (observed: docs/_legacy/04_contexts/family.md, Ubiquitous Language Notes). "Dependent" was a separate concept in the legacy design and is retired: children are people with the Child role (observed: openspec/specs/family/spec.md, NOTE N1; family-member-management.md, "Retire Dependent"). The person roles are Adult, Child and Pet; there is no Caregiver role (decided: Q-0021).

## Boundaries

- Does not schedule plans, run tasks, assign areas, send reminders or plan meals (observed: docs/_legacy/04_contexts/family.md, Design Notes).
- Does not own authentication: it links a person to a sign-in account, and signing in belongs outside the household language (observed: openspec/specs/family/spec.md, NOTE N7).
- How a household is first created during onboarding is not specified (observed: openspec/specs/family/spec.md, NOTE N8). The person who creates the household is its first manager, and only adults can be managers (decided: Q-0022).
- Only managers add people and pets, remove them, manage relationships and change household settings (decided: Q-0024, Q-0025).
- Planned: V1.1 removing people and managing relationships; later, lifecycle states, household roles beyond manager, temporary members, contact and emergency information (observed: openspec/specs/family/spec.md; family-member-management.md, Future roadmap).

## External Relationships

Upstream of every other area: it emits more than it consumes, and no other area may create, change or remove household structure (observed: openspec/specs/family/spec.md, Identity Boundary Enforcement).

- BC-RESPONSIBILITIES refers to people as area owners; Household and Members owns whether a person is valid (observed: docs/_legacy/04_contexts/family.md, Responsibilities Context).
- BC-CALENDAR refers to people and pets as plan participants (observed: family.md, Calendar Context; openspec family spec, Pet Registration).
- BC-TASKS refers to people as task assignees; pets cannot be assignees (observed: family.md, Tasks Context; openspec family spec).
- The Agenda (in BC-CALENDAR) shows household rows only for the Adult and Child roles, never for pets (observed: family.md, Pet).
- BC-MEAL-PLANNING and the Agenda week views start from the household's configured first day of week (decided: Q-0025; household setting in code, referenced by meal-planning and agenda documents outside this area).
