# DomusMind — Roadmap

## Purpose

This document defines the phased evolution of DomusMind.

The roadmap prioritizes a strong V1 core and controlled expansion.

---

## V1 — Household Operating Core

Goal:

Deliver the minimum viable household operating model.

Included contexts:

- Family
- Responsibilities
- Calendar
- Tasks 

Included capabilities:

- create family
- add members
- assign relationships
- create responsibility domains
- assign owners
- schedule and manage events
- create and complete tasks
- create and manage routines
- view family timeline

Expected outcome:

- shared household identity
- explicit accountability
- unified time view
- operational task execution

---

## V1.1 — Operational Hardening

Goal:

Stabilize the V1 architecture and prepare the system for real usage.

Important rule:

V1.1 introduces **no new bounded contexts**.

Work in this phase focuses on:

- reliability
- API maturity
- operational readiness
- security reinforcement

No new major capabilities are added.

---

## V2 — Household Expansion

Goal:

Extend DomusMind beyond the operational core.

Candidate contexts:

- Administration
- Properties
- Inventory
- Food

Possible capabilities:

- document tracking
- contract renewals
- property maintenance
- shopping lists
- pantry tracking
- meal planning

Expected outcome:

- broader household coverage
- stronger anticipation of obligations
- less repetitive coordination work

---

## V3 — Intelligence and Integrations

Goal:

Add interpretation, automation, and external surfaces.

Possible capabilities:

- messaging integrations
- calendar synchronization
- AI-assisted input interpretation
- reminder routing
- automation pipelines
- projections and analytics

Expected outcome:

- faster capture
- lower friction
- better anticipation
- extensible ecosystem

---

## Out of Scope for Current Phase

Not part of the current implementation focus:

- advanced AI agents
- full finance platform
- complex multi-tenant SaaS concerns
- external identity federation as a requirement
- large integration catalog

---

## Planning Rule

The roadmap is additive.

New phases must not weaken:

- domain clarity
- bounded context ownership
- aggregate integrity
- API consistency

---

## Summary

DomusMind evolves in three major steps:

- V1: core household operating model
- V2: household domain expansion
- V3: intelligence and integrations

Current priority is V1.

---

## Architectural Stability Rule

Each roadmap phase must preserve:

- bounded context boundaries
- aggregate ownership
- slice independence
- API consistency

New functionality should extend the model without weakening the domain structure.