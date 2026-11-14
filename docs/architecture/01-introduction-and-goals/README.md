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
[product model](../../product/model/); this section names only the drivers that shape the
architecture.

The backend implements Family, Responsibilities, Calendar, Tasks, Lists, Meal
Planning, Inbox and Chats, alongside authentication and first-run setup. Inbox
passes approved proposals to their owning area through domain events. Household
Assets is accepted product intent and has no backend module at this revision.
The product model describes intent; the implementation evidence determines which
capabilities are delivered.

<!-- pdac:cite id="CON-PRODUCT-CURRENT-SCOPE" digest="sha256:86c4b7c550c3e4edf5c1f614510ece6a13274480c6333c7114e1a103f0219169" -->

The household is the unit around which data and access are
partitioned: every household-owned record carries its family identifier, and the API
checks the caller's access to that family before acting.

<!-- pdac:cite id="CON-PRODUCT-HOUSEHOLD-FIRST" digest="sha256:c8abe3f8c03538b49288c014a327aa3da0f2cf0d44ea2c091aee99d34f8f663a" -->

Each product axis is owned by exactly one module; read surfaces such as the Agenda
combine data from several modules without owning it. This is the main reason the
backend is a modular monolith with strict module boundaries rather than one shared
model.

<!-- pdac:cite id="CON-PRODUCT-SEPARATE-AXES" digest="sha256:f0adb53673dfbfd7051791182578f9ee2f44e0a37b7699c16258854579342a13" -->

## Quality Goals

The highest-priority quality goals for the architecture, in order. Section 10
([Quality Requirements](../10-quality-requirements/README.md)) owns the full mapping from
quality requirements to their realisation and evidence.

| Priority | Quality goal | Why it shapes the architecture |
| --- | --- | --- |
| 1 | Evolvability of the domain model | Areas must grow independently without blurring ownership, so the domain is the stable, framework-free centre and modules collaborate only through identifiers and domain events ([strategy](../04-solution-strategy/README.md)). |
| 2 | Household data isolation | All access is scoped to one household and enforced server-side at the application boundary ([crosscutting concepts](../08-crosscutting-concepts/README.md)). |
| 3 | One product on every screen | Desktop and mobile are the same responsive web app over one API, so there is no client-specific backend. |
| 4 | Today understood at a glance | The Agenda is assembled server-side as a read projection across modules, so the client renders one response instead of joining sources. |
| 5 | Operable by a single household | The whole system ships as one container image plus PostgreSQL ([deployment view](../07-deployment-view/README.md)). No product-model artifact owns this goal; it is an engineering goal evidenced by [`deploy/`](../../../deploy/). |

Goal 3 realises:

<!-- pdac:cite id="QR-WEB-MOBILE-SAME-PRODUCT" digest="sha256:0b38079d5c55a3bcc863ca9b9903dc959910db3df187576c9c9522546b605047" -->

Goal 4 realises:

<!-- pdac:cite id="QR-PRODUCT-TODAY-AT-A-GLANCE" digest="sha256:5de2ebf4d1b4d156a4c05bd53d8bb4aa3145206cf6ad9669f9aeb2a0f85ecb37" -->

## Stakeholders

Product stakeholders and actors are owned by the product model
([`docs/product/model/actors/`](../../product/model/actors/)). The readers of this
architecture documentation are:

| Reader | Expectation |
| --- | --- |
| Maintainers and contributors | Know where a change belongs, which module boundary and layer rule it must respect, and which ADR governs it. |
| Contributors | A stable, citation-checked description of the system boundaries and accepted decisions. |
| Self-hosting operators | What runs, what it depends on and what crosses the system boundary ([03](../03-context-and-scope/README.md), [07](../07-deployment-view/README.md)). |
