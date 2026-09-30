---
id: BC-RESPONSIBILITIES
type: bounded-context
title: Responsibilities (Areas)
status: draft
provenance:
  source: "openspec/specs/areas/spec.md (Purpose, Notes); docs/_legacy/04_contexts/responsibilities.md (Purpose, Boundaries With Other Contexts, Design Notes); docs/_legacy/00_product/surfaces/areas.md (Purpose, Non-Goals); src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs; interview: product owner decision Q-0014 (E-0158); interview: product owner decision Q-0015 (E-0158); interview: product owner decision Q-0018 (E-0158); interview: product owner decision Q-0040 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Responsibility

Makes the household's accountability structure explicit: which Areas of household life exist, who owns each one, who supports each one, and where ownership is missing (observed: openspec/specs/areas/spec.md, Purpose; docs/_legacy/04_contexts/responsibilities.md, Purpose and Design Notes). It is the source of truth for who is accountable for what inside the household (observed: docs/_legacy/04_contexts/responsibilities.md, Responsibilities).

Any Household Member may create, rename, archive and change the ownership of Areas (decided: Q-0014).

It models accountability, not execution: owning an Area does not mean doing every task, plan or routine related to it (observed: openspec/specs/areas/spec.md, Purpose; docs/_legacy/04_contexts/responsibilities.md, Design Notes).

## Language

Household-facing language is Area, Owner and Support; the domain language is Responsibility Domain, Primary Owner and Secondary Owner. Both refer to the same concepts (observed: openspec/specs/areas/spec.md, Purpose and Notes, Terminology convergence; docs/_legacy/04_contexts/responsibilities.md, Purpose and Internal Entities). The Areas surface must avoid role jargon and permissions language (observed: docs/_legacy/00_product/surfaces/areas.md, Interaction).

Words to avoid in this context because they belong elsewhere or are ambiguous: admin, manager, assignee, category, tag (observed: docs/_legacy/04_contexts/responsibilities.md, Ubiquitous Language Notes). "Manager" in particular is the Family context's administrative role, not an Area role.

## Boundaries

Outside this context (observed: docs/_legacy/04_contexts/responsibilities.md, Design Notes; docs/_legacy/00_product/surfaces/areas.md, Non-Goals):

- Scheduling plans, completing tasks, generating reminders, maintaining inventory and planning meals.
- What happened today, what task is due now, what reminder should fire.
- Household membership itself, which belongs to Family.
- Permissions administration, analytics and household reporting; Areas is not a task board, a calendar, a list manager or an admin console.

## External Relationships

- Family (BC-FAMILY): Family defines who exists, Responsibilities defines who is accountable. Areas are scoped to one household and every owner or supporter must be a person of that household (observed: docs/_legacy/04_contexts/responsibilities.md, Boundaries With Other Contexts). The legacy context has Responsibilities react to a new household (to bootstrap default Areas) and to people being added or removed (to reconcile assignments) (observed: docs/_legacy/04_contexts/responsibilities.md, Domain Events Consumed). There are no default Areas for now (decided: Q-0015). Undecided, deferred (Q-0018): what happens to the Areas a removed person owns or supports is left to the definition of member removal (V1.1).
- Calendar (BC-CALENDAR), Tasks (BC-TASKS), Lists (BC-LISTS): may reference an Area for organisational context only; they never change ownership (observed: openspec/specs/areas/spec.md, Cross-Context Referencing; docs/_legacy/04_contexts/responsibilities.md, Boundaries With Other Contexts; docs/_legacy/03_domain/context-map.md). Meal Planning (BC-MEAL-PLANNING) does not reference Areas (decided: Q-0040).
- The Agenda may show Area cues on tasks and plans; owner and supporter names on the Areas surface lead to that person's Agenda (observed: docs/_legacy/00_product/surfaces/areas.md, Relationship with Other Surfaces and Inspector).
- Responsibilities does not consume operational events from Tasks, Food, Inventory or Administration (observed: docs/_legacy/04_contexts/responsibilities.md, Domain Events Consumed).
