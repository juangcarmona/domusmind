---
id: JRN-PRODUCT-ONBOARD-HOUSEHOLD
type: journey
title: "Start a household and reach a working Agenda"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
steps:
  - use-case: "UC-FAMILY-CREATE-HOUSEHOLD"
  - use-case: "UC-FAMILY-ADD-MEMBER"
  - use-case: "UC-CALENDAR-SCHEDULE-PLAN"
  - use-case: "UC-TASKS-CREATE-ROUTINE"
  - use-case: "UC-LISTS-CREATE-LIST"
  - use-case: "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
provenance:
  source: "docs/_legacy/00_product/experience.md (Onboarding, Success Criteria); openspec/specs/web-app/spec.md (Agenda Default Entry State); docs/_legacy/00_product/public-site.md (CTA Hierarchy: Start your household); interview: product owner decision Q-0055 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intended Outcome

A new household has a useful shared system quickly: its people are in it, it can hold first useful state, and the Agenda shows it. The result is a working system, not an empty shell (observed: docs/_legacy/00_product/experience.md, Onboarding). Onboarding does not require a first item (decided: Q-0055).

## Entry Conditions

- A person who does not yet belong to a household decides to start one (observed: experience.md, Onboarding step 1; public-site.md primary CTA "Start your household").

## Journey Narrative

1. The person starts a household and names it (observed: docs/_legacy/00_product/experience.md, Onboarding steps 1-2).
2. They add the other people of the household (observed: docs/_legacy/00_product/experience.md, Onboarding step 3).
3. Optionally, they add first useful state, such as a plan, a routine or a list (decided: Q-0055; observed: docs/_legacy/00_product/experience.md, Onboarding step 4). The order among plans, routines and lists is free; the steps list shows one path (inferred).
4. DomusMind shows the Agenda immediately in its default state: household scope, day mode, today (observed: docs/_legacy/00_product/experience.md, Onboarding step 5; openspec/specs/web-app/spec.md, Agenda Default Entry State).

## Variants and Branches

- The first useful state may be a task or a list item instead of a plan or routine (inferred from experience.md, "such as").
- A person may give others access so they can sign in themselves (inferred; the Family area's access use cases).
- The person skips adding state: DomusMind shows an empty today view with local capture prompts (decided: Q-0055).

## Completion Conditions

The Agenda opens on today's household day and shows the state just added, or, when nothing was added, an empty today view with local capture prompts (observed: docs/_legacy/00_product/experience.md, Onboarding "Show Agenda immediately"; decided: Q-0055).
