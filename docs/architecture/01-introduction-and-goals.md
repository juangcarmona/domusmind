---
title: Introduction and Goals
arc42-section: "01"
description: Architectural orientation for DomusMind, the drivers that shape it, its priority quality goals and the readers of this documentation.
---

# Introduction and Goals

## Requirements Overview

DomusMind is a shared household system delivered as one ASP.NET Core application
(the API plus the web app it serves) over a single PostgreSQL database, with a
separate static public site. What the product does, for whom and why is owned by the
[product model](../product/model/); this section names only the drivers that shape the
architecture.

The scope of the running system is the six product areas, each implemented as one
module in the backend (Family, Responsibilities, Calendar, Tasks, Lists, Meal
Planning), plus local authentication and first-run setup. Anything outside that
scope has no building block today.

<!-- pdac:cite id="CON-PRODUCT-CURRENT-SCOPE" digest="sha256:5f3b2eacf39dd82ebe0c6616860f4f0d9edac9589d0f8889a7e093db12e9e239" -->

The household, not the individual user, is the unit around which data and access are
partitioned: every household-owned record carries its family identifier, and the API
checks the caller's access to that family before acting.

<!-- pdac:cite id="CON-PRODUCT-HOUSEHOLD-FIRST" digest="sha256:9ffd7fd5d31817890e9263cea8286b25102bd41092769a0cfe3e0765792808b3" -->

Each product axis is owned by exactly one module; read surfaces such as the Agenda
combine data from several modules without owning it. This is the main reason the
backend is a modular monolith with strict module boundaries rather than one shared
model.

<!-- pdac:cite id="CON-PRODUCT-SEPARATE-AXES" digest="sha256:740fa2cc41d90ec6afb099c5f97fa44e9092af8d83cd68291f807262387a44b0" -->

## Quality Goals

The highest-priority quality goals for the architecture, in order. Section 10
([Quality Requirements](10-quality-requirements.md)) owns the full mapping from
quality requirements to their realisation and evidence.

| Priority | Quality goal | Why it shapes the architecture |
| --- | --- | --- |
| 1 | Evolvability of the domain model | Areas must grow independently without blurring ownership, so the domain is the stable, framework-free centre and modules collaborate only through identifiers and domain events ([strategy](04-solution-strategy.md)). |
| 2 | Household data isolation | All access is scoped to one household and enforced server-side at the application boundary ([crosscutting concepts](08-crosscutting-concepts.md)). |
| 3 | One product on every screen | Desktop and mobile are the same responsive web app over one API, so there is no client-specific backend. |
| 4 | Today understood at a glance | The Agenda is assembled server-side as a read projection across modules, so the client renders one response instead of joining sources. |
| 5 | Operable by a single household | The whole system ships as one container image plus PostgreSQL ([deployment view](07-deployment-view.md)). No product-model artifact owns this goal; it is an engineering goal evidenced by [`deploy/`](../../deploy/). |

Goal 3 realises:

<!-- pdac:cite id="QR-WEB-MOBILE-SAME-PRODUCT" digest="sha256:c318bd0291b8badc6c509bd6dbdc22cf069487aa03b5d15714210312ac261e5d" -->

Goal 4 realises:

<!-- pdac:cite id="QR-PRODUCT-TODAY-AT-A-GLANCE" digest="sha256:659932b0b836c008118516caa8647d05dc133c1e12c31114f6df3a49527420fa" -->

## Stakeholders

Product stakeholders and actors are owned by the product model
([`docs/product/model/actors/`](../product/model/actors/)). The readers of this
architecture documentation are:

| Reader | Expectation |
| --- | --- |
| Maintainers and contributors | Know where a change belongs, which module boundary and layer rule it must respect, and which ADR governs it. |
| Coding agents | A stable, citation-checked description of the shape they must preserve, consistent with [`AGENTS.md`](../../AGENTS.md). |
| Self-hosting operators | What runs, what it depends on and what crosses the system boundary ([03](03-context-and-scope.md), [07](07-deployment-view.md)). |
